using OctavianLib;
using PichaLib;

using Godot;

using System.Collections.Generic;

public class ChangeLog : Tree
{
    private Dictionary<GenCanvas, CanvasLog> _Logs = new Dictionary<GenCanvas, CanvasLog>();
    private TreeItem _Root;

    public override void _Ready()
    {
        base._Ready();
        this._Root = this.CreateItem();
        this.AddToGroup("inspector");
        this.AddToGroup("change_log");
        this.HideRoot = true;
    }

    private void _ClearTree()
    {
        TreeItem _next;
        var _node = this._Root.GetChildren();
        while(_node != null)
        {
            _next = _node.GetNext();
            this._Root.RemoveChild(_node);
            _node = _next;
        }
    }

    public void LogItem(CanvasLogEvent e)
    {
        // TODO implement
    }

    public void LoadCanvas() {}
    public void LoadCanvas(GenCanvas canvas) {}
}

public class CanvasLog
{
    // TODO implement
}

public class CanvasLogEvent : Reference
{
    // TODO implement
}