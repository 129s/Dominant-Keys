using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml;
using Godot;


struct Tuning {
    public static float A4 = 440.0F;
	public static string temperament = "12-TET";
    public static double TETScaler = Math.Exp(Math.Log(2)/12);
	public enum NameSet {C=1,Db,D,Eb,E,F,Gb,G,Ab,A,Bb,B}
}


[GlobalClass]
public partial class Note : Button
{

    private enum _KeyType {
        White,
        Black
    }

    private enum _KeyName {
        C=1,Db,D,Eb,E,F,Gb,G,Ab,A,Bb,B
    }

    [Export] private _KeyType KeyType {set;get;}
    [Export] private _KeyName KeyName {set;get;}
    [Export(PropertyHint.Range,"0,8")] public byte Range;
    [Export] public int BitFlag;

    
    public override void _Ready()
    {
        AddToGroup("Notes");
        Range = ((NoteGroup)Owner).Range;
        BitFlag = 1<<((int)KeyName-1);//index begins with 1
        // GD.Print(Range);
        ActionMode = ActionModeEnum.Press;
        ToggleMode = true;
    }
  

    // private float _get_pitch(_KeyName Keyname,byte Range){
	// 	float A = (float)(Tuning.A4 * Math.Pow(2,Range-4));
    //     // GD.Print("\nStandard A in this Group:",A);
	// 	byte A_index = (byte)Tuning.NameSet.A;
	// 	byte index = (byte)Keyname;
    //     // GD.Print("keyname:",
    //     //         Keyname);
    //     // GD.Print("Frequency:",(float)(A * Math.Pow(Tuning.TETScaler,index-A_index)));
	// 	return (float)(A * Math.Pow(Tuning.TETScaler,index-A_index));
	// }

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
        PitchNameSwitch = !PitchNameSwitch;
        ((NoteGroup)Owner).BitMask += BitFlag;
        // GD.Print(ButtonPressed);
    }
    public void Reset(){
        this.ButtonPressed = false;
        PitchNameSwitch = this.ButtonPressed;
    }



}