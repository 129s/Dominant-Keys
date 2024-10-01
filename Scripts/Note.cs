using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml;
using Godot;


struct Tuning {
    public static float A4 = 440.0F;
	public static string temperament = "12-TET";
    public static double TETScaler = Math.Exp(Math.Log(2)/12);
	public enum NameSet {C,Db,D,Eb,E,F,Gb,G,Ab,A,Bb,B}
}


[GlobalClass]
public partial class Note : Button
{

    private enum _KeyType {
        White,
        Black
    }

    private enum _KeyName {
        C,Db,D,Eb,E,F,Gb,G,Ab,A,Bb,B
    }

    [Export] private _KeyType KeyType {set;get;}
    [Export] private _KeyName KeyName {set;get;}
    [Export(PropertyHint.Range,"0,8")] public byte Range;
    [Export] public int BitFlag = 1;

    
    public override void _Ready()
    {
        AddToGroup("Notes");
        Range = ((NoteGroup)Owner).Range;
        BitFlag <<= (byte)KeyName;
        // GD.Print(Range);
        // GD.Print(BitFlag);
        ActionMode = ActionModeEnum.Press;
        ToggleMode = true;
    }

    
    private bool PitchNameSwitch = false;
    public void DisplayPitchName(){
        PitchNameSwitch = !PitchNameSwitch;
        string shuffle = "";
        if (KeyType == _KeyType.White){
            shuffle = "\n\n\n\n\n\n";//VerticalAlign for WhiteNotes
        }
        Text = PitchNameSwitch?shuffle+KeyName.ToString()+Range.ToString():"";
    }

    public void _pressed(){
        OscAudioStreamPlayer.Instance.BitMasks[Range] ^= BitFlag;
        // GD.Print(BitFlag);
        // GD.Print(OscAudioStreamPlayer.Instance.BitMasks[Range]);
    }
    public void Reset(){
        this.ButtonPressed = false;
        PitchNameSwitch = this.ButtonPressed;
        OscAudioStreamPlayer.Instance.BitMasks[Range] = 0;
    }


}