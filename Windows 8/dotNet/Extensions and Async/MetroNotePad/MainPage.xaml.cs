using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MetroNotePad
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DataTransferManager dataTransferManager;
        StorageFile file;

        public MainPage()
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
            // Register this page as a share source.
            this.dataTransferManager = DataTransferManager.GetForCurrentView();
            this.dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);

            var settings = ApplicationData.Current.LocalSettings;

            if (settings.Values["FontFamily"] != null)
                this.txtbox.FontFamily = new FontFamily(settings.Values["FontFamily"].ToString());

            if (settings.Values["TextWrapping"] != null)
                this.txtbox.TextWrapping = (TextWrapping)settings.Values["TextWrapping"];

        }

        private void OnFontAppBarButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FontSelect()
            {
                FontFamily = this.txtbox.FontFamily,
            };

            var binding = new Binding()
            {
                Source = dialog,
                Path = new PropertyPath("FontFamily"),
                Mode = BindingMode.TwoWay,
            };

            this.txtbox.SetBinding(TextBox.FontFamilyProperty, binding);

            var popup = new Popup()
            {
                Child = dialog,
                IsLightDismissEnabled = true,
            };

            popup.IsOpen = true;
        }

        private void OnWrapOptionsAppBarButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new WrapOptionsSelect()
            {
                TextWrapping = this.txtbox.TextWrapping
            };

            // Bind dialog to TextBox
            Binding binding = new Binding
            {
                Source = dialog,
                Path = new PropertyPath("TextWrapping"),
                Mode = BindingMode.TwoWay
            };

            txtbox.SetBinding(TextBox.TextWrappingProperty, binding);

            var popup = new Popup()
            {
                Child = dialog,
                IsLightDismissEnabled = true
            };

            popup.IsOpen = true;
        }

        private void OnSaveAsOptionsAppBarButtonClick(object sender, RoutedEventArgs e)
        {
            var picker = new FileSavePicker();

            picker.DefaultFileExtension = ".txt";
            picker.FileTypeChoices.Add("Text", new List<string> { ".txt" });

            picker.PickSaveFileAsync()
                .Completed = (x, y) =>
                    {
                        this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {

                                FileIO.WriteTextAsync(x.GetResults(), txtbox.Text)
                                    .Completed = (a, b) => { };

                            });
                    };
        }

        private void OnOpenAppBarButtonClick(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();

            picker.FileTypeFilter.Add(".txt");

            picker.PickSingleFileAsync()
                .Completed = OnPickSingleFile;
        }

        private void OnPickSingleFile(IAsyncOperation<StorageFile> info, AsyncStatus status)
        {
            var storageFile = info.GetResults();
            file = storageFile;

            storageFile.OpenReadAsync()
                .Completed = (openReadInfo, openReadStatus) =>
                    {
                        Windows.Storage.Streams.IInputStream inputStream = openReadInfo.GetResults();

                        System.IO.Stream stream = inputStream.AsStreamForRead();
                        System.IO.StreamReader reader = new StreamReader(stream);

                        this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            txtbox.Text = reader.ReadToEnd());
                    };
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            var settings = ApplicationData.Current.LocalSettings;
            var position = settings.Values["Position"];

            string dataPackageTitle = "Archivos a Compartir";

            // The title is required.
            if (!String.IsNullOrEmpty(dataPackageTitle))
            {
                if (this.file != null)
                {
                    DataPackage requestData = e.Request.Data;
                    requestData.Properties.Title = string.Format("{0} - On: {1}", dataPackageTitle, position);

                    string dataPackageDescription = this.file.Name;
                    if (dataPackageDescription != null)
                    {
                        requestData.Properties.Description = dataPackageDescription;
                    }
                    requestData.SetStorageItems(new List<StorageFile>() { this.file });
                }
                else
                {
                    e.Request.FailWithDisplayText("Select the files you would like to share and try again.");
                }
            }
            else
            {
                e.Request.FailWithDisplayText("Error");
            }
        }

        //private async Task GetPosition()
        //{
        //    var _geolocator = new Geolocator();
        //    var _cts = new CancellationTokenSource();

        //    CancellationToken token = _cts.Token;

        //    // Carry out the operation
        //    Geoposition pos = await _geolocator.GetGeopositionAsync().AsTask(token);

        //    var settings = ApplicationData.Current.LocalSettings;

        //    settings.Values["Position"] = string.Format("Position: {0} - {1}",
        //        pos.Coordinate.Latitude,
        //        pos.Coordinate.Longitude);
        //}
    }
}

//async void OnOpenAppBarButtonClick(object sender, RoutedEventArgs args)
//{
//    FileOpenPicker picker = new FileOpenPicker();

//    picker.FileTypeFilter.Add(".txt");

//    StorageFile storageFile = await picker.PickSingleFileAsync();

//    if (storageFile == null)
//        return;

//    txtbox.Text = await FileIO.ReadTextAsync(storageFile);
//}


//async void OnSaveAsAppBarButtonClick(object sender, RoutedEventArgs args)
//{
//    FileSavePicker picker = new FileSavePicker();

//    picker.DefaultFileExtension = ".txt";

//    picker.FileTypeChoices.Add("Text", new List<string> { ".txt" });

//    StorageFile storageFile = await picker.PickSaveFileAsync();

//    if (storageFile == null)
//        return;

//    await FileIO.WriteTextAsync(storageFile, txtbox.Text);
//}