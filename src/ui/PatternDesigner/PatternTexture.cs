using System.Collections.Generic;

using Godot;

using OctavianLib;
using PichaLib;

public class PatternTexture : TextureRect
{
    private bool _Hover = false;
    private bool _Drawing = false;
    private PatternDesigner _Owner;
    private (int x, int y) _PrevLoc;

    private Image _Image = new Image();
    private ImageTexture _ImageTex = new ImageTexture();
    private Pixel Current => this._Owner.Selected;

    public Frame Frame;
    private Dictionary<string, Pixel> _Pixels;

    private TextureRect _PixelOutline = new TextureRect() {
        Name = "PixelOutline",
        Visible = false,
        RectScale = new Vector2(.125f, .125f),
        MouseFilter = MouseFilterEnum.Ignore,
    };

    public Vector2 Size {
        get => new Vector2(this.Texture.GetWidth(), this.Texture.GetHeight());
    }

    public override void _Ready()
    {
        this.Connect("mouse_entered", this, "OnMouseEnter");
        this.Connect("mouse_exited", this, "OnMouseExit");
        this._PixelOutline.Texture = this._GetOutline();
        this.AddChild(this._PixelOutline);
        this._Owner = this.GetTree().Root.GetNode<PatternDesigner>("Application/PichaGUI/PatternDesigner");
        this.RectScale = new Vector2(20f, 20f);
    }

    public void LoadLayer(Frame frame, Dictionary<string, Pixel> pixels)
    {
        int _w = frame.GetWidth();
        int _h = frame.GetHeight();

        this.Frame = new Frame() { Data = new string[_h, _w] };
        this._Pixels = pixels;

        this._Image.Create(_w, _h, false, Image.Format.Rgba8);
        this._Image.Lock();

        for(int _x = 0; _x < _w; _x++)
        {
            for(int _y = 0; _y < _h; _y++)
            {
                Chroma _col;
                var _cell = frame.Data[_y, _x];
                this.Frame.Data[_y, _x] = _cell;

                if(_cell != Pixel.NULL)
                {
                    _col = pixels[_cell].Paint;
                }
                else
                {
                    _col = new Chroma(0f, 0f, 0f, 0f);
                }

                this._Image.SetPixel(_x, _y, _col.ToGodotColor());
            }
        }

        this._Image.Unlock();
        this._ImageTex.CreateFromImage(this._Image, 0);
        this.Texture = this._ImageTex;
    }

    public override void _GuiInput(InputEvent @event)
    {
        if(@event is InputEventMouseMotion m && this._Hover)
        {
            var _p = m.Position;
            var _s = this.Texture.GetSize();

            this._PixelOutline.RectPosition = m.Position - new Vector2(.5f, .5f);

            if(_p.x < 0f || _p.x >= _s.x  || _p.y < 0f || _p.y >= _s.y)
            {
                this._Hover = false;
                this._Drawing = false;
                this._PixelOutline.Visible = false;
            }
            
            if(this._Drawing)
            {
                this.SetPixel((int)_p.x, (int)_p.y, this.Current);
            }
        }
        else
        {
            this._Drawing = false;
        }

        if(@event is InputEventMouseButton b && this._Hover)
        {
            if(b.ButtonIndex == (int)ButtonList.Left)
            {
                if(b.Pressed) 
                { 
                    this._Drawing = true;
                    this.SetPixel((int)b.Position.x, (int)b.Position.y, this.Current);
                }
                else { this._Drawing = false; }
            }
        }
    }

    public void OverwriteSize(int w, int h)
    {
        var _newFrame = new Frame() { Data = new string[h, w] };
        var _oldHeight = this.Frame.Data.GetHeight();
        var _oldWidth = this.Frame.Data.GetWidth();

        this._Image.Create(_newFrame.Data.GetWidth(), _newFrame.Data.GetHeight(), false, Image.Format.Rgba8);
        this._Image.Lock();

        for(int _x = 0; _x < _newFrame.Data.GetWidth(); _x++)
        {
            for(int _y = 0; _y < _newFrame.Data.GetHeight(); _y++)
            {
                if(_x >= _oldWidth || _y >= _oldHeight)
                {
                    var _cell = this.Current.Name;
                    _newFrame.Data[_y, _x] = _cell;
                    var _col = this._Pixels[_cell].Paint;
                    this._Image.SetPixel(_x, _y, _col.ToGodotColor());
                }
                else 
                {
                    var _cell = this.Frame.Data[_y, _x];
                    _newFrame.Data[_y, _x] = _cell;
                    var _col = this._Pixels[_cell].Paint;
                    this._Image.SetPixel(_x, _y, _col.ToGodotColor());
                }
            }
        }

        this.Frame = _newFrame;

        this._Image.Unlock();
        this._ImageTex.CreateFromImage(this._Image, 0);
        this.Texture = this._ImageTex;
    }

    private ImageTexture _GetOutline()
    {
        var _im = new Image();
        var _imTex = new ImageTexture();
        var _s = 8;

        _im.Create(_s, _s, false, Image.Format.Rgba8);
        _im.Lock();
    
        for(int _x = 0; _x < _s; _x++) {
            for(int _y = 0; _y < _s; _y++) {
                if(_x <= 1 || _x >= _s-2 || _y <= 1 || _y >= _s-2)
                    { _im.SetPixel(_x, _y, new Color(0f, 0f, 0f, .8f)); }
                else
                    { _im.SetPixel(_x, _y, new Color(1f, 1f, 1f, 0f)); }
            }
        }
        _im.Unlock();
        _imTex.CreateFromImage(_im, 0);

        return _imTex;
    }

    public void SetPixel(int x, int y, Pixel p)
    {
        this._Image.Lock();
        this._Image.SetPixel(x, y, p.Paint.ToGodotColor());
        this._Image.Unlock();
        this._ImageTex.SetData(this._Image);
        this.Frame.Data[y, x] = p.Name;
    }
    
    public void OnMouseEnter()
    {
        this._Hover = true;
        this._PixelOutline.Visible = true;
    }

    public void OnMouseExit()
    {
        this._Hover = false;
        this._PixelOutline.Visible = false;
    }
}