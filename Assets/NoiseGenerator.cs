using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[Serializable]
public class NoiseGenerator {

	private float[,] map = new float[1, 1];
	private bool isDirty = true;

	[SerializeField, Candlelight.PropertyBackingField]
    private int m_Seed = 0;
	public int Seed{
        get {
            return m_Seed;
        }

        set {
			isDirty = true;
            m_Seed = value;
        }
    }

	[Header("Size")]
	[SerializeField, Candlelight.PropertyBackingField]
    private int m_Height = 256;
	public int Height{
        get {
            return m_Height;
        }
        set {
			isDirty = true;
            m_Height = value;
        }
    }

	[SerializeField, Candlelight.PropertyBackingField]
    private int m_Width = 256;
	public int Width{
        get {
            return m_Width;
        }
        set {
			isDirty = true;
            m_Width = value;
        }
    }

	[Header("Properties")]
	[SerializeField, Candlelight.PropertyBackingField]
    private float m_Scale = 10;
	public float Scale{
        get {
            return m_Scale;
        }
        set {
			isDirty = true;
            m_Scale = value;
        }
    }

	[SerializeField, Candlelight.PropertyBackingField]
    private int m_Octaves = 1;
	public int Octaves{
        get {
            return m_Octaves;
        }

        set {
			isDirty = true;
            m_Octaves = value;
        }
    }

	[SerializeField, Candlelight.PropertyBackingField]
    private float m_Lacunarity = 2;
	public float Lacunarity{
        get {
            return m_Lacunarity;
        }

        set {
			isDirty = true;
            m_Lacunarity = value;
        }
    }

	[SerializeField, Candlelight.PropertyBackingField]
    private float m_Redistribution = 1;
	public float Redistribution{
        get {
            return m_Redistribution;
        }

        set {
			isDirty = true;
            m_Redistribution = value;
        }
    }

	[SerializeField, Candlelight.PropertyBackingField]
    private float m_Persistance = 0.5f;
	public float Persistance{
        get {
            return m_Persistance;
        }

        set {
			isDirty = true;
            m_Persistance = value;
        }
    }

	[SerializeField, Candlelight.PropertyBackingField]
    private Vector2 m_Offset = Vector2.zero;
	public Vector2 Offset{
        get {
            return m_Offset;
        }

        set {
			isDirty = true;
            m_Offset = value;
        }
    }

    public float[,] Map {
		get {
			if(isDirty) {
				map = GenerateNoiseMap(Width, Height, Seed, Scale, Octaves, Persistance, Lacunarity, Redistribution, Offset);
				isDirty = false;
			}

			return map;
		}
	}

	public NoiseGenerator() {
		updateMap();
	}

	private float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int m_Seed, float m_Scale, int m_Octaves, float m_Persistance, float m_Lacunarity, float m_Redistribution, Vector2 m_Offset) {
		float[,] noiseMap = new float[mapWidth,mapHeight];

		System.Random prng = new System.Random (m_Seed);
		Vector2[] octaveOffsets = new Vector2[m_Octaves];
		for (int i = 0; i < m_Octaves; i++) {
			float offsetX = prng.Next (-100000, 100000) + m_Offset.x;
			float offsetY = prng.Next (-100000, 100000) + m_Offset.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);
		}

		if (m_Scale <= 0) {
			m_Scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;


		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
		
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < m_Octaves; i++) {
					float sampleX = (x-halfWidth) / m_Scale * frequency + octaveOffsets[i].x;
					float sampleY = (y-halfHeight) / m_Scale * frequency + octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= m_Persistance;
					frequency *= m_Lacunarity;
				}

				if (noiseHeight > maxNoiseHeight) {
					maxNoiseHeight = noiseHeight;
				} else if (noiseHeight < minNoiseHeight) {
					minNoiseHeight = noiseHeight;
				}
				noiseMap [x, y] = Mathf.Pow(noiseHeight, Mathf.Clamp(m_Redistribution, 0.01f, 5f));
			}
		}

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
			}
		}

		return noiseMap;
	}

	public void updateMap() {
		map = GenerateNoiseMap(Width, Height, Seed, Scale, Octaves, Persistance, Lacunarity, Redistribution, Offset);
	}
}