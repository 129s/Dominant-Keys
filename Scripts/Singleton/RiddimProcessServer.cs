using Godot;
using Setting;

public class Note{
    private int Bpm = TimeSettings.Bpm;
    private int Numerator = TimeSettings.Numerator;
    private int Denominator = TimeSettings.Denominator;
    public Key Key;
    public float Duration;
    public int Velocity;
}



public partial class RiddimProcessServer
{

    public static RiddimProcessServer Instance{get;set;}
    private int Bpm = TimeSettings.Bpm;
    private int Numerator = TimeSettings.Numerator;
    private int Denominator = TimeSettings.Denominator;



    public void _ready(){
        Instance = this;
    }

}
