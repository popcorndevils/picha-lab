using System.Collections.Generic;
using Godot;

using PichaLib;

public class SelectLayersDialog : WindowDialog
{
    private Tree _Available;
    private Tree _Selected;

    private Button _Select;
    private Button _SelectAll;
    private Button _Remove;
    private Button _RemoveAll;

    private Button _Accept;
    private Button _Cancel;

    public List<TreeItem> AllAvailableLayers {
        get {
            var _output = new List<TreeItem>();

            TreeItem _root = this._Available.GetRoot();

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

    public List<TreeItem> AllSelectedLayers {
        get {
            var _output = new List<TreeItem>();

            TreeItem _root = this._Selected.GetRoot();

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

    public List<TreeItem> SelectedAvailableTree {
        get {
            
            var _output = new List<TreeItem>();

            TreeItem _item = this._Available.GetNextSelected(null);
            
            while(_item != null)
            {
                _output.Add(_item);
                _item = this._Available.GetNextSelected(_item);
            }

            return _output;
        }
    }

    public List<TreeItem> SelectedSelectedTree {
        get {
            
            var _output = new List<TreeItem>();

            TreeItem _item = this._Selected.GetNextSelected(null);
            
            while(_item != null)
            {
                _output.Add(_item);
                _item = this._Selected.GetNextSelected(_item);
            }

            return _output;
        }
    }

    public override void _Ready()
    {
        this.RectMinSize = this.GetNode<VBoxContainer>("VBox").RectMinSize + new Vector2(40, 40);

        this._Available = this.GetNode<Tree>("VBox/Grid/AvailableTree");
        this._Selected = this.GetNode<Tree>("VBox/Grid/SelectedTree");

        this._Select = this.GetNode<Button>("VBox/Grid/Buttons/Select");
        this._SelectAll = this.GetNode<Button>("VBox/Grid/Buttons/SelectAll");
        this._Remove = this.GetNode<Button>("VBox/Grid/Buttons/Remove");
        this._RemoveAll = this.GetNode<Button>("VBox/Grid/Buttons/RemoveAll");

        this._Select.Connect("pressed", this, "OnSelectLayers");
        this._SelectAll.Connect("pressed", this, "OnSelectAll");
        this._Remove.Connect("pressed", this, "OnRemoveLayers");
        this._RemoveAll.Connect("pressed", this, "OnRemoveAll");

        this._Accept = this.GetNode<Button>("VBox/HBox/Accept");
        this._Cancel = this.GetNode<Button>("VBox/HBox/Cancel");

        this._Accept.Connect("pressed", this, "OnAcceptLayers");
        this._Cancel.Connect("pressed", this, "OnCancel");
    }

    public void Open(Canvas available, Canvas selected)
    {
        TreeItem _root;
        
        this._Available.Clear();
        this._Selected.Clear();

        _root = this._Available.CreateItem();

        foreach(Layer l in available.Layers)
        {
            if(!selected.Layers.Contains(l))
            {
                var _item = this._Available.CreateItem(_root);
                _item.SetText(0, l.Name);
                _item.SetMetadata(0, new TreeLayer() { Data = l });
            }
        }

        _root = this._Selected.CreateItem();

        foreach(Layer l in selected.Layers)
        {
            var _item = this._Selected.CreateItem(_root);
            _item.SetText(0, l.Name);
            _item.SetMetadata(0, new TreeLayer() { Data = l });
        }

        this.PopupCentered();
    }

    public void OnSelectAll() { this.OnSelectLayers(this.AllAvailableLayers); }
    public void OnSelectLayers() { this.OnSelectLayers(this.SelectedAvailableTree); }
    public void OnSelectLayers(List<TreeItem> items)
    {
        TreeItem _root = this._Selected.GetRoot();

        foreach(TreeItem i in items)
        {
            var _item = this._Selected.CreateItem(_root);
            var _layer = i.GetMetadata(0) as TreeLayer;

            _item.SetText(0, _layer.Data.Name);
            _item.SetMetadata(0, new TreeLayer() { Data = _layer.Data });  

            i.GetParent().RemoveChild(i);
        }

        this._Available.Update();
    }

    public void OnRemoveAll() { this.OnRemoveLayers(this.AllSelectedLayers); }
    public void OnRemoveLayers() { this.OnRemoveLayers(this.SelectedSelectedTree); }
    public void OnRemoveLayers(List<TreeItem> items)
    {
        TreeItem _root = this._Available.GetRoot();

        foreach(TreeItem i in items)
        {
            var _item = this._Available.CreateItem(_root);
            var _layer = i.GetMetadata(0) as TreeLayer;

            _item.SetText(0, _layer.Data.Name);
            _item.SetMetadata(0, new TreeLayer() { Data = _layer.Data });  

            i.GetParent().RemoveChild(i);
        }

        this._Selected.Update();
    }

    public void OnAcceptLayers()
    {
        var _output = new List<Layer>();

        foreach(TreeItem i in this.AllSelectedLayers)
        {
            var _layer = i.GetMetadata(0) as TreeLayer;
            _output.Add(_layer.Data);
        }

        this.Hide();
        this.GetParent<ExportDialog>().ProcessLayers(_output);
    }

    public void OnCancel()
    {
        this.Hide();
    }
}
