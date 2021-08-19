using PichaLib;

using Godot;

public class TfxNumber : RichTextEffect
{
    // Syntax: [number][/number]
    public string bbcode = "number";

    private int _COMMA = (int)',';
    private int _SPACE = (int)' ';
    private int _PERIOD = (int)'.';

    private bool _LAST_CHAR_NUM = false;
    private bool _LAST_WORD_NUM = false;

    public override bool _ProcessCustomFx(CharFXTransform charFx)
    {
        var _color = charFx.Env.GetDefault<Color>("color", new Color(1f, 1f, 0f, 1f));

        // reset on first character
        if(charFx.RelativeIndex == 0)
        {
            this._LAST_CHAR_NUM = false;
            this._LAST_WORD_NUM = false;
        }

        // if the following is a word, and it came after a number, color it
        if(charFx.Character == this._SPACE)
        {
            if(this._LAST_CHAR_NUM)
            {
                this._LAST_WORD_NUM = true;
            }
            else 
            {
                this._LAST_WORD_NUM = false;
            }
        }

        // color characters after word, except period
        if(this._LAST_WORD_NUM && charFx.Character != this._PERIOD)
        {
            charFx.Color = _color;
        }

        // if char is number, color it
        if(charFx.Character >= 48 && charFx.Character <= 57)
        {
            charFx.Color = _color;
            this._LAST_CHAR_NUM = true;
        }
        // color trailing commas and periods
        else if(this._LAST_CHAR_NUM && charFx.Character == this._COMMA)
        {
            charFx.Color = _color;
            this._LAST_CHAR_NUM = false;
        }
        else
        {
            this._LAST_CHAR_NUM = false;
        }

        return true;
    }
}