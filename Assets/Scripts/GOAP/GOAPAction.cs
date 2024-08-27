using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Action", menuName ="GOAP/Action")]
public class GOAPAction : ScriptableObject
{
    public string actionName;
    public float cost;
    [HideInInspector]
    public GOAPAgent agent;
    [HideInInspector]
    public GameObject goal;
    
    public GOAPWorldState[] preConditions;
    public GOAPWorldState[] afterEffects;

    public GOAPActionClass actionClass;

    public void PerfomAction()
    {
        Debug.Log("Perform Action on actionclass: " + actionClass.name);
        actionClass.PerformAction(agent, goal);
    }

    public bool IsAchievable()
    {
        return actionClass.IsAchievable();
    }

    public void PrePerform()
    {
        actionClass.actionName = actionName;
        actionClass.PrePerform();
    }

    public void PostPerform()
    {
        actionClass.PostPerform();
    }

    public void AbortAction()
    {
        actionClass.AbortAction(agent, goal);
    }
}
