using Godot;
using System;

public partial class GlobalManager : Node
{
	// --- 1. Static Access Property (Recommended for C# Autoloads) ---
	// This allows you to access the manager anywhere with: GlobalManager.Instance.CollectKey()
	public static GlobalManager Instance { get; private set; }
	
	// --- 2. Global State Variables ---
	public int NumCollectedKeys { get; set; } = 0;
	public int NumTotalKeys { get; set; } = 0; 
	
	// --- 3. Custom Signal for Communication (Best Practice) ---
	[Signal] public delegate void AllKeysCollectedEventHandler();
	[Signal] public delegate void KeyCountUpdatedEventHandler(int collected, int total);
	[Signal] public delegate void LockUnlockedEventHandler();

	// --- 4. Godot Lifecycle: Initialization ---
	public override void _Ready()
	{
		// Set the static instance reference.
		// This is only safe to do because it's an Autoload (we know it's unique).
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			// Optional: Handle if somehow a duplicate is created (shouldn't happen with Autoload)
			GD.PrintErr("GlobalManager already exists! Duplicating Autoload.");
			QueueFree();
			return;
		}
		
		CallDeferred(nameof(InitializeKeyTotal));
	}
	
	private void InitializeKeyTotal()
	{
		
		// Count all nodes in the "Keys" group after the scene is loaded.
		Godot.Collections.Array<Node> keys = GetTree().GetNodesInGroup("Keys");
		NumTotalKeys = keys.Count;
		
		GD.Print($"[GlobalManager] Total Keys Initialized: {NumTotalKeys}");
		EmitSignal(SignalName.KeyCountUpdated, NumCollectedKeys, NumTotalKeys);
		
		// TEMPORARY DEBUG: Print the name and type of every node found
		foreach (Node keyNode in keys)
		{
			GD.Print($"  -> Node Name: {keyNode.Name}, Type: {keyNode.GetType().Name}");
		}
	}
	
	public void HandleLockInteraction()
	{
		if(NumCollectedKeys == NumTotalKeys)
		{
			EmitSignal(SignalName.LockUnlocked);
		}
	}
	
	// --- 5. Core Game Logic Method ---
	public void CollectKey()
	{
		NumCollectedKeys++;
		EmitSignal(SignalName.KeyCountUpdated, NumCollectedKeys, NumTotalKeys);
		
		GD.Print($"NumCollectedKeys: {NumCollectedKeys}");

		if (NumCollectedKeys == NumTotalKeys)
		{
			// Emit the signal so other nodes (like a Door or a UI) can react
			EmitSignal(SignalName.AllKeysCollected); 
		}
	}
}
