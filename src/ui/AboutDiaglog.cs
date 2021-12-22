using Newtonsoft.Json;

using Godot;

using PichaLib;

public class AboutDiaglog : WindowDialog
{
    public SpriteExporter Exporter;
    public Canvas Canvas;

    public TextureRect TextureRect;
    public Timer GenClock;
    public Timer AnimClock;

    private Texture[] _Textures;
    private Texture[] Textures {
        get => this._Textures;
        set {
            this._Textures = value;
            this.Index = 0;
            this.AnimClock.WaitTime = (this.GenClock.WaitTime / value.Length) / 2;
            this.AnimClock.Start();
        }
    }

    private int _Index = 0;
    private int Index {
        get => this._Index;
        set {
            if(value >= this.Textures.Length)
            {
                this._Index = 0;
            }
            else
            {
                this._Index = value;
            }
            this.TextureRect.Texture = this.Textures[this._Index];
        }
    }

    public Texture Texture {
        get => this.TextureRect.Texture;
        set => this.TextureRect.Texture = value;
    }

    public override void _Ready()
    {
        this.AddToGroup("diag_about");

        this.TextureRect = this.GetNode<TextureRect>("HBox/Texture");
        this.GenClock = this.GetNode<Timer>("GenTimer");
        this.AnimClock = this.GetNode<Timer>("AnimTimer");


        this.Exporter = new SpriteExporter();

        using(var _file = new File()) {
            _file.Open("res://res/icons/Riblet.plab", File.ModeFlags.Read);
            this.Canvas = JsonConvert.DeserializeObject<Canvas>(_file.GetAsText());
        }

        this.GenClock.Connect("timeout", this, "OnGenTimeout");
        this.AnimClock.Connect("timeout", this, "OnAnimTimeout");

        this.Textures = this._GetTextures();
    }

    private ImageTexture[] _GetTextures()
    {
        var _frames = this.Canvas.GenerateFrames(false, 30);
        var _output = new ImageTexture[_frames.Length];

        for(int i = 0; i < _frames.Length; i++)
        {
            _output[i] = new ImageTexture();
            _output[i].CreateFromImage(_frames[i].ToImage());
        }

        return _output;
    }

    public void OpenAbout()
    {
        this.PopupCentered();
    }

    public void OnAnimTimeout()
    {
        ++this.Index;
    }

    public void OnGenTimeout()
    {
        this.Textures = this._GetTextures();
    }
}
