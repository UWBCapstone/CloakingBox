using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox
{
    public class RoomCreator : MonoBehaviour
    {
        public TangoApplication TangoManager;

        public void Awake()
        {
            TangoManager = getTangoManager();
        }

        public static GameObject CreateBlankRoom()
        {
            GameObject roomGO = new GameObject();
            roomGO.SetActive(false);
            roomGO.layer = LayerManager.GetLayerMask(CloakLayers.Room);

            roomGO.name = retrieveRoomName();

            var mf = roomGO.AddComponent<MeshFilter>();
            var mr = roomGO.AddComponent<MeshRenderer>();
            mr.material = createRoomMaterial();

            var mc = roomGO.AddComponent<MeshCollider>();

            roomGO.SetActive(true);
            return roomGO;
        }

        public List<Mesh> GetUpdatedMeshes()
        {
            List<Mesh> updatedMeshes = new List<Mesh>();

            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Color32> colors = new List<Color32>();
            
            TangoManager.Tango3DRExtractWholeMesh(vertices, normals, colors, triangles);
            
            // Turn off mesh updating so that the phone doesn't explode
            GameObject.FindObjectOfType<Tango.TangoApplication>().m_tango3DReconstruction.SetEnabled(false);

            GUIDebug.Log("Turning off Tango 3D reconstruction");

            List<int> origIndices = new List<int>();
            for (int i = 0; i < vertices.Count; i++)
            {
                origIndices.Add(i);
            }

            while (vertices.Count > 0)
            {
                Mesh mesh = new Mesh();

                int maxVertices = 64000;
                if(vertices.Count > maxVertices)
                {
                    List<Vector3> meshVertices;
                    List<int> meshTriangles;
                    List<Color32> meshColors;

                    // Find all triangles that are related to the vertices wanted
                    SeparateForSubMesh(maxVertices, out meshVertices, out meshTriangles, out meshColors, origIndices, vertices, triangles, colors);

                    meshVertices = new List<Vector3>();
                    meshTriangles = new List<int>();
                    meshColors = new List<Color32>();

                    mesh.SetVertices(meshVertices);
                    mesh.SetTriangles(meshTriangles, 0);
                    mesh.SetColors(meshColors);
                    mesh.RecalculateNormals();
                }
                else
                {
                    mesh.SetVertices(vertices);
                    mesh.SetTriangles(triangles, 0);
                    mesh.SetColors(colors);
                    mesh.RecalculateNormals();

                    vertices.Clear();
                    triangles.Clear();
                    colors.Clear();
                    normals.Clear();
                }

                mesh.name = "TangoMesh_" + updatedMeshes.Count;
                updatedMeshes.Add(mesh);

                GUIDebug.Log("Mesh " + mesh.name + " created.");
            }

            return updatedMeshes;
        }

        /// <summary>
        /// Will remove triangles from triangleList
        /// </summary>
        /// <param name="verticesToUse"></param>
        /// <param name="triangleList"></param>
        /// <returns></returns>
        private void SeparateForSubMesh(int numVertices, out List<Vector3> meshVertices, out List<int> meshTriangles, out List<Color32> meshColors, List<int> origIndices, List<Vector3> vertices, List<int> triangles, List<Color32> colors)//List<Vector3> verticesToUse, List<Vector3> vertices, List<int> triangleList)
        {
            meshTriangles = new List<int>();
            meshVertices = new List<Vector3>(vertices.GetRange(0, numVertices));
            meshColors = new List<Color32>(colors.GetRange(0, numVertices));

            List<int> origIndicesToConsider = new List<int>(origIndices.GetRange(0, numVertices));

            // Convert the triangles list to the new triangles list values, and keep them separate so we can cull the original triangles list while we traverse the new triangles list
            List<int> copiedTriangles = new List<int>(triangles);
            for(int i = 0; i < copiedTriangles.Count; i++)
            {
                // Find the index in origIndices that is the current int you're looking at from origTriangles
                // new int value is equal to the index found

                int newTriangleIndex = origIndicesToConsider.IndexOf(copiedTriangles[i]);
                //if(newTriangleIndex >= numVertices)
                //{
                //    newTriangleIndex = -1; // Ignore the ones that are going to be 
                //}
                copiedTriangles[i] = newTriangleIndex;
            }

            int intsPerTriangle = 3;
            for(int i = 0; i < copiedTriangles.Count; i += intsPerTriangle)
            {
                int[] triangle = new int[3] { copiedTriangles[i], copiedTriangles[i + 1], copiedTriangles[i + 2] };

                if (triangle[0] != -1
                    && triangle[1] != -1
                    && triangle[2] != -1)
                {
                    // Found a triangle

                    meshTriangles.Add(triangle[0]);
                    //meshVertices.Add(vertices[i]);
                    meshTriangles.Add(triangle[1]);
                    //meshVertices.Add(vertices[i + 1]);
                    meshTriangles.Add(triangle[2]);
                    //meshVertices.Add(vertices[i + 2]);

                    // Remove triangle found from original list so we don't consider them later for other meshes
                    triangles.RemoveRange(i, 3);
                }
                else
                {
                    // Found a border triangle or an unrelated triangle, skip it
                    continue;
                }
            }

            // Cull the original vertices list and original origIndices list and original colors list by a specified amount since we are certain we are done with numVertices amounts of those
            vertices.RemoveRange(0, numVertices);
            origIndices.RemoveRange(0, numVertices);
            colors.RemoveRange(0, numVertices);
        }

        public Mesh GetUpdatedMesh()
        {
            Mesh mesh = new Mesh();

            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Color32> colors = new List<Color32>();

            TangoManager.Tango3DRExtractWholeMesh(vertices, normals, colors, triangles);

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles.ToArray(), 0);
            mesh.SetNormals(normals);
            mesh.SetColors(colors);

            return mesh;
        }

        private static string retrieveRoomName()
        {
            GameObject RoomNameInputField = GameObject.Find("RoomNameInputField");
            if(RoomNameInputField != null)
            {
                var text = RoomNameInputField.GetComponent<UnityEngine.UI.InputField>().text;
                return text;
            }
            else
            {
                return RoomNameHolder.RoomName;
            }
            //return GameObject.Find("RoomNameInputField").GetComponent<UnityEngine.UI.InputField>().text;
        }

        private static Material createRoomMaterial()
        {
            Material mat = new Material(Shader.Find("Custom/RoomShader"));
            return mat;
        }

        private TangoApplication getTangoManager()
        {
            var go = GameObject.Find("Tango Manager");
            if(go != null)
            {
                return go.GetComponent<TangoApplication>();
            }

            return null;
        }
    }
}
#endif