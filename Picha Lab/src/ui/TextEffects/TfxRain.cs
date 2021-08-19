using Godot;

public class TfxRain : RichTextEffect
{
    // Syntax: [rain][/rain]
    public string bbcode = "rain";

    private float _GetRand(CharFXTransform charFx)
    {
        return this._GetRandUnclamped(charFx) % 1f;
    }

    private float _GetRandUnclamped(CharFXTransform charFx)
    {   
        return charFx.Character * 33.33f + charFx.AbsoluteIndex * 4545.5454f;
    }

    public override bool _ProcessCustomFx(CharFXTransform charFx)
    {
        var _time = charFx.ElapsedTime;
        var _r = this._GetRand(charFx);
        var _t = (_r + _time * .5f) % 1f;

        charFx.Offset = new Vector2(charFx.Offset.x, charFx.Offset.y + (_t * 8f));

        var _c = charFx.Color; 
        charFx.Color = _c.LinearInterpolate(new Color(_c.r, _c.g, _c.b, 0f), _t);

        return true;
    }
}