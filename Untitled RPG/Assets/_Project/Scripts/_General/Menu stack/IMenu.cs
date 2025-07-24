using UnityEngine;

public interface IMonobehavior
{
    public Transform Transform => (this as MonoBehaviour).transform;
}

public interface IMenu : IMonobehavior
{
    public void Open (MenuSetupContext context = default)
    {
        Transform.gameObject.SetActive(true);
    }

    public void Close (CloseContext context = default)
    {
        Transform.gameObject.SetActive(false);
    }

    public class CloseContext
    {
        public bool hasClosedThroughOutsideClick;
    }
}