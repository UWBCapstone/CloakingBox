using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox
{
    public class TangoManagerState
    {
        public bool AutoConnectToService;

        public bool MotionTracking_EnableMotionTracking;
        public bool MotionTracking_AutoReset;

        public TangoPoseModes PoseMode; // TangoEnums.TangoPoseType
        public bool EnableDepth;

        public bool VideoOverlay_EnableVideoOverlay;
        public TangoVideoOverlayMethods VideoOverlay_VideoOverlayMethod;

        #region 3DReconstruction (Experimental)
        public bool Reconstruction_Enable3DReconstruction;
        public float Reconstruction_Resolution;
        public bool Reconstruction_GenerateColor;
        public bool Reconstruction_GenerateNormals;
        public bool Reconstruction_GenerateUVs;
        public bool Reconstruction_SpaceClearing;
        public Tango3DReconstruction.UpdateMethod Reconstruction_UpdateMethod;
        public int Reconstruction_MeshMinVertices;
        public bool Reconstruction_UseAreaDescription;
        #endregion

        #region Performance
        public int Performance_PointCloudMaxPoints;
        public bool Performance_KeepScreenAwake;
        public bool Performance_ReduceResolution;
        #endregion

        #region Editor Emulation
        public bool Editor_DepthAndVideo;
        public Mesh Editor_MeshForEmulation;
        public Texture Editor_TextureForEmulation;
        public bool Editor_SimulateLighting;
        public Vector3 Editor_AreaDescriptionO;
        public bool Editor_AllowOutOfDateAPI;
        #endregion

        #region Methods
        #region Constructors
        public TangoManagerState()
        {
            AutoConnectToService = true;

            MotionTracking_EnableMotionTracking = true;
            MotionTracking_AutoReset = false;

            PoseMode = TangoPoseModes.MotionTracking; // TangoEnums.TangoPoseType.MOTION_TRACKING_POSE;
            EnableDepth = true;

            VideoOverlay_EnableVideoOverlay = true;
            VideoOverlay_VideoOverlayMethod = TangoVideoOverlayMethods.Texture_ITangoCameraTexture;

            Reconstruction_Enable3DReconstruction = true;
            Reconstruction_Resolution = 0.025f;
            Reconstruction_GenerateColor = true;
            Reconstruction_GenerateNormals = true;
            Reconstruction_GenerateUVs = true;
            Reconstruction_SpaceClearing = true;
            Reconstruction_UpdateMethod = Tango3DReconstruction.UpdateMethod.PROJECTIVE;
            Reconstruction_MeshMinVertices = 20;
            Reconstruction_UseAreaDescription = false;

            Performance_PointCloudMaxPoints = 0;
            Performance_KeepScreenAwake = false;
            Performance_ReduceResolution = false;

            Editor_DepthAndVideo = true;
            Editor_MeshForEmulation = null;
            Editor_TextureForEmulation = null;
            Editor_SimulateLighting = true;
            Editor_AreaDescriptionO = Vector3.zero;
            Editor_AllowOutOfDateAPI = false;
        }

        public TangoManagerState(TangoApplication tangoManager)
        {
            AutoConnectToService = tangoManager.m_autoConnectToService;

            MotionTracking_EnableMotionTracking = tangoManager.EnableMotionTracking;
            MotionTracking_AutoReset = tangoManager.MotionTrackingAutoReset;

            PoseMode = GetPoseMode(tangoManager);
            EnableDepth = tangoManager.EnableDepth;

            VideoOverlay_EnableVideoOverlay = tangoManager.EnableVideoOverlay;
            VideoOverlay_VideoOverlayMethod = GetVideoOverlayMethod(tangoManager);

            Reconstruction_Enable3DReconstruction = tangoManager.Enable3DReconstruction;
            Reconstruction_Resolution = tangoManager.ReconstructionMeshResolution;
            Reconstruction_GenerateColor = tangoManager.Enable3DReconstructionColors;
            Reconstruction_GenerateNormals = tangoManager.Enable3DReconstructionNormals;
            Reconstruction_GenerateUVs = tangoManager.VideoOverlayUseYUVTextureIdMethod;
            Reconstruction_SpaceClearing = tangoManager.m_3drSpaceClearing;
            Reconstruction_UpdateMethod = tangoManager.m_3drUpdateMethod; // Tango3DReconstruction.UpdateMethod.PROJECTIVE;
            Reconstruction_MeshMinVertices = tangoManager.m_3drMinNumVertices;
            Reconstruction_UseAreaDescription = tangoManager.EnableAreaDescriptions;

            Performance_PointCloudMaxPoints = tangoManager.m_initialPointCloudMaxPoints;
            Performance_KeepScreenAwake = tangoManager.m_keepScreenAwake;
            Performance_ReduceResolution = tangoManager.m_adjustScreenResolution;

#if UNITY_EDITOR
            Editor_DepthAndVideo = tangoManager.EnableDepth;
            Editor_MeshForEmulation = tangoManager.m_emulationEnvironment;
            Editor_TextureForEmulation = tangoManager.m_emulationEnvironmentTexture;
            Editor_SimulateLighting = tangoManager.m_emulationVideoOverlaySimpleLighting;
            Editor_AreaDescriptionO = tangoManager.EmulatedAreaDescriptionStartOffset;
            Editor_AllowOutOfDateAPI = tangoManager.AllowOutOfDateTangoAPI;
#endif
        }
        #endregion

        #region Converters / Helpers
        public TangoPoseModes GetPoseMode(TangoApplication tangoManager)
        {
            // TangoInspector.cs:
            //string[] options = new string[]
            //{
            //    "Motion Tracking",
            //    "Motion Tracking (with Drift Correction)",
            //    "Local Area Description (Load Existing)",
            //    "Local Area Description (Learning)",
            //    "Cloud Area Description"
            //};

            TangoPoseModes poseMode;

            if (tangoManager.m_enableAreaDescriptions)
            {
                if (tangoManager.m_areaDescriptionLearningMode)
                {
                    poseMode = TangoPoseModes.LocalAreaDescription_Learning;
                }
                else if (tangoManager.m_enableCloudADF)
                {
                    poseMode = TangoPoseModes.CloudAreaDescription;
                }
                else
                {
                    // Area learning, load existing local
                    poseMode = TangoPoseModes.LocalAreaDescription_LoadExisting;
                }
            }
            else if (tangoManager.m_enableDriftCorrection)
            {
                poseMode = TangoPoseModes.MotionTracking_withDriftCorrection;
            }
            else
            {
                // Motion Tracking - all false
                poseMode = TangoPoseModes.MotionTracking;
            }

            return poseMode;
        }

        public TangoVideoOverlayMethods GetVideoOverlayMethod(TangoApplication tangoManager)
        {
            // TangoInspector.cs:
            //string[] options = new string[]
            //{
            //    "Texture (ITangoCameraTexture)",
            //    "YUV Texture (IExperimentalTangoVideoOverlay)",
            //    "Raw Bytes (ITangoVideoOverlay)",
            //    "Texture and Raw Bytes",
            //    "YUV Texture and Raw Bytes",
            //    "Texture and YUV Texture",
            //    "All",
            //};

            TangoVideoOverlayMethods method;

            if (tangoManager.m_videoOverlayUseTextureMethod)
            {
                if (tangoManager.m_videoOverlayUseYUVTextureIdMethod)
                {
                    if (tangoManager.m_videoOverlayUseByteBufferMethod)
                    {
                        method = TangoVideoOverlayMethods.All;
                    }
                    else
                    {
                        method = TangoVideoOverlayMethods.Texture_and_YUV_Texture;
                    }
                }
                else
                {
                    if (tangoManager.m_videoOverlayUseByteBufferMethod)
                    {
                        method = TangoVideoOverlayMethods.Texture_and_Raw_Bytes;
                    }
                    else
                    {
                        method = TangoVideoOverlayMethods.Texture_ITangoCameraTexture;
                    }
                }
            }
            else
            {
                if (tangoManager.m_videoOverlayUseYUVTextureIdMethod)
                {
                    if (tangoManager.m_videoOverlayUseByteBufferMethod)
                    {
                        method = TangoVideoOverlayMethods.YUV_Texture_and_Raw_Bytes;
                    }
                    else
                    {
                        method = TangoVideoOverlayMethods.YUV_Texture_ExperimentalTangoVideoOverlay;
                    }
                }
                else
                {
                    if (tangoManager.m_videoOverlayUseByteBufferMethod)
                    {
                        method = TangoVideoOverlayMethods.Raw_Bytes_ITangoVideoOverlay;
                    }
                    else
                    {
                        method = TangoVideoOverlayMethods.NULL;
                    }
                }
            }

            return method;
        }

        #region Apply Settings to Tango Manager
        public void Apply(TangoApplication tangoManager)
        {
            if (tangoManager != null)
            {
                tangoManager.m_autoConnectToService = AutoConnectToService;

                tangoManager.EnableMotionTracking = MotionTracking_EnableMotionTracking;
                tangoManager.MotionTrackingAutoReset = MotionTracking_AutoReset;

                ApplyTangoPoseMode(tangoManager, PoseMode);
                tangoManager.EnableDepth = EnableDepth;

                tangoManager.EnableVideoOverlay = VideoOverlay_EnableVideoOverlay;
                ApplyVideoOverlayMethod(tangoManager, VideoOverlay_VideoOverlayMethod);

                tangoManager.Enable3DReconstruction = Reconstruction_Enable3DReconstruction;
                tangoManager.ReconstructionMeshResolution = Reconstruction_Resolution;
                tangoManager.Enable3DReconstructionColors = Reconstruction_GenerateColor;
                tangoManager.Enable3DReconstructionNormals = Reconstruction_GenerateNormals;
                tangoManager.VideoOverlayUseYUVTextureIdMethod = Reconstruction_GenerateUVs;
                tangoManager.m_3drSpaceClearing = Reconstruction_SpaceClearing;
                tangoManager.m_3drUpdateMethod = Reconstruction_UpdateMethod;
                tangoManager.m_3drMinNumVertices = Reconstruction_MeshMinVertices;
                tangoManager.EnableAreaDescriptions = Reconstruction_UseAreaDescription;
                
                tangoManager.m_initialPointCloudMaxPoints = Performance_PointCloudMaxPoints;
                tangoManager.m_keepScreenAwake = Performance_KeepScreenAwake;
                tangoManager.m_adjustScreenResolution = Performance_ReduceResolution;

#if UNITY_EDITOR
                tangoManager.EnableDepth = Editor_DepthAndVideo;
                tangoManager.m_emulationEnvironment = Editor_MeshForEmulation;
                tangoManager.m_emulationEnvironmentTexture = Editor_TextureForEmulation;
                tangoManager.m_emulationVideoOverlaySimpleLighting = Editor_SimulateLighting;
                tangoManager.EmulatedAreaDescriptionStartOffset = Editor_AreaDescriptionO;
                tangoManager.AllowOutOfDateTangoAPI = Editor_AllowOutOfDateAPI;
#endif
            }
        }
        
        public void ApplyTangoPoseMode(TangoApplication tangoManager, TangoPoseModes poseMode)
        {
            if (tangoManager != null)
            {
                switch (poseMode)
                {
                    case TangoPoseModes.MotionTracking:
                        tangoManager.m_enableDriftCorrection = false;
                        tangoManager.m_enableAreaDescriptions = false;
                        tangoManager.m_areaDescriptionLearningMode = false;
                        tangoManager.m_enableCloudADF = false;
                        break;
                    case TangoPoseModes.MotionTracking_withDriftCorrection:
                        tangoManager.m_enableDriftCorrection = true;
                        tangoManager.m_enableAreaDescriptions = false;
                        tangoManager.m_areaDescriptionLearningMode = false;
                        tangoManager.m_enableCloudADF = false;
                        break;
                    case TangoPoseModes.LocalAreaDescription_LoadExisting:
                        tangoManager.m_enableDriftCorrection = false;
                        tangoManager.m_enableAreaDescriptions = true;
                        tangoManager.m_areaDescriptionLearningMode = false;
                        tangoManager.m_enableCloudADF = false;
                        break;
                    case TangoPoseModes.LocalAreaDescription_Learning:
                        tangoManager.m_enableDriftCorrection = false;
                        tangoManager.m_enableAreaDescriptions = true;
                        tangoManager.m_areaDescriptionLearningMode = true;
                        tangoManager.m_enableCloudADF = false;
                        break;
                    case TangoPoseModes.CloudAreaDescription:
                        tangoManager.m_enableDriftCorrection = false;
                        tangoManager.m_enableAreaDescriptions = true;
                        tangoManager.m_areaDescriptionLearningMode = false;
                        tangoManager.m_enableCloudADF = true;
                        break;
                    default:
                        Debug.LogError("Unidentified Tango Pose mode applied to Tango Application. Defaulting to normal motion tracking.");

                        // Default to Motion Tracking settings
                        tangoManager.m_enableDriftCorrection = false;
                        tangoManager.m_enableAreaDescriptions = false;
                        tangoManager.m_areaDescriptionLearningMode = false;
                        tangoManager.m_enableCloudADF = false;
                        break;
                }
            }
        }

        public void ApplyVideoOverlayMethod(TangoApplication tangoManager, TangoVideoOverlayMethods overlayMethod)
        {
            // TangoInspector.cs:
            //string[] options = new string[]
            //{
            //    "Texture (ITangoCameraTexture)",
            //    "YUV Texture (IExperimentalTangoVideoOverlay)",
            //    "Raw Bytes (ITangoVideoOverlay)",
            //    "Texture and Raw Bytes",
            //    "YUV Texture and Raw Bytes",
            //    "Texture and YUV Texture",
            //    "All",
            //};

            switch (overlayMethod)
            {
                case TangoVideoOverlayMethods.All:
                    tangoManager.m_videoOverlayUseTextureMethod = true;
                    tangoManager.m_videoOverlayUseYUVTextureIdMethod = true;
                    tangoManager.m_videoOverlayUseByteBufferMethod = true;
                    break;
                case TangoVideoOverlayMethods.Texture_and_YUV_Texture:
                    tangoManager.m_videoOverlayUseTextureMethod = true;
                    tangoManager.m_videoOverlayUseYUVTextureIdMethod = true;
                    break;
                case TangoVideoOverlayMethods.YUV_Texture_and_Raw_Bytes:
                    tangoManager.m_videoOverlayUseYUVTextureIdMethod = true;
                    tangoManager.m_videoOverlayUseByteBufferMethod = true;
                    break;
                case TangoVideoOverlayMethods.Texture_and_Raw_Bytes:
                    tangoManager.m_videoOverlayUseTextureMethod = true;
                    tangoManager.m_videoOverlayUseByteBufferMethod = true;
                    break;
                case TangoVideoOverlayMethods.Raw_Bytes_ITangoVideoOverlay:
                    tangoManager.m_videoOverlayUseByteBufferMethod = true;
                    break;
                case TangoVideoOverlayMethods.YUV_Texture_ExperimentalTangoVideoOverlay:
                    tangoManager.m_videoOverlayUseYUVTextureIdMethod = true;
                    break;
                case TangoVideoOverlayMethods.Texture_ITangoCameraTexture:
                    tangoManager.m_videoOverlayUseTextureMethod = true;
                    break;
            }
        }
        #endregion
        #endregion
        #endregion
    }
}
#endif