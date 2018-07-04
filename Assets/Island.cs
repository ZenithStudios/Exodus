using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour {

	public int width, height;

	public NoiseGenerator topGen = new NoiseGenerator();
	public NoiseGenerator underGen = new NoiseGenerator();

	void Start() {
		
	}
}
