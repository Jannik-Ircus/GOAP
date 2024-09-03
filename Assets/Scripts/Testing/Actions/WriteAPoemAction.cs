using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteAPoemAction : GOAPActionClass
{

    private string[] poem =
    {
        "I saw new Worlds beneath the Water ly,",
        "New Peeple; yea, another Sky",
        "And Sun, which seen by Day",
        "Might things more clear display.",
        "Just such another",
        "Of late my Brother",
        "Did in his Travel see, & saw by Night,",
        "A much more strange & wondrous Sight:",
        "Nor could the World exhibit such another,",
        "So Great a Sight, but in a Brother."

    };
    public override void AbortAction(GOAPAgent agent)
    {
        isRunning = false;
    }

    public override bool IsAchievable()
    {
        return true;
    }

    public override IEnumerator PerformAction(GOAPAgent agent, GameObject goal)
    {
        isRunning = true;
        foreach(string line in poem)
        {
            yield return new WaitForSeconds(1);
            if (!isRunning) break;
            Debug.Log(line);
        }
        isRunning = false;
    }

    public override void PostPerform()
    {
        
    }

    public override void PrePerform()
    {
        
    }
}
