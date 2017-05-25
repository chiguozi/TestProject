using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(string))]
public class StringPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect btnRect = new Rect(position);
        position.width -= 60;
        btnRect.x += btnRect.width - 60;
        btnRect.width = 60;
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PropertyField(position, property, true);
        if (GUI.Button(btnRect, "select"))
        {
            string path = property.stringValue;
            string selectStr = EditorUtility.OpenFilePanel("选择文件", path, "");
            if (!string.IsNullOrEmpty(selectStr))
            {
                property.stringValue = selectStr;
            }
        }

        EditorGUI.EndProperty();
    }
}


public class RangeAttribute : PropertyAttribute
{
    public float min;
    public float max;

    public RangeAttribute(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}

[CustomPropertyDrawer(typeof(RangeAttribute))]
public class RangeDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // First get the attribute since it contains the range for the slider
        RangeAttribute range = attribute as RangeAttribute;

        // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
        if (property.propertyType == SerializedPropertyType.Float)
            EditorGUI.Slider(position, property, range.min, range.max, label);
        else if (property.propertyType == SerializedPropertyType.Integer)
            EditorGUI.IntSlider(position, property, (int)range.min, (int)range.max, label);
        else
            EditorGUI.LabelField(position, label.text, "Use Range with float or int.");
    }
}

