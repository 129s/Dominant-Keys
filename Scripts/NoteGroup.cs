using Godot;
using System;

[GlobalClass]
public partial class NoteGroup : PanelContainer
{
	[Export(PropertyHint.Range,"0,8")] public byte Range;
}
