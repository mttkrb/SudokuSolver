using System;
using System.Globalization;
using System.Windows.Data;

namespace SudokuSolver.Gui.Converter
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BooleanInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
