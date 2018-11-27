using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    private static readonly int RESOURCEMAXAMOUNT = 300;

    public int cashOnHand = 0;
    public int inventorySlots = 3;
    public List<Item> inventoryArray = new List<Item>();

    [Header("Resources")]

    //TODO figure out a way to label each element in Inspector for clarity
    //public int[] resources;

    public bool DEBUG = false;



	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //private void InitResourcesList()
    //{
    //    //string[] enumNames = Enum.GetNames(typeof(ResourceTypeENUM));//how many resources exist?
    //    //resources = new int[enumNames.Length];//instantiate list with same length

    //}

public void ModifyMoney(int amount)
    {
        cashOnHand += amount;
        if (DEBUG) Debug.Log("Wallet Balance: " + cashOnHand);
        if(cashOnHand < 0)
        {
            Debug.LogError("ERROR! Too much money spent when not enough had!");
        }
    }

    public void ModifyResources(Resource resource)
    {
        //TODO
        //Resource dictionary

    }

    //is this item stackable?
    //does this item already exist in inventory
    //finish stack, then add rest
    //is there enough space?
    //add

    public bool AddItem(FN_ItemManager itemManager)
    {
        Item item = itemManager.scriptableObject_Item;
        bool canAddItem = false;

        if(item is Ammo)
        {
            //does this item already exist in inventory?
            //is there room in that slot for this amount?
            if (inventoryArray.Count < inventorySlots)
            {
                //
                canAddItem = true;
            }
        }
        else if (item is Attachment)
        {
            if (inventoryArray.Count < inventorySlots)
            {
                //
                canAddItem = true;
            }
        }
        else if (item is Currency)
        {
            ModifyMoney(itemManager.ItemAmount);
            canAddItem = true;
        }
        else if (item is Resource)
        {
            ModifyResources((Resource)item);
            canAddItem = true;
        }
        else if (item is Throwable)
        {
            //does this item already exist in inventory?
            //is there room in that slot for this amount?
            if (inventoryArray.Count < inventorySlots)
            {
                //
                canAddItem = true;
            }
        }
        else if(item is Weapon)
        {
            if (inventoryArray.Count < inventorySlots)
            {
                //
                canAddItem = true;
            }
        }

        //return whether or not the item was accepted
        return canAddItem;
    }//end AddItem()

   
}
