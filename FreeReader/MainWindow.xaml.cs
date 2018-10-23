using FreeReader.HotKey;
using FreeReader.Models;
using FreeReader.Properties;
using FreeReader.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace FreeReader
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 当前窗口句柄
        /// </summary>
        private IntPtr m_Hwnd = new IntPtr();

        /// <summary>
        /// 记录快捷键注册项的唯一标识符
        /// </summary>
        private Dictionary<EHotKeySetting, int> m_HotKeySettings = new Dictionary<EHotKeySetting, int>();

        private ListBoxWrapper editor;
        private ListBoxWrapper sidebar;

        private KeyHandler headerListBoxHandler;
        private KeyHandler paragraphListBoxHandler;

        private int m_CurrentNovelIndex = 0;

        private ObservableCollection<Content> m_NovelConents = new ObservableCollection<Content>();
        private ObservableCollection<Content> m_NovelHeaders = new ObservableCollection<Content>();
        private ObservableCollection<Content> m_CurNovelContents = new ObservableCollection<Content>();

        public ObservableCollection<Content> CurNovelContents
        {
            get
            {
                return m_CurNovelContents;
            }
        }

        public ObservableCollection<Content> NovelHeaders
        {
            get
            {
                return m_NovelHeaders;
            }
        }

        #region 页面初始化

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            editor = new Reader(ParagraphListBox);
            sidebar = new Sidebar(HeaderListBox);

            headerListBoxHandler = new HeaderListBoxKeyHandler(sidebar, editor);
            paragraphListBoxHandler = new ParagraphListBoxKeyHandler(editor, sidebar);

            this.Width = SettingsManager.Instance.ReadSettings.WindowWidth;
            this.Height = SettingsManager.Instance.ReadSettings.WindowHeight;
        }

        /// <summary>
        /// 窗体加载完成后事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HotKeySettingsManager.Instance.RegisterGlobalHotKeyEvent += Instance_RegisterGlobalHotKeyEvent;
            SettingsManager.Instance.ReadSettings.PropertyChanged += ReadSettings_PropertyChanged;

            LoadLastFile();
            SetWindowOpacity();
        }

        /// <summary>
        //  窗体大小改变事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SettingsManager.Instance.ReadSettings.WindowWidth = e.NewSize.Width;
            SettingsManager.Instance.ReadSettings.WindowHeight = e.NewSize.Height;
        }

        /// <summary>
        /// 系统配置发生改变处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "BackgroundOpacity":
                    SetWindowOpacity();
                    break;
                default:
                    break;
            }

            //int totalParagraphs = editor.LineCount();
            //int currentParagraph = editor.CurrentLine() + 1;

            //int totalChapters = sidebar.LineCount();
            //int currentChapter = sidebar.CurrentLine() + 1;

            //if (currentParagraph == totalParagraphs)
            //{
            //    StatusTextBox.Text = "你已经看完了！";
            //}
            //else
            //{
            //    double percentage = ((double)currentParagraph / totalParagraphs) * 100.0;

            //    StatusTextBox.Text = String.Format("你在第{2}/{3}段，{0}/{1}章，已经看了{4:F}%",
            //            currentChapter, totalChapters, currentParagraph, totalParagraphs, percentage);
            //}
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

        /// <summary>
        /// 设置窗体透明度
        /// </summary>
        private void SetWindowOpacity()
        {
            int opacity = (int)(SettingsManager.Instance.ReadSettings.BackgroundOpacity * 255);
            SolidColorBrush brush = this.Background as SolidColorBrush;
            this.Background = new SolidColorBrush(Color.FromArgb(Byte.Parse(opacity.ToString()), brush.Color.R, brush.Color.G, brush.Color.B));
        }

        #endregion

        #region 目录正文

        private void ParagraphListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                LoadFile(files[0]);
            }
        }

        private void HeaderListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editor.FocusOnLineItem(sidebar.CurrentLineItem());
            editor.Focus();

            // save settings
            SettingsManager.Instance.ReadSettings.LastFileChapter = sidebar.CurrentLine();
            SettingsManager.Instance.ReadSettings.LastFileLineNum = editor.CurrentLine();
        }

        private void ParagraphListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //App app = (App)Application.Current;

            //// empty headers
            //if (app.Headers.Count < 1)
            //{
            //    return;
            //}

            //// find nearest chapter
            //object header = app.Headers.First();

            //for (int i = editor.CurrentLine(); i >= 0; i--)
            //{
            //    if (app.Novel[i].IsHeader)
            //    {
            //        header = i;
            //        break;
            //    }
            //}

            //// select chapter
            //sidebar.FocusOnLineItem(header);

            //// save settings
            //SettingsManager.Instance.ReadSettings.LastFileChapter = sidebar.CurrentLine();
            //SettingsManager.Instance.ReadSettings.LastFileLineNum = editor.CurrentLine();
        }

        private void ParagraphListBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var lb = sender as ListBox;
            foreach (var lbi in lb.Items)
            {
                var container = lb.ItemContainerGenerator.ContainerFromItem(lbi) as ListBoxItem;
                if (container != null && IsUserVisible(container, lb))
                {
                    Content topItem = container.Content as Content;
                    return;
                }
            }
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

        private bool IsUserVisible(FrameworkElement element, FrameworkElement container)
        {
            if (!element.IsVisible)
                return false;
            Rect bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
            Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            Rect fudgybounds = new Rect(new Point(bounds.TopLeft.X, bounds.TopLeft.Y), new Point(bounds.BottomRight.X, bounds.BottomRight.Y - 5));
            return rect.Contains(fudgybounds.TopLeft) || rect.Contains(fudgybounds.BottomRight);
        }

        #endregion

        #region 右键菜单

        /// <summary>
        /// 设置事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow win = new SettingsWindow();
            win.ShowDialog();
        }

        /// <summary>
        /// 退出事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
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
        }

        private bool OpenAndLoadFile(string path)
        {
            if (String.IsNullOrEmpty(path))
                return false;

            this.m_NovelConents.Clear();
            this.m_CurNovelContents.Clear();
            this.m_NovelHeaders.Clear();

            NovelFile.Open(path, this.m_NovelConents);
            foreach (Content c in this.m_NovelConents)
            {
                if (c.IsHeader)
                {
                    this.m_NovelHeaders.Add(c);
                }
                if (c.Index <= 3)
                {
                    this.m_CurNovelContents.Add(c);
                }
            }
            this.Title = "FreeReader - " + path;

            return true;
        }

        private void LoadLastFile()
        {
            try
            {
                if (OpenAndLoadFile(SettingsManager.Instance.ReadSettings.LastFilePath))
                {
                    ScrollToLine(SettingsManager.Instance.ReadSettings.LastFileLineNum, SettingsManager.Instance.ReadSettings.LastFileChapter);
                }
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("你上次阅读的文件： " + e.FileName + " 找不到了，看个别的吧。", "读取文件错误！");

                // reset settings
                SettingsManager.Instance.ReadSettings.LastFilePath = "";
                SettingsManager.Instance.ReadSettings.LastFileChapter = -1;
                SettingsManager.Instance.ReadSettings.LastFileLineNum = -1;
            }
        }

        private void LoadFile(string filename)
        {
            try
            {
                if (OpenAndLoadFile(filename))
                {
                    ScrollToLine(0, 0);

                    // save to settings
                    SettingsManager.Instance.ReadSettings.LastFilePath = filename;
                    SettingsManager.Instance.ReadSettings.LastFileChapter = 0;
                    SettingsManager.Instance.ReadSettings.LastFileLineNum = 0;
                }
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("你选择的文件： " + e.FileName + " 读取遇到了问题。", "读取文件错误！");

                // reset settings
                SettingsManager.Instance.ReadSettings.LastFilePath = "";
                SettingsManager.Instance.ReadSettings.LastFileChapter = -1;
                SettingsManager.Instance.ReadSettings.LastFileLineNum = -1;
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
                var win = new SettingsWindow();
                win.ShowDialog();
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
                            this.Show();
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