using Newtonsoft.Json;

using Godot;

using PichaLib;

public class AboutDiaglog : WindowDialog
{
    public SpriteExporter Exporter;
    public Canvas Canvas;

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
        this.Exporter = new SpriteExporter();

        using(var _file = new File()) {
            _file.Open("res://res/icons/Riblet.plab", File.ModeFlags.Read);
            this.Canvas = JsonConvert.DeserializeObject<Canvas>(_file.GetAsText());
        }

        // NOTE: sample code for dealing with canvas sprite
        // var test = this.Canvas.Generate();
        // var test2 = test.ToImage();
        // test2.Unlock();
        // test2.Resize(test2.GetWidth() * 100, test2.GetHeight() * 100, Image.Interpolation.Nearest);
        // test2.Lock();
        // test2.SavePng("test2.png");

        this.Texture = this._GetTexture();
        this.Clock.Connect("timeout", this, "OnTimeout");
    }

    private ImageTexture _GetTexture()
    {
        var _output = new ImageTexture();
        _output.CreateFromImage(this.Exporter.GetSpriteFrame(this.Canvas, 0, 30), 0);
        return _output;
    }

    public void OpenAbout()
    {
        this.PopupCentered();
    }

    public void OnTimeout()
    {
        // this.Texture = this._GetTexture();
    }
}
