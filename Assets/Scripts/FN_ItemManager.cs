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
	public enum ItemRarityEnum
	{
		Common, Uncommon, Rare, Epic, Legendary, Mythic
	}
	public ItemRarityEnum ItemRarity;
	public Color Common;
	public Color Uncommon;
	public Color Rare;
	public Color Epic;
	public Color Legendary;
	public Color Mythic;
	public string ItemAmmount;

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
		TMPTexts = gameObject.GetComponentsInChildren<TextMeshProUGUI> ();

		for (int i = 0; i < TMPTexts.Length; i++)
		{
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
				Debug.Log (TMP_ItemRarity.text);
				break;

			case "_txtAmount":
				PickUpButton = TMPTexts [i];
				PickUpButton.text = ItemAmmount;
				break;
			}
		}

		switch (TMP_ItemRarity.text)
		{
		case "Common":
			ItemBackground.color = Common;
			break;

		case "Uncommon":
			ItemBackground.color = Uncommon;
			break;

		case "Rare":
			ItemBackground.color = Rare;
			break;

		case "Epic":
			ItemBackground.color = Epic;
			break;

		case "Legendary":
			ItemBackground.color = Legendary;
			break;

		case "Mythic":
			ItemBackground.color = Mythic;
			break;
		}
		ToolTipWidget.SetActive(false);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.transform.tag == "Player")
		{
			ToolTipWidget.SetActive (true);
			Debug.Log ("show");
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.transform.tag == "Player")
		{
			ToolTipWidget.SetActive (false);
			Debug.Log ("hide");
		}
	}
}
