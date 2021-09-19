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

    public override void _Ready()
    {
        this.RectMinSize = this.GetNode<VBoxContainer>("VBox").RectMinSize + new Vector2(40, 40);

        this._Available = this.GetNode<Tree>("VBox/Grid/AvailableTree");
        this._Selected = this.GetNode<Tree>("VBox/Grid/SelectedTree");

        this._Select = this.GetNode<Button>("VBox/Grid/Buttons/Select");
        this._SelectAll = this.GetNode<Button>("VBox/Grid/Buttons/SelectAll");
        this._Remove = this.GetNode<Button>("VBox/Grid/Buttons/Remove");
        this._RemoveAll = this.GetNode<Button>("VBox/Grid/Buttons/RemoveAll");

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

    public void OnAcceptLayers()
    {
        this.Hide();
    }

    public void OnCancel()
    {
        this.Hide();
    }
}
