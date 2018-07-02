using UnityEngine;
using UnityEditor;

public class NoiseVisualizer : EditorWindow {

	private const int width = 256, height = 256;
	private int seed = 0;
	private float scale = 1;
	private int octaves = 1;
	private float persistance = 0.5f, lacunarity = 2;
	private float redistribution = 1;
	private Vector2 offset = Vector2.zero;

	public NoiseGenerator noisemap = new NoiseGenerator();

	private Texture2D texture;

	void Awake() {
		texture = new Texture2D(width, height);
	}

	[MenuItem("Window/Noise Visualizer")]
	public static void showWindow() {
		GetWindow<NoiseVisualizer>("Noise Visualizer");
	}

	private void updateTexture() {
		for(int y = 0; y < width; y++) {
			for(int x = 0; x < height; x++) {
				//float value = noisemap.getCoord(x, y);
				Color color = new Color(1, 1, 1);
				texture.SetPixel(x, y, color);
			}
		}

		texture.Apply ();
	}

	void Update() {
		updateTexture();
	}

	void OnGUI() {
		
		// EditorGUI.DrawPreviewTexture(new Rect(3, position.yMax - height, width, height), texture, null, ScaleMode.ScaleToFit);

		// noisemap.Frequency = EditorGUILayout.FloatField("Scale: ", noisemap.Frequency);
		// EditorGUILayout.Space();

		// noisemap.Octaves = EditorGUILayout.IntField("Octaves: ", noisemap.Octaves);
		// noisemap.Redistribution = EditorGUILayout.Slider(noisemap.Redistribution, 0.01f, 10);
		// EditorGUILayout.Space();

		// noisemap.seed = EditorGUILayout.IntField("Seed: ", noisemap.seed);
		// if(GUILayout.Button("Randomize")) {
		// 	noisemap.seed = (int)(Random.Range(-1f, 1f) * int.MaxValue);
		// }
		// EditorGUILayout.Space();
	}
	
}
