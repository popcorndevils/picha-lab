using Godot;

using PichaLib;

public class LayerButton : Button
{
    private GenLayer _Layer;
    public GenLayer Layer {
        get => this._Layer;
        set {
            if(this._Layer != null)
                { this._Layer.Data.OnLayerChanged -= this.HandleLayerChange; }
            this._Layer = value;
            this.Text = value.Data.Name;
            this._Layer.Data.OnLayerChanged += this.HandleLayerChange;
        }
    }

    public void HandleLayerChange(Layer l)
    {
        this.Text = l.Name;
    }
}