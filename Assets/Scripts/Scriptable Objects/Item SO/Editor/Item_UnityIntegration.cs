using UnityEngine;
using UnityEditor;

static class Item_UnityIntegration
{

    [MenuItem("Assets/Create/ScriptableObject/Item")]
    public static void CreateScriptableObject()
    {
        ScriptableObjectUtility.CreateAsset<Item>();
    }

}
