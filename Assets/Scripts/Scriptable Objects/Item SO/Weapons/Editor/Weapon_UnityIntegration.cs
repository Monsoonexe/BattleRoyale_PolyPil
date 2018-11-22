using UnityEditor;

static class Weapon_UnityIntegration
{

    [MenuItem("Assets/Create/ScriptableObject/Weapon")]
    public static void CreateScriptableObject()
    {
        ScriptableObjectUtility.CreateAsset<Weapon>();
    }

}
