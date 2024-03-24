using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FreeReader
{
    public class FontInfo
    {
        private SolidColorBrush brushColor;
        public SolidColorBrush BrushColor
        {
            get
            {
                return brushColor;
            }
            set
            {
                brushColor = value;
                NotifyPropertyChange("BrushColor");
            }
        }

        public FontColor Color
        {
            get
            {
                return AvailableColors.GetFontColor(this.BrushColor);
            }
        }

        private FontFamily family;
        public FontFamily Family
        {
            get
            {
                return family;
            }
            set
            {
                family = value;
                NotifyPropertyChange("Family");
            }
        }

        private double size;
        public double Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                NotifyPropertyChange("Size");
            }
        }

        private FontStretch stretch;
        public FontStretch Stretch
        {
            get
            {
                return stretch;
            }
            set
            {
                stretch = value;
                NotifyPropertyChange("Stretch");
            }
        }

        private FontStyle style;
        public FontStyle Style
        {
            get
            {
                return style;
            }
            set
            {
                style = value;
                NotifyPropertyChange("Style");
            }
        }

        public FamilyTypeface Typeface
        {
            get
            {
                FamilyTypeface ftf = new FamilyTypeface()
                {
                    Stretch = this.Stretch,
                    Weight = this.Weight,
                    Style = this.Style
                };
                return ftf;
            }
        }

        private FontWeight weight;
        public FontWeight Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
                NotifyPropertyChange("Weight");
            }
        }

        private FontColor background;
        public FontColor Background
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
                NotifyPropertyChange("Background");
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

        public FontInfo()
        {
        }

        public FontInfo(FontFamily fam, double sz, FontStyle style, FontStretch strc, FontWeight weight, SolidColorBrush c, SolidColorBrush background)
        {
            this.Family = fam;
            this.Size = sz;
            this.Style = style;
            this.Stretch = strc;
            this.Weight = weight;
            this.BrushColor = c;
            this.Background = AvailableColors.GetFontColor(background);
        }

        public static void ApplyFont(Control control, FontInfo font)
        {
            control.FontFamily = font.Family;
            control.FontSize = font.Size;
            control.FontStyle = font.Style;
            control.FontStretch = font.Stretch;
            control.FontWeight = font.Weight;
            control.Foreground = font.BrushColor;
        }

        public static FontInfo GetControlFont(Control control)
        {
            FontInfo font = new FontInfo()
            {
                Family = control.FontFamily,
                Size = control.FontSize,
                Style = control.FontStyle,
                Stretch = control.FontStretch,
                Weight = control.FontWeight,
                BrushColor = (SolidColorBrush)control.Foreground
            };
            return font;
        }

        public static string TypefaceToString(FamilyTypeface ttf)
        {
            StringBuilder sb = new StringBuilder(ttf.Stretch.ToString());
            sb.Append("-");
            sb.Append(ttf.Weight.ToString());
            sb.Append("-");
            sb.Append(ttf.Style.ToString());
            return sb.ToString();
        }
    }
}