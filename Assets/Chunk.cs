using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider))]
public class Chunk : MonoBehaviour {

	public static float cubeSize = 1f;
	public static int chunkSize = 16;

	public bool drawCubes = false;
	public bool drawPoints = false;

	public Cube[ , , ] cubes;
	public ControlNode[ , , ] nodes;

	private int nodeCount;
	private int cubeCount;
	private List<Vector3> verticies;
	private List<int> triangles;
	
	void Awake() {
		this.nodeCount = chunkSize + 1;
		this.cubeCount = chunkSize;

		nodes = new ControlNode[nodeCount, nodeCount, nodeCount];
		for(int x = 0; x < nodeCount; x++) {
			for(int y = 0; y < nodeCount; y++) {
				for(int z = 0; z < nodeCount; z++) {
					Vector3 nodePos = new Vector3(transform.position.x + (cubeSize * x), transform.position.y + (cubeSize * y), transform.position.z + (cubeSize * z));
					nodes[x, y, z] = new ControlNode(nodePos, true);
				}
			}
		}

		cubes = new Cube[cubeCount, cubeCount, cubeCount];
		for(int x = 0; x < cubeCount; x++) {
			for(int y = 0; y < cubeCount; y++) {
				for(int z = 0; z < cubeCount; z++) {
					cubes[x, y, z] = new Cube(nodes[x, y+1, z+1], nodes[x+1, y+1, z+1], nodes[x, y, z+1], nodes[x+1, y, z+1],
											nodes[x, y+1, z], nodes[x+1, y+1, z], nodes[x, y, z], nodes[x+1, y, z]);
				}
			}
		}
	}

	public void recalcTerrian() {
		verticies = new List<Vector3>();
		triangles = new List<int>();

		for(int x = 0; x < cubeCount; x++) {
			for(int y = 0; y < cubeCount; y++) {
				for(int z = 0; z < cubeCount; z++) {
					addCubeDataToMesh(cubes[x, y, z]);
				}
			}
		}

		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;

		mesh.vertices = verticies.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();
	}

	private void addCubeDataToMesh(Cube cube) {
		switch(cube.config) {
			case 0: break;
			case 1: meshFromPoints(cube.centerTopLeft, cube.forwardCenterTop, cube.forwardCenterLeft); break;
			case 255: meshFromPoints(); break;
		}
	}

	private void meshFromPoints(params Node[] points) {
		assignVerts(points);


	}

	private void assignVerts(Node[] verts) {
		foreach(Node node in verts) {
			if(node.vertexIndex == -1) {
				node.vertexIndex = verticies.Count;
				verticies.Add(node.pos);
			}
		}
	}

	void OnDrawGizmos() {
		if(drawCubes) {
			for(int x = 0; x < cubeCount; x++) {
				for(int y = 0; y < cubeCount; y++) {
					for(int z = 0; z < cubeCount; z++) {
						Gizmos.color = Color.black;
						Gizmos.DrawWireCube(cubes[x, y, z].center, new Vector3(1, 1, 1));
					}
				}
			}
		}

		if(drawPoints) {
			for(int x = 0; x < nodeCount; x++) {
				for(int y = 0; y < nodeCount; y++) {
					for(int z = 0; z < nodeCount; z++) {
						ControlNode node = nodes[x, y, z];

						Gizmos.color = Color.white;
						Gizmos.DrawWireSphere(node.pos, 0.25f);

						Gizmos.color = Color.gray;
						Gizmos.DrawWireSphere(node.forward.pos, 0.1f);
						Gizmos.DrawWireSphere(node.right.pos, 0.1f);
						Gizmos.DrawWireSphere(node.up.pos, 0.1f);
					}
				}
			}
		}
	}

	public class Cube {
		public Vector3 center;

		public ControlNode forwardTopLeft, forwardTopRight, forwardBottomLeft, forwardBottomRight, 
			backwardTopLeft, backwardTopRight, backwardBottomLeft, backwardBottomRight;

		public Node forwardCenterTop, forwardCenterRight, forwardCenterBottom, forwardCenterLeft, 
			centerTopLeft, centerTopRight, centerBottomLeft, centerBottomRight, 
			backwardCenterTop, backwardCenterRight, backwardCenterBottom, backwardCenterLeft;

		public int config = 0;

		public Cube(ControlNode forwardTopLeft, ControlNode forwardTopRight, ControlNode forwardBottomLeft, ControlNode forwardBottomRight, 
			ControlNode backwardTopLeft, ControlNode backwardTopRight, ControlNode backwardBottomLeft, ControlNode backwardBottomRight) {

			this.forwardTopLeft = forwardTopLeft;
			this.forwardTopRight = forwardTopRight;
			this.forwardBottomLeft = forwardBottomLeft;
			this.forwardBottomRight = forwardBottomRight;
			this.backwardTopLeft = backwardTopLeft;
			this.backwardTopRight = backwardTopRight;
			this.backwardBottomLeft = backwardBottomLeft;
			this.backwardBottomRight = backwardBottomRight;

			this.forwardCenterTop = this.forwardTopLeft.right;
			this.forwardCenterRight = this.forwardBottomRight.up;
			this.forwardCenterBottom = this.forwardBottomLeft.right;
			this.forwardCenterLeft = this.forwardBottomLeft.up;
			this.centerTopLeft = this.backwardTopLeft.forward;
			this.centerTopRight = this.backwardTopRight.forward;
			this.centerBottomRight = this.backwardBottomRight.forward;
			this.centerBottomLeft = this.backwardBottomLeft.forward;
			this.backwardCenterTop = this.backwardTopLeft.right;
			this.backwardCenterRight = this.backwardBottomRight.up;
			this.backwardCenterBottom = this.backwardBottomLeft.right;
			this.backwardCenterLeft = this.backwardBottomLeft.up;

			center = new Vector3(this.backwardCenterBottom.pos.x, this.backwardCenterLeft.pos.y, this.centerBottomLeft.pos.z);

			if(forwardTopLeft.isActive) config |= 1;
			if(forwardTopRight.isActive) config |= 2;
			if(forwardBottomLeft.isActive) config |= 4;
			if(forwardBottomRight.isActive) config |= 8;
			if(backwardTopLeft.isActive) config |= 16;
			if(backwardTopRight.isActive) config |= 32;
			if(backwardBottomLeft.isActive) config |= 64;
			if(backwardBottomRight.isActive) config |= 128;
		}
	}

	public class Node {
		public Vector3 pos;
		public int vertexIndex = -1;
		
		public Node(Vector3 pos) {
			this.pos = pos;
		}
	}

	public class ControlNode : Node {
		public bool isActive;
		public Node right, up, forward;

		public ControlNode(Vector3 pos, bool isActive) : base(pos) {
			this.isActive = isActive;

			right = new Node(pos + new Vector3(1, 0, 0) * (cubeSize/2));
			up = new Node(pos + new Vector3(0, 1, 0) * (cubeSize/2));
			forward = new Node(pos + new Vector3(0, 0, 1) * (cubeSize/2));
		}
	}
}
