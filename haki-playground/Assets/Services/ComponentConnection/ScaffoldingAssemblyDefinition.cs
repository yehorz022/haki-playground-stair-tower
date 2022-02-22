using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Services.ComponentConnection
{
    [CreateAssetMenu(fileName = nameof(ScaffoldingAssemblyDefinition), menuName = "ScaffoldingComponent/Connection/Scaffolding Assembly", order = 0)]
    public class ScaffoldingAssemblyDefinition : ScriptableObject
    {
        public List<AssemblyComponentLocalizationInfo> AssemblyComponents;
    }


    public class AssemblyComponentLocalizationInfo
    {
        public ScaffoldingComponent ScaffoldingComponent { get; set; }
        public int OutputConnectionIndex { get; set; }
        public int InputConnectionIndex { get; set; }
    }
    [CustomEditor(typeof(ScaffoldingAssemblyDefinition))]
    public class ScaffoldingAssemblyDefinitionEditor : Editor
    {

        
        public override void OnInspectorGUI()
        {
            ScaffoldingAssemblyDefinition scaffoldingAssembly = target as ScaffoldingAssemblyDefinition;

            if (scaffoldingAssembly != null && scaffoldingAssembly.AssemblyComponents != null)
            {
                for (int i = 0; i < scaffoldingAssembly.AssemblyComponents.Count; i++)
                {
                    AssemblyComponentLocalizationInfo item = scaffoldingAssembly.AssemblyComponents[i] ?? new AssemblyComponentLocalizationInfo();


                    if (GUILayout.Button("Remove"))
                        scaffoldingAssembly.AssemblyComponents.RemoveAt(i);

                    EditorGUILayout.BeginHorizontal();

                    item.ScaffoldingComponent = EditorGUILayout.ObjectField(item.ScaffoldingComponent, typeof(ScaffoldingComponent)) as ScaffoldingComponent;


                    if (item.ScaffoldingComponent != null)
                    {
                        EditorGUILayout.BeginVertical();
                        item.InputConnectionIndex = EditorGUILayout.Popup("Input Index", item.InputConnectionIndex, Create(item.ScaffoldingComponent.ConnectionDefinitionCollection.Count));
                        item.OutputConnectionIndex = EditorGUILayout.Popup("Output Index", item.OutputConnectionIndex, Create(item.ScaffoldingComponent.ConnectionDefinitionCollection.Count));
                        EditorGUILayout.EndVertical();
                    }

                    scaffoldingAssembly.AssemblyComponents[i] = item;


                    EditorGUILayout.EndHorizontal();
                }
            }

            if (GUILayout.Button("Add"))
            {
                if (scaffoldingAssembly != null)
                {

                    if (scaffoldingAssembly.AssemblyComponents == null)
                        scaffoldingAssembly.AssemblyComponents = new List<AssemblyComponentLocalizationInfo>();
                    scaffoldingAssembly.AssemblyComponents.Add(new AssemblyComponentLocalizationInfo());
                }

            }

            AssetDatabase.SaveAssetIfDirty(target);
            base.OnInspectorGUI();
        }

        static string[] Create(int count)
        {
            string[] res = new string[count];

            for (int i = 0; i < count; i++)
            {
                res[i] = i.ToString();
            }

            return res;
        }
    }

}