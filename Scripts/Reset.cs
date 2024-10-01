using Godot;
using System;

public partial class Reset : Button
{
	public void _pressed(){
	GetTree().CallGroup("Notes","Reset");
	}
}
