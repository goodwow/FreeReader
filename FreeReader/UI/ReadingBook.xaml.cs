using FreeReader.HotKey;
using FreeReader.Model;
using FreeReader.Models;
using FreeReader.Properties;
using FreeReader.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace FreeReader
{
    public partial class ReadingBook : Window
    {

        #region 初始化

        /// <summary>
        /// 当前窗口句柄
        /// </summary>
        private IntPtr m_Hwnd = new IntPtr();

        /// <summary>
        /// 记录快捷键注册项的唯一标识符
        /// </summary>
        private Dictionary<EHotKeySetting, int> m_HotKeySettings = new Dictionary<EHotKeySetting, int>();

        private Book m_CurrentBook;

        private ObservableCollection<Content> novel = new ObservableCollection<Content>();
        private ObservableCollection<Content> headers = new ObservableCollection<Content>();

        public ObservableCollection<Content> Novel
        {
            get { return this.novel; }
            set { this.novel = value; }
        }

        public ObservableCollection<Content> Headers
        {
            get { return this.headers; }
            set { this.headers = value; }
        }

        private ListBoxWrapper editor;
        private ListBoxWrapper sidebar;

        private KeyHandler headerListBoxHandler;
        private KeyHandler paragraphListBoxHandler;

        public SettingsModel ReadSettings
        {
            get
            {
                return SettingsManager.Instance.ReadSettings;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReadingBook(Book book)
        {
            InitializeComponent();

            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Activated += Window_Activated;

            editor = new Reader(ParagraphListBox);
            sidebar = new Sidebar(HeaderListBox);

            headerListBoxHandler = new HeaderListBoxKeyHandler(sidebar, editor);
            paragraphListBoxHandler = new ParagraphListBoxKeyHandler(editor, sidebar);

            this.m_CurrentBook = book;
            this.LoadLastFile();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.Activated -= Window_Activated;
            }
        }

        /// <summary>
        /// 窗体加载完成后事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HotKeySettingsManager.Instance.RegisterGlobalHotKeyEvent += Instance_RegisterGlobalHotKeyEvent;
            // 注册快捷键
            InitHotKey();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                //显示
                this.Show();
            }));
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// WPF窗体的资源初始化完成，并且可以通过WindowInteropHelper获得该窗体的句柄用来与Win32交互后调用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            // 获取窗体句柄
            m_Hwnd = new WindowInteropHelper(this).Handle;
            HwndSource hWndSource = HwndSource.FromHwnd(m_Hwnd);
            // 添加处理程序
            if (hWndSource != null) hWndSource.AddHook(WndProc);
        }

        /// <summary>
        /// 所有控件初始化完成后调用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            // 注册热键
            InitHotKey();
        }

        #endregion

        #region 目录正文

        private void HeaderListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editor.FocusOnLineItem(sidebar.CurrentLineItem());
            editor.Focus();

            // save settings
            this.m_CurrentBook.LastFileChapter = sidebar.CurrentLine();
            this.m_CurrentBook.LastFileLineNum = editor.CurrentLine();
            BooksManager.Instance.UpdateBook(this.m_CurrentBook);
        }

        private void ParagraphListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // empty headers
            if (this.Headers.Count < 1)
            {
                return;
            }

            // find nearest chapter
            object header = this.Headers.First();

            for (int i = editor.CurrentLine(); i >= 0; i--)
            {
                if (this.Novel[i].IsHeader)
                {
                    header = this.Novel[i];
                    break;
                }
            }

            // select chapter
            sidebar.FocusOnLineItem(header);

            // save settings
            this.m_CurrentBook.LastFileChapter = sidebar.CurrentLine();
            this.m_CurrentBook.LastFileLineNum = editor.CurrentLine();
            BooksManager.Instance.UpdateBook(this.m_CurrentBook);
        }

        private void HeaderListBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = headerListBoxHandler.Handle(e);
        }

        private void ParagraphListBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = paragraphListBoxHandler.Handle(e);
        }

        private void ScrollToLine(int lineNum, int chapterNum)
        {
            sidebar.FocusOnLine(chapterNum);
            editor.FocusOnLine(lineNum);
            editor.Focus();
        }

        #endregion

        #region 右键菜单

        /// <summary>
        /// 显示目录事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowDirectory_Click(object sender, RoutedEventArgs e)
        {
            DrawerHost.IsLeftDrawerOpen = true;
        }

        /// <summary>
        /// 设置事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            DrawerHost.IsBottomDrawerOpen = true;
        }

        /// <summary>
        /// 退出事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            BooksManager.Instance.UpdateBook(this.m_CurrentBook);
            SystemCommands.CloseWindow(this);
        }


        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            BooksManager.Instance.UpdateBook(this.m_CurrentBook);
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 打开文件事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = "txt";
            fileDialog.Filter = "Text files (*.txt)|*.txt";
            fileDialog.ShowDialog();

            LoadFile(fileDialog.FileName);

            // 将书籍加入书架
            if (!String.IsNullOrWhiteSpace(fileDialog.FileName))
            {
                LoadFile(fileDialog.FileName);

                String fileName = fileDialog.SafeFileName;
                fileName = fileName.Substring(0, fileName.LastIndexOf("."));
                if (fileName.Contains("《"))
                {
                    string tempName = Tool.GetRegexStr(fileName, "《([\\s\\S]*?)》").Trim();
                    if (!String.IsNullOrEmpty(tempName))
                    {
                        fileName = tempName;
                    }
                }
                Book book = new Book()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    FilePath = fileDialog.FileName,
                    Name = fileName
                };

                BooksManager.Instance.BookList.Add(book);
            }
        }

        #endregion

        #region 加载文件内容

        private bool OpenAndLoadFile(string path)
        {
            if (String.IsNullOrEmpty(path))
                return false;

            this.Novel.Clear();
            this.Headers.Clear();

            NovelFile.Open(path, this.Novel);

            foreach (Content c in this.Novel)
            {
                if (c.IsHeader)
                {
                    this.Headers.Add(c);
                }
            }

            return true;
        }


        private void LoadLastFile()
        {
            try
            {
                if (OpenAndLoadFile(this.m_CurrentBook.FilePath))
                {
                    ScrollToLine(this.m_CurrentBook.LastFileLineNum, this.m_CurrentBook.LastFileChapter);
                }
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("你上次阅读的文件： " + e.FileName + " 找不到了，看个别的吧。", "读取文件错误！");

                // reset settings
                this.m_CurrentBook.FilePath = "";
                this.m_CurrentBook.LastFileChapter = -1;
                this.m_CurrentBook.LastFileLineNum = -1;
                BooksManager.Instance.UpdateBook(this.m_CurrentBook);
            }
        }

        public void LoadFile(string filename)
        {
            try
            {
                if (OpenAndLoadFile(filename))
                {
                    ScrollToLine(0, 0);

                    // save to settings
                    this.m_CurrentBook.FilePath = filename;
                    this.m_CurrentBook.LastFileChapter = 0;
                    this.m_CurrentBook.LastFileLineNum = 0;
                    BooksManager.Instance.UpdateBook(this.m_CurrentBook);
                }
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("你选择的文件： " + e.FileName + " 读取遇到了问题。", "读取文件错误！");

                // reset settings
                this.m_CurrentBook.FilePath = "";
                this.m_CurrentBook.LastFileChapter = -1;
                this.m_CurrentBook.LastFileLineNum = -1;
                BooksManager.Instance.UpdateBook(this.m_CurrentBook);
            }
        }

        #endregion

        #region 全局快捷键

        /// <summary>
        /// 通知注册系统快捷键事件处理函数
        /// </summary>
        /// <param name="hotKeyModelList"></param>
        /// <returns></returns>
        private bool Instance_RegisterGlobalHotKeyEvent(ObservableCollection<HotKeyModel> hotKeyModelList)
        {
            return InitHotKey(hotKeyModelList);
        }

        /// <summary>
        /// 初始化注册快捷键
        /// </summary>
        /// <param name="hotKeyModelList">待注册热键的项</param>
        /// <returns>true:保存快捷键的值；false:弹出设置窗体</returns>
        private bool InitHotKey(ObservableCollection<HotKeyModel> hotKeyModelList = null)
        {
            var list = hotKeyModelList ?? HotKeySettingsManager.Instance.LoadDefaultHotKey();
            // 注册全局快捷键
            string failList = HotKeyHelper.RegisterGlobalHotKey(list, m_Hwnd, out m_HotKeySettings);
            if (string.IsNullOrEmpty(failList))
                return true;
            MessageBoxResult mbResult = MessageBox.Show(string.Format("无法注册下列快捷键\n\r{0}是否要改变这些快捷键？", failList), "提示", MessageBoxButton.YesNo);
            // 弹出热键设置窗体
            if (mbResult == MessageBoxResult.Yes)
            {
                //var win = new SettingsWindow();
                //win.ShowDialog();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 窗体回调函数，接收所有窗体消息的事件处理函数
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="msg">消息</param>
        /// <param name="wideParam">附加参数1</param>
        /// <param name="longParam">附加参数2</param>
        /// <param name="handled">是否处理</param>
        /// <returns>返回句柄</returns>
        private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wideParam, IntPtr longParam, ref bool handled)
        {
            switch (msg)
            {
                case HotKeyManager.WM_HOTKEY:
                    int sid = wideParam.ToInt32();
                    if (sid == m_HotKeySettings[EHotKeySetting.全屏])
                    {
                        MessageBox.Show(string.Format("触发【{0}】快捷键", EHotKeySetting.全屏));
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.打开])
                    {
                        MessageBox.Show(string.Format("触发【{0}】快捷键", EHotKeySetting.打开));
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.设置])
                    {
                        MessageBox.Show(string.Format("触发【{0}】快捷键", EHotKeySetting.设置));
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.退出])
                    {
                        MessageBox.Show(string.Format("触发【{0}】快捷键", EHotKeySetting.退出));
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.显示隐藏])
                    {
                        if (!this.IsVisible)
                        {
                            this.Activate();
                        }
                        else
                        {
                            this.Hide();
                        }
                    }
                    handled = true;
                    break;
            }
            return IntPtr.Zero;
        }

        #endregion
    }
}