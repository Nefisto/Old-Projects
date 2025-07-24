using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TS.PageSlider
{
    public class PageDot : MonoBehaviour
    {
        [TitleGroup("References")]
        [SerializeField]
        private Image image;


        [field: TitleGroup("Debug")]
        [field: ReadOnly]
        [field: ShowInInspector]
        public bool IsActive { get; private set; }
        
        [field: TitleGroup("Debug")]
        [field: ShowInInspector]
        public int Index { get; set; }

        public virtual void ChangeActiveState(bool active)
        {
            IsActive = active;
            
            RefreshState();
        }

        private void RefreshState() => image.color = IsActive ? Color.white : Color.grey;
    }
}