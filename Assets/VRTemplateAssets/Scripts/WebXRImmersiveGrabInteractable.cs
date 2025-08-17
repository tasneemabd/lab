using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;

namespace Unity.VRTemplate
{
    /// <summary>
    /// Enhanced grab interactable specifically designed for immersive-web-emulator
    /// Provides advanced grabbing features and immersive interaction capabilities
    /// </summary>
    public class WebXRImmersiveGrabInteractable : XRGrabInteractable
    {
        [Header("Immersive Web Emulator Settings")]
        [SerializeField] private bool enableImmersiveGrabbing = true;
        [SerializeField] private float immersiveGrabDistance = 3f;
        [SerializeField] private bool enableGestureGrabbing = true;
        [SerializeField] private bool enableEyeGazeGrabbing = false;
        [SerializeField] private float eyeGazeDistance = 5f;

        [Header("Enhanced Grab Features")]
        [SerializeField] private bool enableHapticFeedback = true;
        [SerializeField] private float grabHapticIntensity = 0.7f;
        [SerializeField] private float releaseHapticIntensity = 0.3f;
        [SerializeField] private bool enableProximityHaptics = true;
        [SerializeField] private float proximityHapticDistance = 2f;

        [Header("Visual Feedback")]
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private Material grabbedMaterial;
        [SerializeField] private bool enableOutline = true;
        [SerializeField] private Color outlineColor = Color.cyan;
        [SerializeField] private float outlineWidth = 0.02f;

        [Header("Physics Settings")]
        [SerializeField] private bool enableImmersivePhysics = true;
        [SerializeField] private float immersiveMass = 1f;
        [SerializeField] private float immersiveDrag = 0.5f;
        [SerializeField] private float immersiveAngularDrag = 0.5f;
        [SerializeField] private bool enableImmersiveConstraints = true;

        // Immersive Web Emulator specific properties
        private bool isImmersiveModeActive = false;
        private bool isBeingGazedAt = false;
        private bool isInProximity = false;
        private Renderer objectRenderer;
        private Material originalMaterial;
        private Rigidbody objectRigidbody;
        private WebXRImmersiveInteractionManager immersiveManager;

        // Outline effect
        private GameObject outlineObject;
        private Renderer outlineRenderer;

        protected override void Awake()
        {
            base.Awake();
            InitializeImmersiveInteractable();
        }

        protected void Start()
        {
           // base.Start();
            SetupImmersiveFeatures();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetupEventListeners();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            RemoveEventListeners();
        }

        private void Update()
        {
            if (!enableImmersiveGrabbing) return;

            UpdateImmersiveState();
            UpdateVisualFeedback();
            UpdateProximityHaptics();
        }

        private void InitializeImmersiveInteractable()
        {
            // Get components
            objectRenderer = GetComponent<Renderer>();
            objectRigidbody = GetComponent<Rigidbody>();
            immersiveManager = FindObjectOfType<WebXRImmersiveInteractionManager>();

            // Store original material
            if (objectRenderer != null)
            {
                originalMaterial = objectRenderer.material;
            }

            // Setup outline effect
            if (enableOutline)
            {
                CreateOutlineEffect();
            }

            // Setup immersive physics
            if (enableImmersivePhysics && objectRigidbody != null)
            {
                SetupImmersivePhysics();
            }
        }

        private void SetupImmersiveFeatures()
        {
            if (immersiveManager != null)
            {
                isImmersiveModeActive = immersiveManager.IsImmersiveSessionActive();
            }
        }

        private void SetupEventListeners()
        {
            // Subscribe to grab events
            selectEntered.AddListener(OnImmersiveGrabEntered);
            selectExited.AddListener(OnImmersiveGrabExited);
            hoverEntered.AddListener(OnImmersiveHoverEntered);
            hoverExited.AddListener(OnImmersiveHoverExited);
        }

        private void RemoveEventListeners()
        {
            // Unsubscribe from grab events
            selectEntered.RemoveListener(OnImmersiveGrabEntered);
            selectExited.RemoveListener(OnImmersiveGrabExited);
            hoverEntered.RemoveListener(OnImmersiveHoverEntered);
            hoverExited.RemoveListener(OnImmersiveHoverExited);
        }

        private void CreateOutlineEffect()
        {
            if (outlineObject != null) return;

            // Create outline object
            outlineObject = new GameObject($"{gameObject.name}_Outline");
            outlineObject.transform.SetParent(transform);
            outlineObject.transform.localPosition = Vector3.zero;
            outlineObject.transform.localRotation = Quaternion.identity;
            outlineObject.transform.localScale = Vector3.one * (1f + outlineWidth);

            // Add mesh filter and renderer
            MeshFilter meshFilter = outlineObject.AddComponent<MeshFilter>();
            outlineRenderer = outlineObject.AddComponent<MeshRenderer>();

            // Copy mesh from original object
            MeshFilter originalMeshFilter = GetComponent<MeshFilter>();
            if (originalMeshFilter != null && originalMeshFilter.sharedMesh != null)
            {
                meshFilter.sharedMesh = originalMeshFilter.sharedMesh;
            }

            // Setup outline material
            if (outlineRenderer != null)
            {
                outlineRenderer.material = new Material(Shader.Find("Standard"));
                outlineRenderer.material.color = outlineColor;
                outlineRenderer.material.SetFloat("_Mode", 3); // Transparent mode
                outlineRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                outlineRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                outlineRenderer.material.SetInt("_ZWrite", 0);
                outlineRenderer.material.DisableKeyword("_ALPHATEST_ON");
                outlineRenderer.material.EnableKeyword("_ALPHABLEND_ON");
                outlineRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                outlineRenderer.material.renderQueue = 3000;
            }

            // Initially hide outline
            if (outlineObject != null)
                outlineObject.SetActive(false);
        }

        private void SetupImmersivePhysics()
        {
            if (objectRigidbody == null) return;

            objectRigidbody.mass = immersiveMass;
            objectRigidbody.linearDamping = immersiveDrag;
            objectRigidbody.angularDamping = immersiveAngularDrag;

            if (enableImmersiveConstraints)
            {
                // Add constraints based on object type
                SetupImmersiveConstraints();
            }
        }

        private void SetupImmersiveConstraints()
        {
            // This could be customized based on the object type
            // For example, some objects might only allow rotation on certain axes
        }

        private void UpdateImmersiveState()
        {
            if (!isImmersiveModeActive) return;

            // Check if being gazed at
            if (enableEyeGazeGrabbing)
            {
                UpdateEyeGazeState();
            }

            // Check proximity
            UpdateProximityState();
        }

        private void UpdateEyeGazeState()
        {
            if (immersiveManager == null) return;

            Vector3 eyeGazePoint = immersiveManager.GetEyeGazePoint();
            float distanceToEyeGaze = Vector3.Distance(transform.position, eyeGazePoint);

            bool wasGazedAt = isBeingGazedAt;
            isBeingGazedAt = distanceToEyeGaze < eyeGazeDistance;

            if (wasGazedAt != isBeingGazedAt)
            {
                OnEyeGazeStateChanged(isBeingGazedAt);
            }
        }

        private void UpdateProximityState()
        {
            if (immersiveManager == null) return;

            Vector3 immersiveOrigin = immersiveManager.GetImmersiveOrigin();
            float distanceToOrigin = Vector3.Distance(transform.position, immersiveOrigin);

            bool wasInProximity = isInProximity;
            isInProximity = distanceToOrigin < proximityHapticDistance;

            if (wasInProximity != isInProximity)
            {
                OnProximityStateChanged(isInProximity);
            }
        }

        private void UpdateVisualFeedback()
        {
            if (objectRenderer == null) return;

            Material targetMaterial = originalMaterial;

            if (isSelected)
            {
                targetMaterial = grabbedMaterial != null ? grabbedMaterial : highlightMaterial;
            }
            else if (isHovered || isBeingGazedAt || isInProximity)
            {
                targetMaterial = highlightMaterial != null ? highlightMaterial : originalMaterial;
            }

            if (objectRenderer.material != targetMaterial)
            {
                objectRenderer.material = targetMaterial;
            }

            // Update outline visibility
            if (outlineObject != null)
            {
                bool shouldShowOutline = (isHovered || isBeingGazedAt || isInProximity) && !isSelected;
                outlineObject.SetActive(shouldShowOutline);
            }
        }

        private void UpdateProximityHaptics()
        {
            if (!enableProximityHaptics || !isInProximity) return;

            if (immersiveManager != null)
            {
                Vector3 immersiveOrigin = immersiveManager.GetImmersiveOrigin();
                float distance = Vector3.Distance(transform.position, immersiveOrigin);
                float hapticStrength = Mathf.Clamp01(1f - (distance / proximityHapticDistance)) * 0.3f;
                
                // This would trigger haptic feedback on nearby controllers
                // Implementation depends on the specific haptic system being used
            }
        }

        private void OnImmersiveGrabEntered(SelectEnterEventArgs args)
        {
            if (!enableImmersiveGrabbing) return;

            Debug.Log($"WebXR Immersive: Object {gameObject.name} grabbed");

            if (enableHapticFeedback)
            {
                TriggerGrabHaptic();
            }

            // Apply immersive physics
            if (enableImmersivePhysics && objectRigidbody != null)
            {
                ApplyImmersiveGrabPhysics();
            }
        }

        private void OnImmersiveGrabExited(SelectExitEventArgs args)
        {
            if (!enableImmersiveGrabbing) return;

            Debug.Log($"WebXR Immersive: Object {gameObject.name} released");

            if (enableHapticFeedback)
            {
                TriggerReleaseHaptic();
            }

            // Reset immersive physics
            if (enableImmersivePhysics && objectRigidbody != null)
            {
                ResetImmersivePhysics();
            }
        }

        private void OnImmersiveHoverEntered(HoverEnterEventArgs args)
        {
            if (!enableImmersiveGrabbing) return;

            Debug.Log($"WebXR Immersive: Object {gameObject.name} hover entered");
        }

        private void OnImmersiveHoverExited(HoverExitEventArgs args)
        {
            if (!enableImmersiveGrabbing) return;

            Debug.Log($"WebXR Immersive: Object {gameObject.name} hover exited");
        }

        private void OnEyeGazeStateChanged(bool gazedAt)
        {
            if (gazedAt)
            {
                Debug.Log($"WebXR Immersive: Object {gameObject.name} being gazed at");
            }
        }

        private void OnProximityStateChanged(bool inProximity)
        {
            if (inProximity)
            {
                Debug.Log($"WebXR Immersive: Object {gameObject.name} in proximity");
            }
        }

        private void TriggerGrabHaptic()
        {
            // Trigger haptic feedback for grab
            if (immersiveManager != null)
            {
                // This would trigger haptic feedback on the grabbing controller
                Debug.Log($"WebXR Immersive: Grab haptic triggered with intensity {grabHapticIntensity}");
            }
        }

        private void TriggerReleaseHaptic()
        {
            // Trigger haptic feedback for release
            if (immersiveManager != null)
            {
                // This would trigger haptic feedback on the releasing controller
                Debug.Log($"WebXR Immersive: Release haptic triggered with intensity {releaseHapticIntensity}");
            }
        }

        private void ApplyImmersiveGrabPhysics()
        {
            if (objectRigidbody == null) return;

            // Apply immersive-specific physics when grabbed
            objectRigidbody.useGravity = false;
            objectRigidbody.isKinematic = true;
        }

        private void ResetImmersivePhysics()
        {
            if (objectRigidbody == null) return;

            // Reset physics when released
            objectRigidbody.useGravity = true;
            objectRigidbody.isKinematic = false;
        }

        // Public methods for external access
        public bool IsImmersiveModeActive()
        {
            return isImmersiveModeActive;
        }

        public bool IsBeingGazedAt()
        {
            return isBeingGazedAt;
        }

        public bool IsInProximity()
        {
            return isInProximity;
        }

        public void SetImmersiveGrabbing(bool enabled)
        {
            enableImmersiveGrabbing = enabled;
        }

        public void SetGestureGrabbing(bool enabled)
        {
            enableGestureGrabbing = enabled;
        }

        public void SetEyeGazeGrabbing(bool enabled)
        {
            enableEyeGazeGrabbing = enabled;
        }

        public void SetHapticFeedback(bool enabled)
        {
            enableHapticFeedback = enabled;
        }

        public void SetProximityHaptics(bool enabled)
        {
            enableProximityHaptics = enabled;
        }
    }
}
