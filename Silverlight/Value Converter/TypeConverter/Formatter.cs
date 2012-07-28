using System;
using System.Windows.Data;

namespace TypeConverter_CS
{
    public class Formatter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            if ( parameter != null )
            {
                string formatterString = parameter.ToString( );

                if ( !string.IsNullOrEmpty( formatterString ) )
                    return String.Format( culture, formatterString, value );
            }

            return value.ToString( );
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            return value;
        }

        #endregion
    }
}