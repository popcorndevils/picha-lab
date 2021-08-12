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
        this.SetDragPreview(new Label() {Text = this.Text});
        return this;
    }

    public override bool CanDropData(Vector2 position, object data)
    {
        if(data is SectionHeader b)
        {
            return true;
        }
        return false;
    }
}