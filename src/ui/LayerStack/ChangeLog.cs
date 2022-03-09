using OctavianLib;
using PichaLib;

using Godot;

using System.Collections.Generic;

public class ChangeLog : Tree
{
    private Dictionary<GenCanvas, CanvasLog> _Logs = new Dictionary<GenCanvas, CanvasLog>();
    private string _PrevName = null;
    private TreeItem _PrevNameChange;

    public override void _Ready()
    {
        base._Ready();
        var _root = this.CreateItem();
        this.AddToGroup("change_log");
        this.HideRoot = true;
    }

    public void LogItem(CanvasLogEvent e)
    {
        // TODO reimplement events for logging changes
        // two dictionaries, canvas events and layer events
        // layer nodes are embedded in canvas nodes
        // once layer nodes are created beneath canvas node, don't need to continually reference canvas node
    }
}

public class CanvasLog
{
    public TreeItem Node;
    public TreeItem LastAddedChange;
    public LayerChangeType LastChangeType;
    public object LastChangeOldValue;
    public Dictionary<Layer, TreeItem> LayerLog;
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