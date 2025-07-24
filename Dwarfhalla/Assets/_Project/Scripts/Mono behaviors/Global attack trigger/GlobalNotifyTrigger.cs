using System;
using UnityEngine;

public class GlobalNotifyTrigger : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.GlobalNotifyObject = gameObject;
        gameObject.SetActive(false);
    }
}