//------------------------------------------------------------
// <copyright file="DayEvent.cs" company="KingTrevy" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    public class DayEvent
    {
        public DayEvent(BaseEventItemModel eventItem, EventDurationType durationType)
        {
            Event = eventItem;
            DurationType = durationType;
        }

        public BaseEventItemModel Event { get; set; }
        public EventDurationType DurationType { get; set; }
    }
}
