using System.Collections.Generic;

using Godot;

public class TfxCuss : RichTextEffect
{
    // Syntax: [cuss][/cuss]
    public string bbcode = "cuss";
    public bool _WasSpace = false;

    public List<int> VOWELS = new List<int>() {
        (int)'a',
        (int)'e',
        (int)'i',
        (int)'o',
        (int)'u',
        (int)'A',
        (int)'E',
        (int)'I',
        (int)'O',
        (int)'U',
    };

    public List<int> CHARS = new List<int>() {
        (int)'&',
        (int)'$',
        (int)'!',
        (int)'@',
        (int)'*',
        (int)'#',
        (int)'%',
    };

    public int SPACE = (int)' ';

    public override bool _ProcessCustomFx(CharFXTransform charFx)
    {
        var _c = charFx.Character;

        if(!_WasSpace && charFx.RelativeIndex != 0 && _c != SPACE)
        {
            var _t = charFx.ElapsedTime + charFx.Character * 10.2f + charFx.AbsoluteIndex * 2;
            _t *= 4.3f;

            if(VOWELS.Contains(_c) || Mathf.Sin(_t) > 0.0f)
            {
                charFx.Character = CHARS[(int)_t % CHARS.Count];
            }
        }

        _WasSpace = _c == SPACE;

        return true;
    }
}
