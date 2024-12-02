using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAgentButton : MonoBehaviour
{
    public GameStarter gameStarter;
    public GameObject agentToSpawn;
    public TMP_InputField inputField;
    private int numberOfAgents;

    private void Start()
    {
        if(gameStarter == null || agentToSpawn == null ||inputField == null)
        {
            Debug.LogError("Missing references on " + gameObject.name);
            Destroy(this);
        }

        
        if(int.TryParse(inputField.text, out numberOfAgents))
        {
            gameStarter.AddToList(agentToSpawn, numberOfAgents);
        }else
        {
            Debug.LogError("No number found in " + inputField.name);
        }
        
    }

    public void ModifyValue()
    {
        if (int.TryParse(inputField.text, out numberOfAgents))
        {
            gameStarter.ModifyNumber(agentToSpawn, numberOfAgents);
        }
        else
        {
            Debug.LogError("No number found in " + inputField.name);
        }
    }
}
