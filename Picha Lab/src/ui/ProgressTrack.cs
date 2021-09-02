using Godot;
using System;

public class ProgressTrack : PopupPanel
{
    public Label Status;
    public ProgressBar Bar;
    
    public override void _Ready()
    {
        this.Status = this.GetNode<Label>("VBox/Status");
        this.Bar = this.GetNode<ProgressBar>("VBox/Bar");
    }

    public void IncrementProgress(int i, int total)
    {
        float _val = ((float)i / (float)total) * 100f;
        GD.Print(_val);
        this.Bar.Value = _val;
        this.Status.Text = $"Processing sprite {i} of {total}...";
    }
}
