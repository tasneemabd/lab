# WebXR Immersive Web Emulator Integration

This document explains how to use the immersive-web-emulator integration for WebXR controllers in your Unity project.

## Overview

The immersive-web-emulator integration provides enhanced controller support for WebXR applications, including:

- **Enhanced Controller Support**: Improved controller tracking and input handling
- **Gesture Recognition**: Advanced gesture detection and recognition
- **Eye Tracking**: Optional eye gaze interaction support
- **Spatial Audio**: Immersive audio positioning
- **Haptic Feedback**: Enhanced haptic feedback system
- **Proximity Detection**: Object proximity awareness

## Components

### 1. WebXRImmersiveController
The main controller component that handles immersive-web-emulator integration.

**Features:**
- Enhanced input handling
- Haptic feedback support
- Visual feedback (color changes)
- Immersive position and rotation tracking

**Setup:**
1. Add the `WebXRImmersiveController` component to your controller GameObjects
2. Assign the appropriate Input Action References for trigger, grip, buttons, etc.
3. Configure the immersive emulator settings
4. Set up visual feedback materials and colors

### 2. WebXRImmersiveInteractionManager
Manages all immersive interactions and provides centralized control.

**Features:**
- Gesture recognition
- Eye tracking support
- Spatial audio management
- Proximity detection
- Interaction coordination

**Setup:**
1. Add the `WebXRImmersiveInteractionManager` component to a GameObject in your scene
2. Assign left and right controller references
3. Configure interaction settings
4. Enable desired immersive features

### 3. WebXRImmersiveGrabInteractable
Enhanced grab interactable specifically designed for immersive-web-emulator.

**Features:**
- Immersive grabbing with extended range
- Gesture-based grabbing
- Eye gaze grabbing
- Enhanced visual feedback
- Proximity haptics

**Setup:**
1. Replace or add to existing `XRGrabInteractable` components
2. Configure immersive grabbing settings
3. Set up visual feedback materials
4. Enable desired interaction features

### 4. WebXRImmersiveConfig
Configuration manager for easy setup and management of immersive features.

**Features:**
- Automatic setup of immersive components
- Configuration management
- Performance optimization
- Runtime feature toggling

**Setup:**
1. Add the `WebXRImmersiveConfig` component to a GameObject in your scene
2. Configure the settings in the inspector
3. Enable auto-setup for automatic configuration
4. Use public methods for runtime control

## Quick Setup

### Step 1: Enable Immersive Web Emulator
1. Open the WebXR Settings (`Assets/XR/Settings/WebXRSettings.asset`)
2. Ensure `EnableImmersiveWebEmulator` is set to `1`

### Step 2: Add Configuration Component
1. Create an empty GameObject in your scene
2. Add the `WebXRImmersiveConfig` component
3. Enable `Auto Setup Immersive Mode`

### Step 3: Configure Controllers
1. Find your existing XR controllers
2. Add the `WebXRImmersiveController` component
3. Assign Input Action References
4. Configure visual feedback settings

### Step 4: Setup Interaction Manager
1. The `WebXRImmersiveConfig` will automatically create the interaction manager
2. Or manually add `WebXRImmersiveInteractionManager` to a GameObject
3. Assign controller references
4. Configure interaction settings

### Step 5: Configure Interactable Objects
1. For existing interactable objects, the system will automatically convert them
2. Or manually add `WebXRImmersiveGrabInteractable` components
3. Configure grabbing settings and visual feedback

## Configuration Options

### WebXRImmersiveController Settings
- **Enable Immersive Emulator**: Toggle immersive-web-emulator support
- **Haptic Intensity**: Control haptic feedback strength
- **Haptic Duration**: Set haptic feedback duration
- **Controller Model**: Assign 3D model for visual representation
- **Controller Material**: Set material for visual feedback
- **Color Settings**: Configure idle, active, and haptic colors

### WebXRImmersiveInteractionManager Settings
- **Enable Immersive Mode**: Toggle immersive interaction system
- **Interaction Distance**: Set maximum interaction range
- **Interactable Layers**: Define which layers are interactable
- **Enable Hand Tracking**: Toggle hand tracking support
- **Enable Eye Tracking**: Toggle eye tracking support
- **Grab Threshold**: Set grip threshold for grabbing
- **Release Threshold**: Set grip threshold for releasing
- **Enable Haptic Feedback**: Toggle haptic feedback
- **Enable Gesture Recognition**: Toggle gesture recognition
- **Enable Voice Commands**: Toggle voice command support
- **Enable Spatial Audio**: Toggle spatial audio positioning

### WebXRImmersiveGrabInteractable Settings
- **Enable Immersive Grabbing**: Toggle immersive grabbing features
- **Immersive Grab Distance**: Set extended grab range
- **Enable Gesture Grabbing**: Toggle gesture-based grabbing
- **Enable Eye Gaze Grabbing**: Toggle eye gaze grabbing
- **Eye Gaze Distance**: Set eye gaze interaction range
- **Enable Haptic Feedback**: Toggle haptic feedback
- **Grab Haptic Intensity**: Set haptic intensity for grabbing
- **Release Haptic Intensity**: Set haptic intensity for releasing
- **Enable Proximity Haptics**: Toggle proximity-based haptics
- **Proximity Haptic Distance**: Set proximity detection range
- **Enable Outline**: Toggle outline visual feedback
- **Outline Color**: Set outline color
- **Outline Width**: Set outline thickness
- **Enable Immersive Physics**: Toggle immersive physics
- **Immersive Mass**: Set object mass for immersive physics
- **Immersive Drag**: Set drag coefficient
- **Immersive Angular Drag**: Set angular drag coefficient

### WebXRImmersiveConfig Settings
- **Auto Setup Immersive Mode**: Enable automatic setup
- **Enable Immersive Controllers**: Toggle controller setup
- **Enable Immersive Interactions**: Toggle interaction setup
- **Enable Immersive Physics**: Toggle physics setup
- **Controller Prefabs**: Assign controller prefabs
- **Controller Parents**: Assign parent transforms
- **Interaction Manager Prefab**: Assign interaction manager prefab
- **Interaction Manager Parent**: Assign parent transform
- **Enable Gesture Recognition**: Toggle gesture recognition
- **Enable Eye Tracking**: Toggle eye tracking
- **Enable Spatial Audio**: Toggle spatial audio
- **Enable Haptic Feedback**: Toggle haptic feedback
- **Enable Proximity Detection**: Toggle proximity detection
- **Max Interactable Objects**: Set maximum number of interactable objects
- **Interaction Update Rate**: Set interaction update frequency
- **Enable Optimization**: Toggle performance optimization

## Usage Examples

### Basic Controller Setup
```csharp
// Get the immersive controller
WebXRImmersiveController controller = GetComponent<WebXRImmersiveController>();

// Trigger haptic feedback
controller.TriggerHapticFeedback(0.5f);

// Get controller values
Vector2 thumbstickValue = controller.GetThumbstickValue();
float triggerValue = controller.GetTriggerValue();
float gripValue = controller.GetGripValue();
```

### Interaction Manager Usage
```csharp
// Get the interaction manager
WebXRImmersiveInteractionManager manager = FindObjectOfType<WebXRImmersiveInteractionManager>();

// Check immersive session status
bool isActive = manager.IsImmersiveSessionActive();

// Get immersive origin and orientation
Vector3 origin = manager.GetImmersiveOrigin();
Quaternion orientation = manager.GetImmersiveOrientation();

// Get current interactable objects
List<IXRInteractable> interactables = manager.GetCurrentInteractables();

// Enable/disable features
manager.SetGestureRecognition(true);
manager.SetEyeTracking(true);
manager.SetSpatialAudio(true);
```

### Configuration Management
```csharp
// Get the configuration manager
WebXRImmersiveConfig config = FindObjectOfType<WebXRImmersiveConfig>();

// Check configuration status
bool isConfigured = config.IsConfigured();
bool isImmersiveActive = config.IsImmersiveModeActive();

// Get component references
WebXRImmersiveController leftController = config.GetLeftController();
WebXRImmersiveController rightController = config.GetRightController();
WebXRImmersiveInteractionManager manager = config.GetInteractionManager();

// Add/remove interactable objects
config.AddImmersiveInteractable(gameObject);
config.RemoveImmersiveInteractable(gameObject);

// Enable/disable immersive mode
config.SetImmersiveMode(true);

// Optimize system
config.OptimizeSystem();
```

## Troubleshooting

### Common Issues

1. **Controllers not working**
   - Ensure WebXR packages are installed
   - Check Input Action References are assigned
   - Verify WebXR settings are configured

2. **No haptic feedback**
   - Check haptic feedback is enabled
   - Verify controller supports haptics
   - Ensure haptic intensity is above 0

3. **Interactions not working**
   - Check interaction manager is present
   - Verify interactable objects have correct components
   - Ensure interaction distance is appropriate

4. **Performance issues**
   - Enable optimization in config
   - Reduce max interactable objects
   - Lower interaction update rate

### Debug Information

The system provides extensive debug logging. Check the Console for:
- Setup and initialization messages
- Interaction events
- Haptic feedback triggers
- Gesture recognition events
- Error messages

## Performance Considerations

- **Limit interactable objects**: Set appropriate max count
- **Optimize update rates**: Lower rates for better performance
- **Use object pooling**: For frequently created/destroyed objects
- **Enable optimization**: Use built-in optimization features
- **Monitor frame rate**: Ensure smooth VR experience

## Browser Compatibility

The immersive-web-emulator integration works with:
- Chrome (recommended)
- Firefox
- Edge
- Safari (limited support)

Ensure your browser supports WebXR and immersive-web-emulator features.

## Additional Resources

- [WebXR Documentation](https://developer.mozilla.org/en-US/docs/Web/API/WebXR_Device_API)
- [Immersive Web Emulator](https://github.com/immersive-web/webxr-emulator)
- [Unity XR Interaction Toolkit](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@latest)
- [Unity Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest)

## Support

For issues and questions:
1. Check the troubleshooting section
2. Review debug logs
3. Verify configuration settings
4. Test with different browsers
5. Check WebXR compatibility
