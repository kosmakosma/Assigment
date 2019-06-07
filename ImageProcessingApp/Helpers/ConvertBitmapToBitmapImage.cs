using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ImageProcessingApp.Helpers
{
    public class ConvertBitmapToBitmapImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            Bitmap bitmap = value as Bitmap;

            if (value == null)
                return null;

            BitmapImage bitmapImage;

            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, bitmap.RawFormat);
                memory.Position = 0;
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
