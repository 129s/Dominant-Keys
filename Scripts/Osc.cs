using System;
using System.Security.Cryptography.X509Certificates;
using Godot;

struct Tuning{
	public static float A4 = 440.0F;
	public static string temperament = "12-TET";
	public static float mix_rate = 441000F;
	public enum NameSet {C=1,Db,D,Eb,E,F,Gb,G,Ab,A,Bb,B}
	public enum BasicWaves {Sine,Square,Saw,Triangle,Pulse,Random}
};


public partial class Osc : Node
{
//Convert Note -> Pitch
	internal class note{
		public note(string Note){
			Name = Note[0];
			Range= (byte)Note[1];
		}

		internal Tuning.NameSet name;
		internal byte range;


		public char Name{
			set{if (Enum.IsDefined(typeof(Tuning.NameSet),value)){
					name = (Tuning.NameSet)value;
					}
				}

			get{return (char)name;}
		}

		public byte Range{
			set{if (0<value && value<8){
					range = value;
					}
				}

			get{return range;}
		}


        public override string ToString()
        {
            return name.ToString()+range.ToString();
        }
    }

	private float pitch = Tuning.A4;//Default

	internal float get_pitch(note note){
		float A = (float)(Tuning.A4 * Math.Pow(2,(note.range-4)));
		byte A_index = (byte)Tuning.NameSet.A;
		byte index = (byte)note.Name;
		pitch = (float)(A * Math.Pow(2,((index-A_index)/12)));
		return pitch;
	}


	[Export] public AudioStreamPlayer Player{get;set;}
	private AudioStreamGeneratorPlayback playback = null;
	public override void _Ready()
	{
		if (Player.Stream is AudioStreamGenerator generator){
			//AudioStreamGenerator Osc = new GodotObject
			generator.MixRate = Tuning.mix_rate;
			Player.Play();//playback只有在play后才能get到
			playback = (AudioStreamGeneratorPlayback)Player.GetStreamPlayback();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public float phase = 0.0F;
	public void _physics_process (float delta)
	{
		_fill_buffer(Tuning.BasicWaves.Square);
	}
	internal void _fill_buffer(Tuning.BasicWaves wave){
		float increment = pitch / Tuning.mix_rate;
		int to_fill = playback.GetFramesAvailable();
		while (to_fill >0){
			playback.PushFrame(Vector2.One * (float)calculate_value());//Audio frames are stereo.
			phase = (float)Mathf.PosMod(phase+increment,1.0);
			to_fill-=1;
		}
		
		float calculate_value(){
			switch(wave){
				case Tuning.BasicWaves.Sine:
					return (float)Mathf.Sin(Math.Tau*phase);
				case Tuning.BasicWaves.Square:
					return (phase<0.5)?0:1;
				case Tuning.BasicWaves.Triangle:
					return (phase<0.5)?(2*phase-0.5F):-2*(phase-0.75F);
				case Tuning.BasicWaves.Pulse:
					return (phase<0.25)?0:1;
				case Tuning.BasicWaves.Saw:
					return phase-0.5F;
				case Tuning.BasicWaves.Random:
					return 0;
				default:
					return 0;
			}
		}
	}
}