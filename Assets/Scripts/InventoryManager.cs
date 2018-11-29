using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    private static readonly int RESOURCEMAXAMOUNT = 300;

    public int cashOnHand = 0;
    public int inventorySlots = 3;
    public List<InventorySlot> inventoryArray = new List<InventorySlot>();

    [Header("Resources")]

    //TODO figure out a way to label each element in Inspector for clarity
    //public int[] resources;

    public bool DEBUG = false;



	// Use this for initialization
	void Start () {
        for(int i = 0; i < inventorySlots; ++i)
        {
            //add blank slots to each element
            inventoryArray.Add(new InventorySlot());
            
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //private void InitResourcesList()
    //{
    //    //string[] enumNames = Enum.GetNames(typeof(ResourceTypeENUM));//how many resources exist?
    //    //resources = new int[enumNames.Length];//instantiate list with same length

    //}

public bool ModifyMoney(int amount)
    {
        //TODO Implement different kinds of currency
        bool operationSuccessful = true;//prove me wrong
        cashOnHand += amount;
        if (DEBUG) Debug.Log("Wallet Balance: " + cashOnHand);
        if(cashOnHand < 0)
        {
            Debug.LogError("ERROR! Too much money spent when not enough had! Wallet contains $" + cashOnHand);
            operationSuccessful = false;//wrong!
        }

        return operationSuccessful;//returns whether or not the operation was successful
    }

    public bool ModifyResources(Resource resource, int quantity = 1)
    {
        bool operationSuccessful = true;//prove me wrong
        //TODO Resource dictionary

        return operationSuccessful;
    }

    //is this item stackable?
    //does this item already exist in inventory
    //finish stack, then add rest
    //is there enough space?
    //add
    private bool AddToNextAvailableSlot(Item item, int quantity = 1)
    {
        if (DEBUG) Debug.Log("Attempting to add to next available slot.");
        //returns the number of items added
        bool operationSuccessful = false;

        int nextEmptySlot = GetNextEmptySlot();
        if(nextEmptySlot < 0)//inventory full
        {
            operationSuccessful = false;
            if (DEBUG) Debug.Log("Item rejected from inventory: Inventory full.");
        }
        else
        {
            operationSuccessful = inventoryArray[nextEmptySlot].AddToSlot(item, quantity, item.stackQuantity);
            if (DEBUG && operationSuccessful) Debug.Log(inventoryArray[nextEmptySlot].GetItem().name + " was added to slot no: " + nextEmptySlot.ToString());
        }
        
        return operationSuccessful;
    }

    private int GetNextEmptySlot()
    {
        int nextEmptySlot = -1; // 0> implies no free slot
        for(int i = 0; i < inventoryArray.Count; ++i)//loop through each element in list
        {
            if (DEBUG) Debug.Log("Checking if slot " + i.ToString() + " is empty....");
            if (inventoryArray[i].IsEmpty())
            {
                nextEmptySlot = i;
                break;
            }
            else
            {
                if (DEBUG) Debug.Log("Slot " + i.ToString() + " already filled with: " + inventoryArray[i].GetItem().name);
            }
        }

        return nextEmptySlot;
    }

    private List<int> FindIndicesOfItem(Item itemToFind)
    {//returns every index at which the item is kept in a slot
        List<int> indicesOfItem = new List<int>();
        for(int i = 0; i < inventoryArray.Count; ++i)
        {
            if(inventoryArray[i].GetItem() == itemToFind)
            {
                indicesOfItem.Add(i);
            }
        }
        return indicesOfItem;
    }

    private int AddQuantityToSlot(int quantity, int index)
    {
        //returns leftovers, if any
        return inventoryArray[index].AddQuantity(quantity);
        
    }

    private int AddStackableInventory(Item itemToAdd, int quantity)
    {   
        //does this item already exist in inventory? try to stack
        List<int> indicesOfExistingItem = FindIndicesOfItem(itemToAdd);
        if(indicesOfExistingItem.Count > 0)//if this item does exist
        {
            for(int i = 0; i < indicesOfExistingItem.Count;  ++i)
            {
                //subtract the amount that was given back by AddQuantity()
                quantity = AddQuantityToSlot(quantity, i);
                if (quantity <= 0) return 0;//if quantity > 0, the existing stack was not big enough to accept full quantity. look for more

            }
        }

        //if there's room, add the rest to a new slot
        if (inventoryArray.Count < inventorySlots) AddToNextAvailableSlot(itemToAdd, quantity);
               
        return quantity;
    }//end AddStackableInventory()

    public bool AddItem(FN_ItemManager itemManager)
    {
        Item item = itemManager.scriptableObject_Item;
        bool itemAddSuccess = false;
        

        //if(item is Ammo)
        //{
        //}
        //else if (item is Attachment)
        //{
        //}
        if (item is Currency)
        {
            //TODO currency dictionary
            itemAddSuccess = ModifyMoney(itemManager.ItemAmount);
        }
        else if (item is Resource)
        {
            //TODO Resource dictionary
            itemAddSuccess = ModifyResources(item as Resource);
            
        }
        //else if (item is Throwable)
        //{

        //}
        //else if(item is Weapon)
        //{
        //    //done
        //    if (inventoryArray.Count < inventorySlots)
        //    {
        //        itemAddSuccess = AddToNextAvailableSlot(item);
        //    }
        //}
        else
        {

            if (item.stackQuantity <= 1)
            {
                if (DEBUG) Debug.Log("Item is not stackable....");
                itemAddSuccess = AddToNextAvailableSlot(item);
            }
            else
            {
                int quantityRemaining = AddStackableInventory(item, itemManager.ItemAmount);
                itemManager.ItemAmount = quantityRemaining;
                itemAddSuccess = false;//redundant
                
            }
        }
        //if (DEBUG && itemAddSuccess) Debug.Log("Item successfully added to inventory!");
        //if (DEBUG && !itemAddSuccess) Debug.Log("Item was not added inventory.");

        return itemAddSuccess;
    }//end AddItem()

   
}
