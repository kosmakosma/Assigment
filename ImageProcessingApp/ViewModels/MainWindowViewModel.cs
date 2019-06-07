using ImageProcessingApp.ViewModels.Commands;
using ImageProcessingLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageProcessingApp.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ImageProcessing _imageProcessing;

        public event PropertyChangedEventHandler PropertyChanged;

        private Bitmap _sourceImage;

        public Bitmap SourceImage
        {
            get { return _sourceImage; }
            set { _sourceImage = value; NotifyPropertyChanged("SourceImage"); }
        }

        private Bitmap _processedImage;

        public Bitmap ProcessedImage
        {
            get => _processedImage;
            set { _processedImage = value; NotifyPropertyChanged("ProcessedImage"); }
        }
        private string _timeValue = "";

        public string TimeValue
        {
            get { return _timeValue; }
            set
            {
                _timeValue = value;
                NotifyPropertyChanged("TimeValue");
            }
        }

        private Command _processCommand;

        public Command ProcessCommand
        {
            get
            {
                return _processCommand ?? (_processCommand = new Command(() => InvokeImageProcess(), () => CanProcessImage()));
            }
        }

        private Command _loadImageCommand;

        public Command LoadImageCommand
        {
            get
            {
                return _loadImageCommand ?? (_loadImageCommand = new Command(() => SelectImageFile(), () => CanSelectImageFile()));
            }
        }

        private AsyncCommand _processCommandAsync;

        public AsyncCommand ProcessCommandAsync
        {
            get
            {
                return _processCommandAsync ?? (_processCommandAsync = new AsyncCommand(async () => await InvokeImageProcessAsync(), () => CanProcessImage()));
            }
        }


        private void InvokeImageProcess()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Bitmap bitmap = _imageProcessing.ToMainColors();
            stopwatch.Stop();
            ProcessedImage = bitmap;
            TimeValue = $"Elapsed = {stopwatch.ElapsedMilliseconds} ms";
        }

        private async Task InvokeImageProcessAsync()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Bitmap bitmap = await _imageProcessing.ToMainColorsAsync();
            stopwatch.Stop();
            ProcessedImage = bitmap;
            TimeValue = $"Elapsed = {stopwatch.ElapsedMilliseconds} ms";
        }
        private bool CanProcessImage()
        {
            if (SourceImage == null)
                return false;
            return true;
        }

        private void SelectImageFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|BMP Files (*.bmp)|*.bmp";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                _imageProcessing = new ImageProcessing(dlg.FileName);
                SourceImage = _imageProcessing.Image;
                ProcessCommand.RaiseCanExecuteChanged();
                ProcessCommandAsync.RaiseCanExecuteChanged();
            }
        }

        private bool CanSelectImageFile()
        {
            return true;
        }

        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

    }
}
