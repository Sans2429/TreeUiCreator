using SiemensXmlCreation.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SiemensXmlCreation.Converters
{
    public class ValueToArray : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.IsTypeOf(out string[] values))
                return Binding.DoNothing;

            StringBuilder stringBuilder=new StringBuilder();
            for (int position = 0; position < values.Length; position++)
            {
                stringBuilder.Append(values[position]);
                if(position < values.Length - 1)
                    stringBuilder.Append(' ');
            }
            return stringBuilder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!value.IsTypeOf(out string parsedValue))
                return Binding.DoNothing;

            return parsedValue.Split(new char[] {' '},StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
