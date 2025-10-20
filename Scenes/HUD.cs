using Godot;
using System;

public partial class HUD : CanvasLayer
{
	private Label _keyCountLabel;
	
	public override void _Ready()
	{
		
		_keyCountLabel = GetNode<Label>("MarginContainer/KeyCountLabel");
		var globalManager = GlobalManager.Instance; // Use the static Instance property

		if (globalManager == null)
		{
			GD.PrintErr("HUD could not find the GlobalManager instance.");
			return;
		}
		
		globalManager.KeyCountUpdated += UpdateKeyDisplay;
		UpdateKeyDisplay(globalManager.NumCollectedKeys, globalManager.NumTotalKeys);
		
	}
	
	private void UpdateKeyDisplay(int collected, int total)
	{
		// Update the Label's text property
		_keyCountLabel.Text = $"Keys: {collected} / {total}";
	}
	
	public override void _ExitTree()
	{
		// Cleanly unsubscribe from the signal to prevent errors
		if (GlobalManager.Instance != null)
		{
			GlobalManager.Instance.KeyCountUpdated -= UpdateKeyDisplay;
		}
	}
}
