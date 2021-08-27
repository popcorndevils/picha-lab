using Godot;

public partial class CanvasView : VBoxContainer
{
    public GenCanvas Active {
        get {
            if(this.Content.GetChildren().Count > 0)
            {
                return this.Content.GetChild<CanvasContainer>(this.Content.CurrentTab).Canvas;
            }
            return null;
        } 
    }

    public bool FileExists {
        get {
            if(this.Active != null)
            {
                return this.Active.FileExists;
            }
            return false;
        }
    }
}