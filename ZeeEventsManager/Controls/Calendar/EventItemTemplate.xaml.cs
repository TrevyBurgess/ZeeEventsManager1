//------------------------------------------------------------
// <copyright file="EventItemTemplate.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//-----------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using Windows.UI.Xaml.Controls;

    public sealed partial class EventItemTemplate : UserControl
    {
        public DayEvent DayEvent { get { return DataContext as DayEvent; } }
        
        public EventItemTemplate()
        {
            InitializeComponent();

            DataContextChanged += (s, e) => Bindings.Update();
        }
    }
}
