using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChopTree : GOAPActionClass
{
    public override void AbortAction(GOAPAgent agent)
    {
        
    }

    public override float GetCost(GOAPAgent agent)
    {
        return -1;
    }

    public override bool IsAchievable()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        if (trees.Length > 0) return true;
        else return false;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag)
    {
        return null;
    }

    public override void PostPerform(GOAPAgent agent)
    {
        
    }

    public override void PrePerform(GOAPAgent agent)
    {
        
    }
}
