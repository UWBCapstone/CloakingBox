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