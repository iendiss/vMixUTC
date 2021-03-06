﻿using CommonServiceLocator;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using vMixController.ViewModel;

namespace vMixController.Converters
{
    public class ImagePartConverter : MarkupExtension, IMultiValueConverter
    {

        private static IMultiValueConverter _instance;

        /// <summary>
        /// Static instance of this converter.
        /// </summary>
        public static IMultiValueConverter Instance => _instance ?? (_instance = new ImagePartConverter());

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //URI
            //MAX
            //NUMBER

            if (!(values[0] is string && values[1] is int && values[2] is int))
                return null;

            var path = (string)values[0];
            var max = (int)values[1];
            var number = (int)values[2];
            try
            {
                if (!string.IsNullOrWhiteSpace(path))
                {
                    var bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    var relativePath = Classes.Utils.SearchFile(path, Directory.GetCurrentDirectory());
                    string foundPath = Classes.Utils.SearchFile(path, Path.GetDirectoryName(ServiceLocator.Current.GetInstance<MainViewModel>().ControllerPath));
                    if (File.Exists(path))
                        bi.UriSource = new Uri(path);
                    else if (File.Exists(foundPath))
                    {
                        bi.UriSource = new Uri(foundPath);
                    }
                    else if (File.Exists(relativePath))
                        bi.UriSource = new Uri(relativePath);
                    else
                        return null;//throw new FileNotFoundException();

                    bi.EndInit();
                    var img = new FormatConvertedBitmap(new CroppedBitmap(bi, new System.Windows.Int32Rect((bi.PixelWidth / max) * number, 0, bi.PixelWidth / max, bi.PixelHeight)), PixelFormats.Bgra32, null, 0);
                    return img;
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }
    }
}
