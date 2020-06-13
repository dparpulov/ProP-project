using System;
using System.Drawing;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;

namespace SypnosisApp
{
    public class BarcodeReader
    {
        FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoDevice;
        private VideoCapabilities[] videoCapabilities;
        ZXing.BarcodeReader barcodeReader = new ZXing.BarcodeReader();
        bool showFrames = false;

        Window window;
        ComboBox comboBox;
        System.Windows.Controls.Image image;
        //TextBlock textBlock;
        TextBox tb;

        public bool ShowFrames
        {
            get { return showFrames; }
        }

        public BarcodeReader(Window window, ComboBox cb, System.Windows.Controls.Image img, TextBox tb)
        {
            this.window = window;
            this.comboBox = cb;
            this.image = img;
            this.tb = tb;

            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            comboBox.Items.Clear();
            foreach (FilterInfo VideoCaptureDevice in videoDevices)
            {
                comboBox.Items.Add(VideoCaptureDevice.Name);
            }
            if (this.comboBox.Items.Count > 0)
            {
                this.comboBox.SelectedIndex = 0;
                refreshCam(this.comboBox.SelectedIndex);
            }
        }

        private void refreshCam(int camNumber)
        {
            videoDevice = new VideoCaptureDevice(videoDevices[camNumber].MonikerString);
            videoCapabilities = videoDevice.VideoCapabilities;
            videoDevice.VideoResolution = ((from VideoCapabilities vidcap in videoCapabilities where (vidcap.FrameSize.Height >= 600 && vidcap.FrameSize.Width >= 800) orderby vidcap.FrameSize.Height descending select vidcap)).ToList()[0];
        }

        public void OnOff()
        {
            if (this.comboBox.Items.Count > 0)
            {
                if (!showFrames)
                {
                    this.comboBox.IsEnabled = false;
                    showFrames = !showFrames;
                    refreshCam(this.comboBox.SelectedIndex);
                    videoDevice.NewFrame += VideoDevice_NewFrame;
                    videoDevice.Start();
                }
                else
                {
                    this.comboBox.IsEnabled = true;
                    showFrames = !showFrames;
                    videoDevice.NewFrame -= VideoDevice_NewFrame;
                    videoDevice.SignalToStop();
                    videoDevice = null;
                }
            }
        }
        private void VideoDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            decodeframe(eventArgs.Frame);
            BitmapImage bi = new BitmapImage();
            bi = BitmapToImageSource(eventArgs.Frame);
            bi.Freeze();
            var picShow = window.Dispatcher.Invoke(new ThreadStart(delegate { showImage(bi); }));
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }

        bool showImage(BitmapImage bi)
        {
            image.Source = bi;
            return true;
        }

        void decodeframe(Bitmap img)
        {
            var barcodeResult = barcodeReader.Decode(img);
            if (barcodeResult != null)
            {
                window.Dispatcher.Invoke(new ThreadStart(delegate { tb.Text = barcodeResult.Text.ToString(); }));
            }
        }
    }
}
