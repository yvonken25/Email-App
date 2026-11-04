using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfAEmailApp.Converters
{
    public class TaskStrikethroughConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isDone && isDone)
            {
                return Brushes.Gray; // Couleur si cochée
            }
            return Brushes.Black; // Couleur normale
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}