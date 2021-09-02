using System;

using Godot;

public class ExportDialog : WindowDialog
{
    public SpriteExporter Export;

    public MarginContainer Margins;

    public FileDialog FileDialog;
    public ConfirmationDialog Confirmation;
    public ProgressTrack Progress;

    public SpinBox Rows;
    public SpinBox Cols;
    public SpinBox Sheets;
    public SpinBox Scale;

    public CheckBox SplitFrames;
    public CheckBox AsLayers;
    public CheckBox FullSized;

    public LineEdit SpriteName;
    public LineEdit OutputPath;
    public Button FileButton;

    public Button Ok;
    public Button Cancel;

    public override void _Ready()
    {
        this.AddToGroup("diag_export");

        this.Margins = this.GetNode<MarginContainer>("Margins");
        
        this.FileDialog = this.GetNode<FileDialog>("FileDialog");
        this.Confirmation = this.GetNode<ConfirmationDialog>("Confirmation");
        this.Progress = this.GetNode<ProgressTrack>("ProgressTrack");
        
        this.Rows = this.GetNode<SpinBox>("Margins/Contents/OptionBox/rows");
        this.Cols = this.GetNode<SpinBox>("Margins/Contents/OptionBox/cols");
        this.Sheets = this.GetNode<SpinBox>("Margins/Contents/OptionBox/sheets");
        this.Scale = this.GetNode<SpinBox>("Margins/Contents/OptionBox/scale");

        this.SplitFrames = this.GetNode<CheckBox>("Margins/Contents/OptionBox/split_frames");
        this.AsLayers = this.GetNode<CheckBox>("Margins/Contents/OptionBox/as_layers");
        this.FullSized = this.GetNode<CheckBox>("Margins/Contents/OptionBox/full_sized");

        this.SpriteName = this.GetNode<LineEdit>("Margins/Contents/sprite_name");
        this.FileButton = this.GetNode<Button>("Margins/Contents/PathBox/btn_browse");
        this.OutputPath = this.GetNode<LineEdit>("Margins/Contents/PathBox/path");
        
        this.Ok = this.GetNode<Button>("Margins/Contents/BBox/ok");
        this.Cancel = this.GetNode<Button>("Margins/Contents/BBox/cancel");

        this.Confirmation.GetOk().FocusMode = FocusModeEnum.None;
        this.Confirmation.GetCancel().FocusMode = FocusModeEnum.None;

        this.FileButton.Connect("pressed", this, "OnFileButtonPress");
        this.AsLayers.Connect("pressed", this, "OnAsLayersPress");
        this.Ok.Connect("pressed", this, "OnOkPress");
        this.Cancel.Connect("pressed", this, "OnCancelPress");
        this.FileDialog.Connect("dir_selected", this, "OnDirSelect");
        this.Confirmation.Connect("confirmed", this, "OnConfirmExport");
    }


    public void Open(SpriteExporter export)
    {
        this.Export = export;
        this.Export.Connect("ProgressChanged", this.Progress, "OnProgressChanged");
        this.Export.Connect("StatusUpdate", this.Progress, "OnStatusUpdate");

        this.RectSize = this.Margins.RectSize;
        
        this.Rows.Value = 1;
        this.Cols.Value = 1;
        this.Sheets.Value = 1;
        this.Scale.Value = 1;

        this.SplitFrames.Pressed = false;
        this.AsLayers.Pressed = false;
        this.FullSized.Pressed = false;
        this.FullSized.Disabled = true;
        
        this.PopupCentered();
    }


    public void OnFileButtonPress()
    {
        this.FileDialog.PopupCentered();
    }


    public void OnAsLayersPress()
    {
        this.FullSized.Disabled = !this.AsLayers.Pressed;
        this.SpriteName.Editable = !this.AsLayers.Pressed;
        if(SpriteName.Editable)
        {
            this.SpriteName.FocusMode = FocusModeEnum.All;
        }
        else
        {
            this.SpriteName.FocusMode = FocusModeEnum.None;
        }
    }


    public void OnOkPress()
    {
        var _frames = MathX.LCD(this.Export.Canvas.FrameCounts);
        var _word = _frames > 1 ? "frames" : "frame";
        this.Confirmation.DialogText = $"Each Sprite includes {_frames} {_word} of animation.\nWould you like to continue with export?";

        this.Confirmation.PopupCentered();
    }


    public void OnCancelPress()
    {
        this.Hide();
    }


    public void OnDirSelect(string s)
    {
        this.OutputPath.Text = s;
    }

    
    public async void OnConfirmExport()
    {
        var _data = new ExportData(){
            Columns = (int)this.Cols.Value,
            Rows = (int)this.Rows.Value,
            Sheets = (int)this.Sheets.Value,
            Scale = (int)this.Scale.Value,
            SplitFrames = this.SplitFrames.Pressed,
            FullSizedLayers = this.FullSized.Pressed,
            SpriteName = this.SpriteName.Text,
            OutputPath = this.OutputPath.Text,
        };

        this.Progress.PopupCentered();

        var _thread = new Thread();
        _thread.Start(this.Export, "ExportSprite", _data);
        await ToSignal(this.Export, "ProgressFinished");

        this.Progress.Hide();
        this.Hide();
    }
}