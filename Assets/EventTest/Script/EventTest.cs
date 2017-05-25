using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventTest : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        EventManagerObjs.Regist(EventType.First, OnObjsEvent);
        EventManagerDel.Regist(EventType.First, (Action < string, string>) OnDelEvent);
        EventManagerTemp.Regist<string, string>(EventType.First, OnTempEvet);

        var time = Time.realtimeSinceStartup;
        for(int i = 0; i  < 10000; i ++)
        {
            EventManagerObjs.Send(EventType.First, "1", "1");
        }
        Debug.LogError(Time.realtimeSinceStartup - time);
        time = Time.realtimeSinceStartup;
        for (int i = 0; i < 10000; i++)
        {
            EventManagerDel.Send(EventType.First, "1", "1");
        }
        Debug.LogError(Time.realtimeSinceStartup - time);
        time = Time.realtimeSinceStartup;
        for (int i = 0; i < 10000; i++)
        {
            EventManagerTemp.Send(EventType.First, "1", "1");
        }
        Debug.LogError(Time.realtimeSinceStartup - time);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnObjsEvent(object[] args)
    {
        //string arg0 = (string)args[0];
        //string arg1 = (string)args[1];
        //string arg2 = (string)args[2];
        //string arg3 = (string)args[3];

    }

    void OnDelEvent(string arg0, string arg1 )
    {

    }

    void OnTempEvet(string arg1, string arg2)
    {

    }
}
