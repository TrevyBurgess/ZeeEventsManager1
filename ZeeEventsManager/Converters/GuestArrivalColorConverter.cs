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
    using Windows.UI;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media;

    public class GuestArrivalColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is GustArrivalStatus)
            {
                var status = (GustArrivalStatus)value;

                switch (status)
                {
                    case GustArrivalStatus.Arrived:
                        return new SolidColorBrush(Colors.LightGreen);

                    case GustArrivalStatus.ArrivedLate:
                        return new SolidColorBrush(Colors.Orange);

                    case GustArrivalStatus.DidNotCome:
                        return new SolidColorBrush(Colors.Red);

                    case GustArrivalStatus.Unknown:
                        return new SolidColorBrush(Colors.Gray);

                    default:
                        return new SolidColorBrush(Colors.Black);
                }
            }

            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string)
            {


            }

            return GustArrivalStatus.Unknown;
        }
    }
}