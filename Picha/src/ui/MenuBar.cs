using Godot;

public delegate void ItemSelectedHandler(int item);

public class MenuBar : MarginContainer
{
    public event ItemSelectedHandler ItemSelected;

    private MenuButton _File;
    private MenuButton _Edit;

    public override void _Ready()
    {
        this._File = this.GetNode<MenuButton>("OptButtons/OptFile");
        this._Edit = this.GetNode<MenuButton>("OptButtons/OptEdit");

        this._PrepMenu();
    }

    private void _PrepMenu()
    {
        this._File.GetPopup().Connect("id_pressed", this, "_OnMenuSelected");
        this._Edit.GetPopup().Connect("id_pressed", this, "_OnMenuSelected");
    }

    public void _OnMenuSelected(int i)
    {
        ItemSelected?.Invoke(i);
    }
}
