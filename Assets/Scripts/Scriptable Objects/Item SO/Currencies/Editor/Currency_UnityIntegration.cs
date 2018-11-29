using UnityEditor;

static class Currency_UnityIntegration
{

    [MenuItem("Assets/Create/ScriptableObject/Currency")]
    public static void CreateScriptableObject()
    {
        ScriptableObjectUtility.CreateAsset<Currency>();
    }

}
