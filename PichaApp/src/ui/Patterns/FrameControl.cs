using System.Collections.Generic;

using Godot;

using PichaLib;

public class FrameControl : Node2D
{
    public Vector2 Size => this._Texture.Size * this.Scale;
    private PatternDesigner _Owner;

    private PatternTexture _Texture = new PatternTexture() {
        MouseFilter = Control.MouseFilterEnum.Pass,
        SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
        SizeFlagsVertical = (int)Control.SizeFlags.ExpandFill,
    };

    public string[,] Frame;
    public Dictionary<string, Pixel> Pixels;

    public override void _Ready()
    {
        this._Owner = this.GetTree().Root.GetNode<PatternDesigner>("Application/PichaGUI/PatternDesigner");
        this.Scale = new Vector2(20, 20);

        this.AddChild(this._Texture);

        this._Texture.LoadLayer(this.Frame, this.Pixels);
        this.GetParent<TabContainer>().RectMinSize = this.Size;
    }
}