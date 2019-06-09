using ImageProcessingApp.ViewModels;
using NUnit.Framework;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageProcessingAppTest
{

    public class MainWindowViewModelTest
    {
        private Bitmap _sampleImage;
        private MainWindowViewModel _viewModel;
        private readonly int _width = 400;
        private readonly int _height = 200;

        [SetUp]
        public void SetUp()
        {
            _viewModel = new MainWindowViewModel();
            _sampleImage = GenerateSampleImage(_width, _height);
        }

        [TearDown]
        public void TearDown()
        {
            _viewModel = null;

        }


        [Test]
        public void TestIfCanExecuteProcessCommandWithoutImage()
        {
            Assert.False(_viewModel.ProcessCommand.CanExecute(null));
        }

        [Test]
        public void TestIfCanExecuteAsyncProcessCommandWithoutImage()
        {
            Assert.False(_viewModel.ProcessCommandAsync.CanExecute(null));
        }

        [Test]
        public void TestExecutingProcessImageCommand()
        {
            _viewModel.SourceImage = _sampleImage;

            _viewModel.ProcessCommand.Execute(null);

            Assert.NotNull(_viewModel.ProcessedImage);
        }

        [Test]
        public void TestIfProcessedImageIsCorrect()
        {
            _viewModel.SourceImage = _sampleImage;

            _viewModel.ProcessCommand.Execute(null);

            Assert.AreEqual(_viewModel.ProcessedImage,
                new ImageProcessingLibrary.ImageProcessing(_sampleImage).ToMainColors());
        }

        [Test]
        public async Task TestExecutingAsyncProcessImageCommand()
        {
            _viewModel.SourceImage = _sampleImage;

            await _viewModel.ProcessCommandAsync.ExecuteAsync(null);

            Assert.NotNull(_viewModel.ProcessedImage);
        }

        [Test]
        public async Task TestProcessedImageIsCorrectAfterAsyncCommandAsync()
        {
            _viewModel.SourceImage = _sampleImage;

            await _viewModel.ProcessCommandAsync.ExecuteAsync(null);

            Assert.AreEqual(_viewModel.ProcessedImage,
                new ImageProcessingLibrary.ImageProcessing(_sampleImage).ToMainColorsAsync().Result);
        }

        [Test]
        public void TestIfAfterProcessingTimeValueIsUpdated()
        {
            _viewModel.SourceImage = _sampleImage;

            _viewModel.ProcessCommand.Execute(null);

            Assert.True(Regex.IsMatch(_viewModel.TimeValue, @"(Image Processed in)[ ][0-9]+[ ](ms)"));

        }

        [Test]
        public void TestIfTimeValuePropertyHasCorrectValue()
        {
            _viewModel.SourceImage = _sampleImage;

            _viewModel.ProcessCommand.Execute(null);
        }

        private Bitmap GenerateSampleImage(int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            for (int i = 0; i < width / 2; i++)
            {
                for (int j = 0; j < height / 2; j++)
                    bmp.SetPixel(i, j, Color.DarkBlue);
                for (int j = height / 2; j < height; j++)
                    bmp.SetPixel(i, j, Color.BurlyWood);
            }
            for (int i = width / 2; i < width; i++)
            {
                for (int j = 0; j < height / 2; j++)
                    bmp.SetPixel(i, j, Color.ForestGreen);
                for (int j = height / 2; j < height; j++)
                    bmp.SetPixel(i, j, Color.BlueViolet);
            }
            return bmp;
        }

    }
}
