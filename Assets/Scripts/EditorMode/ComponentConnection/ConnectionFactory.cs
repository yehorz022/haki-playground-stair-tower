using System.Collections.Generic;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Constants;
using Assets.Scripts.Shared.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.EditorMode.ComponentConnection
{
    [CustomEditor(typeof(ConnectionFactoryHolder))]
    public class ConnectionFactory : Editor
    {
        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            if (GUILayout.Button("Run"))
            {
                ConnectionFactoryHolder holder = (ConnectionFactoryHolder)target;

                foreach (HakiComponent component in holder.components)
                {
                    var box = component.transform.GetComponent<BoxCollider>();
                    if (component.name.StartsWith("7016"))
                    {

                        Debug.Log(box == null);
                        float size = box.size.y;

                        if (component.TryGetCollectionDefinition(out var def) == false)
                        {
                            List<ConnectionDefinition> definitions = new List<ConnectionDefinition>();

                            void Create(float y, Vector3 normal)
                            {
                                ConnectionDefinition cd = new ConnectionDefinition();
                                cd.name = component.name + definitions.Count;

                                cd.localPosition = Vector3.up * y + normal * .03f;
                                cd.lookAt = normal;
                                definitions.Add(cd);

                                y -= Constants.MilimitersToUnityFactor * 130;
                                ConnectionDefinition cd2 = new ConnectionDefinition();
                                cd2.name = component.name + definitions.Count;

                                cd2.localPosition = Vector3.up * y + normal * .03f;
                                cd2.lookAt = normal;
                                definitions.Add(cd2);
                            }



                            ConnectionDefinition cd1 = new ConnectionDefinition();
                            cd1.localPosition = Vector3.zero;
                            cd1.name = component.name + "bottom";
                            cd1.lookAt = Vector3.down;
                            definitions.Add(cd1);

                            ConnectionDefinition cd2 = new ConnectionDefinition();
                            cd2.localPosition = Vector3.up * size;
                            cd2.name = component.name + "top";
                            cd2.lookAt = Vector3.up;
                            definitions.Add(cd2);

                            var sizeOfIt = int.Parse(component.name.Replace("7016", ""));

                            int count = 0;
                            if (sizeOfIt == 50) count = 1;
                            if (sizeOfIt == 100) count = 2;
                            if (sizeOfIt == 150) count = 3;
                            if (sizeOfIt == 200) count = 4;

                            var a = (215 + holder.offsetForSpires) * Constants.MilimitersToUnityFactor;

                            for (int x = 0; x < count; x++)
                            {
                                Create(a, Vector3.forward);
                                Create(a, Vector3.left);
                                Create(a, Vector3.back);
                                Create(a, Vector3.right);

                                a += .5f;
                            }

                            Debug.Log(count);

                            ConnectionDefinitionCollection cdc = new ConnectionDefinitionCollection();
                            cdc.name = component.name + "cdc";
                            cdc.connectionDefinitions = definitions.ToArray();

                            foreach (ConnectionDefinition definition in definitions)
                            {
                                AssetDatabase.CreateAsset(definition, $"Assets/{definition.name}.asset");

                            }

                            AssetDatabase.CreateAsset(cdc, $"Assets/{cdc.name}.asset");
                        }

                    }
                    else if (component.name.StartsWith("7021"))
                    {
                        Debug.Log(box == null);

                        List<ConnectionDefinition> definitions = new List<ConnectionDefinition>();

                        void Create(float y, float x, Vector3 normal)
                        {
                            ConnectionDefinition cd = new ConnectionDefinition();
                            cd.name = component.name + definitions.Count;

                            cd.localPosition = Vector3.up * y + Vector3.left * x;
                            cd.lookAt = normal;
                            definitions.Add(cd);
                        }

                        var o = holder.ledgerBeamOffsetX * Constants.MilimitersToUnityFactor;

                        Create(holder.offsetForBeamsY * Constants.MilimitersToUnityFactor, holder.offsetForBeamsX * Constants.MilimitersToUnityFactor, Vector3.left);
                        Create(holder.offsetForBeamsY2 * Constants.MilimitersToUnityFactor, holder.offsetForBeamsX2 * Constants.MilimitersToUnityFactor, Vector3.left);

                        Create(holder.offsetForBeamsY * Constants.MilimitersToUnityFactor, -o + holder.offsetForBeamsX * Constants.MilimitersToUnityFactor, Vector3.right);
                        Create(holder.offsetForBeamsY2 * Constants.MilimitersToUnityFactor,-o + holder.offsetForBeamsX2 * Constants.MilimitersToUnityFactor, Vector3.right);

                        ConnectionDefinitionCollection cdc = new ConnectionDefinitionCollection();
                        cdc.name = component.name + "cdc";
                        cdc.connectionDefinitions = definitions.ToArray();


                        foreach (ConnectionDefinition definition in definitions)
                        {
                            AssetDatabase.CreateAsset(definition, $"Assets/{definition.name}.asset");

                        }

                        AssetDatabase.CreateAsset(cdc, $"Assets/{cdc.name}.asset");
                    }
                }
            }
        }

    }
}