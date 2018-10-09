//------------------------------------------------------------
// <copyright file="DeviceFamilyTrigger.cs" company="CyberFeedForward" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Common.WPF
{
    using Windows.ApplicationModel.Resources.Core;
    using Windows.UI.Xaml;

    public class DeviceFamilyTrigger : StateTriggerBase
    {
        private string _deviceFamily;

        public string DeviceFamily
        {
            get { return _deviceFamily; }
            set
            {
                var qualifiers = ResourceContext.GetForCurrentView().QualifierValues;

                if (qualifiers.ContainsKey("DeviceFamily"))
                    SetActive(qualifiers["DeviceFamily"] == (_deviceFamily = value));
                else
                    SetActive(false);
            }
        }
    }
}
