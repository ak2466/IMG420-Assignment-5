using Godot;
using System.Collections.Generic;

public partial class ChainSystem : Node2D
{
	[Export] private int ChainLinks = 10;
	[Export] private float LinkLength = 30.0f;
	[Export] private PackedScene LinkScene;
	[Export] private Marker2D ChainAnchor;
	
	private List<RigidBody2D> links = new List<RigidBody2D>();
	private List<PinJoint2D> joints = new List<PinJoint2D>();
	
	public override void _Ready()
	{
		if(ChainAnchor == null)
		{
			GD.Print("Chain Anchor is NULL, please set a ChainAnchor Marker2D in the editor");
		}
		CreateChain();
	}
	
	private void CreateChain()
	{
		Vector2 startPos = ChainAnchor.Position;
		RigidBody2D previousLink = null;
		
		// Create anchor point
		var anchor = new StaticBody2D();
		//anchor.FreezeMode = RigidBody2D.FreezeModeEnum.Static;
		anchor.Position = startPos;
		AddChild(anchor);
		previousLink = null;
		
		// Create chain links
		for (int i = 0; i < ChainLinks; i++)
		{
			// Create link body
			var link = LinkScene.Instantiate<RigidBody2D>();
			link.Position = startPos + new Vector2(0, LinkLength * (i + 1));
			link.Mass = 0.5f;
			link.GravityScale = 2.0f;
			
			//// Add collision shape
			//var collision = new CollisionShape2D();
			//var shape = new CapsuleShape2D();
			//shape.Radius = 5;
			//shape.Height = LinkLength;
			//collision.Shape = shape;
			//link.AddChild(collision);
			
			AddChild(link);
			links.Add(link);
			
			// Create joint connecting to previous link
			if (i > 0)
			{
				var joint = new PinJoint2D();
				joint.Position = previousLink.Position + new Vector2(0, LinkLength / 2);
				joint.NodeA = previousLink.GetPath();
				joint.NodeB = link.GetPath();
				joint.Softness = 0.5f;
				AddChild(joint);
				joints.Add(joint);
			}
			else
			{
				var joint = new PinJoint2D();
				joint.Position = anchor.Position + new Vector2(0, LinkLength / 2);
				joint.NodeA = anchor.GetPath();
				joint.NodeB = link.GetPath();
				joint.Softness = 0.5f;
				AddChild(joint);
				joints.Add(joint);
			}
			
			
			previousLink = link;
		}
	}
	
	public void ApplyImpulseToEnd(Vector2 impulse)
	{
		if (links.Count > 0)
		{
			links[links.Count - 1].ApplyImpulse(impulse);
		}
	}
}
