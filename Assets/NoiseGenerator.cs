using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[Serializable]
public class NoiseGenerator {

	public int seed = 0;

	public float scale = 10;
	public float amplitude = 1;
	public int octaves = 1;
	public float redistribution = 1;
	public Vector2 offset = Vector2.zero;

	private bool isDirty = false;

	public float[,] getMap(int width, int height) {
		float[,] map = new float[width, height];

		System.Random rng = new System.Random(seed);

		float max = float.MinValue;
		float min = float.MaxValue;

		for(int y = 0; y < height; y++) {
			for(int x = 0; x < width; x++) {
				var value = getCoord(x, y, width, height, offset.x + rng.Next (-100000, 100000), offset.y + rng.Next (-100000, 100000));

				if(value > max) {
					max = value;
				}
				if(value < min) {
					min = value;
				}

				map[x,y] = value;
			}
		}

		for(int y = 0; y < height; y++) {
			for(int x =0; x < width; x++) {
				map[x, y] = Mathf.InverseLerp(min, max, map[x, y]);
			}
		}

		return map;
	}

	private float getCoord(int x, int y, int width, int height, float offsetX, float offsetY) {
		float value = 0;

		float freq = scale;
		float amp = amplitude;

		for(int octave = 1; octave <= octaves; octave++) {
			
			float nx = (float)x / (float)width;
			float ny = (float)y / (float)height;

			value = amp * Mathf.PerlinNoise(nx * freq, ny * freq);

			freq *= 2;
			amp /= 2;
		}

		return value;
	}
}

// [CustomPropertyDrawer(typeof(NoiseGenerator))]
// public class NoiseGeneratorDrawer: PropertyDrawer {

// 	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
// 		EditorGUI.BeginProperty(position, label, property);

		

// 		EditorGUI.EndProperty();
// 	}
// }
