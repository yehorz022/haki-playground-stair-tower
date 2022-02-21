using System;

#if UNITY_EDITOR


using UnityEditor;
using UnityEngine;

namespace Assets.Services.ComponentConnection
{
    [CustomEditor(typeof(ConnectionInfo))]
    public class ConnectionTypeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ConnectionInfo main = target as ConnectionInfo;
            GUILayout.Label($"ID: {main.Id}");

            if (GUILayout.Button("Reset Id"))
            {
                main.Id = Guid.NewGuid();
            }

        }
    }
}
#endif