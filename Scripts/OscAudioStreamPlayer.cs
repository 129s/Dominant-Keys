using Godot;
using System;

struct Setting{
	public static float mix_rate = 441000F;
	public enum BasicWaves {Sine,Square,Triangle,Pulse,Saw,Random}
};



public partial class OscAudioStreamPlayer : AudioStreamPlayer
{
	[Flags]

    private enum _KeyName {
        C=1,Db,D,Eb,E,F,Gb,G,Ab,A,Bb,B
    }

    public static OscAudioStreamPlayer Instance { get; set; }
	private AudioStreamGeneratorPlayback playback = null;

    private Setting.BasicWaves Wave;

    public float value;

    AudioStreamGenerator AudioStreamGenerator = new AudioStreamGenerator();

	public override void _Ready(){
		Instance = this;
        Stream = AudioStreamGenerator;
        // MaxPolyphony = 32767;
        Play();

		// GD.Print(Player);
		var generator = (AudioStreamGenerator)Instance.Stream;
		// GD.Print(generator);
		generator.MixRate = Setting.mix_rate;
		playback = (AudioStreamGeneratorPlayback)Instance.GetStreamPlayback();

		((OptionButton)OptionsBar.Instance.FindChild("Waves")).ItemSelected += 
		(index) => Wave = (Setting.BasicWaves)index;//OptionsBar must precede this singleton into SceneTree
	}

    private float phase = 0.0F;
    private float frequency;
	internal void _fill_buffer(){
		int to_fill = playback.GetFramesAvailable();
		while (to_fill >0){
            value = (float)Mathf.Sin(Math.Tau*phase);
			playback.PushFrame(Vector2.One * value);
            phase = (float)Mathf.PosMod(phase+0.001,1.0);
			to_fill-=1;
		}
        
    }


    private float _get_pitch(_KeyName Keyname,byte Range){
		float A = (float)(Tuning.A4 * Math.Pow(2,Range-4));
        // GD.Print("\nStandard A in this Group:",A);
		byte A_index = (byte)Tuning.NameSet.A;
		byte index = (byte)Keyname;
        // GD.Print("keyname:",
        //         Keyname);
        // GD.Print("Frequency:",(float)(A * Math.Pow(Tuning.TETScaler,index-A_index)));
		return (float)(A * Math.Pow(Tuning.TETScaler,index-A_index));
	}
	
 	internal float calculate_value(Setting.BasicWaves wave){
		float result;
		float increment = frequency / Setting.mix_rate;
		result = (float)Choose_Wave(Wave);
		phase = (float)Mathf.PosMod(phase+increment,1.0);
		return result;
    }

	float Choose_Wave(Setting.BasicWaves Wave){
		switch(Wave){
			case Setting.BasicWaves.Sine:
				return (float)Mathf.Sin(Math.Tau*phase);
			case Setting.BasicWaves.Square:
				return (phase<0.5)?0:1;
			case Setting.BasicWaves.Triangle:
				return (phase<0.5)?(2*phase-0.5F):-2*(phase-0.75F);
			case Setting.BasicWaves.Pulse:
				return (phase<0.25)?0:1;
			case Setting.BasicWaves.Saw:
				return phase-0.5F;
			case Setting.BasicWaves.Random:
				return 0;
			default:
				return 0;
	    }
    }   
}

