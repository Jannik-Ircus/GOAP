using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionWarmUp : GOAPActionClass
{
    //private string firepitTag = "firepit";
    public override void AbortAction(GOAPAgent agent)
    {
    }

    public override float GetCost(GOAPAgent agent)
    {
        return -1;
    }

    public override bool IsAchievable()
    {
        return true;
        //if (!GOAPWorld.Instance.GetWorld().HasState(firepitTag)) return false;
        //return GOAPWorld.Instance.GetWorld().GetStateValue(firepitTag) > 0;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal, string goalTag)
    {
        GameObject firepit = GameObject.FindGameObjectWithTag(goalTag);
        if(firepit==null)
        {
            yield return null;
        }

        if (Vector3.Distance(agent.gameObject.transform.position, firepit.transform.position) > 3)
        {
            NavMeshAgent navAgent = agent.GetComponent<NavMeshAgent>();
            if (navAgent == null)
            {
                Debug.LogError("No NavMeshAgent found on " + agent.gameObject);
                yield return new WaitForEndOfFrame();
            }

            navAgent.isStopped = false;
            navAgent.SetDestination(firepit.gameObject.transform.position);

            yield return new WaitForSeconds(1);
            while (navAgent.remainingDistance >= navAgent.stoppingDistance + 2 && isRunning)
            {
                yield return null;
            }

            navAgent.isStopped = true;

        }
    }

    public override void PostPerform(GOAPAgent agent)
    {
    }

    public override void PrePerform(GOAPAgent agent)
    {
    }

}
