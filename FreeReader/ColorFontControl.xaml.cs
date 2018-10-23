using FreeReader.ColorFont;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for ColorFontDialog.xaml
    /// </summary>
    public partial class ColorFontControl : UserControl
    {
        public event RoutedEventHandler ColorFontChange;

        private FontInfo _selectedFont;

        public FontInfo Font
        {
            get
            {
                return _selectedFont;
            }
            set
            {
                _selectedFont = value;
            }
        }

        private int[] _defaultFontSizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72, 96 };
        private int[] _fontSizes = null;
        public int[] FontSizes
        {
            get
            {
                return _fontSizes ?? _defaultFontSizes;
            }
            set
            {
                _fontSizes = value;
            }
        }
        public ColorFontControl()
        {
            InitializeComponent();

            this.colorFontChooser.PreviewFontInFontList = true;
            this.colorFontChooser.AllowArbitraryFontSizes = true;
            this.colorFontChooser.ShowColorPicker = true;

            this._selectedFont = new FontInfo(new FontFamily("Courier New"), 12, FontStyles.Normal, FontStretches.Normal, FontWeights.Normal, new SolidColorBrush(Colors.Black), new SolidColorBrush(Colors.White));
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Font = this.colorFontChooser.SelectedFont;
            if (ColorFontChange != null)
            {
                ColorFontChange(sender, e);
            }
        }

        private void SyncFontColor()
        {
            int colorIdx = AvailableColors.GetFontColorIndex(this.Font.Color);
            this.colorFontChooser.colorPicker.superCombo.SelectedIndex = colorIdx;
            this.colorFontChooser.txtSampleText.Foreground = this.Font.Color.Brush;
            this.colorFontChooser.colorPicker.superCombo.BringIntoView();
        }

        private void SyncBackground()
        {
            int colorIdx = AvailableColors.GetFontColorIndex(this.Font.Background);
            this.colorFontChooser.backgroundPicker.superCombo.SelectedIndex = colorIdx;
            this.colorFontChooser.txtSampleText.Background = this.Font.Background.Brush;
            this.colorFontChooser.backgroundPicker.superCombo.BringIntoView();
        }

        private void SyncFontName()
        {
            string fontFamilyName = this._selectedFont.Family.Source;
            bool foundMatch = false;
            int idx = 0;
            foreach (object item in (IEnumerable)this.colorFontChooser.lstFamily.Items)
            {
                if (fontFamilyName == item.ToString())
                {
                    foundMatch = true;
                    break;
                }
                idx++;
            }
            if (!foundMatch)
            {
                idx = 0;
            }
            this.colorFontChooser.lstFamily.SelectedIndex = idx;
            this.colorFontChooser.lstFamily.ScrollIntoView(this.colorFontChooser.lstFamily.Items[idx]);
        }

        private void SyncFontSize()
        {
            double fontSize = this._selectedFont.Size;
            this.colorFontChooser.lstFontSizes.ItemsSource = FontSizes;
            this.colorFontChooser.tbFontSize.Text = fontSize.ToString();
        }

        private void SyncFontTypeface()
        {
            string fontTypeFaceSb = FontInfo.TypefaceToString(this._selectedFont.Typeface);
            int idx = 0;
            foreach (object item in (IEnumerable)this.colorFontChooser.lstTypefaces.Items)
            {
                if (fontTypeFaceSb == FontInfo.TypefaceToString(item as FamilyTypeface))
                {
                    break;
                }
                idx++;
            }
            this.colorFontChooser.lstTypefaces.SelectedIndex = idx;
            this.colorFontChooser.lstTypefaces.ScrollIntoView(this.colorFontChooser.lstTypefaces.SelectedItem);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.SyncFontColor();
            this.SyncBackground();
            this.SyncFontName();
            this.SyncFontSize();
            this.SyncFontTypeface();
        }
    }
}
