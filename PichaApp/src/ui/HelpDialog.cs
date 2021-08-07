using Godot;

public class HelpDialog : WindowDialog
{
    public override void _Ready()
    {
        JSONParseResult _result;

        using(var _reader = new File())
        {
            _reader.Open("./res/data/docs.json", File.ModeFlags.Read);
            _result = JSON.Parse(_reader.GetAsText());
        }

        // if(_result.Result is Godot.Collections.Dictionary a)
        // {
        //     foreach(object o in a.Keys)
        //     {
        //         if(o is string section)
        //         {
        //             if(a[section] is Godot.Collections.Dictionary a2)
        //             {
        //                 foreach(object o2 in a2.Keys)
        //                 {
        //                     if(o2 is string title)
        //                     {
        //                         GD.Print($"{section}, {title}, {a2[title]}");
        //                     }
        //                 }
        //             }
        //         }
        //     }
        // }
    }
}
