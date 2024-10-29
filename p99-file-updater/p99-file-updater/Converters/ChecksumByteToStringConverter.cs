using System;
using System.Text;
using Windows.UI.Xaml.Data;

namespace p99FileUpdater.Converters
{
    internal class ChecksumByteToStringConverter : IValueConverter
    { 
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value == null)
                return String.Empty;
            StringBuilder sb = new StringBuilder();
            byte[] checksumToConvert = (byte[])value;
            foreach (byte b in checksumToConvert)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            String objectValue = (String)value;
            return System.Text.Encoding.Unicode.GetBytes(objectValue);
        }
    }
}
