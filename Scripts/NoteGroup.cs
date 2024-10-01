using Godot;
using System;
using System.Runtime.CompilerServices;

[GlobalClass]
public partial class NoteGroup : PanelContainer
{
	[Export(PropertyHint.Range,"0,8")] public byte Range;
}