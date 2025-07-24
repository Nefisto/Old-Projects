using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class MenuStack : SerializedMonoBehaviour, IService, IMenuStack
{
    [TitleGroup("Settings")]
    [SerializeField]
    private Image blockPanelPrefab;

    [TitleGroup("Debug")]
    [OdinSerialize]
    [TableList]
    private List<(IMenu menu, Image blockPanel)> menuStack = new();

    private void Awake()
    {
        menuStack = new List<(IMenu menu, Image blockPanel)>();

        ServiceLocator.MenuStack = this;
    }

    public event Action OnOpenFirstMenu;
    public event Action OnCloseAllMenus;
    public event Action<IMenu> OnCloseMenu;

    [DisableInEditorButton]
    public void OpenMenu (IMenu menu, MenuSetupContext context = default)
    {
        if (!menuStack.Any())
            OnOpenFirstMenu?.Invoke();

        if (!TryGetAlreadyOpenMenu(menu, out var foundTuple))
        {
            StackNewMenu(menu, context);
            menu.Open(context);
        }
        else
        {
            menuStack.Remove(foundTuple);
            menuStack.Add(foundTuple);
        }

        Draw();
    }

    [DisableInEditorButton]
    public void CloseMenu (MenuSetupContext context = default, IMenu.CloseContext closeContext = default)
    {
        var lastMenu = menuStack.LastOrDefault();
        if (lastMenu != default)
        {
            lastMenu.menu.Close(closeContext);
            Destroy(lastMenu.blockPanel.gameObject);
            menuStack.Remove(lastMenu);

            OnCloseMenu?.Invoke(lastMenu.menu);
            context?.onCloseMenu?.Invoke();
        }

        if (!menuStack.Any())
            OnCloseAllMenus?.Invoke();

        Draw();
    }

    private bool TryGetAlreadyOpenMenu (IMenu menu, out (IMenu menu, Image blockPanel) foundTuple)
    {
        foundTuple = menuStack.FirstOrDefault(tuple => tuple.menu == menu);
        return foundTuple != default;
    }

    private void StackNewMenu (IMenu menu, MenuSetupContext context)
    {
        var blockPanel = Instantiate(blockPanelPrefab, menu.Transform.parent, false);
        blockPanel.transform.SetSiblingIndex(Mathf.Max(0, menu.Transform.GetSiblingIndex() - 1));

        blockPanel
            .GetComponent<Button>()
            .onClick
            .AddListener(() => CloseMenu(context, new IMenu.CloseContext()
            {
                hasClosedThroughOutsideClick = true
            }));

        menuStack.Add((menu, blockPanel));
    }

    private void Draw()
    {
        for (var i = 0; i < menuStack.Count; i++)
        {
            menuStack[i].blockPanel.transform.SetSiblingIndex(-1);
            menuStack[i].menu.Transform.SetSiblingIndex(-1);
        }
    }
}