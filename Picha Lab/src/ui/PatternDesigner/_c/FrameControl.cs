using System.Collections.Generic;

using Godot;

using PichaLib;

public class FrameControl : Control
{
    private PatternDesigner _Owner;

    private PatternTexture _Texture = new PatternTexture() {
        MouseFilter = Control.MouseFilterEnum.Pass,
        SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
        SizeFlagsVertical = (int)Control.SizeFlags.ExpandFill,
    };

    public Frame Frame;
    public Dictionary<string, Pixel> Pixels;

    public Frame FinalizedFrame => this._Texture.Frame;

    public override void _Ready()
    {
        this.SizeFlagsHorizontal = (int)SizeFlags.Fill;
        this._Texture.SizeFlagsHorizontal = (int)SizeFlags.Fill;
        this._Owner = this.GetTree().Root.GetNode<PatternDesigner>("Application/PichaGUI/PatternDesigner");

        this.AddChild(this._Texture);

        this._Texture.LoadLayer(this.Frame, this.Pixels);
    }

    public void SetSize(int w, int h)
    {
        this._Texture.OverwriteSize(w, h);
    }
}