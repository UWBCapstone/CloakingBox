using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCameraManager : MonoBehaviour {
    // Set the viewport numbers to be correct for the aspect ratio
    // Draw lines to represent the tango camera's view lines so the user can orient themselves with regard to the room
    /////
    // Set update times?

    public Camera RoomCamera;

    public Color BackgroundColor = new Color(131 / 255.0f, 131 / 255.0f, 77 / 255.0f);
    public float ViewportXOffset = 0.05f;
    public float ViewportYOffset = 0.05f;
    public float ViewportWidth = 0.3f;
    public float ViewportHeight = 0.25f;

    private const float mainCamDepthOffset = 1.0f;

    public void Update()
    {
        float mainCamDepth = Camera.main.depth;
        if(RoomCamera != null)
        {
            RoomCamera.depth = mainCamDepth + mainCamDepthOffset;
        }
    }

    [ExecuteInEditMode]
    public void ApplyViewportSettings()
    {
        if(RoomCamera != null)
        {
            RoomCamera.rect = new Rect(ViewportXOffset, ViewportYOffset, ViewportWidth, ViewportHeight);
        }
    }

    [ExecuteInEditMode]
    public void ApplyBackgroundColor()
    {
        if(RoomCamera != null)
        {
            RoomCamera.backgroundColor = BackgroundColor;
        }
    }
}
