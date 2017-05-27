using System;
using System.Collections.Generic;

public class EventManagerObjs
{
    static Dictionary<EventTypes, Action<object[]>> _eventMap = new Dictionary<EventTypes, Action<object[]>>();
    public static void Regist(EventTypes eventType, Action<object[]> callback)
    {
        if (_eventMap.ContainsKey(eventType))
        {
            _eventMap[eventType] += callback;
        }
        else
        {
            _eventMap.Add(eventType, callback);
        }
    }

    public static void UnRegist(EventTypes eventType, Action<object[]> callback)
    {
        if (!_eventMap.ContainsKey(eventType))
            return;
        _eventMap[eventType] -= callback;
        if (_eventMap[eventType] == null)
            _eventMap.Remove(eventType);
    }

    public static void Send(EventTypes eventType, params object[] objs)
    {
        if (_eventMap.ContainsKey(eventType))
        {
            //不直接调用，防止异常时，中断所有回调。
            var callbacks = _eventMap[eventType].GetInvocationList();
            for (int i = 0; i < callbacks.Length; i++)
            {
                var callback = callbacks[i] as Action<object[]>;
                if (callback != null)
                {
                    callback(objs);
                }
            }
        }
    }
}