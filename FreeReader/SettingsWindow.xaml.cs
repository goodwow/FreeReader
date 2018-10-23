using FreeReader.ColorFont;
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
    /// SettingsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsWindow : Window
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
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.colorFontSelector.Font.Background.Brush.Color = SettingsManager.Instance.ReadSettings.Background;
            this.colorFontSelector.Font.Color.Brush.Color = SettingsManager.Instance.ReadSettings.FontColor;
            this.colorFontSelector.Font.Size = SettingsManager.Instance.ReadSettings.FontSize;
            this.colorFontSelector.Font.Family = SettingsManager.Instance.ReadSettings.FontFamily;
            this.colorFontSelector.Font.Stretch = SettingsManager.Instance.ReadSettings.FontStretch;
            this.colorFontSelector.Font.Style = SettingsManager.Instance.ReadSettings.FontStyle;
            this.colorFontSelector.Font.Weight = SettingsManager.Instance.ReadSettings.FontWeight;
        }

        private void ColorFontControl_ColorFontChange(object sender, RoutedEventArgs e)
        {
            FontInfo fontInfo = this.colorFontSelector.Font;
            SettingsManager.Instance.ReadSettings.Background = fontInfo.Background.Brush.Color;
            SettingsManager.Instance.ReadSettings.FontColor = fontInfo.Color.Brush.Color;
            SettingsManager.Instance.ReadSettings.FontSize = fontInfo.Size;
            SettingsManager.Instance.ReadSettings.FontFamily = fontInfo.Family;
            SettingsManager.Instance.ReadSettings.FontStretch = fontInfo.Stretch;
            SettingsManager.Instance.ReadSettings.FontStyle = fontInfo.Style;
            SettingsManager.Instance.ReadSettings.FontWeight = fontInfo.Weight;
        }
    }
}
