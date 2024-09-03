using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void Start()
    {
        GOAPWorld.Instance.GetWorld().ModifyState("goal", 1);
    }
}
