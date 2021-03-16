using System.Collections.Generic;

using Godot;

using PichaLib;

public class GenLayer : TextureRect
{
    public Layer Data;
    private Timer _Timer;
    private SortedList<int, Texture> _Textures;

    private bool _Hover;

    private float _AnimTime = 2;
    public float AnimTime {
        get => this._AnimTime;
        set {
            this._AnimTime = value;
            this.FrameTime = value / this._Textures.Count;
        }
    }

    private float _FrameTime;
    public float FrameTime {
        get => this._FrameTime;
        set {
            this._FrameTime = value;
            this._Timer.WaitTime = value;
        }
    }

    private int _Frame;
    public int Frame {
        get => this._Frame;
        set {
            this._Frame = value;
            this.Texture = this._Textures[value];
        }
    }

    public override void _Ready()
    {
        this._Textures = new SortedList<int, Texture>();
        this._Timer = new Timer();

        this.AddChild(this._Timer);

        this.Data = PDefaults.Layer;

        this.Connect("mouse_entered", this, "OnMouseIn");
        this.Connect("mouse_exited", this, "OnMouseOut");
        this._Timer.Connect("timeout", this, "OnChange");
        this._Timer.Start();
    }

    public void OnChange()
        { this.Frame = this.Frame >= this._Textures.Count - 1 ? 0 : this.Frame + 1; }

    public void Generate()
    {
        this._Textures.Clear();

        foreach(Pixel p in this.Data.Pixels.Values)
        {
            GD.Print($"{p.Name}: {p.ID}");
        }

        foreach(KeyValuePair<int, Chroma[,]> _p in PFactory.ProcessLayer(this.Data))
            { this._Textures.Add(_p.Key, _p.Value.ToGodotTex()); }

        this.Texture = this._Textures[0];
        this._Frame = 0;
        this._Timer.WaitTime = this.AnimTime / this._Textures.Count;
    }

    public void OnMouseIn() { this._Hover = true; }
    public void OnMouseOut() { this._Hover = false; }
}
