using System;
using System.Xml;
using Godot;



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


    private void OnButtonPressed(){
        
    }

}