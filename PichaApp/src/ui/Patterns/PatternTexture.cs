using Godot;

public class PatternTexture : TextureRect
{
    private bool _Hover = false;
    private bool _Drawing = false;

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
                GD.Print(_p.ToIntPair());
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
                if(b.Pressed) { this._Drawing = true; }
                else { this._Drawing = false; }
            }
        }
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
        _imTex.CreateFromImage(_im, 0);

        return _imTex;
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