using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TestB : MonoBehaviour, OneInterface
{
    public void Show()
        => Debug.Log("BB");
}