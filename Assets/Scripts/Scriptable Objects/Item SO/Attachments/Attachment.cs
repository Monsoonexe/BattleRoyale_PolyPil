using UnityEngine;

public class Attachment : Item
{
    [Header("Attachment")]
    public WeaponTypeENUM fitsWeaponOfType;
    public AttachmentTypeENUM attachmentType;
    public AttachmentLocationENUM attachesTo;

    private void Awake()
    {
        this.stackQuantity = 3;

    }


}
