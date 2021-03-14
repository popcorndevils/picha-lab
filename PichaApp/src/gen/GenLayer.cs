using Godot;

using PichaLib;

public class GenLayer : AnimatedSprite
{
    public Layer _Data;

    public override void _Ready()
    {
        this.Frames = new SpriteFrames();
        this._Data = PDefaults.Layer;
    }

    public void Generate()
    {
        var _layerData = PFactory.ProcessLayer(this._Data);

        this.Frames.ClearAll();
        this.Frames.SetAnimationLoop("default", true);
        this.Frames.SetAnimationSpeed("default", 2);

        foreach(Chroma[,] _c in _layerData.Values)
        {
            this.Frames.AddFrame("default", _c.ToGodotTex());
        }

        this.Play("default");
    }
}
