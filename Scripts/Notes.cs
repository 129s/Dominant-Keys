using Godot;
using System;

public partial class Notes : MarginContainer
{
	// Called when the node enters the scene tree for the first time.
	private static byte GroupAmount = 3;//***todo***

	private static byte InitialRange = 4;

	private NoteGroup[] Group = new NoteGroup[GroupAmount];
	private PackedScene _NoteGroup = GD.Load<PackedScene>("tscn/Interface/note_group.tscn");
	public override void _Ready()
	{
		for (byte i=0;i<GroupAmount;i++){

			var CurrentGroup = _NoteGroup.Instantiate<NoteGroup>();
			CurrentGroup.Range = InitialRange;
			CurrentGroup.Range += i;
			this.GetChild(0).AddChild(CurrentGroup);
			Group[i] = CurrentGroup;//***Todo***
		}
	}
}
