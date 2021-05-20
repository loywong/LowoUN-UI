using UnityEngine;
using System;

namespace LowoUN.Util.Log
{
	public class Exceptions {

		static public BroadcastException CreateBroadcastSignatureException(string eventType)
		{
			return new BroadcastException(string.Format("Broadcasting message {0} but listeners have a different signature than the broadcaster.", eventType));
		}

	}

	public class BroadcastException : Exception
	{
		public BroadcastException(string msg) : base(msg) { }
	}

	public class ListenerException : Exception
	{
		public ListenerException(string msg) : base(msg) { }
	}
}
