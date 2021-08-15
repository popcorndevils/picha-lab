using Godot;

public class TfxMatrix : RichTextEffect
{
    // Syntax: [matrix clean=2.0 dirty=1.0 span=50][/matrix]
    public string bbcode = "matrix";
    public override bool _ProcessCustomFx(CharFXTransform charFx)
    {
        float clear_time = charFx.Env.GetDefault<float>("clean", 2f);
        float dirty_time = charFx.Env.GetDefault<float>("dirty", 1f);
        float text_span = charFx.Env.GetDefault<float>("span", 50);

        var value = charFx.Character;

        var matrix_time = (charFx.ElapsedTime + (charFx.AbsoluteIndex / (float)text_span)) % (clear_time + dirty_time);

        matrix_time = matrix_time < clear_time ? 0f : (matrix_time - clear_time) / dirty_time;

        if(value >= 65 && value < 126 && matrix_time > 0f)
        {
            value -= 65;
            value = value + (int)(1 * matrix_time * (126 - 65));
            value %= (126 - 65);
            value += 65;
        }

        charFx.Character = value;
        return true;
    }
}