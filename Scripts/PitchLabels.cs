using Godot;
using System;

public partial class PitchLabels : CheckButton
{
	private void _pressed(){
		// GD.Print("Pressed");
		GetTree().CallGroup("Notes","DisplayPitchName");
	}

}
