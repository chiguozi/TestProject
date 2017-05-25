using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventManagerTemp
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


    #region send
    public static void Send(EventType eventType)
    {
        Delegate d;
        if (!_eventMap.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action callback = callbacks[i] as Action;
            if (callback != null)
                callback();
        }
    }

    public static void Send<T>(EventType eventType, T arg1)
    {
        Delegate d;
        if (!_eventMap.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action<T> callback = callbacks[i] as Action<T>;
            if(callback != null)
                callback(arg1);
        }
    }

    public static void Send<T, U>(EventType eventType, T arg1, U arg2)
    {
        Delegate d;
        if (!_eventMap.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action<T, U> callback = callbacks[i] as Action<T, U>;
            if (callback != null)
                callback(arg1, arg2);
        }
    }


    public static void Send<T, U, V>(EventType eventType, T arg1, U arg2, V arg3)
    {
        Delegate d;
        if (!_eventMap.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action<T, U, V> callback = callbacks[i] as Action<T, U, V>;
            if (callback != null)
                callback(arg1, arg2, arg3);
        }
    }

    public static void Send<T, U, V, W>(EventType eventType, T arg1, U arg2, V arg3, W arg4)
    {
        Delegate d;
        if (!_eventMap.TryGetValue(eventType, out d))
        {
            return;
        }

        var callbacks = d.GetInvocationList();
        for (int i = 0; i < callbacks.Length; i++)
        {
            Action<T, U, V, W> callback = callbacks[i] as Action<T, U, V, W>;
            if (callback != null)
                callback(arg1, arg2, arg3, arg4);
        }
    }
    #endregion

    #region regist
    public static  void Regist(EventType eventType, Action handler)
    {
        if(CheckAddCallback(eventType, handler))
            _eventMap[eventType] = (Action)_eventMap[eventType] + handler;
    }

    public static void Regist<T>(EventType eventType, Action<T> handler)
    {
        if (CheckAddCallback(eventType, handler))
            _eventMap[eventType] = (Action<T>)_eventMap[eventType] + handler;
    }
    public static void Regist<T, U>(EventType eventType, Action<T, U> handler)
    {
        if (CheckAddCallback(eventType, handler))
            _eventMap[eventType] = (Action<T, U>)_eventMap[eventType] + handler;
    }
    public static void Regist<T, U, V>(EventType eventType, Action<T, U, V> handler)
    {
        if (CheckAddCallback(eventType, handler))
            _eventMap[eventType] = (Action<T, U, V>)_eventMap[eventType] + handler;
    }

    public static void Regist<T, U, V, W>(EventType eventType, Action<T, U, V, W> handler)
    {
        if (CheckAddCallback(eventType, handler))
            _eventMap[eventType] = (Action<T, U, V, W>)_eventMap[eventType] + handler;
    }

    #endregion

    #region unregist
    public static void UnRegist(EventType eventType, Action handler)
    {
        if (CheckRemoveCallback(eventType, handler))
        {
            _eventMap[eventType] = (Action)_eventMap[eventType] - handler;
            if (_eventMap[eventType] == null)
                _eventMap.Remove(eventType);
        }
    }

    public static void UnRegist<T>(EventType eventType, Action<T> handler)
    {
        if (CheckRemoveCallback(eventType, handler))
        {
            _eventMap[eventType] = (Action<T>)_eventMap[eventType] - handler;
            if (_eventMap[eventType] == null)
                _eventMap.Remove(eventType);
        }
    }

    public static void UnRegist<T, U>(EventType eventType, Action<T, U> handler)
    {
        if (CheckRemoveCallback(eventType, handler))
        {
            _eventMap[eventType] = (Action<T, U>)_eventMap[eventType] - handler;
            if (_eventMap[eventType] == null)
                _eventMap.Remove(eventType);
        }
    }

    public static void UnRegist<T, U, V>(EventType eventType, Action<T, U, V> handler)
    {
        if (CheckRemoveCallback(eventType, handler))
        {
            _eventMap[eventType] = (Action<T, U, V>)_eventMap[eventType] - handler;
            if (_eventMap[eventType] == null)
                _eventMap.Remove(eventType);
        }
    }


    public static void UnRegist<T, U, V, W>(EventType eventType, Action<T, U, V, W> handler)
    {
        if (CheckRemoveCallback(eventType, handler))
        {
            _eventMap[eventType] = (Action<T, U, V, W>)_eventMap[eventType] - handler;
            if (_eventMap[eventType] == null)
                _eventMap.Remove(eventType);
        }
    }




    #endregion

    private static bool CheckAddCallback(EventType eventType, Delegate callback)
    {
        if (!_eventMap.ContainsKey(eventType))
        {
            _eventMap.Add(eventType, null);
        }

        Delegate d = _eventMap[eventType];
        if (d != null && d.GetType() != callback.GetType())
        {
            Debug.LogError(string.Format(
                    "Try to add not correct event {0}. Current type is {1}, adding type is {2}.",
                    eventType, d.GetType().Name, callback.GetType().Name));
            return false;
        }
        return true;
    }


    private static bool CheckRemoveCallback(EventType eventType, Delegate callback)
    {
        if (!_eventMap.ContainsKey(eventType))
        {
            return false;
        }

        Delegate d = _eventMap[eventType];
        if (( d != null ) && ( d.GetType() != callback.GetType() ))
        {
           Debug.LogError(string.Format(
                "Remove listener {0}\" failed, Current type is {1}, adding type is {2}.",
                eventType, d.GetType(), callback.GetType()));
            return false;
        }
        else
            return true;
    }
}
