using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestClass))]
public class TestClassEditor  : Editor
{
    SerializedProperty intField;
    SerializedProperty stringField;

    void OnEnable()
    {
        intField = serializedObject.FindProperty("intData");
        stringField = serializedObject.FindProperty("stringData");
    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();
        EditorGUILayout.IntSlider(intField, 0, 100, new GUIContent("initData"));
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(stringField);
        if(GUILayout.Button("Select"))
        {
            stringField.stringValue = EditorUtility.OpenFilePanel("", Application.dataPath, "");
        }
        EditorGUILayout.EndHorizontal();

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }

}

[CustomEditor(typeof(TestClass2))]
public class TestClass2Editor : Editor
{
    Editor cacheEditor;
    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();
        base.OnInspectorGUI();

        GUILayout.Space(20);
        var data = ( (TestClass2)target ).data;
        if(data != null)
        {
            if (cacheEditor == null)
                cacheEditor = Editor.CreateEditor(data);
            GUILayout.Label("this is TestClass2");
            cacheEditor.OnInspectorGUI();
        }
    }
}
