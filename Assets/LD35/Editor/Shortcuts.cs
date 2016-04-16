using UnityEngine;
using UnityEditor;
using LD35;

public class Shortcuts {

    [MenuItem("Select/Shepherd _F7")]
    public static void SelectShephers() {
        Selection.objects = Object.FindObjectsOfType<Shepherd>();
        if (Selection.activeObject) EditorGUIUtility.PingObject(Selection.activeObject);
    }

    [MenuItem("Select/Dog _F8")]
    public static void SelectDogs() {
        Selection.objects = Object.FindObjectsOfType<Dog>();
        if (Selection.activeObject) EditorGUIUtility.PingObject(Selection.activeObject);

    }

    [MenuItem("Select/Sheep _F9")]
    public static void SelectSheep() {
        Selection.objects = Object.FindObjectsOfType<Sheep>();
        if (Selection.activeObject) EditorGUIUtility.PingObject(Selection.activeObject);

    }

    [MenuItem("Select/Scares _F10")]
    public static void SelectScares() {
        Selection.objects = Object.FindObjectsOfType<Scare>();
        if (Selection.activeObject) EditorGUIUtility.PingObject(Selection.activeObject);

    }
}
