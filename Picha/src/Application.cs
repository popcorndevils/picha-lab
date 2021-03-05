using System;
using Godot;

using PichaLib;

public class Application : Node
{
    private MenuBar _Menu;

    public override void _Ready()
    {
        this._Menu = this.GetNode<MenuBar>("PichaGUI/WSVert/MenuBar");
        this._RegisterSignals();

        var _test = new Canvas();
        _test.Layers.Add(0, PDefaults.Layer);
    }

    private void _RegisterSignals()
    {
        this._Menu.ItemSelected += this.HandleMenu;
    }

    public void HandleMenu(int i)
    {
        GD.Print($"Item {i} was Selected");
    }
}
