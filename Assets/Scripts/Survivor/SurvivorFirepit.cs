using System.Collections;
using UnityEngine;

public class SurvivorFirepit : MonoBehaviour
{
    public int burnDuration = 3;
    private int burnStatus = 0;
    public GameObject fire;
    private string firepitTag = "firepit";

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
        GOAPWorld.Instance.GetWorld().SetState(firepitTag, 1);
        burnStatus = 0;
        StartCoroutine(Burning());
    }

    private IEnumerator Burning()
    {
        yield return new WaitForSeconds(1f);
        burnStatus++;
        if (burnStatus >= burnDuration)
        {
            GOAPWorldStates worldStates = GOAPWorld.Instance.GetWorld();
            worldStates.SetState(firepitTag, 0);

            fire.SetActive(false);
        }
        else StartCoroutine(Burning());
    }
}
