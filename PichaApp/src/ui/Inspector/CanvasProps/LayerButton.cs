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

    public override void _Ready()
    {
        this.AddToGroup("LayerButtons");

        this.FocusMode = FocusModeEnum.None;
        this.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;

        this.Connect("mouse_entered", this, "OnMouseEnter");
        this.Connect("mouse_exited", this, "OnMouseExit");
    }

    public override void _Pressed()
    {
        this.GetTree().CallGroup("gp_layer_gui", "LoadLayer", this.Layer);
    }

    public override void DropData(Vector2 position, object data)
    {
        if(data is LayerButton b)
        {
            Node _control = this.GetParent();
            Node _list = _control.GetParent();

            _list.MoveChild(b.GetParent(), _control.GetIndex());
            this.Layer.GetParent().MoveChild(b.Layer, this.Layer.GetIndex());
        }
    }

    public override object GetDragData(Vector2 position)
    {
        this.SetDragPreview(new Label() {Text = this.Text});
        return this;
    }

    public override bool CanDropData(Vector2 position, object data)
    {
        if(data is LayerButton b)
        {
            return true;
        }
        return false;
    }

    public void HandleLayerChange(Layer l)
    {
        this.Text = l.Name;
    }

    public void OnMouseEnter()
    {
        this.Layer.Modulate = new Color(.3f, .3f, .3f, 1f);
    }

    public void OnMouseExit()
    {
        this.Layer.Modulate = new Color(1f, 1f, 1f, 1f);
    }

    public void OnLayerHover(bool hover, GenLayer layer)
    {
        if(hover & layer == this._Layer)
        {
            this.Modulate = new Color(.8f, .8f, .8f, 1f);
        }
        else
        {
            this.Modulate = new Color(1f, 1f, 1f, 1f);
        }
    }
}