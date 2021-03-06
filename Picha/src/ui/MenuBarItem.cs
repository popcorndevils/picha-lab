using Godot;

public class MenuBarItem : Node
{
    [Export] public Texture Icon;
    [Export] public string Text;
    [Export] public string Action;
    [Export] public bool Separate = false;
    [Export] public bool CheckButton = false;
    [Export] public bool IsChecked = false;

    public PopupMenu ParentPopup;
}