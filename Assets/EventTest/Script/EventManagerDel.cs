using System.Collections;
using System.Collections.Generic;
using System;

public class EventManagerDel
{
    static Dictionary<EventType, Delegate> _eventMap = new Dictionary<EventType, Delegate>();
    public static void Regist(EventType eventType, Delegate callback)
    {
        if (_eventMap.ContainsKey(eventType))
        {
            _eventMap[eventType] = Delegate.Combine(callback, _eventMap[eventType]);
        }
        else
        {
            _eventMap.Add(eventType, callback);
        }
    }

    public static void UnRegist(EventType eventType, Delegate callback)
    {
        if (!_eventMap.ContainsKey(eventType))
            return;
        _eventMap[eventType] = Delegate.Remove(_eventMap[eventType], callback);
        if (_eventMap[eventType] == null)
            _eventMap.Remove(eventType);
    }

    public static void Send(EventType eventType, params object[] objs)
    {
        if (_eventMap.ContainsKey(eventType))
        {
            //不直接调用，防止异常时，中断所有回调。
            var callbacks = _eventMap[eventType].GetInvocationList();
            for (int i = 0; i < callbacks.Length; i++)
            {
                callbacks[i].DynamicInvoke(objs);
            }
        }
    }
}
