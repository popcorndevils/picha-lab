using OctavianLib;
using PichaLib;

using Godot;

using System.Collections.Generic;

public class ChangeLog : Tree
{
    private Dictionary<string, TreeItem> _Logs = new Dictionary<string, TreeItem>();
    private string _PrevName = null;
    private TreeItem _PrevNameChange;

    public override void _Ready()
    {
        base._Ready();
        var _root = this.CreateItem();
        this.AddToGroup("change_log");
        this.HideRoot = true;
    }

    public void LogItem(LayerLogEvent log)
    {
        TreeItem _node;

        // TODO: correct renaming one layer right after another
        if(log.Change.Type == LayerChangeType.NAME)
        {
            if(this._Logs.ContainsKey(log.Change.OldValue))
            {
                var _value = this._Logs[log.Change.OldValue];
                this._Logs.Remove(log.Change.OldValue);
                this._Logs.Add(log.Change.Sender.Name, _value);
                _value.SetText(0, log.Change.Sender.Name);
            }

            if(this._Logs.ContainsKey(log.Change.Sender.Name))
            {
                if(this._PrevName != null)
                {
                    this._PrevNameChange.SetTooltip(0, $"{this._PrevName} -> {log.Change.Sender.Name}");
                }
                else
                {
                    this._PrevName = log.Change.OldValue;
                    this._PrevNameChange = this.CreateItem(this._Logs[log.Change.Sender.Name]);
                    this._PrevNameChange.SetText(0, log.Label);
                    this._PrevNameChange.SetTooltip(0, log.Description);
                }
            }
        } 

        else 
        {
            this._PrevName = null;

            if(this._Logs.ContainsKey(log.Change.Sender.Name))
            {   
                _node = this.CreateItem(this._Logs[log.Change.Sender.Name]);
            }
            else
            {
                var _parent = this.CreateItem(this.GetRoot());
                _parent.SetText(0, log.Change.Sender.Name);
                this._Logs[log.Change.Sender.Name] = _parent;
                _node = this.CreateItem(_parent);
            }

            _node.SetText(0, log.Label);
            _node.SetTooltip(0, log.Description);
        }
    }
}

public class LayerLogEvent : Reference
{
    public LayerChangeEvent Change;
    public string Label;
    public string Description;
}