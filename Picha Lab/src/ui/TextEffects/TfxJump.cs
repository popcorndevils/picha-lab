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

        var angle = Mathf.Deg2Rad(charFx.Env.GetDefault<float>("angle", 3.141f));
        charFx.Offset = new Vector2(charFx.Offset.x + Mathf.Sin(angle) * t, charFx.Offset.y + Mathf.Sin(angle) * t);

        return true;
    }
}