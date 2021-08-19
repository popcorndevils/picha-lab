using System.Collections.Generic;
using Godot;

public class TfxJump : RichTextEffect
{
    // Syntax: [jump angle=3.141][/jump]
    public string bbcode = "jump";
    public List<int> SPLITTERS = new List<int>() {
        (int)' ', (int)'.', (int)','
    };

    int _w_char = 0;
    int _last = 999;

    public override bool _ProcessCustomFx(CharFXTransform charFx)
    {
        if((int)charFx.AbsoluteIndex < _last || this.SPLITTERS.Contains(charFx.Character))
        {
            this._w_char = (int)charFx.AbsoluteIndex;
        }

        this._last = (int)charFx.AbsoluteIndex;
        var t = Mathf.Abs(Mathf.Sin(charFx.ElapsedTime * 8f + this._w_char * Mathf.Pi * .025f)) * 4f;

        var _angle = charFx.Env.GetDefault<float>("angle", 0f) + 180f;

        _angle = Mathf.Deg2Rad(Mathf.Wrap(_angle, 0f, 360f));
        
        var _x = charFx.Offset.x + (Mathf.Sin(_angle) * t);
        var _y = charFx.Offset.y + (Mathf.Cos(_angle) * t);

        charFx.Offset = new Vector2(_x, _y);

        return true;
    }
}