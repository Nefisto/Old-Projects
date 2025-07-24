using Sample;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OldSample
{
    [RequireComponent(typeof(EventTrigger))]
    public class Hover : MonoBehaviour
    {
        [Multiline]
        public string description;

        public void OnHover()
        {
            if (string.IsNullOrEmpty(description))
                return;

            if (!SampleSettings.CanHover)
                return;

            var headerArguments = new UpdateHeaderArguments
            {
                Header = new HUDHeader(description)
            };
            GameEvents.OnUpdateHeader(headerArguments);
        }
    }
}