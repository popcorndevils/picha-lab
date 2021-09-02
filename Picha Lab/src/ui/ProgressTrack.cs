using Godot;
using System;

public class ProgressTrack : PopupPanel
{
    public Label Status;
    
    public override void _Ready()
    {
        this.Status = this.GetNode<Label>("VBox/Status");
    }

    public void IncrementProgress(int val)
    {
        this.Status.Text = $"Yoalksjdflkjlkdjslkjdlfkjdlskjdf\n{val.ToString()}";
    }
}
