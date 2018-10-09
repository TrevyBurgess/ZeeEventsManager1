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
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// Return white for enabled, gray for disabled
    /// </summary>
    public class EventBorderConverter : IValueConverter
    {
        /// <summary>
        /// If true, return 1, else return 0
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is EventDurationType)
            {
                switch ((EventDurationType)value)
                {
                    case EventDurationType.None:
                        return new Thickness(1, 1, 2, 1);

                    case EventDurationType.SingleDay:
                        return new Thickness(2);

                    case EventDurationType.BeginMultiday:
                        return new Thickness(2, 2, 0, 2);

                    case EventDurationType.MiddleMultiday:
                        return new Thickness(0, 2, 0, 2);

                    case EventDurationType.EndMultiday:
                        return new Thickness(0, 2, 3, 2);

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