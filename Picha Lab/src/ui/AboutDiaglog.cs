using System;

using Newtonsoft.Json;

using Godot;

using PichaLib;

public class AboutDiaglog : WindowDialog
{
    public SpriteExporter Canvas;
    public TextureRect TextureRect;
    public Timer Clock;

    public Texture Texture {
        get => this.TextureRect.Texture;
        set => this.TextureRect.Texture = value;
    }

    public override void _Ready()
    {
        this.AddToGroup("diag_about");

        this.TextureRect = this.GetNode<TextureRect>("HBox/Texture");
        this.Clock = this.GetNode<Timer>("Timer");

        var _dat = JsonConvert.DeserializeObject<Canvas>(System.IO.File.ReadAllText("./res/icons/Riblet.plab"));
        this.Canvas = new SpriteExporter(_dat);
        this.Texture = this._GetTexture();

        this.Clock.Connect("timeout", this, "OnTimeout");
    }

    private ImageTexture _GetTexture()
    {
        var _output = new ImageTexture();
        _output.CreateFromImage(this.Canvas.GetSpriteFrame(0, 30), 0);
        return _output;
    }

    public void OpenAbout()
    {
        this.PopupCentered();
    }

    public void OnTimeout()
    {
        this.Texture = this._GetTexture();
    }
}
