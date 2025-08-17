using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace Unity.VRTemplate
{
    /// <summary>
    /// WebXR Controller that integrates with immersive-web-emulator for enhanced controller support
    /// </summary>
    public class WebXRImmersiveController : MonoBehaviour
    {
        [Header("Controller Settings")]
        [SerializeField] private XRBaseController xrController;
        [SerializeField] private InputActionReference triggerAction;
        [SerializeField] private InputActionReference gripAction;
        [SerializeField] private InputActionReference primaryButtonAction;
        [SerializeField] private InputActionReference secondaryButtonAction;
        [SerializeField] private InputActionReference thumbstickAction;
        [SerializeField] private InputActionReference trackpadAction;

        [Header("Immersive Web Emulator Settings")]
        [SerializeField] private bool enableImmersiveEmulator = true;
        [SerializeField] private float hapticIntensity = 0.5f;
        [SerializeField] private float hapticDuration = 0.1f;

        [Header("Controller Visualization")]
        [SerializeField] private GameObject controllerModel;
        [SerializeField] private Material controllerMaterial;
        [SerializeField] private Color idleColor = Color.white;
        [SerializeField] private Color activeColor = Color.cyan;
        [SerializeField] private Color hapticColor = Color.yellow;

        private Renderer controllerRenderer;
        private bool isControllerActive = false;
        private float hapticTimer = 0f;
        private bool isHapticActive = false;

        // Immersive Web Emulator specific properties
        private bool isImmersiveModeActive = false;
        private Vector3 immersivePosition;
        private Quaternion immersiveRotation;

        private void Awake()
        {
            InitializeController();
        }

        private void Start()
        {
            SetupInputActions();
            SetupImmersiveEmulator();
        }

        private void Update()
        {
            UpdateControllerState();
            UpdateHapticFeedback();
            UpdateImmersiveEmulator();
        }

        private void InitializeController()
        {
            if (xrController == null)
                xrController = GetComponent<XRBaseController>();

            if (controllerModel != null)
                controllerRenderer = controllerModel.GetComponent<Renderer>();

            // Set initial controller color
            if (controllerRenderer != null && controllerMaterial != null)
            {
                controllerRenderer.material = controllerMaterial;
                controllerRenderer.material.color = idleColor;
            }
        }

        private void SetupInputActions()
        {
            if (triggerAction != null)
                triggerAction.action.performed += OnTriggerPressed;

            if (gripAction != null)
                gripAction.action.performed += OnGripPressed;

            if (primaryButtonAction != null)
                primaryButtonAction.action.performed += OnPrimaryButtonPressed;

            if (secondaryButtonAction != null)
                secondaryButtonAction.action.performed += OnSecondaryButtonPressed;
        }

        private void SetupImmersiveEmulator()
        {
            if (enableImmersiveEmulator)
            {
                // Initialize immersive-web-emulator integration
                isImmersiveModeActive = true;
                Debug.Log("WebXR Immersive Controller: Immersive Web Emulator enabled");
            }
        }

        private void UpdateControllerState()
        {
            // Update controller active state based on input
            bool wasActive = isControllerActive;
            isControllerActive = IsAnyInputActive();

            if (wasActive != isControllerActive)
            {
                UpdateControllerVisuals();
            }
        }

        private void UpdateHapticFeedback()
        {
            if (isHapticActive)
            {
                hapticTimer -= Time.deltaTime;
                if (hapticTimer <= 0f)
                {
                    StopHapticFeedback();
                }
            }
        }

        private void UpdateImmersiveEmulator()
        {
            if (!isImmersiveModeActive) return;

            // Update immersive-web-emulator specific features
            UpdateImmersivePosition();
            UpdateImmersiveRotation();
        }

        private void UpdateImmersivePosition()
        {
            if (xrController != null)
            {
                immersivePosition = xrController.transform.position;
                // Send position data to immersive-web-emulator if needed
            }
        }

        private void UpdateImmersiveRotation()
        {
            if (xrController != null)
            {
                immersiveRotation = xrController.transform.rotation;
                // Send rotation data to immersive-web-emulator if needed
            }
        }

        private bool IsAnyInputActive()
        {
            bool triggerActive = triggerAction?.action?.ReadValue<float>() > 0.1f;
            bool gripActive = gripAction?.action?.ReadValue<float>() > 0.1f;
            bool primaryActive = primaryButtonAction?.action?.ReadValue<float>() > 0.1f;
            bool secondaryActive = secondaryButtonAction?.action?.ReadValue<float>() > 0.1f;

            return triggerActive || gripActive || primaryActive || secondaryActive;
        }

        private void UpdateControllerVisuals()
        {
            if (controllerRenderer == null) return;

            Color targetColor = isControllerActive ? activeColor : idleColor;
            if (isHapticActive)
                targetColor = hapticColor;

            controllerRenderer.material.color = Color.Lerp(controllerRenderer.material.color, targetColor, Time.deltaTime * 5f);
        }

        private void OnTriggerPressed(InputAction.CallbackContext context)
        {
            float triggerValue = context.ReadValue<float>();
            if (triggerValue > 0.1f)
            {
                TriggerHapticFeedback(triggerValue);
            }
        }

        private void OnGripPressed(InputAction.CallbackContext context)
        {
            float gripValue = context.ReadValue<float>();
            if (gripValue > 0.1f)
            {
                TriggerHapticFeedback(gripValue * 0.5f);
            }
        }

        private void OnPrimaryButtonPressed(InputAction.CallbackContext context)
        {
            TriggerHapticFeedback(0.3f);
        }

        private void OnSecondaryButtonPressed(InputAction.CallbackContext context)
        {
            TriggerHapticFeedback(0.3f);
        }

        public void TriggerHapticFeedback(float intensity = 0.5f)
        {
            if (!enableImmersiveEmulator) return;

            hapticIntensity = Mathf.Clamp01(intensity);
            hapticDuration = 0.1f;
            hapticTimer = hapticDuration;
            isHapticActive = true;

            // Send haptic feedback to immersive-web-emulator
            if (xrController != null)
            {
                xrController.SendHapticImpulse(hapticIntensity, hapticDuration);
            }

            Debug.Log($"WebXR Immersive Controller: Haptic feedback triggered with intensity {hapticIntensity}");
        }

        public void StopHapticFeedback()
        {
            isHapticActive = false;
            hapticTimer = 0f;
        }

        public Vector2 GetThumbstickValue()
        {
            return thumbstickAction?.action?.ReadValue<Vector2>() ?? Vector2.zero;
        }

        public Vector2 GetTrackpadValue()
        {
            return trackpadAction?.action?.ReadValue<Vector2>() ?? Vector2.zero;
        }

        public float GetTriggerValue()
        {
            return triggerAction?.action?.ReadValue<float>() ?? 0f;
        }

        public float GetGripValue()
        {
            return gripAction?.action?.ReadValue<float>() ?? 0f;
        }

        public bool IsImmersiveModeActive()
        {
            return isImmersiveModeActive;
        }

        public Vector3 GetImmersivePosition()
        {
            return immersivePosition;
        }

        public Quaternion GetImmersiveRotation()
        {
            return immersiveRotation;
        }

        private void OnDestroy()
        {
            // Clean up input action subscriptions
            if (triggerAction != null)
                triggerAction.action.performed -= OnTriggerPressed;

            if (gripAction != null)
                gripAction.action.performed -= OnGripPressed;

            if (primaryButtonAction != null)
                primaryButtonAction.action.performed -= OnPrimaryButtonPressed;

            if (secondaryButtonAction != null)
                secondaryButtonAction.action.performed -= OnSecondaryButtonPressed;
        }
    }
}
