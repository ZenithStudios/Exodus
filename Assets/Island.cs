using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour {

	public bool randomSeed = true;
	public long seed = 0;

	public const float frequency = 10f;
	public const int maxHeight = 500;

	[ReadOnly]
	public int boundingX = 0, boundingZ = 0;

	// Use this for initialization
	void Start () {
		if(randomSeed) seed = (long)(Random.Range(0f, 1f) * long.MinValue);

		boundingX = (int)(Mathf.FloorToInt(Random.Range(20f, 40f)) * Chunk.chunkSize);
		boundingZ = (int)(Mathf.FloorToInt(Random.Range(20f, 40f)) * Chunk.chunkSize);
	}

	public int calcGroundLevel(int x, int z) {
		float xCoord = (float)x / boundingX * frequency + seed;
		float zCoord = (float)z / boundingZ * frequency + seed;

		float dist = Vector2.Distance(new Vector2(boundingX/2, boundingZ/2), new Vector2(x, z)) / (Mathf.Sqrt(Mathf.Pow(boundingX, 2) + Mathf.Pow(boundingZ, 2))); 

		return (int)(Mathf.PerlinNoise(xCoord, zCoord) * maxHeight + dist);
	}

	public int calcBottomLevel(int x, int z) {

		return 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
