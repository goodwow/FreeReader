using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FreeReader
{
    /// <summary>
    /// ReadSetting.xaml 的交互逻辑
    /// </summary>
    public partial class ReadSetting : UserControl
    {
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
        public ReadSetting()
        {
            InitializeComponent();
        }
    }
}
