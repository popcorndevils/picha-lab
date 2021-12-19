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

    public void OnProgressChanged(int i, int total, string msg = null)
    {
        float _val = ((float)i / (float)total) * 100f;
        this.Bar.Value = _val;
        if(msg == null)
        {
            this.Status.Text = $"Processing sprite {i} of {total}...";
        }
        else
        {
            this.Status.Text = msg;
        }
    }

    public void OnStatusUpdate(string s)
    {
        this.Status.Text = s;
    }
}
