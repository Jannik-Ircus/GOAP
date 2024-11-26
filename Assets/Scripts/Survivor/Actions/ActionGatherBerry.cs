using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGatherBerry : ActionGatherWood
{
    protected override string resource { get; set; } = "Berry";

    /*public override void PostPerform(GOAPAgent agent)
    {
        if (nearestWood == null) return;
        nearestWood.GetComponent<SurvivorBerry>().PickUpBerry(agent);
    }*/
}
