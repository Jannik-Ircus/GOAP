using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="Action", menuName ="GOAP/Action")]
public class GOAPAction : ScriptableObject
{
    public string actionName;
    public float cost;
    [HideInInspector]
    public GameObject goal;
    
    public GOAPWorldState[] preConditions;
    public GOAPWorldState[] afterEffects;

    
    //public MonoScript monoScript;

    public GOAPActionClass action;
    [SerializeField, HideInInspector] public string selectedActionTypeName;

    public GOAPAction(string actionName, float cost, GameObject goal, GOAPWorldState[] preConditions, GOAPWorldState[] afterEffects, GOAPActionClass action, string selectedActionTypeName)
    {
        this.actionName = actionName;
        this.cost = cost;
        this.goal = goal;
        this.preConditions = preConditions;
        this.afterEffects = afterEffects;
        this.action = action;
        this.selectedActionTypeName = selectedActionTypeName;
    }

    public GOAPActionClass GetGOAPActionClassFromCustom()
    {
        if(string.IsNullOrEmpty(selectedActionTypeName))
        {
            Debug.LogError("No Action Type selected");
            return null;
        }
        Type selectedActionType = GetTypeByName(selectedActionTypeName);
        if (selectedActionType == null)
        {
            Debug.LogError("Invalid Action Type selected");
            return null;
        }

        GOAPActionClass actionClass = (GOAPActionClass)Activator.CreateInstance(selectedActionType);
        return actionClass;
    }

    public bool IsRunning()
    {
        GOAPActionClass actionClass = GetGOAPActionClassFromCustom();
        return actionClass.isRunning;
    }

    public bool IsAchievable()
    {
        GOAPActionClass actionClass = GetGOAPActionClassFromCustom();
        return actionClass.IsAchievable();
    }

    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach(GOAPWorldState state in preConditions)
        {
            if (!conditions.ContainsKey(state.key)) return false;
        }
        return true;
    }

    public void AbortAction(GOAPAgent agent)
    {
        GOAPActionClass actionClass = GetGOAPActionClassFromCustom();
        actionClass.AbortAction(agent);
    }

    /*public GOAPActionClass GetGOAPActionClassScript()
    {
        System.Type classType = monoScript.GetClass();

        if (classType != null)
        {
            // Check if the type is assignable from your target class (optional)
            if (typeof(GOAPActionClass).IsAssignableFrom(classType))
            {
                // Create an instance of the type using Activator
                return (GOAPActionClass)System.Activator.CreateInstance(classType);
            }
            else
            {
                Debug.LogError("The selected script does not inherit from GOAPActionClass");
                return null;
            }
        }
        else
        {
            Debug.LogError("MonoScript does not contain a valid class.");
            return null;
        }
    }*/

    private Type GetTypeByName(string typeName)
    {
        // Load all assemblies and find the type by name
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var type = assembly.GetType(typeName);
            if (type != null)
            {
                return type;
            }
        }
        return null;
    }
}
