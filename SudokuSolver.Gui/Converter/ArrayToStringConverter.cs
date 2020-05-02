using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SudokuSolver.Gui.Converter
{
    [ValueConversion(typeof(IEnumerable<int>), typeof(string))]
    public class ArrayToStringConverter : IValueConverter
    {        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = string.Empty;
            if(value is IEnumerable<int> enumerable)
            {
                var array = enumerable.ToArray();
                if(parameter is int newLinesAfter)
                {
                    var x = 0;
                    var lines = new List<string>();
                    var moreLines = true;
                    while (moreLines)
                    {

                        if (x + newLinesAfter < array.Length)
                        {
                            lines.Add(string.Join(' ', array.Skip(x).Take(newLinesAfter)));
                        }
                        else
                        {
                            lines.Add(string.Join(' ', array.Skip(x)));
                            moreLines = false;
                        }

                        x += newLinesAfter;
                    }

                    lines.RemoveAll(a => string.IsNullOrEmpty(a));
                    result = string.Join(Environment.NewLine, lines);
                }
                else
                {
                    result=string.Join(' ', array.Select(s => s.ToString()));
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
