using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Media.Capture;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UsingWinRT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage : Page
    {
        public BlankPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {  
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var ui = new CameraCaptureUI();
            ui.PhotoSettings.CroppedAspectRatio = new Size(16, 9);

            var file = await ui.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (file != null)
            {
                var bitmap = new BitmapImage();

                bitmap.SetSource(await file.OpenAsync(FileAccessMode.Read));
                Photo.Source = bitmap;
            }
        }
    }
}
