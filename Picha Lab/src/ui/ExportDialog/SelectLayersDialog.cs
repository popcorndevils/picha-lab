using Godot;
using System;

public class SelectLayersDialog : WindowDialog
{
    private GridContainer _Grid;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this._Grid = this.GetNode<GridContainer>("Grid");
        this.RectMinSize = this._Grid.RectMinSize + new Vector2(20, 20);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
