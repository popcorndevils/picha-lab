using OctavianLib;
using PichaLib;

using Godot;

using System.Collections.Generic;

public class ChangeLog : Tree
{
    private Dictionary<GenCanvas, CanvasLog> _Logs = new Dictionary<GenCanvas, CanvasLog>();
    // private Dictionary<Layer, TreeItem> _Logs = new Dictionary<Layer, TreeItem>();
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
        // if canvas not in tracker
        if(!this._Logs.ContainsKey(e.Sender))
        {
            var _log = new CanvasLog() {
                Node = this.CreateItem(this.GetRoot()),
                LayerLog = new Dictionary<Layer, TreeItem>(),
            };
            this._Logs.Add(e.Sender, _log);

            _log.Node.SetText(0, e.Sender.Name);
        }

        if(e.Type == CanvasChangeType.LAYER)
        {
            this._LogLayerChange(e);
        }
    }

    private void _LogLayerChange(CanvasLogEvent e)
    {
        CanvasLog _canvasLog;
        TreeItem _layerNode;
        var _canvas = e.Sender;
        var _layer = e.Change.Sender;
        
        _canvasLog = this._Logs[_canvas];

        // if layer not in canvas log
        if(!_canvasLog.LayerLog.ContainsKey(_layer))
        {
            _canvasLog.LayerLog.Add(_layer, this.CreateItem(_canvasLog.Node));
            _canvasLog.LayerLog[_layer].SetText(0, _layer.Name);
        }

        _layerNode = _canvasLog.LayerLog[_layer];

        if(e.Change.Type == LayerChangeType.NAME)
        {
            _layerNode.SetText(0, _layer.Name);
        }

        this._HandleEventNode(_canvasLog, _layerNode, e);
    }

    private void _HandleEventNode(CanvasLog c, TreeItem t, CanvasLogEvent e)
    {
        TreeItem _node;
        object _old;

        if(e.Change.Type == c.LastChangeType)
        {
            _node = c.LastAddedChange;
            
            // TODO kind of hacky way of comparing objects for now. need to define equality tests for values that can change.
            if(e.Change.NewValue.ToString() == c.LastChangeOldValue.ToString())
            {
                _node.GetParent().RemoveChild(_node);
                c.LastChangeType = LayerChangeType.NULL;
                c.LastChangeOldValue = null;
                this.Update();
            }

            else
            {
                _node.SetTooltip(0, $"{c.LastChangeOldValue} -> {e.Change.NewValue}");
            }
        }
        else
        {
            _node = this.CreateItem(t);

            _old = e.Change.OldValue;
            c.LastChangeType = e.Change.Type;
            c.LastChangeOldValue = e.Change.OldValue;
            c.LastAddedChange = _node;
            
            _node.SetText(0, e.Label);
            _node.SetTooltip(0, $"{_old} -> {e.Change.NewValue}");
        }
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