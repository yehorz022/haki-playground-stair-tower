using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.RunMode.ScaffoldingAssemblies;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScaffoldingAssemblyAdder))]
public class ScaffoldingAssemblyAdderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ScaffoldingAssemblyAdder adder = target as ScaffoldingAssemblyAdder;

        if (adder == null)
        {
            Debug.LogError("Adder is null!!!");
            return;
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("add"))
        {
            adder.Add();
        }

        if (GUILayout.Button("remove"))
        {
            adder.Remove();
        }
        GUILayout.EndHorizontal();
    }
}
