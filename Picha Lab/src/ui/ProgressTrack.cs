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

    public void OnProgressChanged(int i, int total)
    {
        float _val = ((float)i / (float)total) * 100f;
        this.Bar.Value = _val;
        this.Status.Text = $"Processing sprite {i} of {total}...";
    }

    public void OnStatusUpdate(string s)
    {
        this.Status.Text = s;
    }
}
