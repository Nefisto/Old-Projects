﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
#endif

namespace Michsky.MUIP
{
    [AddComponentMenu("Modern UI Pack/Context Menu/Context Menu Content")]
    public class ContextMenuContent : MonoBehaviour, IPointerClickHandler
    {
        public enum ContextItemType
        {
            Button,
            Separator
        }

        // Resources
        public ContextMenuManager contextManager;
        public Transform itemParent;

        // Settings
        public bool useIn3D = false;

        // Items
        public List<ContextItem> contexItems = new List<ContextItem>();
        Sprite imageHelper;

        GameObject selectedItem;
        Image setItemImage;
        TextMeshProUGUI setItemText;
        string textHelper;

        void Awake()
        {
            if (contextManager == null)
            {
                try
                {
#if UNITY_2023_2_OR_NEWER
                    contextManager = FindObjectsByType<ContextMenuManager>(FindObjectsSortMode.None)[0];
#else
                    contextManager = (ContextMenuManager)FindObjectsOfType(typeof(ContextMenuManager))[0];
#endif
                    itemParent = contextManager.transform.Find("Content/Item List").transform;
                }

                catch
                {
                    Debug.LogError("<b>[Context Menu]</b> Context Manager is missing.", this);
                    return;
                }
            }

            foreach (Transform child in itemParent)
                Destroy(child.gameObject);
        }

#if !UNITY_IOS && !UNITY_ANDROID
        public void OnMouseOver()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (useIn3D == true && Input.GetMouseButtonDown(1))
#elif ENABLE_INPUT_SYSTEM
            if (useIn3D == true && Mouse.current.rightButton.wasPressedThisFrame)
#endif
            {
                ProcessContent();
            }
        }
#endif

        public void OnPointerClick (PointerEventData eventData)
        {
            if (contextManager.isOn == true)
            {
                contextManager.Close();
            }
            else if (eventData.button == PointerEventData.InputButton.Right && contextManager.isOn == false)
            {
                ProcessContent();
            }
        }

        public void ProcessContent()
        {
            foreach (Transform child in itemParent)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < contexItems.Count; ++i)
            {
                bool nulLVariable = false;

                if (contexItems[i].contextItemType == ContextItemType.Button && contextManager.contextButton != null)
                    selectedItem = contextManager.contextButton;
                else if (contexItems[i].contextItemType == ContextItemType.Separator
                         && contextManager.contextSeparator != null)
                    selectedItem = contextManager.contextSeparator;
                else
                {
                    Debug.LogError(
                        "<b>[Context Menu]</b> At least one of the item presets is missing. "
                        + "You can assign a new variable in Resources (Context Menu) tab. All default presets can be found in "
                        + "<b>Modern UI Pack > Prefabs > Context Menu</b> folder.", this);
                    nulLVariable = true;
                }

                if (nulLVariable == false)
                {
                    if (contexItems[i].subMenuItems.Count == 0)
                    {
                        GameObject go =
                            Instantiate(selectedItem, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        go.transform.SetParent(itemParent, false);

                        if (contexItems[i].contextItemType == ContextItemType.Button)
                        {
                            setItemText = go.GetComponentInChildren<TextMeshProUGUI>();
                            textHelper = contexItems[i].itemText;
                            setItemText.text = textHelper;

                            Transform goImage = go.gameObject.transform.Find("Icon");
                            setItemImage = goImage.GetComponent<Image>();
                            imageHelper = contexItems[i].itemIcon;
                            setItemImage.sprite = imageHelper;

                            if (imageHelper == null)
                                setItemImage.color = new Color(0, 0, 0, 0);

                            Button itemButton = go.GetComponent<Button>();
                            itemButton.onClick.AddListener(contexItems[i].onClick.Invoke);
                            itemButton.onClick.AddListener(contextManager.Close);
                        }
                    }

                    else if (contextManager.contextSubMenu != null && contexItems[i].subMenuItems.Count != 0)
                    {
                        GameObject go = Instantiate(contextManager.contextSubMenu, new Vector3(0, 0, 0),
                            Quaternion.identity) as GameObject;
                        go.transform.SetParent(itemParent, false);

                        ContextMenuSubMenu subMenu = go.GetComponent<ContextMenuSubMenu>();
                        subMenu.cmManager = contextManager;
                        subMenu.cmContent = this;
                        subMenu.subMenuIndex = i;

                        setItemText = go.GetComponentInChildren<TextMeshProUGUI>();
                        textHelper = contexItems[i].itemText;
                        setItemText.text = textHelper;

                        Transform goImage;
                        goImage = go.gameObject.transform.Find("Icon");
                        setItemImage = goImage.GetComponent<Image>();
                        imageHelper = contexItems[i].itemIcon;
                        setItemImage.sprite = imageHelper;
                    }

                    StopCoroutine("ExecuteAfterTime");
                    StartCoroutine("ExecuteAfterTime", 0.01f);
                }
            }

            contextManager.SetContextMenuPosition();
            contextManager.Open();
        }

        IEnumerator ExecuteAfterTime (float time)
        {
            yield return new WaitForSecondsRealtime(time);
            itemParent.gameObject.SetActive(false);
            itemParent.gameObject.SetActive(true);
        }

        public void AddNewItem()
        {
            ContextItem item = new ContextItem();
            contexItems.Add(item);
        }

        [Serializable]
        public class ContextItem
        {
            [Header("Information")]
            [Space(-5)]
            public string itemText = "Item Text";

            public Sprite itemIcon;
            public ContextItemType contextItemType;

            [Header("Sub Menu")]
            public List<SubMenuItem> subMenuItems = new List<SubMenuItem>();

            [Header("Events")]
            public UnityEvent onClick;
        }

        [Serializable]
        public class SubMenuItem
        {
            public string itemText = "Item Text";
            public Sprite itemIcon;
            public ContextItemType contextItemType;
            public UnityEvent onClick;
        }
    }
}