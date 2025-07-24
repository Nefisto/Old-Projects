using UnityEngine;

public class DisableMenuOnStart : MonoBehaviour
{
    private void Start()
    {
        if (TryGetComponent<IMenu>(out var menu))
            menu.Close();
        else
            gameObject.SetActive(false);
    }
}