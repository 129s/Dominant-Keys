using Godot;
using System;
using Setting;


public partial class OscAudioStreamPlayer : AudioStreamPlayer
{
	public Random random = new Random();
	
    private enum _KeyName {
        C,Db,D,Eb,E,F,Gb,G,Ab,A,Bb,B
    }

    public static OscAudioStreamPlayer Instance { get; set; }
	private AudioStreamGeneratorPlayback playback = null;

    private AudioSettings.BasicWaves Wave;

    public float value;

	public int[] BitMasks = new int[7];//Up to 7 KeyGroups

	public float VoiceAmount;

	public bool KeyChanged = false;



    AudioStreamGenerator AudioStreamGenerator = new AudioStreamGenerator();


	public override void _Ready(){

		Instance = this;
        Stream = AudioStreamGenerator;
        // MaxPolyphony = 32767;
        Play();
		// GD.Print(Player);
		var generator = (AudioStreamGenerator)Instance.Stream;
		// GD.Print(generator);
		generator.MixRate = AudioSettings.mix_rate;
		playback = (AudioStreamGeneratorPlayback)Instance.GetStreamPlayback();

		((OptionButton)OptionsBar.Instance.FindChild("Waves")).ItemSelected += 
		(index) => Wave = (AudioSettings.BasicWaves)index;//OptionsBar must precede this singleton into SceneTree
	}

    public override void _PhysicsProcess(double delta)
    {
        _fill_buffer();
    }

    private static int MaxKey = 12*10;
    private float[] phase = new float[MaxKey];
	internal void _fill_buffer(){
		int to_fill = playback.GetFramesAvailable();

		while (to_fill >0){
			
			if (KeyChanged){
				value = 0;
				VoiceAmount = 0;

				for (int range=0;range<7;range++){
					int BitMask = BitMasks[range];
					if (BitMask == 0){
						continue;
					}
					for (int i=0;BitMask!=0;i++){
						if ((BitMask&1)==1){
							VoiceAmount++;
							value += calculate_value(Wave,i,range);
						}
						BitMask >>= 1 ;
					}
				}
			}
			
			playback.PushFrame(Vector2.One * value);
			to_fill-=1;

			KeyChanged ^= true;
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
	

 	internal float calculate_value(AudioSettings.BasicWaves Wave,int index,int range){
		float Amp = 0.2f;
		float result;
		float frequency = _get_frequency(index,range);
		float increment = frequency / AudioSettings.mix_rate;
		result = Amp*(float)Choose_Wave(Wave,index,range);
		phase[index+range*12] = (float)Mathf.PosMod(phase[index+range*12]+increment,1.0);
		return result;
    }

	float Choose_Wave(AudioSettings.BasicWaves Wave,int index,int range){
		float CurrentPhase = phase[index+range*12];
		switch(Wave){
			case AudioSettings.BasicWaves.Sine:
				return (float)Mathf.Sin(Math.Tau*CurrentPhase);
			case AudioSettings.BasicWaves.Square:
				return (CurrentPhase<0.5)?0:1;
			case AudioSettings.BasicWaves.Triangle:
				return (CurrentPhase<0.5)?(2*CurrentPhase-0.5F):-2*(CurrentPhase-0.75F);
			case AudioSettings.BasicWaves.Pulse:
				return (CurrentPhase<0.25)?0:1;
			case AudioSettings.BasicWaves.Saw:
				return CurrentPhase-0.5F;
			case AudioSettings.BasicWaves.Random:
				return 0;
			default:
				return 0;
	    }
    }


}

