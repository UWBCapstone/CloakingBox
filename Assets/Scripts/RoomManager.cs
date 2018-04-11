using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class RoomManager : MonoBehaviour
    {
        public float ReloadInterval = 3.0f;
        public static GameObject CachedRoom;

        public void Awake()
        {
            InvokeRepeating("CacheRoom", 0.0f, ReloadInterval);
        }

        public void CacheRoom()
        {
            GameObject tempRoom = new GameObject();
            tempRoom.name = "Room";
            tempRoom.SetActive(false);

            var mf = tempRoom.AddComponent<MeshFilter>();
            var mr = tempRoom.AddComponent<MeshRenderer>();
            var mc = tempRoom.AddComponent<MeshCollider>();

            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Color32> colors = new List<Color32>();
            List<int> triangles = new List<int>();

            var tangoApp = GameObject.Find("Tango Manager").GetComponent<Tango.TangoApplication>();

            if (tangoApp.Tango3DRExtractWholeMesh(vertices, normals, colors, triangles) == Tango.Tango3DReconstruction.Status.SUCCESS)
            {
                // if successful at extracting mesh...
                Mesh m = new Mesh();
                m.SetVertices(vertices);
                m.SetTriangles(triangles, 0);
                m.SetColors(colors);
                m.RecalculateNormals();

                mf.mesh = m;
                mc.sharedMesh = m;
            }
            else
            {
                Debug.LogError("Tango unable to extract mesh");
            }

            mr.material = new Material(Shader.Find("Custom/RoomShader"));

            tempRoom.layer = LayerManager.GetLayerMask(CloakLayers.Room);

            GameObject oldRoom = CachedRoom;

            tempRoom.SetActive(true);
            if (oldRoom != null)
            {
                oldRoom.SetActive(false);
                GameObject.Destroy(oldRoom);
            }
            CachedRoom = tempRoom;
        }

        public static GameObject GetRoom()
        {
            return CachedRoom;
        }

        public static Mesh GetRoomMesh()
        {
            if(CachedRoom != null)
            {
                MeshFilter mf = CachedRoom.GetComponent<MeshFilter>();
                return mf.mesh;
            }

            return null;
        }
    }
}