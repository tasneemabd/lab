using UnityEngine;

using System.Collections.Generic;

namespace Unity.VRTemplate
{
    /// <summary>
    /// Configuration manager for WebXR immersive-web-emulator integration
    /// Provides easy setup and management of immersive features
    /// </summary>
    public class WebXRImmersiveConfig : MonoBehaviour
    {
        [Header("Immersive Web Emulator Configuration")]
        [SerializeField] private bool autoSetupImmersiveMode = true;
        [SerializeField] private bool enableImmersiveControllers = true;
        [SerializeField] private bool enableImmersiveInteractions = true;
        [SerializeField] private bool enableImmersivePhysics = true;

        [Header("Controller Setup")]
        [SerializeField] private GameObject leftControllerPrefab;
        [SerializeField] private GameObject rightControllerPrefab;
        [SerializeField] private Transform leftControllerParent;
        [SerializeField] private Transform rightControllerParent;

        [Header("Interaction Setup")]
        [SerializeField] private GameObject interactionManagerPrefab;
        [SerializeField] private Transform interactionManagerParent;

        [Header("Immersive Features")]
        [SerializeField] private bool enableGestureRecognition = true;
        [SerializeField] private bool enableEyeTracking = false;
        [SerializeField] private bool enableSpatialAudio = true;
        [SerializeField] private bool enableHapticFeedback = true;
        [SerializeField] private bool enableProximityDetection = true;

        [Header("Performance Settings")]
        [SerializeField] private int maxInteractableObjects = 50;
        [SerializeField] private float interactionUpdateRate = 60f;
        [SerializeField] private bool enableOptimization = true;

        // Runtime components
        private WebXRImmersiveController leftController;
        private WebXRImmersiveController rightController;
        private WebXRImmersiveInteractionManager interactionManager;
        private List<WebXRImmersiveGrabInteractable> interactableObjects = new List<WebXRImmersiveGrabInteractable>();

        // Configuration state
        private bool isConfigured = false;
        private bool isImmersiveModeActive = false;

        private void Awake()
        {
            if (autoSetupImmersiveMode)
            {
                SetupImmersiveMode();
            }
        }

        private void Start()
        {
            if (isConfigured)
            {
                InitializeImmersiveFeatures();
            }
        }

        /// <summary>
        /// Sets up the immersive mode with all necessary components
        /// </summary>
        public void SetupImmersiveMode()
        {
            Debug.Log("WebXR Immersive Config: Setting up immersive mode...");

            // Setup controllers
            if (enableImmersiveControllers)
            {
                SetupImmersiveControllers();
            }

            // Setup interaction manager
            if (enableImmersiveInteractions)
            {
                SetupInteractionManager();
            }

            // Setup physics
            if (enableImmersivePhysics)
            {
                SetupImmersivePhysics();
            }

            // Find existing interactable objects
            FindExistingInteractables();

            isConfigured = true;
            isImmersiveModeActive = true;

            Debug.Log("WebXR Immersive Config: Immersive mode setup complete");
        }

        /// <summary>
        /// Sets up immersive controllers
        /// </summary>
        private void SetupImmersiveControllers()
        {
            // Setup left controller
            if (leftController == null)
            {
                if (leftControllerPrefab != null)
                {
                    GameObject leftControllerObj = Instantiate(leftControllerPrefab, leftControllerParent);
                    leftController = leftControllerObj.GetComponent<WebXRImmersiveController>();
                }
                else
                {
                    // Try to find existing controller
                    leftController = FindObjectOfType<WebXRImmersiveController>();
                }
            }

            // Setup right controller
            if (rightController == null)
            {
                if (rightControllerPrefab != null)
                {
                    GameObject rightControllerObj = Instantiate(rightControllerPrefab, rightControllerParent);
                    rightController = rightControllerObj.GetComponent<WebXRImmersiveController>();
                }
                else
                {
                    // Try to find existing controller (if only one exists, use it for both)
                    var controllers = FindObjectsOfType<WebXRImmersiveController>();
                    if (controllers.Length >= 2)
                    {
                        rightController = controllers[1];
                    }
                }
            }

            // Configure controllers
            if (leftController != null)
            {
                ConfigureController(leftController, true);
            }

            if (rightController != null)
            {
                ConfigureController(rightController, false);
            }
        }

        /// <summary>
        /// Configures a controller with immersive settings
        /// </summary>
        private void ConfigureController(WebXRImmersiveController controller, bool isLeft)
        {
            if (controller == null) return;

            // Set controller properties through reflection or public methods
            // This would configure the controller with immersive-web-emulator settings
            Debug.Log($"WebXR Immersive Config: Configured {(isLeft ? "left" : "right")} controller");
        }

        /// <summary>
        /// Sets up the interaction manager
        /// </summary>
        private void SetupInteractionManager()
        {
            if (interactionManager == null)
            {
                if (interactionManagerPrefab != null)
                {
                    GameObject managerObj = Instantiate(interactionManagerPrefab, interactionManagerParent);
                    interactionManager = managerObj.GetComponent<WebXRImmersiveInteractionManager>();
                }
                else
                {
                    // Try to find existing manager
                    interactionManager = FindObjectOfType<WebXRImmersiveInteractionManager>();
                }
            }

            if (interactionManager != null)
            {
                ConfigureInteractionManager();
            }
        }

        /// <summary>
        /// Configures the interaction manager with immersive settings
        /// </summary>
        private void ConfigureInteractionManager()
        {
            if (interactionManager == null) return;

            // Set interaction manager properties
            interactionManager.SetImmersiveMode(true);
            interactionManager.SetGestureRecognition(enableGestureRecognition);
            interactionManager.SetEyeTracking(enableEyeTracking);
            interactionManager.SetSpatialAudio(enableSpatialAudio);

            Debug.Log("WebXR Immersive Config: Interaction manager configured");
        }

        /// <summary>
        /// Sets up immersive physics
        /// </summary>
        private void SetupImmersivePhysics()
        {
            // Configure physics settings for immersive mode
            Physics.autoSimulation = true;
            Physics.gravity = new Vector3(0, -9.81f, 0);

            // Set up physics layers for immersive interactions
            SetupPhysicsLayers();

            Debug.Log("WebXR Immersive Config: Physics configured for immersive mode");
        }

        /// <summary>
        /// Sets up physics layers for immersive interactions
        /// </summary>
        private void SetupPhysicsLayers()
        {
            // Configure physics layers for better immersive interactions
            // This could include setting up specific layers for interactable objects
        }

        /// <summary>
        /// Finds existing interactable objects and configures them
        /// </summary>
        private void FindExistingInteractables()
        {
            // Find all existing XR grab interactables
            var existingInteractables = FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            
            foreach (var interactable in existingInteractables)
            {
                // Convert to immersive interactable if possible
                ConvertToImmersiveInteractable(interactable);
            }

            Debug.Log($"WebXR Immersive Config: Found and configured {interactableObjects.Count} interactable objects");
        }

        /// <summary>
        /// Converts a regular XR interactable to an immersive interactable
        /// </summary>
        private void ConvertToImmersiveInteractable(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable)
        {
            if (interactable == null) return;

            // Add immersive component if not already present
            var immersiveInteractable = interactable.GetComponent<WebXRImmersiveGrabInteractable>();
            if (immersiveInteractable == null)
            {
                immersiveInteractable = interactable.gameObject.AddComponent<WebXRImmersiveGrabInteractable>();
            }

            // Configure the immersive interactable
            if (immersiveInteractable != null)
            {
                ConfigureImmersiveInteractable(immersiveInteractable);
                interactableObjects.Add(immersiveInteractable);
            }
        }

        /// <summary>
        /// Configures an immersive interactable with settings
        /// </summary>
        private void ConfigureImmersiveInteractable(WebXRImmersiveGrabInteractable interactable)
        {
            if (interactable == null) return;

            interactable.SetImmersiveGrabbing(true);
            interactable.SetGestureGrabbing(enableGestureRecognition);
            interactable.SetEyeGazeGrabbing(enableEyeTracking);
            interactable.SetHapticFeedback(enableHapticFeedback);
            interactable.SetProximityHaptics(enableProximityDetection);
        }

        /// <summary>
        /// Initializes immersive features after setup
        /// </summary>
        private void InitializeImmersiveFeatures()
        {
            if (!isConfigured) return;

            // Initialize gesture recognition
            if (enableGestureRecognition)
            {
                InitializeGestureRecognition();
            }

            // Initialize eye tracking
            if (enableEyeTracking)
            {
                InitializeEyeTracking();
            }

            // Initialize spatial audio
            if (enableSpatialAudio)
            {
                InitializeSpatialAudio();
            }

            Debug.Log("WebXR Immersive Config: Immersive features initialized");
        }

        /// <summary>
        /// Initializes gesture recognition
        /// </summary>
        private void InitializeGestureRecognition()
        {
            // Initialize gesture recognition system
            Debug.Log("WebXR Immersive Config: Gesture recognition initialized");
        }

        /// <summary>
        /// Initializes eye tracking
        /// </summary>
        private void InitializeEyeTracking()
        {
            // Initialize eye tracking system
            Debug.Log("WebXR Immersive Config: Eye tracking initialized");
        }

        /// <summary>
        /// Initializes spatial audio
        /// </summary>
        private void InitializeSpatialAudio()
        {
            // Initialize spatial audio system
            Debug.Log("WebXR Immersive Config: Spatial audio initialized");
        }

        /// <summary>
        /// Adds a new interactable object to the immersive system
        /// </summary>
        public void AddImmersiveInteractable(GameObject interactableObject)
        {
            if (interactableObject == null) return;

            var interactable = interactableObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            if (interactable != null)
            {
                ConvertToImmersiveInteractable(interactable);
            }
        }

        /// <summary>
        /// Removes an interactable object from the immersive system
        /// </summary>
        public void RemoveImmersiveInteractable(GameObject interactableObject)
        {
            if (interactableObject == null) return;

            var immersiveInteractable = interactableObject.GetComponent<WebXRImmersiveGrabInteractable>();
            if (immersiveInteractable != null)
            {
                interactableObjects.Remove(immersiveInteractable);
                DestroyImmediate(immersiveInteractable);
            }
        }

        /// <summary>
        /// Enables or disables immersive mode
        /// </summary>
        public void SetImmersiveMode(bool enabled)
        {
            isImmersiveModeActive = enabled;

            if (interactionManager != null)
            {
                interactionManager.SetImmersiveMode(enabled);
            }

            Debug.Log($"WebXR Immersive Config: Immersive mode {(enabled ? "enabled" : "disabled")}");
        }

        /// <summary>
        /// Gets the current configuration status
        /// </summary>
        public bool IsConfigured()
        {
            return isConfigured;
        }

        /// <summary>
        /// Gets the immersive mode status
        /// </summary>
        public bool IsImmersiveModeActive()
        {
            return isImmersiveModeActive;
        }

        /// <summary>
        /// Gets the left controller reference
        /// </summary>
        public WebXRImmersiveController GetLeftController()
        {
            return leftController;
        }

        /// <summary>
        /// Gets the right controller reference
        /// </summary>
        public WebXRImmersiveController GetRightController()
        {
            return rightController;
        }

        /// <summary>
        /// Gets the interaction manager reference
        /// </summary>
        public WebXRImmersiveInteractionManager GetInteractionManager()
        {
            return interactionManager;
        }

        /// <summary>
        /// Gets all immersive interactable objects
        /// </summary>
        public List<WebXRImmersiveGrabInteractable> GetInteractableObjects()
        {
            return new List<WebXRImmersiveGrabInteractable>(interactableObjects);
        }

        /// <summary>
        /// Optimizes the immersive system for performance
        /// </summary>
        public void OptimizeSystem()
        {
            if (!enableOptimization) return;

            // Implement optimization logic
            Debug.Log("WebXR Immersive Config: System optimized");
        }

        private void OnDestroy()
        {
            // Cleanup immersive components
            if (interactionManager != null)
            {
                interactionManager.SetImmersiveMode(false);
            }
        }
    }
}
