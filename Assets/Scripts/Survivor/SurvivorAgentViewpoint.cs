using System.Collections;
using UnityEngine;

public class SurvivorAgentViewpoint : MonoBehaviour
{
    public CapsuleCollider capsuleCollider;
    public GOAPAgent agent;
    private string spottedEnemyTag = "spottedEnemy";
    [Range(1, 15)]
    public int calmDownTime = 5;
    private bool spottedEnemy = false;

    private void Start()
    {
        if(capsuleCollider == null)
        {
            Debug.LogError("No capsule collider found on: " + name);
            Destroy(this);
        }
        if (agent == null)
        {
            Debug.LogError("No GOAPAgent found on: " + name);
            Destroy(this);
        }

        if (!agent.agentStates.HasState(spottedEnemyTag)) agent.agentStates.AddState(spottedEnemyTag, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SurvivorEnemyUpdater>() && !spottedEnemy)
        {
            if(agent.agentStates.HasState(spottedEnemyTag))agent.agentStates.SetState(spottedEnemyTag, 1);
            spottedEnemy = true;
            StartCoroutine(CalmDown());
        }
    }

    private IEnumerator CalmDown()
    {
        yield return new WaitForSeconds(calmDownTime);
        if (agent.agentStates.HasState(spottedEnemyTag)) agent.agentStates.SetState(spottedEnemyTag, 0);
        spottedEnemy = false;
    }
}
