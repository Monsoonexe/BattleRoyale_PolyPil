using UnityEngine;

public class Throwable : Item {
    [Header("Throwable")]
    public ThrowableTypeENUM throwType;
    public ThrowableImpactTypeENUM impactType;
    public float throwDistance;
    public float cookTime;
}
