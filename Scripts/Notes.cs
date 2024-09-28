using Godot;
using System;

public partial class Notes : MarginContainer
{
	// Called when the node enters the scene tree for the first time.
	private static byte GroupAmount = 2;//***todo***
	private Node[] Group;
	private PackedScene NoteGroup = GD.Load<PackedScene>("Res://tscn/Interface/note_group.tscn");

	public override void _Ready()
	{
		for (int i=0;i<GroupAmount;i++){
			var CurrentGroup = Group[i] = NoteGroup.Instantiate();
			this.GetChild(0).AddChild(CurrentGroup);
			CurrentGroup.
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
