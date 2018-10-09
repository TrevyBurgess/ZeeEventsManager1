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
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media;
    /// <summary>
    /// Return white for enabled, gray for disabled
    /// </summary>
    public class EventBackgroundConverter : IValueConverter
    {
        /// <summary>
        /// If true, return 1, else return 0
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is EventDurationType)
            {
                var background = Colors.LightBlue;
                if (parameter is Colors)
                    background = (Color)parameter;

                switch ((EventDurationType)value)
                {
                    case EventDurationType.None:
                        return new SolidColorBrush(Colors.White);

                    case EventDurationType.SingleDay:
                        return new SolidColorBrush(Colors.LightBlue);

                    case EventDurationType.BeginMultiday:
                        return new SolidColorBrush(Colors.LightBlue);

                    case EventDurationType.MiddleMultiday:
                        return new SolidColorBrush(Colors.LightBlue);

                    case EventDurationType.EndMultiday:
                        return new SolidColorBrush(Colors.LightBlue);

                    default:
                        throw new NotSupportedException();
                }
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Thickness)
            {

            }

            throw new NotSupportedException();
        }
    }
}