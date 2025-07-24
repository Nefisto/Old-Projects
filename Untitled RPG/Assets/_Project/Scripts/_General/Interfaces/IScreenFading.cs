using System.Collections;
using UnityEngine;

public interface IScreenFading
{
    public IEnumerator FadeOut (Settings settings = null);
    public IEnumerator FadeIn (Settings settings = null);

    public class Settings
    {
        public Color32 color = Color.black;

        /// <summary>
        /// In seconds
        /// </summary>
        public float duration = .5f;
    }
}