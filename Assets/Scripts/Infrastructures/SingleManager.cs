using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsibility: Manage singletons classes
/// </summary>
public static class SingleManager
{
    #region Members
    private static Dictionary<Type, object> singletons = new Dictionary<Type, object>();
    #endregion

    #region Functions
    public static void Register<T>(T instance) where T : class
    {
        if (instance == null)
        {
            Debug.LogError("SingleManager.Register: instance can't be null");
            return;
        }
        if (singletons.ContainsKey(typeof(T)))
            return;

        singletons.Add(typeof(T), instance);
    }

    public static void Remove<T>() where T : class
    {
        if (singletons.ContainsKey(typeof(T)))
            singletons.Remove(typeof(T));
        else
            Debug.LogError("You tery to remove unexist class: "+ typeof(T));
    }

    public static T Get<T>() where T : class
    {
        if (!singletons.ContainsKey(typeof(T)))
        {
            Debug.LogError("SingleManager.Get: can't find instance with type of: " + typeof(T));
            return null;
        }
        else
            return (T)singletons[typeof(T)];
    }
    #endregion
}