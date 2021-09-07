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

        using(var _file = new File()) {
            _file.Open("res://res/icons/Riblet.plab", File.ModeFlags.Read);
            var _dat = JsonConvert.DeserializeObject<Canvas>(_file.GetAsText());
            this.Canvas = new SpriteExporter(_dat);
        }

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
