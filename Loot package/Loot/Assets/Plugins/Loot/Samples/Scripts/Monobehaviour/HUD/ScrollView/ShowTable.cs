using Sample;
using UnityEngine;

namespace OldSample
{
    public class ShowTable : MonoBehaviour
    {
        public GameObject tableItemPrefab;
        public GameObject refToContentFolder;

        #region Monobehaviour callbacks

        private void OnEnable()
            => GameEvents.OnUpdateScrollView += CreateItems;

        private void OnDisable()
            => GameEvents.OnUpdateScrollView -= CreateItems;

        #endregion

        #region Private Methods

        private void CreateItems (UpdateScrollViewArguments arguments)
        {
            CleanItems();

            foreach (var item in arguments.ItemsToShow)
            {
                var newItem = Instantiate(tableItemPrefab, refToContentFolder.transform, false);

                newItem.GetComponent<ShowTableItem>().Fill(item);
            }
        }

        private void CleanItems()
        {
            foreach (Transform child in refToContentFolder.transform)
                Destroy(child.gameObject);
        }

        #endregion
    }
}