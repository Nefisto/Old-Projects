using System.Collections;
using UnityEngine;

namespace AllIn1SpriteShader
{
    public class DemoCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform targetedItem;

        [SerializeField]
        private All1ShaderDemoController demoController;

        [SerializeField]
        private float speed;

        private bool canUpdate;

        private Vector3 offset;
        private Vector3 target;

        private void Awake()
        {
            offset = transform.position - targetedItem.position;
            StartCoroutine(SetCamAfterStart());
        }

        private void Update()
        {
            if (!canUpdate)
                return;
            target.y = demoController.GetCurrExpositor() * demoController.expositorDistance;
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        }

        private IEnumerator SetCamAfterStart()
        {
            yield return null;
            transform.position = targetedItem.position + offset;
            target = transform.position;
            canUpdate = true;
        }
    }
}