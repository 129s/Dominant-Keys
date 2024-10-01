using Godot;
using System;

public partial class OptionsBar : HBoxContainer
{
    public static OptionsBar Instance { get; private set; }
    public override void _Ready()
    {
        Instance = this;
    }
}
