using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLibrary
{
    enum PrimaryColor
    {
        Red, Blue, Green
    }

    public class ImageProcessing
    {

        public ImageProcessing(Bitmap image)
        {
            Image = image;
        }

        public ImageProcessing(string filepath)
        {
            LoadImage(filepath);
        }

        public ImageProcessing()
        {

        }

        public void LoadImage(string filepath)
        {
            Image = new Bitmap(filepath);
        }

        public void SaveImage(string filepath)
        {
            Image.Save(filepath);
        }

        public Bitmap Image { get; set; }

        public int Width
        {
            get { return Image.Width; }
        }

        public int Height
        {
            get { return Image.Height; }
        }

        public Bitmap ToMainColors()
        {
            
            BitmapData from = Image.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, Image.PixelFormat);

            
            unsafe
            {
                int bytesPerPixel = Bitmap.GetPixelFormatSize(Image.PixelFormat) / 8;

                Parallel.For(0, from.Height, y =>
                {
                    byte* currentLine = (byte*)from.Scan0 + (y * from.Stride);

                    for (int x = 0; x < from.Width * bytesPerPixel; x += bytesPerPixel)
                    {
                        switch (GetMaxFromRGB(currentLine[x + 2], currentLine[x + 1], currentLine[x]))
                        {
                            case PrimaryColor.Red:
                                currentLine[x] = 0;
                                currentLine[x + 1] = 0;
                                currentLine[x + 2] = 0xFF;
                                break;
                            case PrimaryColor.Green:
                                currentLine[x] = 0;
                                currentLine[x + 1] = 0xFF;
                                currentLine[x + 2] = 0;
                                break;
                            case PrimaryColor.Blue:
                                currentLine[x] = 0xFF;
                                currentLine[x + 1] = 0;
                                currentLine[x + 2] = 0;
                                break;
                        }
                    }

                });

            }
            Image.UnlockBits(from);

            return Image;
        }

        private PrimaryColor GetMaxFromRGB(int r, int g, int b)
        {
            if (r >= Math.Max(g, b))
                return PrimaryColor.Red;
            else if (g >= Math.Max(r, b))
                return PrimaryColor.Green;
            else return PrimaryColor.Blue;
        }

        public async Task<Bitmap> ToMainColorsAsync()
        {
            return await Task.Run(() =>
            {
                return ToMainColors();
            });
        }
    }
}
