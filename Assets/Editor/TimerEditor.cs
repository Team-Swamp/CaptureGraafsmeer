﻿using UnityEditor;

using FrameWork;

namespace Editor
{
    [CustomEditor(typeof(Timer))]
    public sealed class TimerEditor : UnityEditor.Editor
    {
        private SerializedProperty _isCountingUp;
        private SerializedProperty _startingTime;
        private SerializedProperty _timerThreshold;
        private SerializedProperty _canCount;
        private SerializedProperty _canCountOnAwake;
        private SerializedProperty _onTimerDone;
        private SerializedProperty _onTimerPassedThreshold;
        private SerializedProperty _onStart;
        private SerializedProperty _onReset;

        /// <summary>
        /// Display every serialized variable, headers and space.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
    
            EditorGUILayout.LabelField("Set up", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_isCountingUp);
            EditorGUILayout.PropertyField(_canCountOnAwake);
            
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_isCountingUp.boolValue ? _timerThreshold : _startingTime);
            EditorGUILayout.PropertyField(_canCount);
            
            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_isCountingUp.boolValue ? _onTimerPassedThreshold : _onTimerDone);
            EditorGUILayout.PropertyField(_onStart);
            EditorGUILayout.PropertyField(_onReset);
    
            serializedObject.ApplyModifiedProperties();
        }
        
        private void OnEnable()
        {
            _isCountingUp = serializedObject.FindProperty("isCountingUp");
            _startingTime = serializedObject.FindProperty("startingTime");
            _timerThreshold = serializedObject.FindProperty("timerThreshold");
            _canCount = serializedObject.FindProperty("canCount");
            _canCountOnAwake = serializedObject.FindProperty("canCountOnAwake");
            _onTimerDone = serializedObject.FindProperty("onTimerDone");
            _onTimerPassedThreshold = serializedObject.FindProperty("onTimerPassedThreshold");
            _onStart = serializedObject.FindProperty("onStart");
            _onReset = serializedObject.FindProperty("onReset");
        }
    }
}