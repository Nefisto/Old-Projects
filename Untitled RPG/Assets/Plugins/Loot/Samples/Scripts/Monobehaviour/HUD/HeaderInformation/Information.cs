using Sample;
using TMPro;
using UnityEngine;

namespace OldSample
{
    public class Information : MonoBehaviour
    {
        public TextMeshProUGUI refToTitle;
        public TextMeshProUGUI refToDescription;

        #region Private Methods

        public void Fill (UpdateHeaderArguments headerArguments)
        {
            var hudInformation = headerArguments.Header;

            if (hudInformation.description != null)
                refToDescription.text = hudInformation.description;
        }

        #endregion

        #region Monobehaviour callbacks

        private void OnEnable()
            => GameEvents.OnUpdateHeader += Fill;

        private void OnDisable()
            => GameEvents.OnUpdateHeader -= Fill;

        #endregion
    }
}