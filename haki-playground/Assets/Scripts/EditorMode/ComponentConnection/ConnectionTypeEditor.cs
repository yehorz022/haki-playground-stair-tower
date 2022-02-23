using System;
using Assets.Scripts.Shared.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.EditorMode.ComponentConnection
{
    [CustomEditor(typeof(ComponentConnectionInfo))]
    public class ConnectionTypeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ComponentConnectionInfo main = target as ComponentConnectionInfo;
            GUILayout.Label($"ID: {main.Id}");

            if (GUILayout.Button("Reset Id"))
            {
                main.Id = Guid.NewGuid();
            }

        }
    }
}