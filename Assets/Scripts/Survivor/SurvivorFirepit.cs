using System.Collections;
using UnityEngine;

public class SurvivorFirepit : MonoBehaviour
{
    public int burnDuration = 3;
    private int burnStatus = 0;
    [HideInInspector]
    private void Start()
    {
        GOAPWorld.Instance.GetWorld().ModifyState("firepit", 1);
        StartCoroutine(Burning());
    }

    private IEnumerator Burning()
    {
        yield return new WaitForSeconds(1);
        burnStatus++;
        if (burnStatus >= burnDuration)
        {
            GOAPWorldStates worldStates = GOAPWorld.Instance.GetWorld();
            if(worldStates.HasState("firepit"))
            {
                worldStates.ModifyState("firepit", -1);
                if (worldStates.GetStates()["firepit"] <= 0) worldStates.RemoveState("firepit");
            }
            Destroy(this.gameObject);
        }
        else StartCoroutine(Burning());
    }
}
