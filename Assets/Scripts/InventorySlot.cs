using UnityEngine;
[System.Serializable]
public class InventorySlot{
    private Item _item;
    private int _quantity;
    private int _stackQuantity;

    public InventorySlot(Item item, int quantity = 1, int stackQuantity = 1)
    {
        if(quantity >= 0)
        {
            _quantity = quantity;
        }
        else
        {
            //complain
            Debug.LogError("ERROR! Cannot create an inventory slot with less than nothing in it!");
        }

        _stackQuantity = stackQuantity;//value 1>= means it does not stack.
        _item = item;
        

    }

    public Item GetItem()
    {
        return _item;
    }

    public int GetQuantity()
    {
        return _quantity;
    }

    public int GetStackQuantity()
    {
        return _stackQuantity;
    }

    public int AddQuantity(int amount)
    {
        //any remainder left over after adding?
        int amountOver = 0;
        //input validation
        if (amount <= 0)
        {
            Debug.LogError("ERROR! Do not Add 0 or less items to Inventory Slot! Or do! I don't care! But I won't let you.");
        }//input validation

        else
        {//input verified
            //quick exit if limit already met
            if(_quantity == _stackQuantity)
            {
                return amount;
            }
            //add the amount
            _quantity += amount;
            //does amount exceed limit
            if(_quantity > _stackQuantity)
            {
                //note the amount to leave behind
                amountOver = _stackQuantity - _quantity;
                //set to limit
                _quantity = _stackQuantity;
            }
        }
        return amountOver;//0 or more
    }

    public void RemoveQuantity(int amount)
    {
        //validate input
        if(amount <= 0)
        {
            Debug.LogError("ERROR! Do not try to remove a negative amount! Or do, I don't care! But I won't let you!");
        }
        else
        {
            _quantity -= amount;//it's up to the caller to make sure it is asking for enough. always count your change

        }

    }
    
}
