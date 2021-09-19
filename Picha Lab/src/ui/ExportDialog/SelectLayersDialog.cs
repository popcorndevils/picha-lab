using Godot;
using System;

public class SelectLayersDialog : WindowDialog
{
    public override void _Ready()
    {
        this.RectMinSize = this.GetNode<VBoxContainer>("VBox").RectMinSize + new Vector2(40, 40);
    }
}
