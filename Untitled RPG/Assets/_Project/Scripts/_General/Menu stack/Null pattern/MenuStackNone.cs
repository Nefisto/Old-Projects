using System;

#pragma warning disable 0067

public class MenuStackNone : IMenuStack
{
    public event Action OnOpenFirstMenu;
    public event Action OnCloseAllMenus;
    public event Action<IMenu> OnCloseMenu;

    public void OpenMenu (IMenu menu, MenuSetupContext context) { }
    public void CloseMenu (MenuSetupContext context = default, IMenu.CloseContext closeContext = default) { }
}