using System;
using Godot;

public class CanvasInspect : ScrollContainer
{
    public GenCanvas Canvas;

    private VBoxContainer _Contents;
    private GridContainer _Settings;

    private CheckBox _AutoGenEdit;
    private SpinBox _AutoGenTimeEdit;

    public override void _Ready()
    {
        this.AddToGroup("gp_canvas_gui");
        
        this._Contents = new VBoxContainer() {
            SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        };

        this._Settings = new GridContainer() {
            Columns = 2,
            SizeFlagsHorizontal = (int)SizeFlags.ExpandFill,
        };

        var _autoGenLabel = new Label() {
            Text = "Auto-Gen",
            Align = Label.AlignEnum.Right,
        };


        this._AutoGenEdit = new CheckBox() {
            SizeFlagsHorizontal = 0,
        };

        var _autoGenTimeLabel = new Label() {
            Text = "Time-to-Gen",
            Align = Label.AlignEnum.Right,
        };

        this._AutoGenTimeEdit = new SpinBox() {
            MinValue = 0,
            MaxValue = 99,
            Step = .1f,
        };

        this._Contents.AddChild(this._Settings);
        this.AddChild(this._Contents);

        this._Settings.AddChild(_autoGenLabel);
        this._Settings.AddChild(this._AutoGenEdit);
        this._Settings.AddChild(_autoGenTimeLabel);
        this._Settings.AddChild(this._AutoGenTimeEdit);

        this._AutoGenEdit.Connect("pressed", this, "OnCanvasEdit");
        this._AutoGenTimeEdit.Connect("value_changed", this, "OnCanvasEdit");
    }

    public void LoadCanvas(GenCanvas c)
    {
        this.Canvas = c;

        this._AutoGenEdit.Pressed = c.AutoGen;

        this._AutoGenTimeEdit.Disconnect("value_changed", this, "OnCanvasEdit");
        this._AutoGenTimeEdit.Value = c.TimeToGen;
        this._AutoGenTimeEdit.Connect("value_changed", this, "OnCanvasEdit");
    }

    public void OnCanvasEdit(params object[] args)
        { this.OnCanvasEdit(); }
    public void OnCanvasEdit()
    {
        this.Canvas.AutoGen = this._AutoGenEdit.Pressed;
        this.Canvas.TimeToGen = (float)this._AutoGenTimeEdit.Value;
    }
}
