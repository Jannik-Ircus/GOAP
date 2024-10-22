using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorFirepit : MonoBehaviour
{
    public int burnDuration = 3;
    private int burnStatus = 0;
    public GameObject fire;
    public GameObject progressBar;
    private string firepitTag = "firepit";
    private string warmthTag = "warmth";

    private void Awake()
    {
        //GOAPWorld.Instance.GetWorld().ModifyState("firepit", 0);
        if (!GOAPWorld.Instance.GetWorld().HasState(firepitTag)) GOAPWorld.Instance.GetWorld().AddState(firepitTag, 0);
        GOAPWorld.Instance.GetWorld().SetState(firepitTag, 0);
        if (fire==null)
        {
            Debug.LogError("No fireObject set in " + name);
            Destroy(this.gameObject);
        }
        fire.SetActive(false);
        //StartCoroutine(Burning());
    }

    public void StartFire()
    {
        fire.SetActive(true);
        progressBar.GetComponent<SurvivorProgressBar>().duration = burnDuration;
        progressBar.SetActive(true);
        GOAPWorld.Instance.GetWorld().SetState(firepitTag, 1);
        burnStatus = 0;
        StartCoroutine(Burning());
    }

    private IEnumerator Burning()
    {
        yield return new WaitForSeconds(1f);
        burnStatus++;
        WarmUpAgents();
        if (burnStatus >= burnDuration)
        {
            GOAPWorldStates worldStates = GOAPWorld.Instance.GetWorld();
            worldStates.SetState(firepitTag, 0);

            fire.SetActive(false);
        }
        else StartCoroutine(Burning());
    }

    private void WarmUpAgents()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
        foreach(Collider collider in colliders)
        {
            GOAPAgent agent = collider.GetComponent<GOAPAgent>();
            if (agent != null)
            {
                if (agent.agentStates.HasState(warmthTag) && agent.agentStates.GetStateValue(warmthTag)<10) agent.agentStates.ModifyState(warmthTag, 2);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(transform.position, 5);

    }
}
