using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTester : MonoBehaviour
{
    public GameObject agent;
    public GameObject goal;
    private NavMeshAgent navAgent;
    public GOAPAgent goapAgent;

    private GOAPActionClass actionClass;

    private void Start()
    {
        //actionClass = goapAgent.actions[0].GetGOAPActionClassScript();
        actionClass = goapAgent.actions[0].GetGOAPActionClassFromCustom();
        if(actionClass == null)
        {
            Debug.LogError("No GOAP Action class found!");
            return;
        }
        
    }

    private void Update()
    {
        //if (navAgent == null) return;
        //navAgent.SetDestination(goal.transform.position);
    }

    public void StartActionCoroutine()
    {
        if(!actionClass.IsAchievable())
        {
            Debug.Log("Action is not achievable");
            return;
        }
        if (actionClass.isRunning) return;
        StartCoroutine(StartAction());
    }

    private IEnumerator StartAction()
    {
        Debug.Log("Coroutine started");
        //MonoScript monoScript = goapAgent.actions[0].monoScript;
        

        yield return StartCoroutine(actionClass.PerformAction(goapAgent, goal));
        Debug.Log("Coroutine is finished");
    }

    public void AbortAction()
    {
        actionClass.AbortAction(goapAgent);
        StopCoroutine(StartAction());
        StopCoroutine(actionClass.PerformAction(goapAgent, goal));
    }
}
