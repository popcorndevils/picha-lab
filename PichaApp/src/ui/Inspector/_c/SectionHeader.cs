using Godot;

public delegate void DragEventHandler(SectionHeader header, SectionHeader dropped);

public class SectionHeader : Button
{
    public event DragEventHandler HeaderDropped;
    public bool Draggable = false;

    public override void DropData(Vector2 position, object data)
    {
        if(data is SectionHeader b)
        {
            this.HeaderDropped?.Invoke(this, b);
        }
    }

    public override object GetDragData(Vector2 position)
    {
        if(this.Draggable)
        {
            var _preview = new SectionHeader() {
                Text = this.Text,
                RectMinSize = this.RectSize
            };        
            
            var c = new Control();
            c.AddChild(_preview);
            _preview.RectPosition = new Vector2(_preview.RectMinSize.x * -.5f, _preview.RectMinSize.y * -.5f);

            this.SetDragPreview(c);
            return this;
        }
        return null;
    }

    public override bool CanDropData(Vector2 position, object data)
    {
        if(data is SectionHeader b && data != this)
        {
            return true;
        }
        return false;
    }
}