using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

[CustomEditor(typeof(GOAPAction))]
public class ActionConfigEditor : Editor
{
    private string[] typeNames;
    private Type[] types;

    private void OnEnable()
    {
        // Populate the types
        PopulateTypes();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw the default properties
        DrawDefaultInspector();

        // Cast the target to ActionConfig
        var config = (GOAPAction)target;

        // Display the custom dropdown for selecting the Action type
        int selectedIndex = Array.IndexOf(typeNames, config.selectedActionTypeName ?? string.Empty);

        selectedIndex = EditorGUILayout.Popup("Select Action Type", selectedIndex, typeNames);

        if (selectedIndex >= 0 && selectedIndex < types.Length)
        {
            config.selectedActionTypeName = types[selectedIndex].FullName;
        }

        // Apply any changes to the serializedObject
        serializedObject.ApplyModifiedProperties();
    }

    private void PopulateTypes()
    {
        // Retrieve all types in the current assembly that are derived from Action
        types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(GOAPActionClass)) && !t.IsAbstract)
            .ToArray();

        typeNames = types.Select(t => t.FullName).ToArray();
    }
}