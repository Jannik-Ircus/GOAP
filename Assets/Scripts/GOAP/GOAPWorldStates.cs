using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GOAPWorldState
{
    public string key;
    public int value;
}

public class GOAPWorldStates
{
    public Dictionary<string, int> states;

    public GOAPWorldStates()
    {
        states = new Dictionary<string, int>();
    }

    public bool HasState(string key)
    {
        return states.ContainsKey(key);
    }

    public void AddState(string key, int value)
    {
        if (states.ContainsKey(key)) return;
        states.Add(key, value);
    }

    public void ModifyState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] += value;
        }
        else states.Add(key, value);

    }

    public void RemoveState(string key)
    {
        states.Remove(key);
    }

    public void SetState(string key, int value)
    {
        if (states.ContainsKey(key)) states[key] = value;
        else states.Add(key, value);
    }

    public Dictionary<string, int> GetStates()
    {
        return states;
    }

    public int GetStateValue(string key)
    {
        foreach(KeyValuePair<string, int> k in states)
        {
            if (k.Key == key) return k.Value;
        }
        Debug.LogWarning("no value found for: " + key);
        return -1;
    }

}
