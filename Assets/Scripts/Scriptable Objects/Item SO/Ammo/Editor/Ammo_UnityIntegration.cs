using UnityEditor;

static class Ammo_UnityIntegration
{

    [MenuItem("Assets/Create/ScriptableObject/Ammo")]
    public static void CreateScriptableObject()
    {
        ScriptableObjectUtility.CreateAsset<Ammo>();
    }

}
