# Godot Assignment 5

## Shader & Particles: Wave Distortion Shader

For this, I modified one of the particle scenes from my previous game in Assignment 4. I replaced the particle texture with the Godot logo, so the wave distortion would be visible. The wave distortion works by modulating the UV texture's X value with a sine function with time as an input so it changes over time. The gradient works by having a GradientTexture1D that is interpolated with the texture function. The direction is determined by an angle, which is then split into X and Y values for the UV by utilizing Cos and Sin respectively.

## Physics & Joints: Interactive Chain

This code works by generating multiple RigidBody2D's, linked by PinJoints. The top PinJoint is attached to an invisible StaticBody2D which prevents the chain from falling due to Gravity. Player interaction is handled by monitoring for collisions with RigidBody2D's in the Player script. If a collision is detected, an impulse is applied to the chain using the Player's velocity Vector2.

## Raycasting: Laser Detector

RayCasting is implemented using a RayCast2D node generated programatically in code. The visuals are handled by programatically generating a Line2D in the LaserDetector script. The RayCast2D's IsColliding method is listened to. If it detects the player, it triggers the alarm function, which makes the laser turn red and pulse. The alarm also emits a signal other scripts can listen to.


