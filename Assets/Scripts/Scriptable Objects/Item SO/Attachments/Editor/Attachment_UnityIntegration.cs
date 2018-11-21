using UnityEditor;

static class Attachment_UnityIntegration
{

    [MenuItem("Assets/Create/ScriptableObject/Attachment")]
    public static void CreateScriptableObject()
    {
        ScriptableObjectUtility.CreateAsset<Attachment>();
    }

}
