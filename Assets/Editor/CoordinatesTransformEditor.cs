using UnityEditor;

using Framework.GeoLocation;

namespace Editor
{
    [CustomEditor(typeof(CoordinatesTransform))]
    public sealed class CoordinatesTransformEditor : UnityEditor.Editor
    {
        private SerializedProperty _cords;
        private SerializedProperty _scale;
        private SerializedProperty _isStatic;
        private SerializedProperty _isDebugTesting;
        private SerializedProperty _isPlayer;
        private SerializedProperty _lerp;
        private SerializedProperty _update;
        private SerializedProperty _others;
        private SerializedProperty _text;

        /// <summary>
        /// Display every serialized variable, headers and space.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
    
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_cords);
            EditorGUILayout.PropertyField(_scale);
            EditorGUILayout.PropertyField(_isStatic);
            
            if (_isStatic.boolValue)
                EditorGUILayout.PropertyField(_isDebugTesting);
            
            EditorGUILayout.PropertyField(_isPlayer);

            if (_isPlayer.boolValue)
            {
                EditorGUILayout.LabelField("Player settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_lerp);
                EditorGUILayout.PropertyField(_update);
                EditorGUILayout.PropertyField(_others);
            }

            EditorGUILayout.PropertyField(_text);
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void OnEnable()
        {
            _cords = serializedObject.FindProperty("coordinates");
            _scale = serializedObject.FindProperty("scaleFactor");
            _isStatic = serializedObject.FindProperty("isStatic");
            _isPlayer = serializedObject.FindProperty("isPlayer");
            _isDebugTesting = serializedObject.FindProperty("isDebugTesting");
            _lerp = serializedObject.FindProperty("lerpTime");
            _update = serializedObject.FindProperty("updateTime");
            _others = serializedObject.FindProperty("others");
            _text = serializedObject.FindProperty("locationText");
        }
    }
}