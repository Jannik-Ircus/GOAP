using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GOAPAgentDebugAction : MonoBehaviour
{
    public GOAPAgent agent;
    public TextMeshProUGUI text;
    private void Update()
    {
        if(agent == null || text == null)
        {
            Debug.LogError("No agent or text found");
            Destroy(this);
        }
        if (agent.GetCurrentAction() != null) text.text = agent.GetCurrentAction().actionName;
        else text.text = "no current action";
    }
}
