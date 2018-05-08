using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox {
    public class DebugViewLine : MonoBehaviour
    {        
        public bool Visible = true;
        private Color lineColor_m = Color.gray;
        private Vector3 start_m = Vector3.zero;
        private Vector3 stop_m = Vector3.zero;

        public LineRenderer lineRenderer;
        public float LineWidth = 0.05f;


        public void Toggle()
        {
            Visible = !Visible;
        }

        public void SetStartStop(Vector3 start, Vector3 stop)
        {
            start_m = start;
            stop_m = stop;
        }

        public void Awake()
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Custom/MyLineShader"));
            lineRenderer.material.SetColor("Color", LineColor);
            gameObject.layer = LayerManager.GetLayerMask(CloakLayers.Debug);
        }

        public void Update()
        {
            lineRenderer.SetPositions(new Vector3[2] { start_m, stop_m });
            lineRenderer.startColor = LineColor;
            lineRenderer.endColor = LineColor;
            lineRenderer.enabled = Visible;
            lineRenderer.startWidth = LineWidth;
            lineRenderer.endWidth = LineWidth;
            lineRenderer.material.SetColor("Color", LineColor);
        }

        public Color LineColor
        {
            get
            {
                return lineColor_m;
            }
            set
            {
                lineColor_m = value;
            }
        }
    }
}