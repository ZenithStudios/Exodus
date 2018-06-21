using UnityEngine;
using UnityEditor;

public class NoiseVisualizer : EditorWindow {

	private Texture2D texture;
	
	private float frequency = 20f;
	private int seed = 0;

	private int octaves = 1;

	private bool useRadialMask  = false;
	private float radius = 1f;

	private int width = 256;
	private int height = 256;
	private bool isDirty = false;

	private int Width {
		get {
			return width;
		}
		set{
			isDirty = true;
			width = value;
		}
	}

	private int Height {
		get {
			return height;
		}
		set{
			isDirty = true;
			height = value;
		}
	}

	[MenuItem("Window/Noise Visualizer")]
	public static void showWindow() {
		GetWindow<NoiseVisualizer>("Noise Visualizer");
	}

	public Color calcPixel(int x, int y) {
		float xCoord = (float)x / width * frequency + seed;
		float yCoord = (float)y / height * frequency + seed;

		float value = (Mathf.PerlinNoise(xCoord, yCoord));
		return new Color(value, value, value);
	}

	private void updateTexture() {
		for(int x = 0; x < width; x++) {
			for(int y = 0; y < height; y++) {
				texture.SetPixel(x, y, calcPixel(x, y));
			}
		}

		texture.Apply();
	}

	private void recreateTexture() {
		texture = new Texture2D(width, height);
		updateTexture();
		
		isDirty = false;
	}

	void Update() {
		if(isDirty) {
			recreateTexture();
		} else {
			updateTexture();
		}
	}

	void Awake() {
		recreateTexture();
	}

	void OnGUI() {
		
		EditorGUI.DrawPreviewTexture(new Rect(3, position.yMax - height, width, height), texture, null, ScaleMode.ScaleToFit);

		EditorGUILayout.BeginHorizontal();
		width = EditorGUILayout.IntField("Width: ", width);
		height = EditorGUILayout.IntField("Height: ", height);
		EditorGUILayout.EndHorizontal();
		frequency = EditorGUILayout.FloatField("Frequency: ", frequency);
		EditorGUILayout.Space();

		seed = EditorGUILayout.IntField("Seed: ", seed);
		if(GUILayout.Button("Randomize")) {
			seed = (int)(Random.Range(-1f, 1f) * int.MaxValue);
		}
		EditorGUILayout.Space();

		useRadialMask = EditorGUILayout.Toggle("Use Radial Mask: ", useRadialMask);
		if(useRadialMask) {

		}
	}
	
}
