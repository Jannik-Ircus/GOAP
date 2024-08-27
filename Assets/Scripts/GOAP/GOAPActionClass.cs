using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPActionClass : ScriptableObject
{
    [HideInInspector]
    public bool isRunning = false;
    [HideInInspector]
    public string actionName;

    public abstract void PrePerform();
    public abstract void PostPerform();
    public abstract bool IsAchievable();
    public abstract void PerformAction(GOAPAgent agent, GameObject goal);
    public abstract void AbortAction(GOAPAgent agent, GameObject goal);

    protected void LogError(object message)
    {
        Debug.LogError("GOAP - Action: " + actionName + " ->" + message);
    }
}
