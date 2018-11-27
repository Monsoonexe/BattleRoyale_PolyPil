using UnityEngine;

public class Throwable : Item {
    [Header("Throwable")]
    public ThrowableTypeENUM throwType;
    public ThrowableImpactTypeENUM impactType;
    public float throwDistance = 3.0f;
    public float cookTime = 3.0f;

    private void Awake()
    {
        this.stackQuantity = 5;
    }
}
