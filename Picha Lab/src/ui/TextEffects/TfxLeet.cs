using System.Collections.Generic;

using Godot;

public class TfxLeet : RichTextEffect
{
    //  Syntax: [leet][/leet]
    public string bbcode = "leet";

    public Dictionary<int, int> LEET = new Dictionary<int, int>() {
        {(int)'L', (int)'1'},
        {(int)'l', (int)'1'},
        {(int)'I', (int)'1'},
        {(int)'i', (int)'1'},
        {(int)'E', (int)'3'},
        {(int)'e', (int)'3'},
        {(int)'T', (int)'7'},
        {(int)'t', (int)'7'},
        {(int)'S', (int)'5'},
        {(int)'s', (int)'5'},
        {(int)'A', (int)'4'},
        {(int)'a', (int)'4'},
        {(int)'O', (int)'0'},
        {(int)'o', (int)'0'},
    };

    public override bool _ProcessCustomFx(CharFXTransform charFx)
    {
        if(LEET.ContainsKey(charFx.Character))
        {
            charFx.Character = LEET[charFx.Character];
        }
        return true;
    }
}