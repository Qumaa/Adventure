using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;

[CustomEditor(typeof(UIConsumer))]
public class UITest : Editor
{
    const int CELL_SIZE = 10;
    const int WINDOW_HEIGHT = 150;
    const string SIZE_PROPERTY_NAME = "_size";


    public override void OnInspectorGUI()
    {
        DrawSizeProperty(SIZE_PROPERTY_NAME);

        DrawGridRectangle(WINDOW_HEIGHT);
        
    }

    private void DrawSizeProperty(string propName)
    {
        SerializedProperty sizeProperty = base.serializedObject.FindProperty(propName);

        EditorGUILayout.PropertyField(sizeProperty);
    }
    private void DrawGridRectangle(float height)
    {
        Rect window = EditorGUILayout.GetControlRect(true, height);
    }
}
