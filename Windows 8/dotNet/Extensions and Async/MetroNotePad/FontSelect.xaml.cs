using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MetroNotePad
{
    public sealed partial class FontSelect : UserControl
    {
        public FontSelect()
        {
            this.InitializeComponent();
        }

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontFamily.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(FontSelect), new PropertyMetadata(new FontFamily("Verdana"), FontFamilyChanged));

        private static void FontFamilyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FontSelect).OnFontFamilyChanged(e);
        }

        void OnFontFamilyChanged(DependencyPropertyChangedEventArgs e)
        {
            foreach (var element in this.stackPanel.Children)
            {
                var radioButton = element as RadioButton;
                radioButton.IsChecked = (radioButton.Tag as FontFamily).Source == (e.NewValue as FontFamily).Source;
            }
        }

        private void OnFontChecked(object sender, RoutedEventArgs e)
        {
            var settings = ApplicationData.Current.LocalSettings;

            this.FontFamily = (FontFamily)(sender as RadioButton).Tag;

            settings.Values["FontFamily"] = this.FontFamily.Source;
        }
    }
}