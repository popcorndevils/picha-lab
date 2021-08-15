using Godot;

public class TfxGhost : RichTextEffect
{
    // Syntax: [ghost freq=5.0 span=10.0][/ghost]
    public string bbcode = "ghost";
    public override bool _ProcessCustomFx(CharFXTransform charFx)
    {
        var speed = charFx.Env.GetDefault<float>("freq", 5f);
        var span = charFx.Env.GetDefault<float>("span", 10f);
        var alpha = Mathf.Sin(charFx.ElapsedTime * speed + (charFx.AbsoluteIndex / span)) * .5f + .5f;
        charFx.Color = new Color(charFx.Color.r, charFx.Color.g, charFx.Color.b, alpha);
        return true;
    }
}