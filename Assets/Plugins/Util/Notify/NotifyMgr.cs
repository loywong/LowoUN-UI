using System;

namespace LowoUN.Util.Notify
{
    public static class NotifyMgr
    {
        #region add listener
        public static void AddListener(string eventType, Action callback)
        {
            Messenger.AddListener(eventType, callback);
        }

        public static void AddListener<T>(string eventType, Action<T> callback)
        {
            Messenger<T>.AddListener(eventType, callback);
        }

        public static void AddListener<T, U>(string eventType, Action<T, U> callback)
        {
            Messenger<T, U>.AddListener(eventType, callback);
        }

        public static void AddListener<T, U, V>(string eventType, Action<T, U, V> callback)
        {
            Messenger<T, U, V>.AddListener(eventType, callback);
        }

        public static void AddListener<TReturn>(string eventType, Func<TReturn> callback)
        {
            Messenger.AddListener<TReturn>(eventType, callback);
        }

        public static void AddListener<T, TReturn>(string eventType, Func<T, TReturn> callback)
        {
            Messenger<T>.AddListener<TReturn>(eventType, callback);
        }

        public static void AddListener<T, U, TReturn>(string eventType, Func<T, U, TReturn> callback)
        {
            Messenger<T, U>.AddListener<TReturn>(eventType, callback);
        }

        public static void AddListener<T, U, V, TReturn>(string eventType, Func<T, U, V, TReturn> callback)
        {
            Messenger<T, U, V>.AddListener(eventType, callback);
        }
        #endregion

        #region remove listener
        public static void RemoveListener(string eventType, Action callback)
        {
            Messenger.RemoveListener(eventType, callback);
        }

        public static void RemoveListener<T>(string eventType, Action<T> callback)
        {
            Messenger<T>.RemoveListener(eventType, callback);
        }

        public static void RemoveListener<T, U>(string eventType, Action<T, U> callback)
        {
            Messenger<T, U>.RemoveListener(eventType, callback);
        }

        public static void RemoveListener<T, U, V>(string eventType, Action<T, U, V> callback)
        {
            Messenger<T, U, V>.RemoveListener(eventType, callback);
        }

        public static void RemoveListener<TReturn>(string eventType, Func<TReturn> callback)
        {
            Messenger.RemoveListener<TReturn>(eventType, callback);
        }

        public static void RemoveListener<T, TReturn>(string eventType, Func<T, TReturn> callback)
        {
            Messenger<T>.RemoveListener<TReturn>(eventType, callback);
        }

        public static void RemoveListener<T, U, TReturn>(string eventType, Func<T, U, TReturn> callback)
        {
            Messenger<T, U>.RemoveListener<TReturn>(eventType, callback);
        }

        public static void RemoveListener<T, U, V, TReturn>(string eventType, Func<T, U, V, TReturn> callback)
        {
            Messenger<T, U, V>.RemoveListener<TReturn>(eventType, callback);
        }
        #endregion

		private static bool isTestMode = false;
		public static void SetTestMode (bool isTest) {
			isTestMode = isTest;
		}

        #region broadcast
        public static void Broadcast(string eventType)
        {
			if(!isTestMode)
          		Messenger.Broadcast(eventType);
        }

        public static void Broadcast<T>(string eventType, T t)
        {
			if(!isTestMode)
           		Messenger<T>.Broadcast(eventType, t);
        }

        public static void Broadcast<T, U>(string eventType, T t, U u)
        {
			if(!isTestMode)
				Messenger<T, U>.Broadcast(eventType, t, u);
        }

        public static void Broadcast<T, U, V>(string eventType, T t, U u, V v)
        {
			if(!isTestMode)
            Messenger<T, U, V>.Broadcast(eventType, t, u, v);
        }

        public static void Broadcast<TReturn>(string eventType, Action<TReturn> callback)
        {
			if(!isTestMode)
            	Messenger.Broadcast<TReturn>(eventType, callback);
        }

        public static void Broadcast<T, TReturn>(string eventType, T t, Action<TReturn> callback)
        {
			if(!isTestMode)
            	Messenger<T>.Broadcast<TReturn>(eventType, t, callback);
        }

        public static void Broadcast<T, U, TReturn>(string eventType, T t, U u, Action<TReturn> callback)
        {
			if(!isTestMode)
            	Messenger<T, U>.Broadcast<TReturn>(eventType, t, u, callback);
        }

        public static void Broadcast<T, U, V, TReturn>(string eventType, T t, U u, V v, Action<TReturn> callback)
        {
			if(!isTestMode)
            	Messenger<T, U, V>.Broadcast<TReturn>(eventType, t, u, v, callback);
        }
        public static void Broadcast(string eventType, MessengerMode mode)
        {
			if(!isTestMode)
            	Messenger.Broadcast(eventType, mode);
        }

        public static void Broadcast<T>(string eventType, T t, MessengerMode mode)
        {
			if(!isTestMode)
            	Messenger<T>.Broadcast(eventType, t, mode);
        }

        public static void Broadcast<T, U>(string eventType, T t, U u, MessengerMode mode)
        {
			if(!isTestMode)
            	Messenger<T, U>.Broadcast(eventType, t, u, mode);
        }

        public static void Broadcast<T, U, V>(string eventType, T t, U u, V v, MessengerMode mode)
        {
			if(!isTestMode)
            	Messenger<T, U, V>.Broadcast(eventType, t, u, v, mode);
        }

        public static void Broadcast<TReturn>(string eventType, Action<TReturn> callback, MessengerMode mode)
        {
			if(!isTestMode)
            	Messenger.Broadcast<TReturn>(eventType, callback, mode);
        }

        public static void Broadcast<T, TReturn>(string eventType, T t, Action<TReturn> callback, MessengerMode mode)
        {
			if(!isTestMode)
            	Messenger<T>.Broadcast<TReturn>(eventType, t, callback, mode);
        }

        public static void Broadcast<T, U, TReturn>(string eventType, T t, U u, Action<TReturn> callback, MessengerMode mode)
        {
			if(!isTestMode)
            	Messenger<T, U>.Broadcast<TReturn>(eventType, t, u, callback, mode);
        }

        public static void Broadcast<T, U, V, TReturn>(string eventType, T t, U u, V v, Action<TReturn> callback, MessengerMode mode)
        {
			if(!isTestMode)
            	Messenger<T, U, V>.Broadcast<TReturn>(eventType, t, u, v, callback, mode);
        }
        #endregion
    }
}