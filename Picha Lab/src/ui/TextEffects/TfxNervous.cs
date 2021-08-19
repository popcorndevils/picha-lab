using System.Collections.Generic;
using Godot;

public class TfxNervous : RichTextEffect
{
    // Syntax: [nervous scale=1.0 freq=8.0][/nervous]
    public string bbcode = "nervous";

    public int _Word = 0;

    public List<int> SPLITTERS = new List<int> {
        (int)' ', (int)'.', (int)',', (int)'-'
    };

    public override bool _ProcessCustomFx(CharFXTransform charFx)
    {
        if(charFx.RelativeIndex == 0)
        {
            this._Word = 0;
        }

        float _scale = charFx.Env.GetDefault<float>("scale", 1f);
        float _freq = charFx.Env.GetDefault<float>("freq", 8f);

        if(this.SPLITTERS.Contains(charFx.Character))
        {
            this._Word += 1;
        }

        float _s = ((this._Word + charFx.ElapsedTime) * Mathf.Pi * 1.25f) % (Mathf.Pi * 2f);
        var _p = Mathf.Sin(charFx.ElapsedTime * _freq);

        float _x = charFx.Offset.x + Mathf.Sin(_s) * _p * _scale;
        float _y = charFx.Offset.y + Mathf.Cos(_s) * _p * _scale;

        charFx.Offset = new Vector2(_x, _y);
        
        return true;
    }
}