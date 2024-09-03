using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWorldText : MonoBehaviour
{
    public TextMeshProUGUI states;

    private void LateUpdate()
    {
        Dictionary<string, int> worldstates = GOAPWorld.Instance.GetWorld().GetStates();
        states.text = "";
        foreach (KeyValuePair<string, int> s in worldstates)
        {
            states.text += s.Key + ", " + s.Value;
        }
    }
}
