using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorEnemyBTUpdater : MonoBehaviour
{
    public Animator animator;
    private int attackHash;
    [Range(0.1f, 5)]
    public float agroRate = 3;
    [Range(1f, 15)]
    public int calmDownRate = 5;

    [SerializeField]
    private int agroValue = 0;


    public void Start()
    {
        attackHash = Animator.StringToHash("Attack");
        agroValue = 0;
        StartCoroutine(GetAgro());
    }

    public void AttackBT(SurvivorAgentUpdaterBT agent)
    {
        if (agent == null)
        {
            Debug.LogError("No agent found while attacking with: " + gameObject.name);
            return;
        }
        PlayAttackAnimation();

        agent.health -= 1;
        if (agent.health <= 0)
        {
            Debug.Log(agent.gameObject.name + " has no more health and was killed");
            Destroy(agent.gameObject);
        }

        agroValue -= 3;
    }

    private void PlayAttackAnimation()
    {
        animator.SetTrigger(attackHash);
    }

    private IEnumerator GetAgro()
    {
        if (agroValue < 10)
        {
            agroValue++;
        }
        yield return new WaitForSeconds(agroRate);

        StartCoroutine(GetAgro());
    }

    public void StartHunt()
    {
        StartCoroutine(CalmDown());
    }

    private IEnumerator CalmDown()
    {
        yield return new WaitForSeconds(calmDownRate);

        if (agroValue < 10)
        {
            agroValue = 0;
        }
    }

    public int GetAgroValue()
    {
        return agroValue;
    }
}
