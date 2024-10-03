using Godot;
using System;

public partial class PlayMode : Button
{
	int ModeAmount = 2;
	enum _Mode {
		Chord,
		Arpeggio
	}

	[Export] private _Mode Mode = _Mode.Arpeggio;
	private void _pressed(){
		Mode++;
		int Index = (int)Mode;
		Index %= ModeAmount;
		GD.Print(sizeof(_Mode));
		((AtlasTexture)this.Icon).Region=new Rect2(Index*26,0,26,18);//TextureSize:26*18
	}


}
