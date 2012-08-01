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
    public sealed partial class WrapOptionsSelect : UserControl
    {
        public WrapOptionsSelect()
        {
            this.InitializeComponent();
        }

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextWrapping.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(WrapOptionsSelect), new PropertyMetadata(TextWrapping.NoWrap, OnTextWrappingChanged));

        private static void OnTextWrappingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as WrapOptionsSelect).OnTextWrappingChanged(e);
        }

        void OnRadioButtonChecked(object sender, RoutedEventArgs args)
        {
            var settings = ApplicationData.Current.LocalSettings;

            this.TextWrapping = (TextWrapping)(sender as RadioButton).Tag;

            settings.Values["TextWrapping"] = (int)this.TextWrapping;
        }

        void OnTextWrappingChanged(DependencyPropertyChangedEventArgs e)
        {
            foreach (UIElement child in stackPanel.Children)
            {
                RadioButton radioButton = child as RadioButton;
                radioButton.IsChecked = (TextWrapping)radioButton.Tag == (TextWrapping)e.NewValue;
            }
        }

    }
}
