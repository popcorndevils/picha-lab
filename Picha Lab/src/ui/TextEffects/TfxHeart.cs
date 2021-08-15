using System.Collections.Generic;

using PichaLib;

using Godot;

public class TfxHeart : RichTextEffect
{
    // Syntax: [heart scale=1.0 freq=8.0][/heart]
    public string bbcode = "heart";
    
    int HEART = (int)'♡';
    List<int> TO_CHANGE = new List<int>() {(int)'o', (int)'O', (int)'a', (int)'A'};

    public override bool _ProcessCustomFx(CharFXTransform charFx)
    {
        var scale = charFx.Env.GetDefault<float>("scale", 16f);
        var freq = charFx.Env.GetDefault<float>("freq", 2f);

        var x = charFx.AbsoluteIndex / scale - charFx.ElapsedTime * freq;

        var t = Mathf.Abs(Mathf.Cos(x)) * Mathf.Max(0f, Mathf.SmoothStep(0.712f, 0.99f, Mathf.Sin(x))) * 2.5f;

        var _blue = Chroma.CreateFromName("blue").ToGodotColor();
        var _red = Chroma.CreateFromName("red").ToGodotColor();

        charFx.Color = charFx.Color.LinearInterpolate(_blue.LinearInterpolate(_red, t), t);
        charFx.Offset = new Vector2(charFx.Offset.x, charFx.Offset.y - (t * 4f));

        var c = charFx.Character;
        if(charFx.Offset.y < -1f && TO_CHANGE.Contains(c))
        {   
            charFx.Character = HEART;
        }

        return true;
    }
}