using System;

using Godot;

public class ExportDialog : WindowDialog
{
    public ExportManager Export;

    public MarginContainer Margins;

    public FileDialog FileDialog;
    public ConfirmationDialog Confirmation;

    public SpinBox Rows;
    public SpinBox Cols;
    public SpinBox Sheets;

    public Button FileButton;
    public Button AsLayers;
    public Button FullSized;

    public LineEdit Path;

    public Button Ok;
    public Button Cancel;

    public override void _Ready()
    {
        this.AddToGroup("diag_export");

        this.Margins = this.GetNode<MarginContainer>("Margins");
        
        this.FileDialog = this.GetNode<FileDialog>("FileDialog");
        this.Confirmation = this.GetNode<ConfirmationDialog>("Confirmation");
        
        this.Rows = this.GetNode<SpinBox>("Margins/Contents/rows");
        this.Cols = this.GetNode<SpinBox>("Margins/Contents/cols");
        this.Sheets = this.GetNode<SpinBox>("Margins/Contents/sheets");

        this.FileButton = this.GetNode<Button>("Margins/Contents/PathBox/btn_browse");
        this.AsLayers = this.GetNode<Button>("Margins/Contents/OptionBox/as_layers");
        this.FullSized = this.GetNode<Button>("Margins/Contents/OptionBox/full_sized");

        this.Path = this.GetNode<LineEdit>("Margins/Contents/PathBox/path");
        
        this.Ok = this.GetNode<Button>("Margins/Contents/BBox/ok");
        this.Cancel = this.GetNode<Button>("Margins/Contents/BBox/cancel");

        this.FileButton.Connect("pressed", this, "OnFileButtonPress");
        this.AsLayers.Connect("pressed", this, "OnAsLayersPress");
        this.Ok.Connect("pressed", this, "OnOkPress");
        this.Cancel.Connect("pressed", this, "OnCancelPress");
        this.FileDialog.Connect("dir_selected", this, "OnDirSelect");

        this.Confirmation.GetOk().FocusMode = FocusModeEnum.None;
        this.Confirmation.GetCancel().FocusMode = FocusModeEnum.None;
    }


    public void Open(ExportManager export)
    {
        this.Export = export;

        this.RectSize = this.Margins.RectSize;
        
        this.Rows.Value = 1;
        this.Cols.Value = 1;
        this.Sheets.Value = 1;

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

        if(this.FullSized.Disabled)
        {
            this.FullSized.Pressed = false;
        }
    }


    public void OnOkPress()
    {
        this.Confirmation.DialogText = $"Each Sprite includes {MathX.LCD(this.Export.Canvas.FrameCount)} frames of animation.\nWould you like to continue with export?";

        this.Confirmation.PopupCentered();
    }


    public void OnCancelPress()
    {
        this.Hide();
    }


    public void OnDirSelect(string s)
    {
        this.Path.Text = s;
    }
}
