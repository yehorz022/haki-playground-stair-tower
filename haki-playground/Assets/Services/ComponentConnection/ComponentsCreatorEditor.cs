#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Services.ComponentConnection
{
    [CustomEditor(typeof(ComponentCreator))]
    public class ComponentsCreatorEditor : Editor
    {


        ConnectionDefinition Create(Vector3 pos, string tName, Orientation orientation)
        {

            ConnectionDefinition cd = CreateInstance<ConnectionDefinition>();
            cd.name = tName + orientation + ".asset";
            cd.localPosition = pos;
            cd.lookAt = CreateLookAt(orientation);

            cd.ConnectionInfo = AssetDatabase.LoadAssetAtPath<ConnectionInfo>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("PillarTop").First()));
            return cd;
        }

        private UnityEngine.Object top;
        private Vector2 scroll;
        public override void OnInspectorGUI()
        {

            ComponentCreator main = this.target as ComponentCreator;
            Assert.IsNotNull(main);
            main.HandleCount();


            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Create Connection"))
            {
                CreateConnections(main);
            }

            if (GUILayout.Button("Merge Meshes"))
            {
                MergeMeshes(main);
            }

            if (GUILayout.Button("Center"))
            {

                for (int i = 0; i < main.connectors.Length; i++)
                {

                    if (main.connectors[i] != null)
                        Center(main.connectors[i]);
                }
            }

            GUILayout.EndHorizontal();
            base.OnInspectorGUI();



            scroll = GUILayout.BeginScrollView(scroll, false, true);

            for (int i = 0; i < main.Count; i++)
            {

                GUILayout.BeginHorizontal();

                Transform connector = main.connectors[i];
                ConnectionTypeA ct = main.componentType[i];

                main.connectors[i] = EditorGUILayout.ObjectField("Connector", connector, typeof(Transform), main) as Transform;
                main.componentType[i] = (ConnectionTypeA)EditorGUILayout.EnumPopup(ct);

                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();

        }

        private static void Center(Transform connectors)
        {

            Vector3[] points = new Vector3[connectors.childCount];
            for (int i = 0; i < connectors.childCount; i++)
            {
                Transform con = connectors.GetChild(i);

                points[i] = con.position;
            }

            foreach (Vector3 point in points)
            {
                Debug.DrawLine(connectors.parent.position, point, Color.yellow, 5);
            }

            Vector3 avg = Vector3.zero;
            Debug.Log(avg);

            foreach (Vector3 point in points)
            {
                avg = avg + point;
            }

            Debug.Log(avg);
            avg = (avg - connectors.parent.position) / points.Length;
            Debug.Log(avg);

            connectors.parent.GetChild(0).localPosition = avg * -1;

            for (int i = 0; i < connectors.childCount; i++)
            {
                Transform con = connectors.GetChild(i);

                points[i] = con.position;
            }

            foreach (Vector3 point in points)
            {
                Debug.DrawLine(connectors.parent.position, point, Color.red, 5);
            }
        }

        private static void MergeMeshes(ComponentCreator main)
        {
            MeshFilter current = main.GetComponent<MeshFilter>();
            current.sharedMesh = null;
            MeshFilter[] mfs = main.GetComponentsInChildren<MeshFilter>(true);

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector4> tangents = new List<Vector4>();
            List<Vector2> uvs = new List<Vector2>();



            for (int i = 0; i < mfs.Length; i++)
            {
                int vCount = vertices.Count;

                MeshFilter mf = mfs[i];



                if (mf.sharedMesh == null)
                    continue;

                if (mf.name.StartsWith("Fjädersprint wire"))
                {
                    mf.gameObject.SetActive(true);
                    continue;
                }
                else mf.gameObject.SetActive(false);

                normals.AddRange(mf.sharedMesh.normals);
                tangents.AddRange(mf.sharedMesh.tangents);
                uvs.AddRange(mf.sharedMesh.uv);


                foreach (Vector3 vertex in mf.sharedMesh.vertices)
                {
                    vertex.Scale(mf.transform.localScale);
                    Vector3 v = mf.transform.localRotation * vertex + mf.transform.localPosition;

                    vertices.Add(v);
                }

                foreach (int triangle in mf.sharedMesh.triangles)
                {
                    triangles.Add(vCount + triangle);
                }
            }




            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.RecalculateBounds();


            float minx = float.MaxValue;
            float maxx = float.MinValue;

            for (int i = 0; i < mesh.vertices.Length; i++)
            {



                Vector3 v = vertices[i];

                v.y += mesh.bounds.size.y / 2;
                v.z += mesh.bounds.size.z / 2;

                vertices[i] = v;
            }

            float diff = (maxx - minx) / 2;

            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                Vector3 v = vertices[i];
                v.x -= .016f;
                vertices[i] = v;
            }

            Debug.Log($"MIN: {minx} MAX: {maxx}");


            mesh.vertices = vertices.ToArray();
            mesh.normals = normals.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.tangents = tangents.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.RecalculateTangents();
            mesh.RecalculateNormals(60);
            mesh.RecalculateBounds();





            current.sharedMesh = mesh;

            string fullPath = CreateFolderIfNoExist(main);

            AssetDatabase.CreateAsset(mesh, $"{fullPath}/{main.name}.mesh");

            AssetDatabase.SaveAssetIfDirty(main);
        }

        private void CreateConnections(ComponentCreator main)
        {
            string fullPath = CreateFolderIfNoExist(main);

            List<ConnectionDefinition> list = new List<ConnectionDefinition>();

            if (HandleElement(main, list) == false)
                return;

            foreach (ConnectionDefinition definition in list)
            {
                AssetDatabase.CreateAsset(definition, $"{fullPath}/{definition.name}");
            }


            ConnectionDefinitionCollection cdc = ScriptableObject.CreateInstance<ConnectionDefinitionCollection>();
            cdc.name = main.name + $" {nameof(ConnectionDefinitionCollection)}.asset";

            cdc.connectionDefinitions = list.ToArray();
            AssetDatabase.CreateAsset(cdc, $"{fullPath}/{cdc.name}");

            ScaffoldingComponent ccs = main.GetComponent<ScaffoldingComponent>();
            ccs.ConnectionDefinitionCollection = cdc;

            AssetDatabase.SaveAssets();
        }

        private static string CreateFolderIfNoExist(ComponentCreator main)
        {
            string path = "Assets/Services/ComponentConnection/Components";

            string fullPath = path + "/" + main.name;


            if (AssetDatabase.IsValidFolder(fullPath) == false)
            {
                AssetDatabase.CreateFolder(path, main.name);
            }

            return fullPath;
        }
        private static Vector3 CreateLookAt(object obj)
        {

            switch (obj)
            {
                case null:
                    throw new Exception(
                        $"parameter {nameof(obj)} in call {nameof(CreateLookAt)} at {Environment.StackTrace} is null");
                case Orientation orientation:
                    switch (orientation)
                    {
                        case Orientation.Forward: return Vector3.forward;
                        case Orientation.Backward: return Vector3.back;
                        case Orientation.RightWards: return Vector3.right;
                        case Orientation.Leftwards: return Vector3.left;
                        case Orientation.Upwards: return Vector3.up;
                        case Orientation.Downwards: return Vector3.down;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                case Vector3 vec:
                    vec.y = 0;
                    vec.Normalize();
                    return vec;
            }

            throw new Exception($"Unexpected type parameters in {nameof(CreateLookAt)}: {obj.GetType().FullName}");
        }
        private bool HandleElement(ComponentCreator main, List<ConnectionDefinition> list)
        {
            for (int i = 0; i < main.connectors.Length; i++)
            {
                if (HandleConnectionType(list, main.componentType[i], main.connectors[i], main) == false)
                {
                    list.Clear();
                    return false;
                }
            }

            return list.Count > 0;
        }

        private bool HandleConnectionType(ICollection<ConnectionDefinition> connectionDefinitions, ConnectionTypeA connectionTypeA, Transform connectors, ComponentCreator main)
        {
            switch (connectionTypeA)
            {
                case ConnectionTypeA.PillarSideFemale:
                    {
                        for (int i = 0; i < connectors.childCount; i++)
                        {
                            Transform t = connectors.GetChild(i).transform;
                            float y = connectors.parent.InverseTransformPoint(t.position).y;

                            Vector3 vf = new Vector3(0, y, .03f);
                            Vector3 vb = new Vector3(0, y, -.03f);

                            Vector3 vr = new Vector3(0.03f, y, 0);
                            Vector3 vl = new Vector3(-0.03f, y, 0);

                            connectionDefinitions.Add(Create(vf, t.name, Orientation.Forward));
                            connectionDefinitions.Add(Create(vb, t.name, Orientation.Backward));
                            connectionDefinitions.Add(Create(vr, t.name, Orientation.RightWards));
                            connectionDefinitions.Add(Create(vl, t.name, Orientation.Leftwards));
                        }

                        return true;
                    }

                case ConnectionTypeA.PillarSideMale:
                    {

                        Vector3 pos = main.reference.position;
                        Vector3 look = main.look.localPosition;

                        Vector3 connectorWorldSpace = pos + look;


                        var ttt = connectorWorldSpace - main.transform.position;

                        var point = main.transform.TransformPoint(ttt);


                        connectionDefinitions.Add(Create(point, main.name, Orientation.Forward));

                        return true;

                    }

                case ConnectionTypeA.PillarMainMale:
                    {
                        return ByLocalPosition(connectionDefinitions, connectors, Orientation.Upwards);
                    }
                case ConnectionTypeA.PillarMainFemale:
                    {
                        return ByLocalPosition(connectionDefinitions, connectors, Orientation.Downwards);

                    }
                default:
                    return false;
            }
        }

        private bool ByLocalPosition(ICollection<ConnectionDefinition> connectionDefinitions, Transform connectors, Orientation orientation)
        {
            Vector3 vec = connectors.localPosition;

            connectionDefinitions.Add(Create(vec, connectors.name, orientation));
            return true;
        }
    }




    public static class NormalSolver
    {
        /// <summary>
        ///     Recalculate the normals of a mesh based on an angle threshold. This takes
        ///     into account distinct vertices that have the same position.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="angle">
        ///     The smoothing angle. Note that triangles that already share
        ///     the same vertex will be smooth regardless of the angle! 
        /// </param>
        public static void RecalculateNormals(this Mesh mesh, float angle)
        {
            float cosineThreshold = Mathf.Cos(angle * Mathf.Deg2Rad);

            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = new Vector3[vertices.Length];

            // Holds the normal of each triangle in each sub mesh.
            Vector3[][] triNormals = new Vector3[mesh.subMeshCount][];

            Dictionary<VertexKey, List<VertexEntry>> dictionary = new Dictionary<VertexKey, List<VertexEntry>>(vertices.Length);

            for (int subMeshIndex = 0; subMeshIndex < mesh.subMeshCount; ++subMeshIndex)
            {

                int[] triangles = mesh.GetTriangles(subMeshIndex);

                triNormals[subMeshIndex] = new Vector3[triangles.Length / 3];

                for (int i = 0; i < triangles.Length; i += 3)
                {
                    int i1 = triangles[i];
                    int i2 = triangles[i + 1];
                    int i3 = triangles[i + 2];

                    // Calculate the normal of the triangle
                    Vector3 p1 = vertices[i2] - vertices[i1];
                    Vector3 p2 = vertices[i3] - vertices[i1];
                    Vector3 normal = Vector3.Cross(p1, p2).normalized;
                    int triIndex = i / 3;
                    triNormals[subMeshIndex][triIndex] = normal;

                    List<VertexEntry> entry;
                    VertexKey key;

                    if (!dictionary.TryGetValue(key = new VertexKey(vertices[i1]), out entry))
                    {
                        entry = new List<VertexEntry>(4);
                        dictionary.Add(key, entry);
                    }
                    entry.Add(new VertexEntry(subMeshIndex, triIndex, i1));

                    if (!dictionary.TryGetValue(key = new VertexKey(vertices[i2]), out entry))
                    {
                        entry = new List<VertexEntry>();
                        dictionary.Add(key, entry);
                    }
                    entry.Add(new VertexEntry(subMeshIndex, triIndex, i2));

                    if (!dictionary.TryGetValue(key = new VertexKey(vertices[i3]), out entry))
                    {
                        entry = new List<VertexEntry>();
                        dictionary.Add(key, entry);
                    }
                    entry.Add(new VertexEntry(subMeshIndex, triIndex, i3));
                }
            }

            // Each entry in the dictionary represents a unique vertex position.

            foreach (List<VertexEntry> vertList in dictionary.Values)
            {
                for (int i = 0; i < vertList.Count; ++i)
                {

                    Vector3 sum = new Vector3();
                    VertexEntry lhsEntry = vertList[i];

                    for (int j = 0; j < vertList.Count; ++j)
                    {
                        VertexEntry rhsEntry = vertList[j];

                        if (lhsEntry.VertexIndex == rhsEntry.VertexIndex)
                        {
                            sum += triNormals[rhsEntry.MeshIndex][rhsEntry.TriangleIndex];
                        }
                        else
                        {
                            // The dot product is the cosine of the angle between the two triangles.
                            // A larger cosine means a smaller angle.
                            float dot = Vector3.Dot(
                                triNormals[lhsEntry.MeshIndex][lhsEntry.TriangleIndex],
                                triNormals[rhsEntry.MeshIndex][rhsEntry.TriangleIndex]);
                            if (dot >= cosineThreshold)
                            {
                                sum += triNormals[rhsEntry.MeshIndex][rhsEntry.TriangleIndex];
                            }
                        }
                    }

                    normals[lhsEntry.VertexIndex] = sum.normalized;
                }
            }

            mesh.normals = normals;
        }

        private struct VertexKey
        {
            private readonly long _x;
            private readonly long _y;
            private readonly long _z;

            // Change this if you require a different precision.
            private const int Tolerance = 100000;

            // Magic FNV values. Do not change these.
            private const long FNV32Init = 0x811c9dc5;
            private const long FNV32Prime = 0x01000193;

            public VertexKey(Vector3 position)
            {
                _x = (long)(Mathf.Round(position.x * Tolerance));
                _y = (long)(Mathf.Round(position.y * Tolerance));
                _z = (long)(Mathf.Round(position.z * Tolerance));
            }

            public override bool Equals(object obj)
            {
                VertexKey key = (VertexKey)obj;
                return _x == key._x && _y == key._y && _z == key._z;
            }

            public override int GetHashCode()
            {
                long rv = FNV32Init;
                rv ^= _x;
                rv *= FNV32Prime;
                rv ^= _y;
                rv *= FNV32Prime;
                rv ^= _z;
                rv *= FNV32Prime;

                return rv.GetHashCode();
            }
        }

        private struct VertexEntry
        {
            public int MeshIndex;
            public int TriangleIndex;
            public int VertexIndex;

            public VertexEntry(int meshIndex, int triIndex, int vertIndex)
            {
                MeshIndex = meshIndex;
                TriangleIndex = triIndex;
                VertexIndex = vertIndex;
            }
        }
    }
}

#endif