using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class RoomCameraPersonifier : MonoBehaviour
    {
        // Draw debug lines for the tango camera
        public bool Active = true;
        public GameObject MainCamera;
        public Color LineColor = Color.gray;

        private DebugViewLine frustumBottomLeft_m;
        private DebugViewLine frustumTopLeft_m;
        private DebugViewLine frustumTopRight_m;
        private DebugViewLine frustumBottomRight_m;
        private DebugViewLine nearLeft_m;
        private DebugViewLine nearTop_m;
        private DebugViewLine nearRight_m;
        private DebugViewLine nearBottom_m;
        private DebugViewLine farLeft_m;
        private DebugViewLine farTop_m;
        private DebugViewLine farRight_m;
        private DebugViewLine farBottom_m;

        public void Awake()
        {
            CreateDebugViewLines();
        }

        public void Update()
        {
            updateLines();
            ActivateDebugViewLines(Active);
        }

        public void CreateDebugViewLines()
        {
            //createFrustumBottomLeftDebugViewLine(LineColor);
            //createFrustumTopLeftDebugViewLine(LineColor);
            //createFrustumTopRightDebugViewLine(LineColor);
            //createFrustumBottomRightDebugViewLine(LineColor);
            //createNearLeftDebugViewLine(LineColor);
            //createNearTopDebugViewLine(LineColor);
            //createNearRightDebugViewLine(LineColor);
            //createNearBottomDebugViewLine(LineColor);
            //createFarLeftDebugViewLine(LineColor);
            //createFarTopDebugViewLine(LineColor);
            //createFarRightDebugViewLine(LineColor);
            //createFarBottomDebugViewLine(LineColor);

            createDebugViewLine(CameraViewLineTypes.FrustumBottomLeft, LineColor);
            createDebugViewLine(CameraViewLineTypes.FrustumTopLeft, LineColor);
            createDebugViewLine(CameraViewLineTypes.FrustumTopRight, LineColor);
            createDebugViewLine(CameraViewLineTypes.FrustumBottomRight, LineColor);
            createDebugViewLine(CameraViewLineTypes.NearLeft, LineColor);
            createDebugViewLine(CameraViewLineTypes.NearTop, LineColor);
            createDebugViewLine(CameraViewLineTypes.NearRight, LineColor);
            createDebugViewLine(CameraViewLineTypes.NearBottom, LineColor);
            createDebugViewLine(CameraViewLineTypes.FarLeft, LineColor);
            createDebugViewLine(CameraViewLineTypes.FarTop, LineColor);
            createDebugViewLine(CameraViewLineTypes.FarRight, LineColor);
            createDebugViewLine(CameraViewLineTypes.FarBottom, LineColor);
        }

        public void ActivateDebugViewLines(bool activate)
        {
            //frustumBottomLeft_m.Visible = activate;
            //frustumTopLeft_m.Visible = activate;
            //frustumTopRight_m.Visible = activate;
            //frustumBottomRight_m.Visible = activate;
            //nearLeft_m.Visible = activate;
            //nearTop_m.Visible = activate;
            //nearRight_m.Visible = activate;
            //nearBottom_m.Visible = activate;
            //farLeft_m.Visible = activate;
            //farTop_m.Visible = activate;
            //farRight_m.Visible = activate;
            //farBottom_m.Visible = activate;

            var arr = System.Enum.GetValues(typeof(CameraViewLineTypes));
            for (int i = 0; i < arr.Length; i++)
            {
                string goName = ((CameraViewLineTypes)i).ToString();
                var go = GameObject.Find(goName);
                if (go != null)
                {
                    var dvl = go.GetComponent<DebugViewLine>();
                    if (dvl != null)
                    {
                        dvl.Visible = activate;
                    }
                }
            }
        }

        public void ToggleDebugViewLines()
        {
            //if(frustumBottomLeft_m == null
            //    || frustumTopLeft_m == null
            //    || frustumTopRight_m == null
            //    || frustumBottomRight_m == null)
            //{
            //    frustumBottomLeft_m.Toggle();
            //    frustumTopLeft_m.Toggle();
            //    frustumTopRight_m.Toggle();
            //    frustumBottomRight_m.Toggle();
            //    nearLeft_m.Toggle();
            //    nearTop_m.Toggle();
            //    nearRight_m.Toggle();
            //    nearBottom_m.Toggle();
            //    farLeft_m.Toggle();
            //    farTop_m.Toggle();
            //    farRight_m.Toggle();
            //    farBottom_m.Toggle();
            //}

            var arr = System.Enum.GetValues(typeof(CameraViewLineTypes));
            for(int i = 0; i < arr.Length; i++)
            {
                string goName = ((CameraViewLineTypes)i).ToString();
                var go = GameObject.Find(goName);
                if(go != null)
                {
                    var dvl = go.GetComponent<DebugViewLine>();
                    if(dvl != null)
                    {
                        dvl.Toggle();
                    }
                }
            }
        }

        #region Update Lines
        private void updateLines()
        {
            //updateFrustumBottomLeftDebugViewLine(LineColor);
            //updateFrustumTopLeftDebugViewLine(LineColor);
            //updateFrustumTopRightDebugViewLine(LineColor);
            //updateFrustumBottomRightDebugViewLine(LineColor);
            //updateNearLeftDebugViewLine(LineColor);
            //updateNearTopDebugViewLine(LineColor);
            //updateNearRightDebugViewLine(LineColor);
            //updateNearBottomDebugViewLine(LineColor);
            //updateFarLeftDebugViewLine(LineColor);
            //updateFarTopDebugViewLine(LineColor);
            //updateFarRightDebugViewLine(LineColor);
            //updateFarBottomDebugViewLine(LineColor);

            updateDebugViewLine(CameraViewLineTypes.FrustumBottomLeft);
            updateDebugViewLine(CameraViewLineTypes.FrustumTopLeft);
            updateDebugViewLine(CameraViewLineTypes.FrustumTopRight);
            updateDebugViewLine(CameraViewLineTypes.FrustumBottomRight);
            updateDebugViewLine(CameraViewLineTypes.NearLeft);
            updateDebugViewLine(CameraViewLineTypes.NearTop);
            updateDebugViewLine(CameraViewLineTypes.NearRight);
            updateDebugViewLine(CameraViewLineTypes.NearBottom);
            updateDebugViewLine(CameraViewLineTypes.FarLeft);
            updateDebugViewLine(CameraViewLineTypes.FarTop);
            updateDebugViewLine(CameraViewLineTypes.FarRight);
            updateDebugViewLine(CameraViewLineTypes.FarBottom);
        }

        private void updateDebugViewLine(CameraViewLineTypes viewlineType)
        {
            GameObject dvlGO = GameObject.Find(viewlineType.ToString());
            if(dvlGO != null)
            {
                DebugViewLine dvl = dvlGO.GetComponent<DebugViewLine>();

                if (dvl != null)
                {
                    Vector3 start;
                    Vector3 stop;

                    getViewLineStartStop(MainCamera, viewlineType, out start, out stop);
                    dvl.SetStartStop(start, stop);
                    dvl.LineColor = LineColor;
                }
            }
        }




        //private void updateFrustumBottomLeftDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FrustumBottomLeft, out start, out stop);
        //    frustumBottomLeft_m.SetStartStop(start, stop);
        //    frustumBottomLeft_m.LineColor = lineColor;
        //}
        
        //private void updateFrustumTopLeftDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FrustumTopLeft, out start, out stop);
        //    frustumTopLeft_m.SetStartStop(start, stop);
        //    frustumTopLeft_m.LineColor = lineColor;
        //}
        
        //private void updateFrustumTopRightDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FrustumTopRight, out start, out stop);
        //    frustumTopRight_m.SetStartStop(start, stop);
        //    frustumTopRight_m.LineColor = lineColor;
        //}
        
        //private void updateFrustumBottomRightDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FrustumBottomRight, out start, out stop);
        //    frustumBottomRight_m.SetStartStop(start, stop);
        //    frustumBottomRight_m.LineColor = lineColor;
        //}

        //private void updateNearLeftDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.NearLeft, out start, out stop);
        //    nearLeft_m.SetStartStop(start, stop);
        //    nearLeft_m.LineColor = lineColor;
        //}

        //private void updateNearTopDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.NearTop, out start, out stop);
        //    nearTop_m.SetStartStop(start, stop);
        //    nearTop_m.LineColor = lineColor;
        //}

        //private void updateNearRightDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.NearRight, out start, out stop);
        //    nearRight_m.SetStartStop(start, stop);
        //    nearRight_m.LineColor = lineColor;
        //}

        //private void updateNearBottomDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.NearBottom, out start, out stop);
        //    nearBottom_m.SetStartStop(start, stop);
        //    nearBottom_m.LineColor = lineColor;
        //}

        //private void updateFarLeftDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FarLeft, out start, out stop);
        //    farLeft_m.SetStartStop(start, stop);
        //    farLeft_m.LineColor = lineColor;
        //}

        //private void updateFarTopDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FarTop, out start, out stop);
        //    farTop_m.SetStartStop(start, stop);
        //    farTop_m.LineColor = lineColor;
        //}

        //private void updateFarRightDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FarRight, out start, out stop);
        //    farRight_m.SetStartStop(start, stop);
        //    farRight_m.LineColor = lineColor;
        //}

        //private void updateFarBottomDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FarBottom, out start, out stop);
        //    farBottom_m.SetStartStop(start, stop);
        //    farBottom_m.LineColor = lineColor;
        //}

        #endregion

        #region Create Lines
        private GameObject createDebugViewLineGO(string name)
        {
            GameObject go = new GameObject();
            go.SetActive(false);
            go.name = name;
            go.transform.parent = gameObject.transform;

            go.AddComponent<DebugViewLine>();

            go.SetActive(true);

            return go;
        }

        private GameObject createDebugViewLine(CameraViewLineTypes viewlineType, Color lineColor)
        {
            Vector3 start;
            Vector3 stop;

            string goName = viewlineType.ToString();

            GameObject go = createDebugViewLineGO(goName);
            DebugViewLine dvl = go.GetComponent<DebugViewLine>();

            getViewLineStartStop(MainCamera, viewlineType, out start, out stop);
            dvl.SetStartStop(start, stop);
            dvl.LineColor = lineColor;

            return go;
        }



        //private DebugViewLine createFrustumBottomRightDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FrustumBottomRight, out start, out stop);
        //    //frustumBottomRight_m = gameObject.AddComponent<DebugViewLine>();


        //    frustumBottomRight_m.SetStartStop(start, stop);
        //    //bottomRight_m = new DebugViewLine(start, stop);
        //    frustumBottomRight_m.LineColor = lineColor;

        //    return frustumBottomRight_m;
        //}

        //private DebugViewLine createFrustumTopRightDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FrustumTopRight, out start, out stop);
        //    frustumTopRight_m = gameObject.AddComponent<DebugViewLine>();
        //    frustumTopRight_m.SetStartStop(start, stop);
        //    //topRight_m = new DebugViewLine(start, stop);
        //    frustumTopRight_m.LineColor = lineColor;

        //    return frustumTopRight_m;
        //}

        //private DebugViewLine createFrustumTopLeftDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FrustumTopLeft, out start, out stop);
        //    frustumTopLeft_m = gameObject.AddComponent<DebugViewLine>();
        //    frustumTopLeft_m.SetStartStop(start, stop);
        //    //topLeft_m = new DebugViewLine(start, stop);
        //    frustumTopLeft_m.LineColor = lineColor;

        //    return frustumTopLeft_m;
        //}

        //private DebugViewLine createFrustumBottomLeftDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FrustumBottomLeft, out start, out stop);
        //    frustumBottomLeft_m = gameObject.AddComponent<DebugViewLine>();
        //    frustumBottomLeft_m.SetStartStop(start, stop);
        //    //bottomLeft_m = new DebugViewLine(start, stop);
        //    frustumBottomLeft_m.LineColor = lineColor;

        //    return frustumBottomLeft_m;
        //}

        //private DebugViewLine createNearLeftDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.NearLeft, out start, out stop);
        //    nearLeft_m = gameObject.AddComponent<DebugViewLine>();
        //    nearLeft_m.SetStartStop(start, stop);
        //    nearLeft_m.LineColor = lineColor;

        //    return nearLeft_m;
        //}

        //private DebugViewLine createNearTopDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.NearTop, out start, out stop);
        //    nearTop_m = gameObject.AddComponent<DebugViewLine>();
        //    nearTop_m.SetStartStop(start, stop);
        //    nearTop_m.LineColor = lineColor;

        //    return nearTop_m;
        //}

        //private DebugViewLine createNearRightDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.NearRight, out start, out stop);
        //    nearRight_m = gameObject.AddComponent<DebugViewLine>();
        //    nearRight_m.SetStartStop(start, stop);
        //    nearRight_m.LineColor = lineColor;

        //    return nearRight_m;
        //}

        //private DebugViewLine createNearBottomDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.NearBottom, out start, out stop);
        //    nearBottom_m = gameObject.AddComponent<DebugViewLine>();
        //    nearBottom_m.SetStartStop(start, stop);
        //    nearBottom_m.LineColor = lineColor;

        //    return nearBottom_m;
        //}

        //private DebugViewLine createFarLeftDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FarLeft, out start, out stop);
        //    farLeft_m = gameObject.AddComponent<DebugViewLine>();
        //    farLeft_m.SetStartStop(start, stop);
        //    farLeft_m.LineColor = lineColor;

        //    return farLeft_m;
        //}

        //private DebugViewLine createFarTopDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FarTop, out start, out stop);
        //    farTop_m = gameObject.AddComponent<DebugViewLine>();
        //    farTop_m.SetStartStop(start, stop);
        //    farTop_m.LineColor = lineColor;

        //    return farTop_m;
        //}

        //private DebugViewLine createFarRightDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FarRight, out start, out stop);
        //    farRight_m = gameObject.AddComponent<DebugViewLine>();
        //    farRight_m.SetStartStop(start, stop);
        //    farRight_m.LineColor = lineColor;

        //    return farRight_m;
        //}

        //private DebugViewLine createFarBottomDebugViewLine(Color lineColor)
        //{
        //    Vector3 start;
        //    Vector3 stop;

        //    getViewLineStartStop(MainCamera, CameraViewLineTypes.FarBottom, out start, out stop);
        //    farBottom_m = gameObject.AddComponent<DebugViewLine>();
        //    farBottom_m.SetStartStop(start, stop);
        //    farBottom_m.LineColor = lineColor;

        //    return farBottom_m;
        //}
        #endregion

        private bool getViewLineStartStop(GameObject MainCamera, CameraViewLineTypes viewline, out Vector3 start, out Vector3 stop)
        {
            if(MainCamera == null
                || MainCamera.GetComponent<Camera>() == null)
            {
                start = Vector3.zero;
                stop = Vector3.zero;

                return false;
            }
            else
            {
                Vector3 up = MainCamera.transform.up;
                Vector3 right = MainCamera.transform.right;
                Vector3 forward = MainCamera.transform.forward;
                Camera cam = MainCamera.GetComponent<Camera>();
                
                float nearDis = cam.nearClipPlane;
                float dis = cam.farClipPlane;
                float adjacent = dis;
                float adjacentNear = nearDis;

                Vector3 nearCent = cam.transform.position + cam.nearClipPlane * forward;
                Vector3 farCent = cam.transform.position + cam.farClipPlane * forward;

                float verticalAngle = cam.fieldOfView / 2.0f;
                float horizontalAngle = cam.aspect * verticalAngle;

                float xHalfFar = adjacent * Mathf.Tan(Mathf.Deg2Rad * horizontalAngle);
                float yHalfFar = adjacent * Mathf.Tan(Mathf.Deg2Rad * verticalAngle);
                float xHalfNear = adjacentNear * Mathf.Tan(Mathf.Deg2Rad * horizontalAngle);
                float yHalfNear = adjacentNear * Mathf.Tan(Mathf.Deg2Rad * verticalAngle);

                if (viewline == CameraViewLineTypes.FrustumBottomLeft)
                {
                    start = new Vector3(nearCent.x, nearCent.y, nearCent.z);
                    start -= right * xHalfNear;
                    start -= up * yHalfNear;
                    stop = new Vector3(farCent.x, farCent.y, farCent.z);
                    stop -= right * xHalfFar;
                    stop -= up * yHalfFar;
                }
                else if (viewline == CameraViewLineTypes.FrustumTopLeft)
                {
                    start = new Vector3(nearCent.x, nearCent.y, nearCent.z);
                    start -= right * xHalfNear;
                    start += up * yHalfNear;
                    stop = new Vector3(farCent.x, farCent.y, farCent.z);
                    stop -= right * xHalfFar;
                    stop += up * yHalfFar;
                }
                else if (viewline == CameraViewLineTypes.FrustumTopRight)
                {
                    start = new Vector3(nearCent.x, nearCent.y, nearCent.z);
                    start += right * xHalfNear;
                    start += up * yHalfNear;
                    stop = new Vector3(farCent.x, farCent.y, farCent.z);
                    stop += right * xHalfFar;
                    stop += up * yHalfFar;
                }
                else if (viewline == CameraViewLineTypes.FrustumBottomRight)
                {
                    start = new Vector3(nearCent.x, nearCent.y, nearCent.z);
                    start += right * xHalfNear;
                    start -= up * yHalfNear;
                    stop = new Vector3(farCent.x, farCent.y, farCent.z);
                    stop += right * xHalfFar;
                    stop -= up * yHalfFar;
                }
                else if (viewline == CameraViewLineTypes.FarLeft)
                {
                    start = new Vector3(farCent.x, farCent.y, farCent.z);
                    start -= right * xHalfFar;
                    start -= up * yHalfFar;
                    stop = new Vector3(farCent.x, farCent.y, farCent.z);
                    stop -= right * xHalfFar;
                    stop += up * yHalfFar;
                }
                else if (viewline == CameraViewLineTypes.FarTop)
                {
                    start = new Vector3(farCent.x, farCent.y, farCent.z);
                    start -= right * xHalfFar;
                    start += up * yHalfFar;
                    stop = new Vector3(farCent.x, farCent.y, farCent.z);
                    stop += right * xHalfFar;
                    stop += up * yHalfFar;
                }
                else if(viewline == CameraViewLineTypes.FarRight)
                {
                    start = new Vector3(farCent.x, farCent.y, farCent.z);
                    start += right * xHalfFar;
                    start += up * yHalfFar;
                    stop = new Vector3(farCent.x, farCent.y, farCent.z);
                    stop += right * xHalfFar;
                    stop -= up * yHalfFar;
                }
                else if(viewline == CameraViewLineTypes.FarBottom)
                {
                    start = new Vector3(farCent.x, farCent.y, farCent.z);
                    start += right * xHalfFar;
                    start -= up * yHalfFar;
                    stop = new Vector3(farCent.x, farCent.y, farCent.z);
                    stop -= right * xHalfFar;
                    stop -= up * yHalfFar;
                }
                else if (viewline == CameraViewLineTypes.NearLeft)
                {
                    start = new Vector3(nearCent.x, nearCent.y, nearCent.z);
                    start -= right * xHalfNear;
                    start -= up * yHalfNear;
                    stop = new Vector3(nearCent.x, nearCent.y, nearCent.z);
                    stop -= right * xHalfNear;
                    stop += up * yHalfNear;
                }
                else if (viewline == CameraViewLineTypes.NearTop)
                {
                    start = new Vector3(nearCent.x, nearCent.y, nearCent.z);
                    start -= right * xHalfNear;
                    start += up * yHalfNear;
                    stop = new Vector3(nearCent.x, nearCent.y, nearCent.z);
                    stop += right * xHalfNear;
                    stop += up * yHalfNear;
                }
                else if (viewline == CameraViewLineTypes.NearRight)
                {
                    start = new Vector3(nearCent.x, nearCent.y, nearCent.z);
                    start += right * xHalfNear;
                    start += up * yHalfNear;
                    stop = new Vector3(nearCent.x, nearCent.y, nearCent.z);
                    stop += right * xHalfNear;
                    stop -= up * yHalfNear;
                }
                else if (viewline == CameraViewLineTypes.NearBottom)
                {
                    start = new Vector3(nearCent.x, nearCent.y, nearCent.z);
                    start += right * xHalfNear;
                    start -= up * yHalfNear;
                    stop = new Vector3(nearCent.x, nearCent.y, nearCent.z);
                    stop -= right * xHalfNear;
                    stop -= up * yHalfNear;
                }
                else
                {
                    Debug.LogError("CameraViewLine not recognized...");
                    start = Vector3.zero;
                    stop = Vector3.zero;
                }

                return true;
            }
        }
    }
}