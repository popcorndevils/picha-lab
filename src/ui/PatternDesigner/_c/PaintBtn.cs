using System.Collections.Generic;

using Godot;

using PichaLib;
using OctavianLib;

public class PaintBtn : MenuButton
{
    private CrossTable<int, string> _PixelIDs = new CrossTable<int, string>();
    private Dictionary<string, Pixel> _Pixels = new Dictionary<string, Pixel>();
    private Image _Image = new Image();
    private ImageTexture _IconTexture = new ImageTexture();

    public Pixel Selected;

    public Color Color {
        get => this._Pixels[this._PixelIDs[this.GetPopup().GetCurrentIndex()]].Paint.ToGodotColor();
        private set {
            this._Image.Lock();
            this._Image.Fill(value);
            this._Image.Unlock();
            this._IconTexture.CreateFromImage(this._Image, 0);
            this.Icon = this._IconTexture;
        }
    }

    public override void _Ready()
    {
        this._Image.Create(8, 8, false, Image.Format.Rgba8);
        this.Color = new Color(1f, 1f, 1f, 1f);
        this.GetPopup().Connect("id_pressed", this, "OnItemSelected");
    }

    public void LoadLayer(GenLayer l)
    {
        this._PixelIDs.Clear();
        this._Pixels.Clear();
        var _pop = this.GetPopup();

        _pop.Clear();

        foreach(KeyValuePair<string, Pixel> _pair in l.Pixels)
        {

            int _i = this._PixelIDs.Count;
            this._Pixels.Add(_pair.Key, _pair.Value);
            this._PixelIDs.Add(_i, _pair.Value.Name);

            var _im = new Image();
            var _imTex = new ImageTexture();

            _im.Create(8, 8, false, Image.Format.Rgba8);

            _im.Lock();
            _im.Fill(_pair.Value.Paint.ToGodotColor());
            _im.Unlock();

            _imTex.CreateFromImage(_im, 0);

            _pop.AddIconItem(_imTex, _pair.Value.Name, _i);
        }

        var _pixel = this._Pixels[this._PixelIDs[0]];

        this.Color = _pixel.Paint.ToGodotColor();
        this.Text = _pixel.Name;
        this.Selected = _pixel;
    }

    public void OnItemSelected(int i)
    {
        var _pixel = this._Pixels[this._PixelIDs[i]];
        this.Color = _pixel.Paint.ToGodotColor();
        this.Text = _pixel.Name;
        this.Selected = _pixel;
    }

}
