using System.Collections.Generic;

using Godot;

public delegate void ItemSelectedHandler(MenuBarItem action);

public class MenuBar : MarginContainer
{
    public event ItemSelectedHandler ItemSelected;
    private Dictionary<int, MenuBarItem> _ActionList;
    private Dictionary<string, (PopupMenu pop, int idx)> _ItemList;

    public override void _Ready()
    {
        this._ActionList = new Dictionary<int, MenuBarItem>();
        this._ItemList = new Dictionary<string, (PopupMenu, int)>();
        this._PrepMenu();
    }

    private void _PrepMenu()
    {
        var _menuButtons = this.GetNode<HBoxContainer>("OptButtons").GetChildren();

        foreach(Node _n in _menuButtons)
        {
            if(_n is MenuButton _button)
            {
                var _pop = _button.GetPopup();
                _pop.Connect("id_pressed", this, "_OnMenuSelected");
                this._CycleItems(_pop, _button.GetChildren());
            }
        }
    }

    private void _CycleItems(PopupMenu p, Godot.Collections.Array items)
    {
        foreach(Node _b in items)
        {
            if(_b is MenuBarItem _item)
            {
                this._SetMenuItem(p, _item);
            }
        }
    }

    private void _SetMenuItem(PopupMenu p, MenuBarItem item)
    {
        var _subitems = item.GetChildren();
        var _itemID = this._ActionList.Count;

        item.ParentPopup = p;

        this._ActionList.Add(_itemID, item);
        this._ItemList.Add(item.Action, (p, _itemID));

        if(_subitems.Count > 0)
        {
            var _subName = $"SubMenu_{item.Text}";
            var _subPop = new PopupMenu() { Name = _subName };

            p.AddChild(_subPop);
            p.AddSubmenuItem(item.Text, _subName);
            _subPop.Connect("id_pressed", this, "_OnMenuSelected");
            this._CycleItems(_subPop, _subitems);
        }
        else
        {
            if(item.Separate)
                { p.AddSeparator(); } 
            if(item.Icon is null)
                { p.AddItem(item.Text, _itemID); } 
            else
                { p.AddIconItem(item.Icon, item.Text, _itemID); }
            if(item.CheckButton)
                { p.SetItemAsCheckable(p.GetItemIndex(_itemID), true); }
            if(item.StartDisabled)
                { p.SetItemDisabled(_itemID, true); }
        }
    }

    public void _OnMenuSelected(int i)
    {
        var _item = this._ActionList[i];

        if(_item.CheckButton)
        {
            var _pop = _item.ParentPopup;
            _item.IsChecked = !_item.IsChecked;
            _pop.SetItemChecked(_pop.GetItemIndex(i), _item.IsChecked);
        }

        ItemSelected?.Invoke(this._ActionList[i]);
    }

    public void SetDisabled(string action, bool disabled)
    {
        var _opt = this._ItemList[action];
        _opt.pop.SetItemDisabled(_opt.idx, disabled);
    }
}
