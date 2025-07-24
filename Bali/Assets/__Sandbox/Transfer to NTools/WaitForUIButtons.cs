using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WaitForUIButtons : CustomYieldInstruction, IDisposable
{
    private readonly List<ButtonCallback> buttons = new();
    private Action<Button> onClickedCallback;

    public WaitForUIButtons (Action<Button> onClickedCallback, params Button[] buttons)
    {
        this.onClickedCallback = onClickedCallback;
        this.buttons.Capacity = buttons.Length;
        foreach (var b in buttons)
        {
            if (b == null)
                continue;

            var bc = new ButtonCallback { Button = b };
            bc.Listener = () => OnButtonPressed(bc.Button);

            this.buttons.Add(bc);
        }

        Reset();
    }

    public WaitForUIButtons (params Button[] aButtons) : this(null, aButtons) { }

    public override bool keepWaiting => PressedButton == null;
    public Button PressedButton { get; private set; }

    public void Dispose()
    {
        RemoveListeners();
        onClickedCallback = null;
        buttons.Clear();
    }

    private void OnButtonPressed (Button button)
    {
        PressedButton = button;
        RemoveListeners();

        onClickedCallback?.Invoke(button);
    }

    private void InstallListeners()
    {
        foreach (var bc in buttons.Where(bc => bc.Button != null))
            bc.Button.onClick.AddListener(bc.Listener);
    }

    private void RemoveListeners()
    {
        foreach (var bc in buttons.Where(bc => bc.Button != null))
            bc.Button.onClick.RemoveListener(bc.Listener);
    }

    public new WaitForUIButtons Reset()
    {
        RemoveListeners();
        PressedButton = null;
        InstallListeners();
        base.Reset();
        return this;
    }

    public WaitForUIButtons ReplaceCallback (Action<Button> aCallback)
    {
        onClickedCallback = aCallback;
        return this;
    }

    private struct ButtonCallback
    {
        public Button Button;
        public UnityAction Listener;
    }
}