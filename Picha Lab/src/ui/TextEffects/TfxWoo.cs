using Godot;

public class TfxWoo : RichTextEffect
{
    // Syntax: [woo scale=1.0 freq=8.0][/woo]
    public string bbcode = "woo";

    public override bool _ProcessCustomFx(CharFXTransform charFx)
    {
        var _scale = charFx.Env.GetDefault<float>("scale", 1f);
        var _freq = charFx.Env.GetDefault<float>("freq", 8f);

        if(Mathf.Sin(charFx.ElapsedTime * _freq + charFx.AbsoluteIndex * _scale) < 0)
        {
            if(charFx.Character >= 65 && charFx.Character <= 90)
            {
                charFx.Character += 32;
            }
            else if(charFx.Character >= 97 && charFx.Character <= 122)
            {
                charFx.Character -= 32;
            }
        }

        return true;
    }
}