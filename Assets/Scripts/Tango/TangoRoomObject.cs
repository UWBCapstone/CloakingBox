using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox
{
    public class TangoRoomObject : MonoBehaviour
    {
        GameObject roomObject;

        public static GameObject GenerateRoom(string roomName, List<Vector3> vertices, List<Vector3> normals, List<Color32> colors, List<int> triangles)
        {
            GameObject room = new GameObject();
            room.SetActive(false);
            room.name = roomName;

            Mesh mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles.ToArray(), 0);
            mesh.SetNormals(normals);
            mesh.SetColors(colors);
            
            // Add Mesh Filter
            var mf = room.AddComponent<MeshFilter>();
            mf.mesh = mesh;

            // Add Mesh Renderer
            var mr = room.AddComponent<MeshRenderer>();
            mr.material = new Material(Shader.Find("Custom/RoomShader"));

            // Add Mesh Collider
            var mc = room.AddComponent<MeshCollider>();
            mc.sharedMesh = mesh;

            var tro = room.AddComponent<TangoRoomObject>();
            tro.roomObject = room;

            // Set layer
            room.layer = LayerManager.GetLayerMask(CloakLayers.Room);

            room.SetActive(true);

            return room;
        }

        public void SetActive(bool activate)
        {
            SetActive(activate);
        }
    }
}
#endif