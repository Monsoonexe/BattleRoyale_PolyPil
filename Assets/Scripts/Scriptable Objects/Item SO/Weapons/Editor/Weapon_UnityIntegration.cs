using UnityEngine;
using UnityEditor;

static class Weapon_UnityIntegration
{

    [MenuItem("Assets/Create/Weapon")]
    public static void CreateScriptableObject()
    {
        ScriptableObjectUtility.CreateAsset<Weapon>();
    }

}
