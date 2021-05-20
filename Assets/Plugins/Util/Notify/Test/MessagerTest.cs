using UnityEngine;
using LowoUN.Util.Notify;

public class MessagerTest : MonoBehaviour
{
    private bool hasCalled = false;

    // Use this for initialization
    void Start()
    {
        //initialize the event config information
        EventMgr.instance.Init();
        this.AddListener();
        this.BroadcastMsg();
        this.Removelistener();
    }

    private void AddListener()
    {
        NotifyMgr.AddListener(EventMgr.instance["group_1"].nameList[0].eventName, this.TestEventCall);
        NotifyMgr.AddListener<float>(EventMgr.instance["group_1"].nameList[1].eventName, this.TestEventCallFloat);
        NotifyMgr.AddListener<int, float>(EventMgr.instance["group_2"].nameList[0].eventName, this.TestEventCallIntFloat);
    }

    private void BroadcastMsg()
    {
        this.hasCalled = false;
        NotifyMgr.Broadcast(EventMgr.instance["group_1"].nameList[0].eventName);
        if (!this.hasCalled) { Debug.LogError("Unit test failure - event handler appears to have not been called."); }
        this.hasCalled = false;
        NotifyMgr.Broadcast<float>(EventMgr.instance["group_1"].nameList[1].eventName, 1.0f);
        if (!this.hasCalled) { Debug.LogError("Unit test failure - event handler appears to have not been called."); }
        this.hasCalled = false;
        NotifyMgr.Broadcast<int, float>(EventMgr.instance["group_2"].nameList[0].eventName, 1, 1.0f);
        if (!this.hasCalled) { Debug.LogError("Unit test failure - event handler appears to have not been called."); }
    }

    private void Removelistener()
    {
        NotifyMgr.RemoveListener(EventMgr.instance["group_1"].nameList[0].eventName, this.TestEventCall);
        NotifyMgr.RemoveListener<float>(EventMgr.instance["group_1"].nameList[1].eventName, this.TestEventCallFloat);
        NotifyMgr.RemoveListener<int, float>(EventMgr.instance["group_2"].nameList[0].eventName, this.TestEventCallIntFloat);
    }

    private void TestEventCall()
    {
        this.hasCalled = true;
        Debug.Log("get the register event call");
    }

    private void TestEventCallFloat(float value)
    {
        this.hasCalled = true;
        Debug.Log("get the event call value: " + string.Format("{0:f2}", value));
        if (value != 1.0f)
        {
            Debug.LogError("Unit test failure - wrong value on float argument");
        }
    }

    private void TestEventCallIntFloat(int intvalue, float floatvalue)
    {
        this.hasCalled = true;
        Debug.Log("get the event call value: " + intvalue + " - " + string.Format("{0:f2}", floatvalue));
    }
}