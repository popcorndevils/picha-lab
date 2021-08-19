using Godot;

public class TfxUwu : RichTextEffect
{
    // Syntax: [uwu][/uwu]
    public string bbcode = "uwu";

    public override bool _ProcessCustomFx(CharFXTransform charFx)
    {
        if(charFx.Character == (int)'R' || charFx.Character == (int)'L')
        {
            charFx.Character = (int)'W';
        }
        else if(charFx.Character == (int)'r' || charFx.Character == (int)'l')
        {
            charFx.Character = (int)'w';
        }

        return true;
    }
}