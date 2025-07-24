using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class SlotIconGrid : MonoBehaviour, IMenu
{
    [TitleGroup("References")]
    [SerializeField]
    private SlotIconEntry slotEntry;

    public event Action<SlotIconClickContext> OnClick;

    public IEnumerator Setup()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        OnClick = null;
        foreach (var (key, value) in Database.GameIcons.SaveSlotIcons.EnumToIcon)
        {
            var instance = Instantiate(slotEntry, transform, false);
            yield return instance.Setup(key, value.icon, value.isUnlocked);

            instance.OnClick += ctx => OnClick?.Invoke(ctx);
        }
    }
}