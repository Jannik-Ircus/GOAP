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
    public abstract IEnumerator PerformAction(GOAPAgent agent, GameObject goal);
    public abstract void AbortAction(GOAPAgent agent, GameObject goal);

    protected void LogError(object message)
    {
        Debug.LogError("GOAP - Action: " + actionName + " ->" + message);
    }
}

public class Moving : GOAPActionClass
{
    public override void AbortAction(GOAPAgent agent, GameObject goal)
    {
        throw new System.NotImplementedException();
    }

    public override bool IsAchievable()
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal)
    {
        throw new System.NotImplementedException();
    }

    public override void PostPerform()
    {
        throw new System.NotImplementedException();
    }

    public override void PrePerform()
    {
        throw new System.NotImplementedException();
    }
}

public class Walking : GOAPActionClass
{
    public override void AbortAction(GOAPAgent agent, GameObject goal)
    {
        throw new System.NotImplementedException();
    }

    public override bool IsAchievable()
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal)
    {
        throw new System.NotImplementedException();
    }

    public override void PostPerform()
    {
        throw new System.NotImplementedException();
    }

    public override void PrePerform()
    {
        throw new System.NotImplementedException();
    }
}
