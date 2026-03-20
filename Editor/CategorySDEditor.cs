using UnityEditor;
using UnityEngine;

namespace BilliotGames
{
    [CustomEditor(typeof(CategorySD))]
    public class CategorySDEditor : Editor
    {
        public override void OnInspectorGUI() {
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, "categoryList", "m_Script");
            var prop = serializedObject.FindProperty("categoryList");
            EditorGUILayout.PropertyField(prop, new GUIContent("Parent Categories"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
