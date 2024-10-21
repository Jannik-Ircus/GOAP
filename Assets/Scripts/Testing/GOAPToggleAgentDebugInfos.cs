using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPToggleAgentDebugInfos : MonoBehaviour
{
    public GameObject agents;
    private List<GameObject> agentDebugCanvases = new List<GameObject>();

    private void Start()
    {
        foreach(Transform agent in agents.transform)
        {
            Transform canvas = agent.GetChild(0);
            if (canvas.gameObject.GetComponent<GOAPAgentDebugAction>() != null) agentDebugCanvases.Add(canvas.gameObject);
            
        }
    }

    public void ToggleInformations(bool state)
    {
        foreach(GameObject canvas in agentDebugCanvases)
        {
            canvas.SetActive(state);
        }
    }
}
