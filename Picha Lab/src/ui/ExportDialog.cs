using Godot;
using System;

public class ExportDialog : AcceptDialog
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.GetOk().Visible = false;
    }
}
