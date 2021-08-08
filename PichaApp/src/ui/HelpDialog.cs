using System.Collections.Generic;

using Godot;

public class HelpDialog : WindowDialog
{

    public SortedList<int, DocsObject> Docs = new SortedList<int, DocsObject>();

    private LineEdit _DocTitle;
    private TextEdit _DocText;
    private Tree _DocTree;

    public override void _Ready()
    {
        this._DocTitle = this.GetNode<LineEdit>("HBox/VBoxContainer/DocTitle");
        this._DocText = this.GetNode<TextEdit>("HBox/VBoxContainer/DocTextBox/DocText");
        this._DocTree= this.GetNode<Tree>("HBox/DocTree");

        this._DocTree.Connect("item_selected", this, "OnTreeItemSelect");

        this._PopulateDocs();
        this._PopulateTree();
    }

    private void _PopulateDocs()
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
                    this.Docs.Add(this.Docs.Count, this._GetDocsObject(d));
                }
            }
        }
    }

    private void _PopulateTree()
    {
        var _root = this._DocTree.CreateItem();
        _root.SetText(0, "ROOT");

        if(this.Docs.Count > 0)
        {
            foreach(DocsObject d in this.Docs.Values)
            {
                this._ProcessBranch(_root, d);
            }
        }
    }

    private void _ProcessBranch(TreeItem root, DocsObject doc)
    {
        var _item = this._DocTree.CreateItem(root);
        _item.SetText(0, doc.Title);
        _item.SetMetadata(0, doc);
        _item.Collapsed = true;

        if(doc.SubDocs != null)
        {
            foreach(DocsObject d in doc.SubDocs.Values)
            {
                this._ProcessBranch(_item, d);
            }
        }
    }

    private DocsObject _GetDocsObject(Godot.Collections.Dictionary document)
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
                    _output.SubDocs.Add(_output.SubDocs.Count, this._GetDocsObject(d));
                }
            }
        }
        else
        {
            _output.SubDocs = null;
        }

        return _output;
    }

    public void OnTreeItemSelect()
    {
        var _item = this._DocTree.GetSelected();
        var _data = (DocsObject)_item.GetMetadata(0);
        this._DocTitle.Text = _data.Title;
        this._DocText.Text = _data.Text;
    }
}

public class DocsObject : Node
{
    public string Title;
    public string Text;
    public SortedList<int, DocsObject> SubDocs = new SortedList<int, DocsObject>();
}
