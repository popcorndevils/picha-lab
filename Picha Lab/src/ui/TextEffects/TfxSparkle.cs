using System.Collections.Generic;

using Godot;

public class TfxSparkle : RichTextEffect
{
    // Syntax: [sparkle freq c1 c2 c3][/sparkle]
    public string bbcode = "sparkle";

    private Color _LerpColors(List<Color> colors, float t)
    {
        if(colors.Count == 1)
        {
            return colors[0];
        }
        
        t = Mathf.Wrap(t, 0f, 1f);

        var _scaled = t * (colors.Count - 1f);
        var _from = colors[Mathf.Wrap(Mathf.FloorToInt(_scaled), 0, colors.Count)];
        var _to = colors[Mathf.Wrap(Mathf.FloorToInt(_scaled + 1), 0, colors.Count)];

        t = _scaled - Mathf.Floor(_scaled);
        return _from.LinearInterpolate(_to, t);
    }

    private float _GetRandUnclamped(CharFXTransform charFx)
    {
        return charFx.Character * 33.33f + charFx.AbsoluteIndex * 4545.5454f;
    }

    public override bool _ProcessCustomFx(CharFXTransform charFx)
    {
        var _freq = charFx.Env.GetDefault<float>("freq", 2f);
        Color _c1 = charFx.Env.GetDefault<Color>("c1", charFx.Color);
        Color _c2 = (Color)charFx.Env["c2"];
        Color _c3 = (Color)charFx.Env["c3"];

        var _colors = new List<Color>();
        if(_c1 != null) { _colors.Add(_c1); }
        if(_c2 != null) { _colors.Add(_c2); }
        if(_c3 != null) { _colors.Add(_c3); }

        if(_colors.Count >= 1)
        {
            var _t = Mathf.Sin(charFx.ElapsedTime * _freq + this._GetRandUnclamped(charFx)) * .5f + .5f;
            charFx.Color = this._LerpColors(_colors, _t);
        }

        return true;
    }
}