#if UNITY_EDITOR

using System;
using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Shared.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.EditorMode.ComponentConnection
{
    [CustomEditor(typeof(ScaffoldingElement))]
    public class Lol : Editor
    {
        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            

            if (GUILayout.Button("adads") && target is ScaffoldingElement se)
            {
                ScaffoldingComponent sc = se.gameObject.AddComponent<ScaffoldingComponent>();

                sc.elementWeight = se.elementWeight;
                sc.elementHeightInMillimeters = se.elementHeightInMillimeters;
                sc.elementLengthInMillimeters = se.elementLengthInMillimeters;
                sc.elementWidthInMillimeters = se.elementWidthInMillimeters;
                
                DestroyImmediate(sc.GetComponent<ScaffoldingElement>(), true);

                AssetDatabase.SaveAssetIfDirty(target);
            }
        }
    }

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

#endif