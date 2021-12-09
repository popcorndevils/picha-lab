using System.Collections.Generic;
using Godot;

using PichaLib;

public class SelectLayersDialog : WindowDialog
{
    private LayersTree _Available;
    private LayersTree _Selected;

    private Button _Select;
    private Button _SelectAll;
    private Button _Remove;
    private Button _RemoveAll;

    private Button _Accept;
    private Button _Cancel;


    public override void _Ready()
    {
        this.RectMinSize = this.GetNode<VBoxContainer>("VBox").RectMinSize + new Vector2(40, 40);

        this._Available = this.GetNode<LayersTree>("VBox/Grid/AvailableTree");
        this._Selected = this.GetNode<LayersTree>("VBox/Grid/SelectedTree");

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
        var _avail = new List<Layer>();

        foreach(Layer l in available.Layers)
        {
            if(!selected.Layers.Contains(l))
            {
                _avail.Add(l);
            }
        }

        this._Available.LoadLayers(_avail);
        this._Selected.LoadLayers(selected.Layers);

        this.PopupCentered();
    }

    public void OnSelectAll() { this.OnSelectLayers(this._Available.AllItems); }
    public void OnSelectLayers() { this.OnSelectLayers(this._Available.SelectedItems); }
    public void OnSelectLayers(List<TreeItem> items)
    {
        this._Selected.AddItems(items);
        this._Available.RemoveItems(items);
    }

    public void OnRemoveAll() { this.OnRemoveLayers(this._Selected.AllItems); }
    public void OnRemoveLayers() { this.OnRemoveLayers(this._Selected.SelectedItems); }
    public void OnRemoveLayers(List<TreeItem> items)
    {
        this._Available.AddItems(items);
        this._Selected.RemoveItems(items);
    }

    public void OnAcceptLayers()
    {
        var _output = new List<Layer>();

        foreach(TreeItem i in this._Selected.AllItems)
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
