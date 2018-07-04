using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(NoiseGenerator))]
public class NoiseGenoratorDrawer : PropertyDrawer {

	private Texture2D tex = new Texture2D(256, 256);

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.PropertyField(position, property, label, true);

		if(property.isExpanded) {
			float[,] map = (fieldInfo.GetValue(property.serializedObject.targetObject) as NoiseGenerator).Map;
			tex = new Texture2D(map.GetLength(1), map.GetLength(0));

			for(int y = 0; y < map.GetLength(1); y++) {
				for(int x = 0; x < map.GetLength(0); x++) {
					var value = map[x, y];
					var color = new Color(value, value, value);
					tex.SetPixel(x, y, color);
				}
			}

			tex.Apply();

			Rect texPos = new Rect(position.x, EditorGUI.GetPropertyHeight(property) + 200, position.width, tex.height);
			EditorGUI.DrawPreviewTexture(texPos, tex);
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		if(property.isExpanded) {
			EditorUtility.SetDirty(property.serializedObject.targetObject); 
			return EditorGUI.GetPropertyHeight(property) + tex.height + 6;
		} else {
			EditorUtility.SetDirty(property.serializedObject.targetObject); 
			return base.GetPropertyHeight(property, label);
		}
	}
}
