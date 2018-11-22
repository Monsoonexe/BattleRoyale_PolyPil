﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FN_ItemManager : MonoBehaviour
{
    public Item scriptableObject_Item;
	[Header("---Pick Up Item Tool Tip Parameters---")]
	public string ItemName;
	public ItemTypeENUM ItemType;
	public ItemRarityENUM ItemRarity;
	public int ItemAmount;

	public char PickUpButtonText;

	[Header("--Setup Parameters---")]
	public GameObject ToolTipWidget;
    public GameObject itemModelHolder;
    public RawImage ItemBackground;

    private GameObject itemModel;

	private TextMeshProUGUI[] TMPTexts;
	private TextMeshProUGUI PickUpButton;
	private TextMeshProUGUI TMP_ItemType;
	private TextMeshProUGUI TMP_ItemName;
	private TextMeshProUGUI TMP_ItemRarity;
	private TextMeshProUGUI TMP_ItemAmount;

    private void InitFromScriptableObject()
    {
        //read stats from SO
        ItemName = scriptableObject_Item.itemName;
        ItemType = scriptableObject_Item.itemType;
        ItemRarity = scriptableObject_Item.itemRarity;
        ItemAmount = scriptableObject_Item.quantity;
        PickUpButtonText = scriptableObject_Item.buttonToPickUp;

        //instantiate item model. rotation matches that of parent
        this.itemModel = Instantiate(scriptableObject_Item.itemModel, itemModelHolder.transform.position, itemModelHolder.transform.rotation, itemModelHolder.transform);
    }

	// Use this for initialization
	void Start ()
	{
        //init from scriptable object if provided
        if (scriptableObject_Item != null) InitFromScriptableObject();

        //get all TextMeshPro GUI references in children
        TMPTexts = gameObject.GetComponentsInChildren<TextMeshProUGUI> ();

        //for each element
		for (int i = 0; i < TMPTexts.Length; i++)
		{
            //what is the element controlling
			switch (TMPTexts [i].name)
			{
			case "_PickUpBtnText":
				PickUpButton = TMPTexts [i];
				PickUpButton.text = PickUpButtonText.ToString();
				break;

			case "_txtType":
				TMP_ItemType = TMPTexts [i];
				TMP_ItemType.text = ItemType.ToString();
				break;

			case "_txtItemName":
				TMP_ItemName = TMPTexts [i];
				TMP_ItemName.text = ItemName;
				break;

			case "_txtRarity":
				TMP_ItemRarity = TMPTexts [i];
				TMP_ItemRarity.text = ItemRarity.ToString ();
                ItemBackground.color = Color_Rarity.GetRarityColor(ItemRarity);
                //Debug.Log (TMP_ItemRarity.text);
                break;

			case "_txtAmount":
                TMP_ItemAmount = TMPTexts [i];
                TMP_ItemAmount.text = ItemAmount.ToString();
				break;
			}
		}

        //do not show widget by default
		ToolTipWidget.SetActive(false);
	}

    public GameObject GetItemModel()
    {
        return this.itemModel;
    }

    public void ToggleModelVisible(bool _isActive)
    {
        ToolTipWidget.SetActive(_isActive);
        //Debug.Log("Show item " + ItemName + ": " + _isActive);

    }
}
