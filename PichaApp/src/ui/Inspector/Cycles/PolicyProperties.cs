using System;
using System.Collections.Generic;

using Godot;

using PichaLib;
using OctavianLib;

public delegate void PolicyChangedHandler(Policy p);

public class PolicyProperties : BaseSection
{
    public event PolicyChangedHandler PolicyChanged;
    public Policy Policy;
    public GenLayer Layer;

    private bool _IgnoreSignals = false;

    private Button _Delete = new Button() {
        Text = "x",
        SizeFlagsHorizontal = 0,
        FocusMode = FocusModeEnum.None,
    };

    // SETTINGS
    private OptionButton _InputEdit;
    private OptionButton _OutputEdit;
    private SpinBox _RateEdit;
    private OptionButton _ConditionAEdit;
    private OptionButton _ConditionLogicEdit;
    private OptionButton _ConditionBEdit;

    private CrossTable<string, int> _PixelTable = new CrossTable<string, int>();

    public override void _Ready()
    {
        base._Ready();
        
        this.SectionGrid.Columns = 2;

        this._InputEdit = new OptionButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill
        };

        this._OutputEdit = new OptionButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill
        };

        this._RateEdit = new SpinBox() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill,
            MinValue = 0.0f,
            MaxValue = 1.0f,
            Step = .05f,
        };

        this._ConditionAEdit = new OptionButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill
        };

        this._ConditionLogicEdit = new OptionButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill
        };

        this._ConditionBEdit = new OptionButton() {
            SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill
        };

        var _inputLabel = new Label() {
            Text = "Input",
            Align = Label.AlignEnum.Right,
        };

        var _outputLabel = new Label() {
            Text = "Output",
            Align = Label.AlignEnum.Right,
        };

        var _rateLabel = new Label() {
            Text = "Rate",
            Align = Label.AlignEnum.Right,
        };

        var _conditionLabel = new Label() {
            Text = "WHERE",
            Align = Label.AlignEnum.Center,
        };

        this.SectionGrid.AddChildren(_inputLabel, this._InputEdit, _outputLabel, this._OutputEdit, 
            _rateLabel, this._RateEdit, _conditionLabel, this._ConditionAEdit, new Control(),
            this._ConditionLogicEdit, new Control(), this._ConditionBEdit);

        this._InputEdit.Connect("item_selected", this, "OnPolicySettingEdit");
        this._OutputEdit.Connect("item_selected", this, "OnPolicySettingEdit");
        this._RateEdit.Connect("value_changed", this, "OnPolicySettingEdit");
        this._ConditionAEdit.Connect("item_selected", this, "OnPolicySettingEdit");
        this._ConditionLogicEdit.Connect("item_selected", this, "OnPolicySettingEdit");
        this._ConditionBEdit.Connect("item_selected", this, "OnPolicySettingEdit");
        this._Delete.Connect("pressed", this, "OnPolicyDelete");

        this.HeaderContainer.AddChild(this._Delete);
    }

    public void LoadPolicy(GenLayer l, Policy p)
    {
        this.Layer = l;
        this.Policy = p;

        this._PixelTable.Clear();
        foreach(string _pN in l.Pixels.Keys) { this._PixelTable.Add(_pN, this._PixelTable.Count); }

        var _optBtns = new OptionButton[] {this._InputEdit, this._OutputEdit, this._ConditionBEdit};

        foreach(OptionButton _b in _optBtns)
        {
            _b.Clear();
            foreach(KeyValuePair<string, int> _pair in this._PixelTable)
            {
                _b.AddItem(_pair.Key, _pair.Value);
            }
        }

        foreach(int i in Enum.GetValues(typeof(ConditionTarget)))
        {
            this._ConditionAEdit.AddItem(Enum.GetName(typeof(ConditionTarget), i), i);
        }

        foreach(int i in Enum.GetValues(typeof(ConditionExpression)))
        {
            this._ConditionLogicEdit.AddItem(Enum.GetName(typeof(ConditionExpression), i), i);
        }
        
        this._InputEdit.Selected = this._PixelTable[p.Input];
        this._OutputEdit.Selected = this._PixelTable[p.Output];

        this._IgnoreSignals = true;
        this._RateEdit.Value = p.Rate;
        this._IgnoreSignals = false;

        this._ConditionAEdit.Selected = (int)p.ConditionA;
        this._ConditionLogicEdit.Selected = (int)p.ConditionLogic;
        this._ConditionBEdit.Selected = this._PixelTable[p.ConditionB];
    }

    public void OnPolicySettingEdit(params object[] args) { this.OnPolicySettingEdit(); }
    public void OnPolicySettingEdit()
    {
        if(!this._IgnoreSignals)
        {
            var _inputName = $"{this._PixelTable[this._InputEdit.Selected]}";
            var _outputName = $"{this._PixelTable[this._OutputEdit.Selected]}";

            this.SectionTitle = $"{_inputName} -> {_outputName}";

            this.Policy.Input = this._PixelTable[this._InputEdit.Selected];
            this.Policy.Output = this._PixelTable[this._OutputEdit.Selected];
            this.Policy.Rate = (float)this._RateEdit.Value;
            this.Policy.ConditionA = (ConditionTarget)this._ConditionAEdit.Selected;
            this.Policy.ConditionLogic = (ConditionExpression)this._ConditionLogicEdit.Selected;
            this.Policy.ConditionB = this._PixelTable[this._ConditionBEdit.Selected];
        }
    }

    public void RenamePixel(string oldName, string newName)
    {
        var _optBtns = new OptionButton[] {this._InputEdit, this._OutputEdit, this._ConditionBEdit};

        int[] _selected = new int[] {
            this._InputEdit.Selected,
            this._OutputEdit.Selected,
            this._ConditionBEdit.Selected
        };

        int _sel = this._PixelTable[oldName];
        this._PixelTable.Remove(oldName);
        this._PixelTable.Add(newName, _sel);

        foreach(OptionButton _b in _optBtns)
        {
            _b.Clear();
            foreach(KeyValuePair<string, int> _pair in this._PixelTable)
            {
                _b.AddItem(_pair.Key, _pair.Value);
            }
        }

        this._InputEdit.Selected = _selected[0];
        this._OutputEdit.Selected = _selected[1];
        this._ConditionBEdit.Selected = _selected[2];
        
        var _inputName = $"{this._PixelTable[this._InputEdit.Selected]}";
        var _outputName = $"{this._PixelTable[this._OutputEdit.Selected]}";
        this.SectionTitle = $"{_inputName} -> {_outputName}";
    }

    public void AddNewPixel(Pixel p)
    {
        this._PixelTable.Add(p.Name, this._PixelTable.Count); 
        var _optBtns = new OptionButton[] {this._InputEdit, this._OutputEdit, this._ConditionBEdit};

        foreach(OptionButton _b in _optBtns)
        {
            _b.AddItem(p.Name, this._PixelTable.Count);
        }
    }

    public void OnPolicyDelete()
    {
        GD.Print($"DELETING POLICY {this.SectionTitle}");
    }
}