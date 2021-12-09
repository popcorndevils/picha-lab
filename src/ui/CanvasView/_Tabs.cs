using Godot;

public partial class CanvasView : VBoxContainer
{
    public void OnTabChange(int tab)
    {
        this.Content.CurrentTab = tab;
    }

    public void OnTabReposition(int tab)
    {
        this.Content.MoveChild(this.Content.GetChild(this.Tabs.CurrentTab), tab);
    }

    public void OnTabClose(int tab)
    {
        var n = this.Content.GetChild(tab);

        this.Tabs.RemoveTab(tab);
        this.Content.RemoveChild(n);

        n.QueueFree();

        if(this.Content.GetChildCount() == 0)
        {
            this.GetTree().CallGroup("gp_canvas_gui", "LoadCanvas");
            this.GetTree().CallGroup("gp_layer_gui", "LoadCanvas");
        }
    }
}