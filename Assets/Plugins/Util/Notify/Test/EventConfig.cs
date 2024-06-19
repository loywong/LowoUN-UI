using System.Collections.Generic;

namespace LowoUN.Util.Notify {
    public class EventConfig {
        public static List<EventGroup> groupList = null;

        public static List<EventGroup> GetInitEventGroupList () {
            groupList = new List<EventGroup> ();
            groupList.Add (new EventGroup () { groupName = "group_1", nameList = new List<EventName> () { new EventName () { eventName = "event_1_1" }, new EventName () { eventName = "event_1_2" } } });
            groupList.Add (new EventGroup () { groupName = "group_2", nameList = new List<EventName> () { new EventName () { eventName = "event_2_1" }, new EventName () { eventName = "event_2_2" } } });
            return groupList;
        }
    }
}