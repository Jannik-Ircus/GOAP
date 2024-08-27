using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTester : MonoBehaviour
{
    public GameObject agent;
    public GameObject goal;
    private NavMeshAgent navAgent;
    public GOAPAgent goapAgent;

    private void Start()
    {
        //navAgent =agent.GetComponent<NavMeshAgent>();
        //if (navAgent == null) Debug.LogError("Failed to get NavMeshAgent Component.");
        goapAgent.actions[0].agent = goapAgent;
        goapAgent.actions[0].goal = goal;
        goapAgent.actions[0].PerfomAction();
        StartCoroutine(AbortAction());
    }

    private void Update()
    {
        //if (navAgent == null) return;
        //navAgent.SetDestination(goal.transform.position);
    }

    private IEnumerator AbortAction()
    {
        yield return new WaitForSeconds(2);
        goapAgent.actions[0].AbortAction();
    }
}
