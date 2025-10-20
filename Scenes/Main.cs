using Godot;
using System;

public partial class Main : Node2D
{
	private GlobalManager _globalManager;

	public override void _Ready()
	{
		// 1. Get the reference to the Autoload/Singleton instance.
		// We use the path /root/GlobalManager (assuming you named the Autoload 'GlobalManager').
		_globalManager = GetNode<GlobalManager>("/root/GlobalManager");

		if (_globalManager == null)
		{
			GD.PrintErr("CRITICAL: GlobalManager Autoload not found. Check Project Settings -> Autoload.");
			return;
		}

		// 2. Connect to the manager's signal for the victory condition.
		// The '+' operator is the C# way to subscribe to an event-like delegate (Signal).
		_globalManager.AllKeysCollected += OnAllKeysCollected;
		
		GD.Print("Main scene is ready and subscribed to GlobalManager events.");
	}

	// 3. Handler method that executes when the GlobalManager emits the signal.
	private void OnAllKeysCollected()
	{
		GD.Print("LEVEL MANAGER: All keys have been collected! Opening the final door...");
		
		// TO DO: Add logic here to:
		// a) Find the door node (e.g., GetNode<Door>("DoorNode"))
		// b) Call a method to open it (e.g., door.OpenDoor())
		// c) Play a congratulatory sound or animation.
	}
	
	// You may want to unsubscribe from the signal when the node leaves the tree
	public override void _ExitTree()
	{
		// Safely unsubscribe to prevent errors if the manager persists across scenes
		if (_globalManager != null)
		{
			_globalManager.AllKeysCollected -= OnAllKeysCollected;
		}
	}
}
