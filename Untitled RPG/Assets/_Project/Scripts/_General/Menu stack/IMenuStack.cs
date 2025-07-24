using System;

public interface IMenuStack
{
    public event Action OnOpenFirstMenu;
    public event Action OnCloseAllMenus;
    public event Action<IMenu> OnCloseMenu;

    public void OpenMenu (IMenu menu, MenuSetupContext context = default);
    public void CloseMenu (MenuSetupContext context = default, IMenu.CloseContext closeContext = default);
}