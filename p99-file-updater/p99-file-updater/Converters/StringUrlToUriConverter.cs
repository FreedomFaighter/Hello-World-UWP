using System;
using Windows.UI.Xaml.Data;

namespace p99FileUpdater.Converters
{
    internal class StringUrlToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {   if(value != null)
                return new Uri((String)value);
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return ((Uri)value).OriginalString;
        }
    }
}
