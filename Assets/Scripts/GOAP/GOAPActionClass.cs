using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class GOAPActionClass
{
    [HideInInspector]
    public bool isRunning = false;
    [HideInInspector]
    public string actionName;

    public abstract void PrePerform();
    public abstract void PostPerform();
    public abstract bool IsAchievable();
    public abstract IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag);
    public abstract void AbortAction(GOAPAgent agent);
    public abstract float GetCost();

    protected void LogError(object message)
    {
        Debug.LogError("GOAP - Action: " + actionName + " ->" + message);
    }
}