using UnityEngine;

public class Currency : Item {

    private void Awake()
    {
        this.stackQuantity = 99999999;
        this.itemCondition = 1.0f;
    }

}
