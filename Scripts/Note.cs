using System;
using System.Diagnostics;
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

    [Export] private _KeyName KeyName {set;get;}
    [Export(PropertyHint.Range,"0,8")] public byte Range;

    PackedScene Osc = GD.Load<PackedScene>("tscn/AudioGenerator/Osc.tscn");
    
    public override void _Ready()
    {
        Osc _Osc = Osc.Instantiate<Osc>();
        this.AddChild(_Osc);
        Range = ((NoteGroup)Owner).Range;
        Text = KeyName.ToString();
        // GD.Print(Range);
        _Osc.frequency = _get_pitch(KeyName,Range);
    }
    

	private float _get_pitch(_KeyName Keyname,byte Range){
		float A = (float)(Tuning.A4 * Math.Pow(2,Range-4));
        GD.Print("\nStandard A in this Group:",A);
		byte A_index = (byte)Tuning.NameSet.A;
		byte index = (byte)Keyname;
        GD.Print("keyname:",
                Keyname);
        GD.Print("Frequency:",(float)(A * Math.Pow(Tuning.TETScaler,index-A_index)));
		return (float)(A * Math.Pow(Tuning.TETScaler,index-A_index));
	}


    public void _pressed(){
        Osc _Osc = this.GetChild<Osc>(0);
        _Osc.Switch = this.ButtonPressed;
        // GD.Print(ButtonPressed);
    }

}