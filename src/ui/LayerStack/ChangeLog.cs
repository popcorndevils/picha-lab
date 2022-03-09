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
        // TODO reimplement events for logging changes
        // two dictionaries, canvas events and layer events
        // layer nodes are embedded in canvas nodes
        // once layer nodes are created beneath canvas node, don't need to continually reference canvas node
    }

    public void LoadCanvas()
    {
        this._ClearTree();
    }

    public void LoadCanvas(GenCanvas canvas)
    {
        this._ClearTree();
        if(this._Logs.ContainsKey(canvas))
        {
            foreach(TreeItem t in this._Logs[canvas].Changes.Values)
            {
                var _nT = this.CreateItem(this._Root);
                _nT.SetText(0, t.GetText(0));
                _nT.SetTooltip(0, t.GetTooltip(0));
            }
        }
        else
        {
            var _log = new CanvasLog();
            foreach(GenLayer l in canvas.Layers)
            {
                var _nT = this.CreateItem(this._Root);
                _nT.SetText(0, l.LayerName);
                _nT.SetTooltip(0, l.Position.ToString());
                _log.Changes.Add(l, _nT);
            }
            this._Logs.Add(canvas, _log);
        }
    }
}

public class CanvasLog
{
    public Dictionary<GenLayer, TreeItem> Changes = new Dictionary<GenLayer, TreeItem>();
}

public class CanvasLogEvent : Reference
{
    public GenCanvas Sender;
    public CanvasChangeType Type;
    public string Label;
    public LayerChangeEvent Change;
}

public enum CanvasChangeType
{
    NULL, NAME, LAYER,
}