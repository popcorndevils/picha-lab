using System;
using System.Collections.Generic;

using Godot;
using PichaLib;

public class ExportDialog : WindowDialog
{
    private Canvas _OriginalCanvas;
    private TabContainer _Tabs;
    private VBoxContainer _Layers;
    private VBoxContainer _Sprites;
    
    public Tree SelectedLayers;
    public List<TreeItem> TreeNodes;

    public SpriteExporter Export = new SpriteExporter();

    public MarginContainer Margins;

    public FileDialog FileDialog;
    public ConfirmationDialog Confirmation;
    public SelectLayersDialog SelectLayersDialog;
    public ProgressTrack Progress;

    public SpinBox Rows;
    public SpinBox Cols;
    public SpinBox Sheets;
    public SpinBox Scale;

    public CheckBox ClipContent;
    public CheckBox SplitFrames;
    public CheckBox AsLayers;
    public CheckBox MapToCanvas;
    public CheckBox NoCopies;

    public LineEdit SpriteName;
    public LineEdit OutputPath;
    public Button FileButton;

    public Button SetLayers;
    public Button Ok;
    public Button Cancel;

    public Canvas SubmitCanvas {
        get {
            var _output = this._OriginalCanvas.Copy();
            _output.Layers.Clear();

            TreeItem _root = this.SelectedLayers.GetRoot();

            if(_root != null)
            {
                TreeItem _child = _root.GetChildren();
                while(_child != null)
                {
                    var _item = _child.GetMetadata(0) as TreeLayer;
                    _output.Layers.Add(_item.Data);
                    _child = _child.GetNext();
                }
            }

            return _output;
        }
    }

    public override void _Ready()
    {
        this.AddToGroup("diag_export");

        this.Margins = this.GetNode<MarginContainer>("Margins");
        this._Tabs = this.GetNode<TabContainer>("Margins/Tabs");
        this._Layers = this.GetNode<VBoxContainer>("Margins/Tabs/Layers");

        this.SelectedLayers = this.GetNode<Tree>("Margins/Tabs/Layers/LayersBox/selected_layers");
        
        this.FileDialog = this.GetNode<FileDialog>("FileDialog");
        this.Confirmation = this.GetNode<ConfirmationDialog>("Confirmation");
        this.SelectLayersDialog = this.GetNode<SelectLayersDialog>("SelectLayersDialog");
        this.Progress = this.GetNode<ProgressTrack>("ProgressTrack");
        
        this.Rows = this.GetNode<SpinBox>("Margins/Tabs/Layers/OptionBox/rows");
        this.Cols = this.GetNode<SpinBox>("Margins/Tabs/Layers/OptionBox/cols");
        this.Sheets = this.GetNode<SpinBox>("Margins/Tabs/Layers/OptionBox/sheets");
        this.Scale = this.GetNode<SpinBox>("Margins/Tabs/Layers/OptionBox/scale");

        this.ClipContent = this.GetNode<CheckBox>("Margins/Tabs/Layers/OptionBox/clip_content");
        this.SplitFrames = this.GetNode<CheckBox>("Margins/Tabs/Layers/OptionBox/split_frames");
        this.AsLayers = this.GetNode<CheckBox>("Margins/Tabs/Layers/OptionBox/as_layers");
        this.MapToCanvas = this.GetNode<CheckBox>("Margins/Tabs/Layers/OptionBox/map_to_canvas");
        this.NoCopies = this.GetNode<CheckBox>("Margins/Tabs/Layers/OptionBox/no_copies");

        this.SpriteName = this.GetNode<LineEdit>("Margins/Tabs/Layers/sprite_name");
        this.FileButton = this.GetNode<Button>("Margins/Tabs/Layers/PathBox/btn_browse");
        this.OutputPath = this.GetNode<LineEdit>("Margins/Tabs/Layers/PathBox/path");
        
        this.SetLayers = this.GetNode<Button>("Margins/Tabs/Layers/LayersBox/set_layers");
        this.Ok = this.GetNode<Button>("Margins/Tabs/Layers/BBox/ok");
        this.Cancel = this.GetNode<Button>("Margins/Tabs/Layers/BBox/cancel");

        this.Confirmation.GetOk().FocusMode = FocusModeEnum.None;
        this.Confirmation.GetCancel().FocusMode = FocusModeEnum.None;

        this.AsLayers.Connect("pressed", this, "OnOptionButtonPress");
        this.MapToCanvas.Connect("pressed", this, "OnOptionButtonPress");

        this.FileButton.Connect("pressed", this, "OnFileButtonPress");
        this.SetLayers.Connect("pressed", this, "OnSetLayersPress");
        this.Ok.Connect("pressed", this, "OnOkPress");
        this.Cancel.Connect("pressed", this, "OnCancelPress");
        this.FileDialog.Connect("dir_selected", this, "OnDirSelect");
        this.SpriteName.Connect("text_changed", this, "OnSpriteNameChange");
        this.Confirmation.Connect("confirmed", this, "OnConfirmExport");
        this.Export.Connect("ProgressChanged", this.Progress, "OnProgressChanged");
        this.Export.Connect("StatusUpdate", this.Progress, "OnStatusUpdate");

        this.RectMinSize = this._Layers.RectSize + new Vector2(60, 60);
        // this._Tabs.RectMinSize = this.Margins.RectSize;

        this.Rows.Value = 1;
        this.Cols.Value = 1;
        this.Sheets.Value = 1;
        this.Scale.Value = 1;
        this.SplitFrames.Pressed = false;
        this.AsLayers.Pressed = false;
        this.MapToCanvas.Pressed = false;
        this.ClipContent.Pressed = true;
        this.NoCopies.Pressed = false;
    }

    public void Open(Canvas canvas)
    {
        this._OriginalCanvas = canvas;

        this.ProcessCanvas(canvas);

        this.SpriteName.Text = "";
        this.OutputPath.Text = "";

        this.OnOptionButtonPress();
        this.PopupCentered();
    }


    public void ProcessCanvas(Canvas canvas) { this.ProcessLayers(canvas.Layers); }
    public void ProcessLayers(List<Layer> layers)
    {
        this.SelectedLayers.Clear();

        var _root = this.SelectedLayers.CreateItem();

        foreach(Layer l in layers)
        {
            var _item = this.SelectedLayers.CreateItem(_root);
            _item.SetText(0, l.Name);
            _item.SetMetadata(0, new TreeLayer() {Data = l});
        }
    }


    public void OnFileButtonPress()
    {
        this.FileDialog.PopupCentered();
    }

    public void OnOptionButtonPress()
    {
        this.MapToCanvas.Disabled = !this.AsLayers.Pressed;
        this.NoCopies.Disabled = !this.AsLayers.Pressed;
        this.SpriteName.Editable = !this.AsLayers.Pressed;
        this.ClipContent.Disabled = this.AsLayers.Pressed ? !this.MapToCanvas.Pressed : false;

        if(SpriteName.Editable)
            { this.SpriteName.FocusMode = FocusModeEnum.All; }
        else
            { this.SpriteName.FocusMode = FocusModeEnum.None; }

        this.Ok.Disabled = this.OutputPath.Text == "" || (this.AsLayers.Pressed ? false : this.SpriteName.Text == "");
    }

    public void OnSetLayersPress()
    {
        this.SelectLayersDialog.Open(this._OriginalCanvas, this.SubmitCanvas);
    }

    public void OnOkPress()
    {
        var _frames = ExMath.LCD(this._OriginalCanvas.FrameCounts);
        var _word = _frames > 1 ? "frames" : "frame";
        this.Confirmation.DialogText = $"Each Sprite includes {_frames} {_word} of animation.\nWould you like to continue with export?";

        this.Confirmation.PopupCentered();
    }

    public void OnCancelPress()
    {
        this.Hide();
    }

    public void OnSpriteNameChange(string s)
    {
        this.Ok.Disabled = this.OutputPath.Text == "" || (this.AsLayers.Pressed ? false : this.SpriteName.Text == "");
    }

    public void OnDirSelect(string s)
    {
        this.OutputPath.Text = s;
        this.Ok.Disabled = this.OutputPath.Text == "" || (this.AsLayers.Pressed ? false : this.SpriteName.Text == "");
    }

    
    public async void OnConfirmExport()
    {
        var _data = new ExportData(){
            Canvas = this.SubmitCanvas,
            Columns = (int)this.Cols.Value,
            Rows = (int)this.Rows.Value,
            Sheets = (int)this.Sheets.Value,
            Scale = (int)this.Scale.Value,
            SplitFrames = this.SplitFrames.Pressed,
            MapToCanvas = this.MapToCanvas.Pressed,
            ClipContent = this.ClipContent.Pressed,
            NoCopies = this.NoCopies.Pressed,
            SpriteName = this.SpriteName.Text,
            OutputPath = this.OutputPath.Text,
        };

        this.Progress.PopupCentered();

        var _thread = new Thread();

        if(this.AsLayers.Pressed)
            { _thread.Start(this.Export, "ExportLayers", _data); }
        else
            { _thread.Start(this.Export, "ExportSprite", _data); }

        await ToSignal(this.Export, "ProgressFinished");

        this.Progress.Hide();
        this.Hide();
    }
}

public class TreeLayer : Node
{
    public Layer Data;
}