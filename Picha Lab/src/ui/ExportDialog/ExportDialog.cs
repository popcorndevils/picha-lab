using System;

using Godot;
using PichaLib;

public class ExportDialog : WindowDialog
{
    private Canvas _OriginalCanvas;
    private Canvas _SubmitCanvas;

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

    public override void _Ready()
    {
        this.AddToGroup("diag_export");

        this.Margins = this.GetNode<MarginContainer>("Margins");
        
        this.FileDialog = this.GetNode<FileDialog>("FileDialog");
        this.Confirmation = this.GetNode<ConfirmationDialog>("Confirmation");
        this.SelectLayersDialog = this.GetNode<SelectLayersDialog>("SelectLayersDialog");
        this.Progress = this.GetNode<ProgressTrack>("ProgressTrack");
        
        this.Rows = this.GetNode<SpinBox>("Margins/Contents/OptionBox/rows");
        this.Cols = this.GetNode<SpinBox>("Margins/Contents/OptionBox/cols");
        this.Sheets = this.GetNode<SpinBox>("Margins/Contents/OptionBox/sheets");
        this.Scale = this.GetNode<SpinBox>("Margins/Contents/OptionBox/scale");

        this.ClipContent = this.GetNode<CheckBox>("Margins/Contents/OptionBox/clip_content");
        this.SplitFrames = this.GetNode<CheckBox>("Margins/Contents/OptionBox/split_frames");
        this.AsLayers = this.GetNode<CheckBox>("Margins/Contents/OptionBox/as_layers");
        this.MapToCanvas = this.GetNode<CheckBox>("Margins/Contents/OptionBox/map_to_canvas");
        this.NoCopies = this.GetNode<CheckBox>("Margins/Contents/OptionBox/no_copies");

        this.SpriteName = this.GetNode<LineEdit>("Margins/Contents/sprite_name");
        this.FileButton = this.GetNode<Button>("Margins/Contents/PathBox/btn_browse");
        this.OutputPath = this.GetNode<LineEdit>("Margins/Contents/PathBox/path");
        
        this.SetLayers = this.GetNode<Button>("Margins/Contents/LayersBox/set_layers");
        this.Ok = this.GetNode<Button>("Margins/Contents/BBox/ok");
        this.Cancel = this.GetNode<Button>("Margins/Contents/BBox/cancel");

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

        this.RectMinSize = this.Margins.RectSize;

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

        this.SpriteName.Text = "";
        this.OutputPath.Text = "";

        this.OnOptionButtonPress();
        this.PopupCentered();
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
        this.SelectLayersDialog.PopupCentered();
    }

    public void OnOkPress()
    {
        var _frames = MathX.LCD(this._OriginalCanvas.FrameCounts);
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
            Canvas = this._OriginalCanvas,
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