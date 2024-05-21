using System.Linq;
using UnityEditor;

using Framework.GeoLocation;

namespace Editor
{
    [CustomEditor(typeof(CoordinatesTransform)), CanEditMultipleObjects]
    public sealed class CoordinatesTransformEditor : UnityEditor.Editor
    {
        private const string SETTINGS_HEADER = "Settings";
        private const string PLAYER_SETTINGS_HEADER = "Player settings";
        private const string CORDS = "coordinates";
        private const string TYPE = "type";
        private const string LERP_TIME = "lerpTime";
        private const string UPDATE_TIME = "updateTime";
        
        private SerializedProperty _cords;
        private SerializedProperty _type;
        private SerializedProperty _lerp;
        private SerializedProperty _update;

        /// <summary>
        /// Display every serialized variable, headers and space.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
    
            DrawScriptField();
            
            EditorGUILayout.LabelField(SETTINGS_HEADER, EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_cords);
            EditorGUILayout.PropertyField(_type);

            if (ShouldShowPlayerSettings())
            {
                EditorGUILayout.LabelField(PLAYER_SETTINGS_HEADER, EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_lerp);
                EditorGUILayout.PropertyField(_update);
            }
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void OnEnable()
        {
            _cords = serializedObject.FindProperty(CORDS);
            _type = serializedObject.FindProperty(TYPE);
            _lerp = serializedObject.FindProperty(LERP_TIME);
            _update = serializedObject.FindProperty(UPDATE_TIME);
        }
        
        private void DrawScriptField()
        {
            MonoScript script = MonoScript.FromMonoBehaviour((CoordinatesTransform)target);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();
        }
        
        private bool ShouldShowPlayerSettings()
            => targets.Select(t => new SerializedObject(t)).Select(obj
            => obj.FindProperty(TYPE)).Any(typeProp => typeProp.enumValueIndex is 2 or 3);
    }
}