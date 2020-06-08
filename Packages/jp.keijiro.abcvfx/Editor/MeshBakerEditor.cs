using UnityEditor;

namespace Abcvfx.Editor {

[CanEditMultipleObjects]
[CustomEditor(typeof(MeshBaker))]
class MeshBakerEditor : UnityEditor.Editor
{
    SerializedProperty _meshFilter;
    SerializedProperty _texture;
    SerializedProperty _hapPlayer;
    SerializedProperty _vertexCount;

    void OnEnable()
    {
        _meshFilter = serializedObject.FindProperty("_meshFilter");
        _texture = serializedObject.FindProperty("_texture");
        _hapPlayer = serializedObject.FindProperty("_hapPlayer");
        _vertexCount = serializedObject.FindProperty("_vertexCount");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_meshFilter);

        // Show the HAP Player field only when it's available.
        if (_hapPlayer != null)
            EditorGUILayout.PropertyField(_hapPlayer);

        // Show the texture field only when the HAP Player field
        // is not complete.
        if (_hapPlayer == null ||
            _hapPlayer.hasMultipleDifferentValues ||
            _hapPlayer.objectReferenceValue == null)
            EditorGUILayout.PropertyField(_texture);

        EditorGUILayout.PropertyField(_vertexCount);

        serializedObject.ApplyModifiedProperties();
    }
}

}
