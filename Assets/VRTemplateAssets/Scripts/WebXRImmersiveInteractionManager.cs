using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

namespace Unity.VRTemplate
{
    /// <summary>
    /// Manages WebXR interactions specifically for immersive-web-emulator
    /// Provides enhanced interaction capabilities and immersive features
    /// </summary>
    public class WebXRImmersiveInteractionManager : MonoBehaviour
    {
        [Header("Immersive Web Emulator Settings")]
        [SerializeField] private bool enableImmersiveMode = true;
        [SerializeField] private float interactionDistance = 10f;
        [SerializeField] private LayerMask interactableLayers = -1;
        [SerializeField] private bool enableHandTracking = true;
        [SerializeField] private bool enableEyeTracking = false;

        [Header("Controller References")]
        [SerializeField] private WebXRImmersiveController leftController;
        [SerializeField] private WebXRImmersiveController rightController;
        [SerializeField] private XRInteractionManager interactionManager;

        [Header("Interaction Settings")]
        [SerializeField] private float grabThreshold = 0.8f;
        [SerializeField] private float releaseThreshold = 0.2f;
        [SerializeField] private bool enableHapticFeedback = true;
        [SerializeField] private float hapticIntensity = 0.5f;

        [Header("Immersive Features")]
        [SerializeField] private bool enableGestureRecognition = true;
        [SerializeField] private bool enableVoiceCommands = false;
        [SerializeField] private bool enableSpatialAudio = true;
        [SerializeField] private float spatialAudioRadius = 10f;

        // Immersive Web Emulator specific properties
        private bool isImmersiveSessionActive = false;
        private Vector3 immersiveOrigin;
        private Quaternion immersiveOrientation;
        private List<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable> currentInteractables = new List<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable>();
        private Dictionary<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable, float> interactableDistances = new Dictionary<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable, float>();

        // Gesture recognition
        private Vector3[] handPositions = new Vector3[10];
        private Quaternion[] handRotations = new Quaternion[10];
        private bool[] handTracked = new bool[2];

        // Eye tracking
        private Vector3 eyeGazePoint;
        private bool eyeTrackingActive = false;

        private void Start()
        {
            InitializeImmersiveManager();
        }

        private void Update()
        {
            if (!isImmersiveSessionActive) return;

            UpdateImmersiveSession();
            UpdateInteractionDetection();
            UpdateGestureRecognition();
            UpdateEyeTracking();
            UpdateSpatialAudio();
        }

        private void InitializeImmersiveManager()
        {
            if (enableImmersiveMode)
            {
                isImmersiveSessionActive = true;
                immersiveOrigin = Vector3.zero;
                immersiveOrientation = Quaternion.identity;

                // Initialize interaction manager
                if (interactionManager == null)
                    interactionManager = FindObjectOfType<XRInteractionManager>();

                // Initialize controllers
                if (leftController == null || rightController == null)
                {
                    var controllers = FindObjectsOfType<WebXRImmersiveController>();
                    if (controllers.Length >= 2)
                    {
                        leftController = controllers[0];
                        rightController = controllers[1];
                    }
                }

                Debug.Log("WebXR Immersive Interaction Manager: Immersive mode initialized");
            }
        }

        private void UpdateImmersiveSession()
        {
            // Update immersive session state
            UpdateImmersiveOrigin();
            UpdateImmersiveOrientation();
        }

        private void UpdateImmersiveOrigin()
        {
            // Calculate immersive origin based on user position
            Vector3 leftPos = leftController != null ? leftController.GetImmersivePosition() : Vector3.zero;
            Vector3 rightPos = rightController != null ? rightController.GetImmersivePosition() : Vector3.zero;
            
            immersiveOrigin = (leftPos + rightPos) * 0.5f;
        }

        private void UpdateImmersiveOrientation()
        {
            // Calculate immersive orientation based on controller orientations
            Quaternion leftRot = leftController != null ? leftController.GetImmersiveRotation() : Quaternion.identity;
            Quaternion rightRot = rightController != null ? rightController.GetImmersiveRotation() : Quaternion.identity;
            
            immersiveOrientation = Quaternion.Slerp(leftRot, rightRot, 0.5f);
        }

        private void UpdateInteractionDetection()
        {
            currentInteractables.Clear();
            interactableDistances.Clear();

            // Find all interactable objects within range
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance, interactableLayers);
            
            foreach (var collider in colliders)
            {
                var interactable = collider.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable>();
                if (interactable != null)
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    currentInteractables.Add(interactable);
                    interactableDistances[interactable] = distance;

                    // Provide haptic feedback when approaching interactable objects
                    if (enableHapticFeedback && distance < 2f)
                    {
                        float hapticStrength = Mathf.Clamp01(1f - (distance / 2f)) * hapticIntensity;
                        TriggerProximityHaptic(hapticStrength);
                    }
                }
            }
        }

        private void UpdateGestureRecognition()
        {
            if (!enableGestureRecognition) return;

            // Update hand tracking data
            UpdateHandTracking();

            // Recognize gestures
            RecognizeGestures();
        }

        private void UpdateHandTracking()
        {
            // Simulate hand tracking data for immersive-web-emulator
            if (leftController != null)
            {
                Vector3 leftPos = leftController.GetImmersivePosition();
                Quaternion leftRot = leftController.GetImmersiveRotation();
                
                handPositions[0] = leftPos;
                handRotations[0] = leftRot;
                handTracked[0] = true;
            }

            if (rightController != null)
            {
                Vector3 rightPos = rightController.GetImmersivePosition();
                Quaternion rightRot = rightController.GetImmersiveRotation();
                
                handPositions[1] = rightPos;
                handRotations[1] = rightRot;
                handTracked[1] = true;
            }
        }

        private void RecognizeGestures()
        {
            // Implement gesture recognition logic
            // This could include pinch, grab, point, wave, etc.
            
            if (leftController != null && rightController != null)
            {
                float leftGrip = leftController.GetGripValue();
                float rightGrip = rightController.GetGripValue();

                // Detect grab gesture
                if (leftGrip > grabThreshold && rightGrip > grabThreshold)
                {
                    OnGrabGestureDetected();
                }

                // Detect release gesture
                if (leftGrip < releaseThreshold && rightGrip < releaseThreshold)
                {
                    OnReleaseGestureDetected();
                }
            }
        }

        private void UpdateEyeTracking()
        {
            if (!enableEyeTracking) return;

            // Simulate eye tracking for immersive-web-emulator
            eyeTrackingActive = true;
            
            // Calculate eye gaze point based on camera forward direction
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                eyeGazePoint = mainCamera.transform.position + mainCamera.transform.forward * 5f;
            }
        }

        private void UpdateSpatialAudio()
        {
            if (!enableSpatialAudio) return;

            // Update spatial audio based on immersive position
            AudioListener audioListener = FindObjectOfType<AudioListener>();
            if (audioListener != null)
            {
                audioListener.transform.position = immersiveOrigin;
                audioListener.transform.rotation = immersiveOrientation;
            }
        }

        private void OnGrabGestureDetected()
        {
            Debug.Log("WebXR Immersive: Grab gesture detected");
            
            if (enableHapticFeedback)
            {
                TriggerHapticFeedback(hapticIntensity);
            }

            // Trigger grab events for nearby interactable objects
            foreach (var interactable in currentInteractables)
            {
                if (interactableDistances[interactable] < 1f)
                {
                    // Attempt to grab the interactable
                    TryGrabInteractable(interactable);
                }
            }
        }

        private void OnReleaseGestureDetected()
        {
            Debug.Log("WebXR Immersive: Release gesture detected");
            
            if (enableHapticFeedback)
            {
                TriggerHapticFeedback(hapticIntensity * 0.3f);
            }

            // Release all currently held objects
            ReleaseAllInteractables();
        }

        private void TryGrabInteractable(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable interactable)
        {
            // Implementation for grabbing interactable objects
            // This would integrate with the XR Interaction Toolkit
            Debug.Log($"WebXR Immersive: Attempting to grab interactable {interactable}");
        }

        private void ReleaseAllInteractables()
        {
            // Implementation for releasing all interactable objects
            Debug.Log("WebXR Immersive: Releasing all interactables");
        }

        private void TriggerProximityHaptic(float intensity)
        {
            if (leftController != null)
                leftController.TriggerHapticFeedback(intensity);
            
            if (rightController != null)
                rightController.TriggerHapticFeedback(intensity);
        }

        private void TriggerHapticFeedback(float intensity)
        {
            TriggerProximityHaptic(intensity);
        }

        // Public methods for external access
        public bool IsImmersiveSessionActive()
        {
            return isImmersiveSessionActive;
        }

        public Vector3 GetImmersiveOrigin()
        {
            return immersiveOrigin;
        }

        public Quaternion GetImmersiveOrientation()
        {
            return immersiveOrientation;
        }

        public List<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable> GetCurrentInteractables()
        {
            return new List<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable>(currentInteractables);
        }

        public Vector3 GetEyeGazePoint()
        {
            return eyeGazePoint;
        }

        public bool IsEyeTrackingActive()
        {
            return eyeTrackingActive;
        }

        public Vector3[] GetHandPositions()
        {
            return handPositions;
        }

        public Quaternion[] GetHandRotations()
        {
            return handRotations;
        }

        public bool[] GetHandTracked()
        {
            return handTracked;
        }

        public void SetImmersiveMode(bool enabled)
        {
            enableImmersiveMode = enabled;
            if (enabled && !isImmersiveSessionActive)
            {
                InitializeImmersiveManager();
            }
        }

        public void SetGestureRecognition(bool enabled)
        {
            enableGestureRecognition = enabled;
        }

        public void SetEyeTracking(bool enabled)
        {
            enableEyeTracking = enabled;
        }

        public void SetSpatialAudio(bool enabled)
        {
            enableSpatialAudio = enabled;
        }
    }
}
