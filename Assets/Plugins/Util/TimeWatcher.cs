using UnityEngine;
using System;
using System.Timers;
using System.Collections.Generic;
using LowoUN.Util.Log;

namespace LowoUN.Util
{
	public class TimeWatcher: MonoBehaviour
    {
        private static TimeWatcher _instance;
		public static TimeWatcher instance
		{
			get
			{
				if (_instance == null) {
                    Debug.LogWarning("====== LowoUI-UN ===> no TimeWatcher ins found !");
                    _instance = new GameObject("TimeWatcher").AddComponent<TimeWatcher>();
                }

				return _instance;
			}
		}

        private Dictionary<string, WatchData> _name2watch = new Dictionary<string, WatchData>();
        private Dictionary<string, Action> _name2action = new Dictionary<string, Action>();
        private Queue<Action> _cbqueue = new Queue<Action>();

		void Awake () {
			_instance = this;
		}

        /// <summary>
        /// add delay event
        /// </summary>
        /// <param name="name">uniqueue name for each delay event</param>
        /// <param name="mseconds">milliseconds for delay</param>
        /// <param name="loop">repeat or not</param>
        /// <param name="callback"></param>
		public void AddWatcher(string name, uint mseconds, bool loop, Action callback, bool isTimeScaleAff = true)
        {
			if (mseconds <= 0) {
				Debug.LogWarning (Format.Util("TimeWatcher") + "watch time can not be less or equal 0");
				return;
			}
			
            if (!this._name2watch.ContainsKey(name))
            {
				uint ms = isTimeScaleAff ? (uint)((float)mseconds * (1f/Time.timeScale)) : mseconds;
				if (ms <= 0)
					return;
				
				this._name2watch[name] = new WatchData(name, ms, loop, this.OnCallback);
                this._name2action[name] = callback;
                this._name2watch[name].Start();
            }
            else
            {
				Debug.LogWarning(Format.Util("TimeWatcher") + "This name" + name + " of timer is exist!");
            }
        }

        /// <summary>
        /// add specific time event
        /// </summary>
        /// <param name="name">uniqueue name for each delay event</param>
        /// <param name="time">specific time</param>
        /// <param name="loop">repeat ot not</param>
        /// <param name="callback"></param>
        public void AddWatcher(string name, DateTime time, bool loop, Action callback)
        {
            if (!this._name2watch.ContainsKey(name))
            {
                this._name2watch[name] = new WatchData(name, time, loop, this.OnCallback);
                this._name2action[name] = callback;
                this._name2watch[name].Start();
            }
            else
            {
                Debug.LogError("This name" + name + " of timer is exist!");
            }
        }

        /// <summary>
        /// add specific time event with time string
        /// </summary>
        /// <param name="name">uniqueue name for each delay event</param>
        /// <param name="time">specific time string include hour, minute, second, eg."18:00:00"</param>
        /// <param name="loop">repeat or not</param>
        /// <param name="callback"></param>
        public void AddWatcher(string name, string time, bool loop, Action callback)
        {
            this.AddWatcher(name, DateTime.Parse(time), loop, callback);
        }

        /// <summary>
        /// remove watcher
        /// </summary>
        /// <param name="name"></param>
        public void RemoveWatcher(string name)
        {
            if (this._name2watch.ContainsKey(name))
            {
                this._name2watch[name].End();
                this._name2watch.Remove(name);
                this._name2action.Remove(name);
            }
        }

        private void OnCallback(string name)
        {
            if (this._name2action.ContainsKey(name))
            {
                this._cbqueue.Enqueue(this._name2action[name]);
                if (!this._name2watch[name].Loop)
                {
                    this.RemoveWatcher(name);
                }
            }
        }

		void Update()
        //public void OnUpdate()
        {
            if (this._cbqueue.Count > 0)
            {
                Action action = this._cbqueue.Dequeue();
                if (action != null) { action(); }
            }
        }

		void OnApplicationQuit()
		//public void OnApplicationQuit()
        {
            foreach (var kvp in this._name2watch)
            {
                kvp.Value.End();
            }
            this._name2watch.Clear();
            this._name2action.Clear();
            this._cbqueue.Clear();
        }

        /// <summary>
        /// Check time event is exist
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainKey(string key)
        {
			if (key == null)
				return false;
            return this._name2watch.ContainsKey(key);
        }
    }

    public class WatchData
    {
        public string _name;
		public Timer _watcher;
        public DateTime _time;
        public Action<string> _callback;
        private string _curDate;
        private bool _loop;
        private bool _specific;

        public WatchData(string name, uint mseconds, bool loop, Action<string> callback)
        {
            this._name = name;
            this._specific = false;
            this._watcher = new Timer(mseconds);
            this._watcher.Elapsed += this.OnElapsed;
            this._loop = loop;
            this._callback = callback;
            this._watcher.AutoReset = loop;
        }

        public WatchData(string name, DateTime time, bool loop, Action<string> callback)
        {
            this._name = name;
            this._time = time;
            this._specific = true;
            TimeSpan span = time - DateTime.Now;
            if (span.TotalMilliseconds > 0)
            {
                this._watcher = new Timer(span.TotalMilliseconds);
            }
            else
            {
                span = time.AddDays(1) - DateTime.Now;
                this._watcher = new Timer(span.TotalMilliseconds);
            }
            this._loop = loop;
            this._watcher.Elapsed += this.OnElapsed;
            this._callback = callback;
            this._watcher.AutoReset = false;
        }

        public void OnElapsed(object sender, ElapsedEventArgs e)
        {
            if (!this._specific)
            {
                if (!this._loop) { this._watcher.Stop(); }
                if (this._callback != null) { this._callback(this._name); }
            }
            else
            {
                if (!this._loop)
                {
                    this._watcher.Stop();
                    if (this._callback != null) { this._callback(this._name); }
                }
                else
                {
                    this._watcher.Stop();
                    if (this._callback != null) { this._callback(this._name); }
                    this._watcher.Interval = new TimeSpan(24, 0, 0).Milliseconds;
                    this._watcher.Start();
                }
            }
        }

        public void Start()
        {
            this._watcher.Start();
        }

        public void Stop()
        {
            this._watcher.Stop();
        }

        public void End()
        {
            this._watcher.Stop();
            this._watcher.Elapsed -= this.OnElapsed;
            this._watcher.Dispose();
        }

        public bool Loop { get { return this._loop; } }
    }
}