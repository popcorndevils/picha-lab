using System.Collections.Generic;

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

    public List<GenCanvas> Canvases {
        get {
            var _output = new List<GenCanvas>();
            foreach(Node n in this.Content.GetChildren())
            {
                if(n is CanvasContainer c)
                {
                    _output.Add(c.Canvas);
                }
            }
            return _output;
        }
    }
}