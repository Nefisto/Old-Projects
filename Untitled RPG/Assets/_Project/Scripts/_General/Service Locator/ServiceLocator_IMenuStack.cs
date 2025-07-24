using System.Linq;
using UnityEngine;

public static partial class ServiceLocator
{
    private static MenuStack menuStack;

    public static MenuStack MenuStack
    {
        set => menuStack = value;
        get
        {
            if (menuStack != null)
                return menuStack;

            menuStack = Object.FindObjectOfType<MenuStack>();
            if (menuStack == null)
            {
                var prefab = Database.ServiceMenuStack.Data.First().GetComponent<MenuStack>();
                menuStack = Object.Instantiate(prefab);
            }

            menuStack.name = "[Service] MenuStack";
            Object.DontDestroyOnLoad(menuStack);

            return menuStack;
        }
    }
}