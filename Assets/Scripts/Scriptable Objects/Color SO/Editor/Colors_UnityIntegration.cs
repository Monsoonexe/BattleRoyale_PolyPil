using UnityEngine;
using UnityEditor;

static class Colors_UnityIntegration
{

    [MenuItem("Assets/Create/Colors_Rarity")]
    public static void CreateScriptableObject()
    {
        ScriptableObjectUtility.CreateAsset<Colors_Rarity>();
    }

}
