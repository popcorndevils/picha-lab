using PichaLib;

using Godot;

using System.Collections.Generic;

public class ChangeLog : Tree
{
    private Dictionary<string, TreeItem> _Logs = new Dictionary<string, TreeItem>();

    public override void _Ready()
    {
        base._Ready();
        var _root = this.CreateItem();
        this.AddToGroup("change_log");
        this.HideRoot = true;
    }

    public void LogItem(string layer_name, string label, string description)
    {
        TreeItem _node;
        if(this._Logs.ContainsKey(layer_name))
        {
            _node = this.CreateItem(this._Logs[layer_name]);
        }
        else
        {
            var _parent = this.CreateItem(this.GetRoot());
            _parent.SetText(0, layer_name);
            this._Logs[layer_name] = _parent;
            _node = this.CreateItem(_parent);
        }

        _node.SetText(0, label);
        _node.SetTooltip(0, description);
    }
}