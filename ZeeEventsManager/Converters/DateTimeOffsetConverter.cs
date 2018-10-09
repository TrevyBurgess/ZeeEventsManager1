//------------------------------------------------------------
// <copyright file="GustArrivalColorConverter.cs" company="CyberFeedForward" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------ 
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using System;
    using Windows.UI.Xaml.Data;

    public class DateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime)
            {
                return new DateTimeOffset((DateTime)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset)
            {
                var date = (DateTimeOffset)value;

                return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}