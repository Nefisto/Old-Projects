using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
#endif
using UnityEngine;

[Serializable]
public class Dialog : IEnumerable<string>
{
    [SerializeField]
    private DialogCommonEvents battleEvent;

    [ShowIf("@BattleEvent == DialogCommonEvents.Custom")]
    [SerializeField]
    private string customEvent;

    [SerializeField]
    private Sprite bigSprite;

    [TextArea]
    [LabelText("Message")]
    public List<string> messages;
    
    public DialogCommonEvents BattleEvent => battleEvent;
    public string CustomEvent => customEvent;

    public Sprite BigSprite => bigSprite;

    public IEnumerator<string> GetEnumerator()
        => messages.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}