using System.Collections.Generic;

using Godot;

public class HelpDialog : WindowDialog
{

    public SortedList<int, DocsObject> Docs = new SortedList<int, DocsObject>();

    public override void _Ready()
    {
        JSONParseResult _docs;

        using(var _reader = new File())
        {
            _reader.Open("./res/data/docs.json", File.ModeFlags.Read);
            _docs = JSON.Parse(_reader.GetAsText());
        }

        if(_docs.Result is Godot.Collections.Array a)
        {
            foreach(object o in a)
            {
                if(o is Godot.Collections.Dictionary d)
                {
                    this.Docs.Add(this.Docs.Count, this.GetDocsObject(d));
                }
            }
        }
    }

    private DocsObject GetDocsObject(Godot.Collections.Dictionary document)
    {
        var _output = new DocsObject(){
            Title = (string)document["section_name"],
            Text = (string)document["section_text"],
        };

        if(document.Contains("sub_section"))
        {
            var _subItems = (Godot.Collections.Array)document["sub_section"];
            foreach(object o in _subItems)
            {
                if(o is Godot.Collections.Dictionary d)
                {
                    _output.SubDocs.Add(_output.SubDocs.Count, this.GetDocsObject(d));
                }
            }
        }
        else
        {
            _output.SubDocs = null;
        }

        return _output;
    }
}

public class DocsObject
{
    public string Title;
    public string Text;
    public SortedList<int, DocsObject> SubDocs = new SortedList<int, DocsObject>();
}
