using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public List<EventTrigger> buttons;

    public void ShowEnter()
        => Debug.Log("Enter");

    public void ShowExit()
        => Debug.Log("Exit");
    
    public IEnumerator Start()
    {
        var waitForUI = new WaitForUIEventTrigger(EventTriggerType.PointerEnter,  bt => Debug.Log(bt.name), buttons.ToArray());

        yield return waitForUI;
    }

    [Button]
    public void A()
    {
        StartCoroutine(ALocal());
    }

    private IEnumerator ALocal()
    {
        Debug.Log($"B: {Time.frameCount}");
        yield return BLocal();
        Debug.Log($"E: {Time.frameCount}");
    }

    private IEnumerator BLocal()
    {
        yield break;
    }
}