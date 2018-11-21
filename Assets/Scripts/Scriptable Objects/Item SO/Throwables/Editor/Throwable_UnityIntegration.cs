using UnityEditor;

static class Throwable_UnityIntegration
{

    [MenuItem("Assets/Create/ScriptableObject/Throwable")]
    public static void CreateScriptableObject()
    {
        ScriptableObjectUtility.CreateAsset<Throwable>();
    }

}
