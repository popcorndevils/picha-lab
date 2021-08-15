using Godot;

public class TfxPulse : RichTextEffect
{
    // Syntax: [pulse color=#00FFAA height=0.0 freq=2.0][/pulse]
    public string bbcode = "pulse";
    public override bool _ProcessCustomFx(CharFXTransform charFx)
    {
        var color = charFx.Env.GetDefault<Color>("color", charFx.Color);
        float height = charFx.Env.GetDefault<float>("height", 0f);
        float freq = charFx.Env.GetDefault<float>("freq", 2f);

        float sined_time = (float)((Mathf.Sin(charFx.ElapsedTime * freq) + 1.0f) / 2.0);
        float y_off = sined_time * height;
        
        color = new Color(color.r, color.g, color.b, 1f);

        charFx.Color = charFx.Color.LinearInterpolate(color, sined_time);
        charFx.Offset = new Vector2(0, -1f * y_off);

        return true;
    }
}