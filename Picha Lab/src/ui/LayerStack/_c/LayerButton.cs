using Godot;

using PichaLib;

public class LayerButton : Button
{
    private GenLayer _Layer;
    public GenLayer Layer {
        get => this._Layer;
        set {
            if(this._Layer != null)
            {
                this._Layer.LayerChanged -= this.OnLayerChange; 
            }
            if(value != null)
            {
                this._Layer = value;
                this.Text = value.Data.Name;
                this._Layer.LayerChanged += this.OnLayerChange;
            }
        }
    } 

    public override void _Ready()
    {
        this.FocusMode = FocusModeEnum.None;
        this.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;

        this.Connect("mouse_entered", this, "OnMouseEnter");
        this.Connect("mouse_exited", this, "OnMouseExit");
    }

    public override void _Pressed()
    {
        this.GetTree().CallGroup("gp_layer_gui", "LoadLayer", this.Layer);
        this.GetTree().CallGroup("gp_layer_gui", "ActivateLayerInspectorTab");
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
        var _preview = new LayerButton() {
            Text = this.Text,
            RectMinSize = this.RectSize,
            Modulate = new Color(1f, 1f, 1f, .65f),
        };

        var c = new Control();
        c.AddChild(_preview);
        _preview.RectPosition = new Vector2(_preview.RectMinSize.x * -.5f, _preview.RectMinSize.y * -.5f);

        this.SetDragPreview(c);
        return this;
    }

    public override bool CanDropData(Vector2 position, object data)
    {
        if(data is LayerButton b && data != this)
        {
            return true;
        }
        return false;
    }

    public void OnLayerChange(Layer layer, bool major)
    {
        this.Text = layer.Name;
    }

    public void OnMouseEnter()
    {
        this.Layer.Modulate = new Color(.75f, .75f, .75f, 1f);
    }

    public void OnMouseExit()
    {
        this.Layer.Modulate = new Color(1f, 1f, 1f, 1f);
    }
}