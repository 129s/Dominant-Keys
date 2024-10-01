using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Godot;

struct Setting{
	public static float mix_rate = 441000F;
	public enum BasicWaves {Sine,Square,Saw,Triangle,Pulse,Random}
};


public partial class Osc : Node
{
	[Export] public AudioStreamPlayer Player{get;set;}
	[Export] public bool Switch = false;
	[Export] public float frequency;
	[Export] public float phase = 0.0F;
	private AudioStreamGeneratorPlayback playback = null;
	public override void _Ready()
	{
		if (Player.Stream is AudioStreamGenerator generator){
			generator.MixRate = Setting.mix_rate;
			Player.Play();
			// GD.Print("new");
			playback = (AudioStreamGeneratorPlayback)Player.GetStreamPlayback();
		}
	}
	public void _physics_process (float delta)
	{
		if (Switch){
			_fill_buffer(Setting.BasicWaves.Sine);
		}
	}
	internal void _fill_buffer(Setting.BasicWaves wave){
		float increment = frequency / Setting.mix_rate;
		int to_fill = playback.GetFramesAvailable();
		while (to_fill >0){
			playback.PushFrame(Vector2.One * (float)calculate_value());//Audio frames are stereo.
			phase = (float)Mathf.PosMod(phase+increment,1.0);
			to_fill-=1;
		}
		float calculate_value(){
			switch(wave){
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
}