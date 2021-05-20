using System.Collections.Generic;

namespace LowoUN.Util.Notify
{
    public class EventMgr
    {
        private static EventMgr _instance = null;
        private Dictionary<string, EventGroup> _group2event = null;

        public void Init()
        {
            this._group2event = new Dictionary<string, EventGroup>();
            foreach(EventGroup group in EventConfig.GetInitEventGroupList())
            {
                this._group2event[group.groupName] = group;
            }
        }

        public EventGroup this[string group]
        {
            get
            {
                if(this._group2event.ContainsKey(group))
                {
                    return this._group2event[group];
                }
                return null;
            }
        }
        public static EventMgr instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new EventMgr();
                }
                return _instance;
            }
        }
    }

    public class EventGroup
    {
        public string groupName { get; set; }
        public List<EventName> nameList { get; set; }
    }

    public class EventName
    {
        public string eventName { get; set; }
    }
}