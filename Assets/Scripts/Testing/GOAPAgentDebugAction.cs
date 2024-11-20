using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GOAPAgentDebugAction : MonoBehaviour
{
    public GOAPAgent agent;
    public GameObject canvas;
    public TextMeshProUGUI text;
    public TextMeshProUGUI stateText;
    private void Update()
    {
        canvas.transform.rotation = Quaternion.Euler(90, -agent.gameObject.transform.rotation.y, 0);
        if (agent == null || text == null || stateText == null)
        {
            Debug.LogError("No agent or text found");
            Destroy(this);
        }
        if (agent.GetCurrentAction() != null) text.text = agent.GetCurrentAction().actionName;
        else text.text = "no current action";

        if (agent.agentStates.states.Count <= 0) stateText.text = "-";
        else
        {
            string displayText = "States: \n";
            foreach(KeyValuePair<string, int> state in agent.agentStates.states)
            {
                displayText += state.Key + ": " + state.Value + "   \n";
            }
            stateText.text = displayText;
        }
    }
}
