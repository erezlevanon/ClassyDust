using UnityEngine;
using UnityEditor;

namespace Infra.Utils {
[CustomPropertyDrawer(typeof(RangedFloat), true)]
public class RangedFloatDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        SerializedProperty minProp = property.FindPropertyRelative("minValue");
        SerializedProperty maxProp = property.FindPropertyRelative("maxValue");

        var minValue = minProp.floatValue;
        var maxValue = maxProp.floatValue;

        var style = EditorStyles.numberField;
        if (minValue > maxValue) {
            style = new GUIStyle(style);
            style.normal.textColor = Color.red;
        }

        var rangeMin = 0f;
        var rangeMax = 1f;

        var ranges = (MinMaxRangeAttribute[])fieldInfo.GetCustomAttributes(typeof(MinMaxRangeAttribute), true);
        if (ranges.Length > 0) {
            rangeMin = ranges[0].Min;
            rangeMax = ranges[0].Max;
        }

        const float rangeBoundsLabelWidth = 40f;

        EditorGUI.BeginChangeCheck();
        var rangeBoundsLabel1Rect = new Rect(position);
        rangeBoundsLabel1Rect.width = rangeBoundsLabelWidth - 5;
        minValue = EditorGUI.FloatField(rangeBoundsLabel1Rect, minValue, style);
        position.xMin += rangeBoundsLabelWidth;

        var rangeBoundsLabel2Rect = new Rect(position);
        rangeBoundsLabel2Rect.xMin = rangeBoundsLabel2Rect.xMax - rangeBoundsLabelWidth + 5;
        maxValue = EditorGUI.FloatField(rangeBoundsLabel2Rect, maxValue, style);
        position.xMax -= rangeBoundsLabelWidth;

        EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, rangeMin, rangeMax);
        if (EditorGUI.EndChangeCheck()) {
            minProp.floatValue = minValue;
            maxProp.floatValue = maxValue;
        }

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
}