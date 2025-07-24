using UnityEngine;

public class WaitForMouseDown : CustomYieldInstruction
{
    public override bool keepWaiting => !Input.GetMouseButtonDown(1);
}