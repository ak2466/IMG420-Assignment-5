# Godot 2D C# Starter Kit

This starter kit provides a minimal Godot 4.4 project skeleton written in C#.  It is designed to help you get started on the 2D game assignment described in the accompanying specification.  The project demonstrates how to implement player movement with simple physics, an enemy that follows the player using 2D navigation, animated sprites, particle effects for collectibles, and a logical project structure.

## Structure

```
godot_starter_kit/
├── Scenes/           # Scene files (.tscn) – you will create these in Godot
├── Scripts/          # C# scripts used by the scenes
│   ├── Player.cs     # Player movement, animation and physics
│   ├── Enemy.cs      # Enemy navigation using NavigationAgent2D
│   └── Pickup.cs     # Simple collectible item that spawns particles
└── README.md         # This file
```

### Player.cs

The **Player** script extends `CharacterBody2D` and handles horizontal movement, gravity and jumping.  It reads input from the `ui_left`, `ui_right` and `ui_accept` actions, applies gravity each frame and calls `MoveAndSlide()` to let the physics engine handle collisions.  It also plays idle or walk animations on an `AnimatedSprite2D` child and flips the sprite based on movement direction.

### Enemy.cs

The **Enemy** script also extends `CharacterBody2D`.  It uses a `NavigationAgent2D` child to compute a path to the player and moves along that path each physics frame.  Assign the target (player) via the `TargetPath` export in the editor.  To enable navigation, you need to add a NavigationLayer to your TileSet and paint navigation on walkable tiles, then add a `NavigationRegion2D` node to your `TileMap`.

### Pickup.cs

The **Pickup** script demonstrates how to trigger a particle effect when the player collects an item.  Attach this script to an `Area2D` with a `CollisionShape2D`, assign a `PackedScene` containing a `Particles2D` to the `ParticlesScene` property, and connect the `body_entered` signal.  When the player touches the pickup, the particle effect is spawned and the pickup node is removed from the scene.

## Next steps

1. **Create scenes**: In Godot, create your scenes for the player, enemy and pickups.  Add the corresponding C# scripts to the root nodes.  For the player, ensure you add an `AnimatedSprite2D` and define at least `idle` and `walk` animations in the SpriteFrames resource.  For the enemy, add a `NavigationAgent2D` child and set its `TargetPath` to the player instance.
2. **TileMap and TileSet**: Build your level using a `TileMap` node and a `TileSet` resource.  Add collision shapes and navigation layers to the tileset so that physics and pathfinding work correctly.  The Godot documentation notes that a `TileMap` is a grid of tiles used to lay out the game and that you must create a `TileSet` first.
3. **Physics and input**: Adjust the exported speed, gravity and jump values in the `Player` script to suit your game.  The top‑down movement recipe shows how to multiply an input vector by a speed and call `MoveAndSlide().
4. **Navigation**: Make sure you enable navigation in your tileset by adding a navigation layer, and attach a `NavigationRegion2D` to your tilemap.  The pathfinding guide explains that you must expand the Navigation Layers section in the TileSet editor and paint walkable areas.
5. **Particle effects**: Create one or more `Particles2D` scenes for use with the pickup script.  Adjust lifetime, speed and randomness to achieve effects such as sparks or smoke.  The Godot 2D particles documentation states that particle systems simulate complex effects like sparks, fire and mist.

With this starter kit you should have a solid foundation for building your assignment 4 project. Review the project requirements and expand upon these scripts, add UI elements, additional enemies, and polish to complete your game.