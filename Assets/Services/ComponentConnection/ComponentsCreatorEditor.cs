#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Services.ComponentConnection
{

    [CustomEditor(typeof(ConnectionType))]
    public class ConnectionTypeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var main = target as ConnectionType;
            GUILayout.Label($"ID: {main.Id}");

            if (GUILayout.Button("Reset Id"))
            {
                main.Id = Guid.NewGuid();
            }

            

        }
    }

    [CustomEditor(typeof(ComponentCreator))]
    public class ComponentsCreatorEditor : Editor
    {


        ConnectionDefinition Create(Vector3 pos, string tName, string name, Func<Vector3, Vector3> lookAtImplementation)
        {

            ConnectionDefinition cd = CreateInstance<ConnectionDefinition>();
            cd.name = tName + name + ".asset";
            cd.localPosition = pos;
            cd.lookAt = lookAtImplementation(pos);


            return cd;
        }
        public override void OnInspectorGUI()
        {

            ComponentCreator main = this.target as ComponentCreator;


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
                Vector3[] points = new Vector3[main.connectors.childCount];
                for (int i = 0; i < main.connectors.childCount; i++)
                {
                    Transform con = main.connectors.GetChild(i);

                    points[i] = con.position;
                }
                foreach (Vector3 point in points)
                {
                    Debug.DrawLine(main.transform.position, point, Color.yellow, 5);
                }
                Vector3 avg = Vector3.zero;
                Debug.Log(avg);

                foreach (Vector3 point in points)
                {
                    avg = avg + point;
                }

                Debug.Log(avg);
                avg = (avg - main.transform.position) / points.Length;
                Debug.Log(avg);

                main.transform.GetChild(0).localPosition = avg * -1;

                for (int i = 0; i < main.connectors.childCount; i++)
                {
                    Transform con = main.connectors.GetChild(i);

                    points[i] = con.position;
                }
                foreach (Vector3 point in points)
                {
                    Debug.DrawLine(main.transform.position, point, Color.red, 5);
                }
            }

            GUILayout.EndHorizontal();

            ComponentConnectionService service = main.GetComponent<ComponentConnectionService>();

            string[] arr = new string[service.connectionDefinitionCollection.Count];

            for (int i = 0; i < service.connectionDefinitionCollection.Count; i++)
            {
                arr[i] = service.connectionDefinitionCollection.GetElementAt(i).name;

            }

            var test = EditorGUILayout.Popup("Label", 0, arr);

            base.OnInspectorGUI();
        }

        private void MergeMeshes(ComponentCreator main)
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
                    var v = mf.transform.localRotation * vertex + mf.transform.localPosition;

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



                var v = vertices[i];

                v.y += mesh.bounds.size.y / 2;
                v.z += mesh.bounds.size.z / 2;

                vertices[i] = v;
            }

            var diff = (maxx - minx) / 2;

            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                var v = vertices[i];
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

            //AssetDatabase.SaveAssetIfDirty(main);
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

            ComponentConnectionService ccs = main.GetComponent<ComponentConnectionService>();
            ccs.connectionDefinitionCollection = cdc;

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
        Vector3 CreateLookAt(Vector3 pos)
        {
            pos.y = 0;
            pos.Normalize();


            return pos;
        }
        private bool HandleElement(ComponentCreator main, List<ConnectionDefinition> list)
        {

            if (main.componentType == ComponentType.Pillar)
            {


                for (int i = 0; i < main.connectors.childCount; i++)
                {
                    Transform t = main.connectors.GetChild(i).transform;
                    float y = main.transform.InverseTransformPoint(t.position).y;

                    Vector3 vf = new Vector3(.03f, y, 0);
                    Vector3 vb = new Vector3(-.03f, y, 0);

                    Vector3 vr = new Vector3(0, y, 0.03f);
                    Vector3 vl = new Vector3(0, y, -0.03f);

                    list.Add(Create(vf, t.name, "Front", CreateLookAt));
                    list.Add(Create(vb, t.name, "Back", CreateLookAt));
                    list.Add(Create(vr, t.name, "Right", CreateLookAt));
                    list.Add(Create(vl, t.name, "Left", CreateLookAt));
                }

                return true;
            }
            else if (main.componentType == ComponentType.Foot)
            {
                Vector3 vec = main.connectors.localPosition;

                list.Add(Create(vec, main.connectors.name, "Top", x => x + Vector3.up));
                return true;
            }
            else if (main.componentType == ComponentType.SideBarrier)
            {
                var other = main.GetComponent<ComponentConnectionService>();

                Vector3[] points = new Vector3[4];
                for (int i = 0; i < main.connectors.childCount; i++)
                {
                    Transform con = main.connectors.GetChild(i);

                    Debug.DrawLine(main.transform.position, con.position, Color.blue, 10);

                    points[i] = main.transform.InverseTransformPoint(con.position);
                }
                foreach (Vector3 point in points)
                {
                    Debug.DrawLine(main.transform.position, main.transform.position + point, Color.yellow, 5);
                    list.Add(Create(point, main.name, point.ToString(), CreateLookAt));
                }



            }

            return false;
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
            var cosineThreshold = Mathf.Cos(angle * Mathf.Deg2Rad);

            var vertices = mesh.vertices;
            var normals = new Vector3[vertices.Length];

            // Holds the normal of each triangle in each sub mesh.
            var triNormals = new Vector3[mesh.subMeshCount][];

            var dictionary = new Dictionary<VertexKey, List<VertexEntry>>(vertices.Length);

            for (var subMeshIndex = 0; subMeshIndex < mesh.subMeshCount; ++subMeshIndex)
            {

                var triangles = mesh.GetTriangles(subMeshIndex);

                triNormals[subMeshIndex] = new Vector3[triangles.Length / 3];

                for (var i = 0; i < triangles.Length; i += 3)
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

            foreach (var vertList in dictionary.Values)
            {
                for (var i = 0; i < vertList.Count; ++i)
                {

                    var sum = new Vector3();
                    var lhsEntry = vertList[i];

                    for (var j = 0; j < vertList.Count; ++j)
                    {
                        var rhsEntry = vertList[j];

                        if (lhsEntry.VertexIndex == rhsEntry.VertexIndex)
                        {
                            sum += triNormals[rhsEntry.MeshIndex][rhsEntry.TriangleIndex];
                        }
                        else
                        {
                            // The dot product is the cosine of the angle between the two triangles.
                            // A larger cosine means a smaller angle.
                            var dot = Vector3.Dot(
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
                var key = (VertexKey)obj;
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