using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FN_ItemManager : MonoBehaviour
{
	[Header("---Pick Up Item Tool Tip Parameters---")]
	public string ItemName;
	public string ItemType;
	public ItemRarityENUM ItemRarity;
	public int ItemAmount;

	public string PickUpButtonText;

	[Header("--Setup Parameters---")]
	public GameObject ToolTipWidget;
	public RawImage ItemBackground;

	private TextMeshProUGUI[] TMPTexts;
	private TextMeshProUGUI PickUpButton;
	private TextMeshProUGUI TMP_ItemType;
	private TextMeshProUGUI TMP_ItemName;
	private TextMeshProUGUI TMP_ItemRarity;
	private TextMeshProUGUI TMP_ItemAmount;

	// Use this for initialization
	void Start ()
	{
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
				PickUpButton.text = PickUpButtonText;
				break;

			case "_txtType":
				TMP_ItemType = TMPTexts [i];
				TMP_ItemType.text = ItemType;
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
				PickUpButton = TMPTexts [i];
				PickUpButton.text = ItemAmount.ToString();
				break;
			}
		}

        //do not show widget by default
		ToolTipWidget.SetActive(false);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Player"))
		{
			ToolTipWidget.SetActive (true);
			Debug.Log ("Showing item: ", this.gameObject);
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.CompareTag("Player"))

        {
			ToolTipWidget.SetActive (false);
			Debug.Log ("Hiding item: ", this.gameObject);
		}
	}
}
