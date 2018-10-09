//------------------------------------------------------------
// <copyright file="GustArrivalSymbolConverter.cs" company="CyberFeedForward" >
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

    public class GuestArrivalSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is GustArrivalStatus)
            {
                var status = (GustArrivalStatus)value;

                switch (status)
                {
                    case GustArrivalStatus.Arrived:
                        return SettingsModel.Arrived;

                    case GustArrivalStatus.ArrivedLate:
                        return SettingsModel.ArrivedLate;

                    case GustArrivalStatus.DidNotCome:
                        return SettingsModel.DidNotCome;

                    case GustArrivalStatus.Unknown:
                       return SettingsModel.UnknownArrival;

                    default:
                        throw new NotSupportedException("Unknown GustArrivalStatus");
                }
            }

            throw new NotSupportedException("Unknown GustArrivalStatus");
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