using System.Collections.Generic;

using Godot;

using PichaLib;

public class LayersTree : Tree
{
    public bool Dragging = false;

    public List<TreeItem> AllItems {
        get {
            var _output = new List<TreeItem>();

            TreeItem _root = this.GetRoot();

            if(_root != null)
            {
                TreeItem _child = _root.GetChildren();
                while(_child != null)
                {
                    _output.Add(_child);
                    _child = _child.GetNext();
                }
            }

            return _output;
        }
    }

    public List<TreeItem> SelectedItems {
        get {
            
            var _output = new List<TreeItem>();

            TreeItem _item = this.GetNextSelected(null);
            
            while(_item != null)
            {
                _output.Add(_item);
                _item = this.GetNextSelected(_item);
            }

            return _output;
        }
    }
    
    public void AddItems(List<TreeItem> items)
    {
        TreeItem _root = this.GetRoot();

        foreach(TreeItem i in items)
        {
            var _item = this.CreateItem(_root);
            var _layer = i.GetMetadata(0) as TreeLayer;

            _item.SetText(0, _layer.Data.Name);
            _item.SetMetadata(0, new TreeLayer() { Data = _layer.Data });
        }
    }

    public void RemoveItems(List<TreeItem> items)
    {
        foreach(TreeItem i in items)
        {
            i.GetParent().RemoveChild(i);
        }
        this.Update();
    }

    public void LoadLayers(List<Layer> layers)
    {
        this.Clear();
        
        var _root = this.CreateItem();

        foreach(Layer l in layers)
        {
            var _item = this.CreateItem(_root);
            _item.SetText(0, l.Name);
            _item.SetMetadata(0, new TreeLayer() { Data = l });
        }
    }

    public override object GetDragData(Vector2 position)
    {
        var _selected = this.GetSelected();
        var preview = new Label() { Text = _selected.GetText(0) };
        this.SetDragPreview(preview);

        this.DropModeFlags = (int)DropModeFlagsEnum.Inbetween + (int)DropModeFlagsEnum.OnItem;

        return _selected;
    }

    public override bool CanDropData(Vector2 position, object data)
    {
        return data.GetType() == typeof(TreeItem);
    }

    public override void DropData(Vector2 position, object data)
    {
        this.DropModeFlags = (int)DropModeFlagsEnum.Disabled;
     
        var _fromItem = data as TreeItem;
        var _toItem = this.GetItemAtPosition(position) as TreeItem;
        var _shift = this.GetDropSectionAtPosition(position);

        if(_fromItem.GetParent() == this.GetRoot() && _toItem != null)
        {
            foreach(TreeItem i in this.AllItems)
            {
                if(i == _toItem)
                {
                    if(_shift == -1)
                    {
                        _fromItem.MoveToBottom();
                        i.MoveToBottom();
                    }
                    else
                    {
                        i.MoveToBottom();
                        _fromItem.MoveToBottom();
                    }
                }
                else if(i != _fromItem)
                {
                    i.MoveToBottom();
                }
            }
        }
    }
}
