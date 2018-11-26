using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    private static readonly int RESOURCEMAXAMOUNT = 300;

    public int cashOnHand = 0;
    public int inventorySlots = 3;

    [Header("Resources")]

    //TODO figure out a way to label each element in Inspector for clarity
    public int[] resources;

    public bool DEBUG = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InitResourcesList()
    {
        string[] enumNames = Enum.GetNames(typeof(ResourceTypeENUM));//how many resources exist?
        resources = new int[enumNames.Length];//instantiate list with same length

    }

public void ModifyMoney(int amount)
    {
        cashOnHand += amount;
        if (DEBUG) Debug.Log("Wallet Balance: " + cashOnHand);
        if(cashOnHand < 0)
        {
            Debug.LogError("ERROR! Too much money spent when not enough had!");
        }
    }

    public void ModifyResources(ResourceTypeENUM _resourceType, int _amount)
    {
        int index = (int)_resourceType;
        //clamp maximum resource amount
        resources[index] = (resources[index] + _amount > RESOURCEMAXAMOUNT) ? RESOURCEMAXAMOUNT : (resources[index] + _amount);

        if(resources[index] < 0)
        {
            Debug.LogError("ERROR! Resources have gone below 0!!! " + ((ResourceTypeENUM)index).ToString());
        }

    }

    public void AddItem(Item item)
    {

        //best implementation so far
        Weapon weaponItem;
        Throwable throwItem;
        Resource resourceItem;

        switch (item.itemType)
        {
            case ItemTypeENUM.weapon:
                weaponItem = (Weapon)item;
                Debug.Log("WeaponItem.ReloadSpeed: " + weaponItem.reloadSpeed);
                break;
            case ItemTypeENUM.throwable:
                throwItem = (Throwable)item;
                Debug.Log("WeaponItem.ReloadSpeed: " + throwItem.cookTime);
                break;
            case ItemTypeENUM.resource:
                resourceItem = (Resource)item;
                Debug.Log("WeaponItem.ReloadSpeed: " + resourceItem.itemName);
                break;

        }
    }

   
}
