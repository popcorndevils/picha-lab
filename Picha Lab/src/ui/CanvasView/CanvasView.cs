using Godot;

public partial class CanvasView : VBoxContainer
{
    public TabContainer Content = new TabContainer() {
        TabsVisible = false,
        SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        SizeFlagsVertical = (int)SizeFlags.ExpandFill,
        RectMinSize = new Vector2(500, 0),
    };

    public Tabs Tabs = new Tabs() {
        TabAlign = Tabs.TabAlignEnum.Left,
        SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        DragToRearrangeEnabled = true,
        TabCloseDisplayPolicy = Tabs.CloseButtonDisplayPolicy.ShowActiveOnly,
    };

    public override void _Ready()
    {
        this.AddToGroup("gp_canvas_handler");

        this.AddChildren(this.Tabs, this.Content);
        this.AddConstantOverride("separation", 0);

        this.Tabs.Connect("tab_changed", this, "OnTabChange");
        this.Tabs.Connect("reposition_active_tab_request", this, "OnTabReposition");
        this.Tabs.Connect("tab_close", this, "OnTabClose");

        this.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
        this.SizeFlagsVertical = (int)SizeFlags.ExpandFill;
    }

    public bool PathExists(string path)
    {
        foreach(GenCanvas c in this.Canvases)
        {
            if(c.PathName == path)
            {
                return true;
            }
        }
        return false;
    }
}
