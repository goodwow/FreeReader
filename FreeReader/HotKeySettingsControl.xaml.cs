using FreeReader.HotKey;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FreeReader
{
    /// <summary>
    /// 快捷键设置窗体交互逻辑
    /// </summary>
    public partial class HotKeySettingsControl : UserControl
    {
        private ObservableCollection<HotKeyModel> m_HotKeyList = new ObservableCollection<HotKeyModel>();

        /// <summary>
        /// 快捷键设置项集合
        /// </summary>
        public ObservableCollection<HotKeyModel> HotKeyList
        {
            get { return m_HotKeyList; }
            set { m_HotKeyList = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public HotKeySettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载完成后事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitHotKey();
        }

        /// <summary>
        /// 初始化快捷键
        /// </summary>
        private void InitHotKey()
        {
            HotKeyList.Clear();

            var list = HotKeySettingsManager.Instance.LoadDefaultHotKey();
            list.ToList().ForEach(x => HotKeyList.Add(x));
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveSetting_Click(object sender, RoutedEventArgs e)
        {
            if (!HotKeySettingsManager.Instance.RegisterGlobalHotKey(HotKeyList))
                return;
        }
    }
}
