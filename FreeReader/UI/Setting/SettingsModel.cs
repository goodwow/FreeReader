using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FreeReader
{
    public class SettingsModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 字体颜色(变量)
        /// </summary>
        private Color fontColor = Colors.Black;

        /// <summary>
        /// 背景颜色(变量)
        /// </summary>
        private Color background = Color.FromRgb(250, 250, 250);

        /// <summary>
        /// 字体大小(变量)
        /// </summary>
        private double fontSize = 14;

        /// <summary>
        /// 字体(变量)
        /// </summary>
        private FontFamily fontFamily = new FontFamily("Microsoft Yahei");

        /// <summary>
        /// 字体拉伸(变量)
        /// </summary>
        private FontStretch fontStretch = FontStretches.Normal;

        /// <summary>
        /// 字体样式(变量)
        /// </summary>
        private FontStyle fontStyle = FontStyles.Normal;

        /// <summary>
        /// 字体密度(变量)
        /// </summary>
        private FontWeight fontWeight = FontWeights.Normal;

        /// <summary>
        /// 字体不透明度(变量)
        /// </summary>
        private double fontOpacity = 1.0;

        /// <summary>
        /// 背景不透明度(变量)
        /// </summary>
        private double backgroundOpacity = 1.0;

        /// <summary>
        /// Item选中后背景颜色(变量)
        /// </summary>
        private Color selectedBackground = Color.FromRgb(250, 250, 250);

        /// <summary>
        /// Item选中后背景颜色(变量)
        /// </summary>
        private Color selectedFontColor = Colors.Black;

        /// <summary>
        /// 窗口宽度(变量)
        /// </summary>
        private double windowWidth = 900;

        /// <summary>
        /// 窗口高度(变量)
        /// </summary>
        private double windowHeight = 600;

        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color FontColor
        {
            set
            {
                Byte colorA = Byte.Parse(((int)(255 * fontOpacity)).ToString());
                fontColor = Color.FromArgb(colorA, value.R, value.G, value.B);
                selectedFontColor = Color.FromArgb(value.A, value.R, value.G, value.B);
                NotifyPropertyChange("FontColor");
                NotifyPropertyChange("SelectedFontColor");
            }
            get
            {
                return fontColor;
            }
        }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color Background
        {
            set
            {
                Byte colorA = Byte.Parse(((int)(255 * backgroundOpacity)).ToString());
                background = Color.FromArgb(colorA, value.R, value.G, value.B);
                selectedBackground = Color.FromArgb(value.A, value.R, value.G, value.B);
                NotifyPropertyChange("Background");
                NotifyPropertyChange("SelectedBackground");
            }
            get
            {
                return background;
            }
        }

        /// <summary>
        /// 字体大小
        /// </summary>
        public double FontSize
        {
            set
            {
                fontSize = value;
                NotifyPropertyChange("FontSize");
            }
            get
            {
                return fontSize;
            }
        }

        /// <summary>
        /// 字体
        /// </summary>
        public FontFamily FontFamily
        {
            set
            {
                fontFamily = value;
                NotifyPropertyChange("FontFamily");
            }
            get
            {
                return fontFamily;
            }
        }

        /// <summary>
        /// 字体拉伸
        /// </summary>
        public FontStretch FontStretch
        {
            set
            {
                fontStretch = value;
                NotifyPropertyChange("FontStretch");
            }
            get
            {
                return fontStretch;
            }
        }

        /// <summary>
        /// 字体样式
        /// </summary>
        public FontStyle FontStyle
        {
            set
            {
                fontStyle = value;
                NotifyPropertyChange("FontStyle");
            }
            get
            {
                return fontStyle;
            }
        }

        /// <summary>
        /// 字体密度
        /// </summary>
        public FontWeight FontWeight
        {
            set
            {
                fontWeight = value;
                NotifyPropertyChange("FontWeight");
            }
            get
            {
                return fontWeight;
            }
        }

        /// <summary>
        /// 字体不透明度
        /// </summary>
        public double FontOpacity
        {
            set
            {
                fontOpacity = value;
                NotifyPropertyChange("FontOpacity");

                FontColor = Color.FromRgb(fontColor.R, fontColor.G, fontColor.B);
            }
            get
            {
                return fontOpacity;
            }
        }

        /// <summary>
        /// 背景不透明度
        /// </summary>
        public double BackgroundOpacity
        {
            set
            {
                backgroundOpacity = value;
                NotifyPropertyChange("BackgroundOpacity");

                Background = Color.FromRgb(background.R, background.G, background.B);
            }
            get
            {
                return backgroundOpacity;
            }
        }

        /// <summary>
        /// Item选中背景颜色
        /// </summary>
        public Color SelectedBackground
        {
            get
            {
                return selectedBackground;
            }
        }

        /// <summary>
        /// Item选中字体颜色
        /// </summary>
        public Color SelectedFontColor
        {
            get
            {
                return selectedFontColor;
            }
        }

        /// <summary>
        /// 窗口宽度
        /// </summary>
        public double WindowWidth
        {
            set
            {
                windowWidth = value;
            }
            get
            {
                return windowWidth;
            }
        }

        /// <summary>
        /// 窗口高度
        /// </summary>
        public double WindowHeight
        {
            set
            {
                windowHeight = value;
            }
            get
            {
                return windowHeight;
            }
        }

        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 通知属性改变函数
        /// </summary>
        private void NotifyPropertyChange(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
