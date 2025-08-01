﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AllIn1SpriteShader
{
    [ExecuteInEditMode]
    public class All1CreateUnifiedOutline : MonoBehaviour
    {
        [SerializeField]
        private Material outlineMaterial;

        [SerializeField]
        private Transform outlineParentTransform;

        [Space]
        [Header("Only needed if Sprite (ignored if UI)")]
        [SerializeField]
        private int duplicateOrderInLayer = -100;

        [SerializeField]
        private string duplicateSortingLayer = "Default";

        [Space]
        [Header("This operation will delete the component")]
        [SerializeField]
        private bool createUnifiedOutline;

        private void Update()
        {
            if (createUnifiedOutline)
            {
                if (outlineMaterial == null)
                {
                    createUnifiedOutline = false;
                    MissingMaterial();
                    return;
                }

                var children = new List<Transform>();
                GetAllChildren(transform, ref children);
                foreach (var t in children)
                    CreateOutlineSpriteDuplicate(t.gameObject);
                CreateOutlineSpriteDuplicate(gameObject);

                DestroyImmediate(this);
            }
        }

        private void CreateOutlineSpriteDuplicate (GameObject target)
        {
            var objectIsUi = false;
            var ownSr = target.GetComponent<SpriteRenderer>();
            var ownImage = target.GetComponent<Image>();
            if (ownSr != null)
                objectIsUi = false;
            else if (ownImage != null)
                objectIsUi = true;
            else if (ownSr == null && ownImage == null && !transform.Equals(outlineParentTransform))
                return;

            var objDuplicate = new GameObject();
            objDuplicate.name = target.name + "Outline";
            objDuplicate.transform.position = target.transform.position;
            objDuplicate.transform.rotation = target.transform.rotation;
            objDuplicate.transform.localScale = target.transform.lossyScale;
            if (outlineParentTransform == null)
                objDuplicate.transform.parent = target.transform;
            else
                objDuplicate.transform.parent = outlineParentTransform;

            if (!objectIsUi)
            {
                var sr = objDuplicate.AddComponent<SpriteRenderer>();
                sr.sprite = ownSr.sprite;
                sr.sortingOrder = duplicateOrderInLayer;
                sr.sortingLayerName = duplicateSortingLayer;
                sr.material = outlineMaterial;
                sr.flipX = ownSr.flipX;
                sr.flipY = ownSr.flipY;
            }
            else
            {
                var image = objDuplicate.AddComponent<Image>();
                image.sprite = ownImage.sprite;
                image.material = outlineMaterial;
            }
        }

        private void MissingMaterial()
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("Missing Material", "Please assign a Material For New Duplicate and try again", "Ok");
#endif
        }

        private void GetAllChildren (Transform parent, ref List<Transform> transforms)
        {
            foreach (Transform child in parent)
            {
                transforms.Add(child);
                GetAllChildren(child, ref transforms);
            }
        }
    }
}