using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

struct Setting{
	public static float mix_rate = 441000F;
	public enum BasicWaves {Sine,Square,Triangle,Pulse,Saw,Random}
};



public partial class OscAudioStreamPlayer : AudioStreamPlayer
{
	[Flags]

    private enum _KeyName {
        C,Db,D,Eb,E,F,Gb,G,Ab,A,Bb,B
    }

    public static OscAudioStreamPlayer Instance { get; set; }
	private AudioStreamGeneratorPlayback playback = null;

    private Setting.BasicWaves Wave;

    public float value;

	public int[] BitMasks = new int[7];//Up to 7 NoteGroups



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

    public override void _PhysicsProcess(double delta)
    {
        _fill_buffer();
    }

    private static int MaxNote = 12*10;
    private float[] phase = new float[MaxNote];

	internal void _fill_buffer(){
		int to_fill = playback.GetFramesAvailable();

		while (to_fill >0){
			value = 0;

			for (int range=0;range<7;range++){
				int BitMask = BitMasks[range];
				if (BitMask == 0){
					continue;
				}
				for (int i=0;BitMask!=0;i++){
					if ((BitMask&1)==1){
						value += calculate_value(Wave,i,range);
					}
					BitMask >>= 1 ;
				}
			}

			playback.PushFrame(Vector2.One * value);
			to_fill-=1;
		}
    }

    private float _get_frequency(int i,int range){
		float A = (float)(Tuning.A4 * Math.Pow(2,range-4));
        // GD.Print("\nStandard A in this Group:",A);
		byte A_index = (byte)Tuning.NameSet.A;
		byte index = (byte)i;
        // GD.Print("i:",i);
        // GD.Print("Frequency:",(float)(A * Math.Pow(Tuning.TETScaler,index-A_index)));
		return (float)(A * Math.Pow(Tuning.TETScaler,index-A_index));
	}
	

 	internal float calculate_value(Setting.BasicWaves Wave,int index,int range){
		float result;
		float frequency = _get_frequency(index,range);
		float increment = frequency / Setting.mix_rate;
		result = (0.1f)*(float)Choose_Wave(Wave,index,range);
		phase[index+range*12] = (float)Mathf.PosMod(phase[index+range*12]+increment,1.0);
		return result;
    }

	float Choose_Wave(Setting.BasicWaves Wave,int index,int range){
		float CurrentPhase = phase[index+range*12];
		switch(Wave){
			case Setting.BasicWaves.Sine:
				return (float)Mathf.Sin(Math.Tau*CurrentPhase);
			case Setting.BasicWaves.Square:
				return (CurrentPhase<0.5)?0:1;
			case Setting.BasicWaves.Triangle:
				return (CurrentPhase<0.5)?(2*CurrentPhase-0.5F):-2*(CurrentPhase-0.75F);
			case Setting.BasicWaves.Pulse:
				return (CurrentPhase<0.25)?0:1;
			case Setting.BasicWaves.Saw:
				return CurrentPhase-0.5F;
			case Setting.BasicWaves.Random:
				return 0;
			default:
				return 0;
	    }
    }


}

