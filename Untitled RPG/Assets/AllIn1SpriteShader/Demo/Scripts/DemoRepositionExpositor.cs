using UnityEngine;

namespace AllIn1SpriteShader
{
    public class DemoRepositionExpositor : MonoBehaviour
    {
        [SerializeField]
        private float paddingX = 10f;

        [ContextMenu("RepositionExpositor")]
        private void RepositionExpositor()
        {
            var i = 0;
            var tempLocalPos = Vector3.zero;
            foreach (Transform child in transform)
            {
                tempLocalPos.x = i * paddingX;
                child.localPosition = tempLocalPos;
                i++;
            }
        }
    }
}