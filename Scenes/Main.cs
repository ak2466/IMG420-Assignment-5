// In Main.cs:
using Godot;
using System;
using System.Linq;

public partial class Main : Node2D
{
	// Handler method—this is what runs when the signal is received
	private void OnKeyCollected()
	{
		GD.Print("MANAGER: Signal RECEIVED! Key (Pickup.cs) was collected."); 
		// Logic for tracking keys collected...
	}

	public override void _Ready()
	{
		// CRITICAL STEP: Defer the connection to run after the TileMap instantiates children.
		CallDeferred(nameof(ConnectAllKeys));
	}

	private void ConnectAllKeys()
	{
		// 1. Get the collection using the case-sensitive GROUP NAME: "Keys"
		var nodesInGroup = GetTree().GetNodesInGroup("Keys");
		
		// 2. Iterate and safely cast each node to the C# CLASS NAME: 'Pickup'
		foreach (Node node in nodesInGroup)
		{
			// The 'is' operator checks if it's the right type and casts it to 'pickupInstance'
			if (node is Pickup pickupInstance) 
			{
				// 3. Connect the C# event-like delegate to the local handler method
				pickupInstance.ItemCollected += OnKeyCollected;
				GD.Print($"MANAGER: Successfully connected to pickup instance: {pickupInstance.Name}.");
			}
		}
		
		if (nodesInGroup.Count == 0)
		{
			GD.PrintErr("CRITICAL: GetNodesInGroup('Keys') returned zero. Check group name and spelling.");
		}
	}
}
