using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator {

	public int seed = 0;

	public int width = 256;
	public int height = 256;

	private float frequency = 10;
	private int octaves = 1;
	private float redistribution = 1;

	public float Frequency {
		get {
			return frequency;
		}
		set {
			frequency = Mathf.Clamp(value, 1f, float.MaxValue);
		}
	}

	public int Octaves {
		get {
			return octaves;
		}
		set {
			octaves = Mathf.Clamp(value, 1, 5);
		}
	}

	public float Redistribution {
		get {
			return redistribution;
		}
		set {
			redistribution = Mathf.Clamp(value, 0.01f, 10f);
		}
	}

	public float[,] getMap(int width, int height) {
		float[,] map = new float[width, height];

		for(int y = 0; y < height; y++) {
			for(int x = 0; x < width; x++) {
				map[x,y] = getCoord(x, y);
			}
		}

		return map;
	}

	public float getCoord(int x, int y) {
		float value = 0;
		float freq = frequency;

		// for(int octave = 1; octave <= octaves; octave++) {
		// 	float nx = (float)x / (float)width;
		// 	float ny = (float)y / (float)height;

		// 	float co = 1f/(float)octaves;
		// 	value += co * Mathf.PerlinNoise(freq * nx, freq * ny);
		// 	freq *= 2;
		// }

		float nx = (float)x / (float)width;
		float ny = (float)y / (float)height;

		value = Mathf.PerlinNoise(frequency * nx, frequency * ny)
				+ 0.5f * Mathf.PerlinNoise((frequency + 2) * nx, (frequency + 2) * ny)
				+ 0.25f * Mathf.PerlinNoise((frequency + 4) * nx, (frequency + 4) * ny);

		value -= 0.25f;
		return Mathf.Pow(value, redistribution);
	}
}
