#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(Pathfinder))]
public class PathfinderCustomInspector : Editor
{


    private SerializedProperty width;
    private SerializedProperty height;
    private SerializedProperty cellSize;

    private SerializedProperty debug;


    private void OnEnable()
    {
        width = serializedObject.FindProperty("width");
        height = serializedObject.FindProperty("height");
        cellSize = serializedObject.FindProperty("cellSize");

        debug = serializedObject.FindProperty("debug");
    }


    public override void OnInspectorGUI()
    {

        Pathfinder pathfinder = (Pathfinder)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(width);
        EditorGUILayout.PropertyField(height);
        EditorGUILayout.PropertyField(cellSize);

        EditorGUILayout.PropertyField(debug);

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Bake"))
        {
            pathfinder.BakePathfindingGrid();
        }
    }


}

#endif