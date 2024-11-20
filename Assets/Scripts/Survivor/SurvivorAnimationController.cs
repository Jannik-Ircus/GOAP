using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorAnimationController : MonoBehaviour
{
    public Animator animator;
    private int choppingHash;

    private void Start()
    {
        choppingHash = Animator.StringToHash("Chopping");
    }

    public void SetChoppingAnimation(bool value)
    {
        animator.SetBool(choppingHash, value);
    }
}
