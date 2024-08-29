using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="Action", menuName ="GOAP/Action")]
public class GOAPAction : ScriptableObject
{
    public string actionName;
    public float cost;
    
    public GOAPWorldState[] preConditions;
    public GOAPWorldState[] afterEffects;

    
    public MonoScript monoScript;
    
    public GOAPActionClass GetGOAPActionClassScript()
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
    }
}
