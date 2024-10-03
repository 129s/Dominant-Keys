


using Godot;
using System;

public partial class Keys : MarginContainer
{
	// Called when the node enters the scene tree for the first time.
	private static byte GroupAmount = 3;//***todo***

	private static byte InitialRange = 4;

	private KeyGroup[] Group = new KeyGroup[GroupAmount];
	private PackedScene _KeyGroup = GD.Load<PackedScene>("tscn/Interface/Key_group.tscn");
	public override void _Ready()
	{
		for (byte i=0;i<GroupAmount;i++){

			var CurrentGroup = _KeyGroup.Instantiate<KeyGroup>();
			CurrentGroup.Range = InitialRange;
			CurrentGroup.Range += i;
			this.GetChild(0).AddChild(CurrentGroup);
			Group[i] = CurrentGroup;//***Todo***
		}
	}
}
