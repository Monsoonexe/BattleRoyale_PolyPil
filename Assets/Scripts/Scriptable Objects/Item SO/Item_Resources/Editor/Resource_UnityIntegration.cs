using UnityEditor;

static class Resource_UnityIntegration
{

    [MenuItem("Assets/Create/ScriptableObject/Resource")]
    public static void CreateScriptableObject()
    {
        ScriptableObjectUtility.CreateAsset<Resource>();
    }

}
