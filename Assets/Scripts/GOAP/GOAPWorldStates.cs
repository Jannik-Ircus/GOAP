using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
