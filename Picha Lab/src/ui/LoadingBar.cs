using Godot;
using System;

public class LoadingBar : Popup
{

    private RichTextLabel _Text;

    public string Text {
        get => this._Text.BbcodeText;
        set {
            this._Text.Clear();
            this._Text.AppendBbcode(value);
        }
    }
    

    public override void _Ready()
    {
        this.RectSize = new Vector2(300, 20);

        this._Text = this.GetNode<RichTextLabel>("Panel/Margin/VBox/Label");
        this._Text.BbcodeEnabled = true;
    }
}
