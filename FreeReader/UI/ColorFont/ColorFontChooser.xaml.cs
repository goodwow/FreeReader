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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FreeReader
{
    /// <summary>
    /// Interaction logic for ColorFontChooser.xaml
    /// </summary>
    public partial class ColorFontChooser : UserControl
    {
        private SettingsModel ReadSettings
        {
            get
            {
                return SettingsManager.Instance.ReadSettings;
            }
        }

        private FontInfo SelectedFont
        {
            get;
            set;
        }

        public ColorFontChooser()
        {
            InitializeComponent();

            this.SelectedFont = new FontInfo(
              ReadSettings.FontFamily,
              ReadSettings.FontSize,
              ReadSettings.FontStyle,
              ReadSettings.FontStretch,
              ReadSettings.FontWeight,
              new SolidColorBrush(ReadSettings.SelectedFontColor),
              new SolidColorBrush(ReadSettings.SelectedBackground));

            lstFamily.ItemTemplate = (DataTemplate)Resources["fontFamilyData"];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.SyncFontColor();
            this.SyncBackground();
            this.SyncFontName();
            this.SyncFontTypeface();

            this.SelectedFont.PropertyChanged += (object s, System.ComponentModel.PropertyChangedEventArgs arg) =>
            {
                ReadSettings.Background = this.SelectedFont.Background.Brush.Color;
                ReadSettings.FontColor = this.SelectedFont.Color.Brush.Color;
                ReadSettings.FontSize = this.SelectedFont.Size;
                ReadSettings.FontFamily = this.SelectedFont.Family;
                ReadSettings.FontStretch = this.SelectedFont.Stretch;
                ReadSettings.FontStyle = this.SelectedFont.Style;
                ReadSettings.FontWeight = this.SelectedFont.Weight;
            };
        }

        private void SyncFontColor()
        {
            int colorIdx = AvailableColors.GetFontColorIndex(this.SelectedFont.Color);
            this.colorPicker.superCombo.SelectedIndex = colorIdx;
            this.colorPicker.superCombo.BringIntoView();
        }

        private void SyncBackground()
        {
            int colorIdx = AvailableColors.GetFontColorIndex(this.SelectedFont.Background);
            this.backgroundPicker.superCombo.SelectedIndex = colorIdx;
            this.backgroundPicker.superCombo.BringIntoView();
        }

        private void SyncFontName()
        {
            string fontFamilyName = this.SelectedFont.Family.Source;
            bool foundMatch = false;
            int idx = 0;
            foreach (object item in (IEnumerable)this.lstFamily.Items)
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
            this.lstFamily.SelectedIndex = idx;
            this.lstFamily.ScrollIntoView(this.lstFamily.Items[idx]);
        }

        private void SyncFontTypeface()
        {
            string fontTypeFaceSb = FontInfo.TypefaceToString(this.SelectedFont.Typeface);
            int idx = 0;
            foreach (object item in (IEnumerable)this.lstTypefaces.Items)
            {
                if (fontTypeFaceSb == FontInfo.TypefaceToString(item as FamilyTypeface))
                {
                    break;
                }
                idx++;
            }
            this.lstTypefaces.SelectedIndex = idx;
            this.lstTypefaces.ScrollIntoView(this.lstTypefaces.SelectedItem);
        }

        private void colorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            this.SelectedFont.BrushColor = this.colorPicker.SelectedColor.Brush;
        }

        private void backgroundPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            this.SelectedFont.Background = AvailableColors.GetFontColor(this.backgroundPicker.SelectedColor.Brush);
        }

        private void lstFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FontFamily fontFamily = lstFamily.SelectedItem as FontFamily;
            if (fontFamily != null && this.SelectedFont != null)
            {
                this.SelectedFont.Family = fontFamily;
            }
        }

        private void lstTypefaces_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FamilyTypeface familyTypeface = lstTypefaces.SelectedItem as FamilyTypeface;
            if (familyTypeface != null && this.SelectedFont != null)
            {
                this.SelectedFont.Weight = familyTypeface.Weight;
                this.SelectedFont.Stretch = familyTypeface.Stretch;
                this.SelectedFont.Style = familyTypeface.Style;
            }
        }
    }
}
