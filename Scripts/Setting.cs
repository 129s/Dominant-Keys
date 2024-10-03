using System;
namespace Setting;
struct Tuning {
	public static float A4 = 440.0F;
	public static string temperament = "12-TET";
	public static double TETScaler = Math.Exp(Math.Log(2)/12);
	public enum NameSet {C,Db,D,Eb,E,F,Gb,G,Ab,A,Bb,B}
}
struct AudioSettings {
	public static float mix_rate = 441000F;
	public enum BasicWaves {Sine,Square,Triangle,Pulse,Saw,Random}
}

struct TimeSettings {
    public static int Bpm = 120;
    public static int Numerator = 4;
    public static int Denominator = 4;
}