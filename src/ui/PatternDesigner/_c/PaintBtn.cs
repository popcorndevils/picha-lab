using Godot;

using PichaLib;

public delegate void PaintSelectHandler(Pixel p);

public class PaintBtn : Button
{
    public PaintSelectHandler PaintSelected;
    public Pixel Pixel;

    public PaintBtn()
    {
        this.Pixel = new Pixel();
    }

    public PaintBtn(Pixel p)
    {
        this.Pixel = p;

        var _im = new Image();
        var _imTex = new ImageTexture();

        _im.Create(8, 8, false, Image.Format.Rgba8);

        _im.Lock();
        _im.Fill(p.Paint.ToGodotColor());
        _im.Unlock();

        _imTex.CreateFromImage(_im, 0);

        this.Icon = _imTex;
        this.HintTooltip = p.Name;
    }

    public override void _Pressed()
    {
        this.PaintSelected?.Invoke(this.Pixel);
        base._Pressed();
    }

}