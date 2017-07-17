using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

	public float terrainWidth;
	public float terrainHeight;
	public float terrainLength;
	public float distanceAboveSeaLevel;
	public int heightMapResolution;


	public GameObject[] trees;
	private SplatPrototype[] terrainTexture;
	private TreePrototype[] treePrototypes;
	private GameObject terrainObject;
	private List<TreeInstance> treeList;
	// Use this for initialization
	void Start () {
		treeList = new List<TreeInstance> ();
		InitializeTerrainTextures ();
		//InitializeTreePrototypes ();
		CreateTerrain ();
		//GeneratePond (new Vector3(400.0f,0.0f,400.0f));
		GenerateMountains (50);
		//GenerateMountains (50);
		//GenerateMountains (6);
		GenerateTrees(1500);
	}

	void GenerateMountains(int mountainCount){
		TerrainData tData = terrainObject.GetComponent<Terrain> ().terrainData;
		float mountainRadius = Random.Range (30.0f, 40.0f);
		int xCoord = Random.Range ((int)mountainRadius+1,tData.heightmapHeight-(int)mountainRadius-1);
		int yCoord = Random.Range ((int)mountainRadius+1,tData.heightmapWidth-(int)mountainRadius-1);
		for (int i = 0; i < mountainCount; i++) {

			GenerateMountain (xCoord,yCoord,mountainRadius);
			float newMountainRadius = Random.Range (30.0f, 40.0f);
			xCoord += (int)Random.Range (mountainRadius * 0.75f, mountainRadius*0.90f);
			yCoord += (int)Random.Range (mountainRadius * 0.75f, mountainRadius*0.90f);
			xCoord = Mathf.Clamp (xCoord,(int)newMountainRadius+1,tData.heightmapHeight-(int)newMountainRadius-1);
			yCoord = Mathf.Clamp (yCoord, (int)newMountainRadius + 1, tData.heightmapWidth - (int)newMountainRadius - 1);
			mountainRadius = newMountainRadius;
		}
	}
	void InitializeTerrainTextures(){
		
		terrainTexture = new SplatPrototype[4]; 
		terrainTexture[0] = new SplatPrototype();
		terrainTexture[0].texture = (Texture2D)Resources.Load("Standard Assets/Environment/TerrainAssets/SurfaceTextures/GrassHillAlbedo");
		terrainTexture[0].tileSize = new Vector2(5.0f,5.0f);
		terrainTexture[1] = new SplatPrototype();
		terrainTexture[1].texture = (Texture2D)Resources.Load("Standard Assets/Environment/TerrainAssets/SurfaceTextures/SandAlbedo");
		terrainTexture[1].tileSize = new Vector2(5.0f,5.0f);
		terrainTexture[2] = new SplatPrototype();
		terrainTexture[2].texture = (Texture2D)Resources.Load("Standard Assets/Environment/TerrainAssets/SurfaceTextures/MudRockyAlbedoSpecular");
		terrainTexture[2].normalMap = (Texture2D)Resources.Load("Standard Assets/Environment/TerrainAssets/SurfaceTextures/MudRockyNormals");
		terrainTexture[2].tileSize = new Vector2(5.0f,5.0f);
		terrainTexture[3] = new SplatPrototype();
		terrainTexture[3].texture = (Texture2D)Resources.Load("Standard Assets/Environment/TerrainAssets/SurfaceTextures/GrassRockyAlbedo");
		terrainTexture[3].tileSize = new Vector2(5.0f,5.0f);

	}

//	void InitializeTreePrototypes(){
//		treePrototypes = new TreePrototype[1];
//		treePrototypes [0] = new TreePrototype ();
//		treePrototypes[0].prefab =  broadleafTree;//(GameObject)Resources.Load("Standard Assets/Environment/SpeedTree/Broadleaf/BroadLeaf_Broadleaf_Desktop");//broadleafTree;
//	}
		
	void CreateTerrain(){
		
		TerrainData tData = new TerrainData();
		tData.heightmapResolution = heightMapResolution;
		tData.size = new Vector3(terrainWidth,terrainHeight,terrainLength);

		tData.splatPrototypes = terrainTexture;
		tData.treePrototypes = treePrototypes;

		float[,] heights = tData.GetHeights(0,0,tData.heightmapWidth,tData.heightmapHeight );

		for (int i = 0; i < tData.heightmapWidth; i++) 
		{
			for (int j = 0; j < tData.heightmapHeight; j++)
			{
				if (i == tData.heightmapWidth - 1 || j == tData.heightmapHeight - 1 || i == 0 || j == 0)
					heights [i, j] = 0.0f;
				else if (i == tData.heightmapWidth - 2 || j == tData.heightmapHeight - 2 || i == 1 || j == 1)
					heights [i, j] = 3 / terrainHeight;
				else if (i == tData.heightmapWidth - 3 || j == tData.heightmapHeight - 3 || i == 2 || j == 2)
					heights [i, j] = 5 / terrainHeight;
				else if (i == tData.heightmapWidth - 4 || j == tData.heightmapHeight - 4 || i == 3 || j == 3)
					heights [i, j] = 8 / terrainHeight;
				else
					heights [i, j] = distanceAboveSeaLevel / terrainHeight;// + Random.Range (0.002f,0.003f);
			}
		}
		tData.SetHeights(0,0,heights);

		float[,,] maps = tData.GetAlphamaps(0, 0, tData.alphamapWidth, tData.alphamapHeight);

		for (int i = 0; i < tData.alphamapWidth; i++) {

			for (int k = tData.alphamapHeight - 1; k > tData.alphamapHeight - 4; k--) {
				maps[i,k, 0] =	1.0f-(float)(k- tData.alphamapHeight - 4)/(float)(tData.alphamapHeight - 1 - tData.alphamapHeight - 4);
				maps[i,k, 1] =	(float)(k- tData.alphamapHeight - 4)/(float)(tData.alphamapHeight - 1 - tData.alphamapHeight - 4);
			}
			for (int k = 0; k < 4; k++) {
				maps[i,k, 0] =	1.0f-(4.0f-k)/4.0f;
				maps[i,k, 1] =	(4.0f-k)/4.0f;
			}

		}

		for (int j = 0; j < tData.alphamapHeight; j++) {

			for (int k = tData.alphamapWidth - 1; k > tData.alphamapWidth - 4; k--) {
				maps[k,j, 0] =	1.0f - (float)(k- tData.alphamapWidth - 4)/(float)(tData.alphamapWidth - 1 - tData.alphamapWidth - 4);
				maps[k,j, 1] =	(float)(k- tData.alphamapWidth - 4)/(float)(tData.alphamapWidth - 1 - tData.alphamapWidth - 4);
			}
			for (int k = 0; k < 4; k++) {
				maps[k,j, 0] =	1.0f-(4.0f-k)/4.0f;
				maps[k,j, 1] =	(4.0f-k)/4.0f;
			}

		}

		tData.SetAlphamaps(0, 0, maps);
		terrainObject = Terrain.CreateTerrainGameObject(tData);
	}

	void GenerateTrees(int count){
		for (int i = 0; i < count; i++) {
			if (!GenerateTree (new Vector3 (Random.Range (100.0f, 1950.0f), 0.0f, Random.Range (100.0f, 1950.0f))))
				i--;
		}
	}

	void GenerateMountain(int xCoord,int yCoord,float mountainRadius){

		TerrainData tData = terrainObject.GetComponent<Terrain> ().terrainData;
	
		float[,] heights = tData.GetHeights(0,0,tData.heightmapWidth,tData.heightmapHeight);
		float[,,] maps = tData.GetAlphamaps(0, 0, tData.alphamapWidth, tData.alphamapHeight);

		//float mountainRadius = Random.Range (30.0f, 60.0f);
		//xCoord = Random.Range ((int)mountainRadius+1,tData.heightmapWidth-(int)mountainRadius-1);
		//yCoord = Random.Range ((int)mountainRadius+1,tData.heightmapHeight-(int)mountainRadius-1);


		float mountainAltitude = Random.Range (terrainHeight/6.0f,terrainHeight/4.0f);



		float mrs = mountainRadius * mountainRadius;
		//float random = Random.Range (0.001f, 0.01f);
		for (int i = xCoord-(int)mountainRadius; i < xCoord+(int)mountainRadius; i++) {
			float random = Random.Range (0.001f, 0.01f);
			for (int j = yCoord-(int)mountainRadius; j < yCoord + (int)mountainRadius; j++) {
				float distance = (xCoord - i) * (xCoord - i) + (yCoord - j) * (yCoord - j);
				if (distance > mrs)
					continue;
				distance = Mathf.Sqrt (distance);
				float val = 1.0f - distance / mountainRadius;
				if (val >= 0.90f)
					val += Random.Range (0.02f,0.04f);
				val = val * mountainAltitude/terrainHeight+ distanceAboveSeaLevel/terrainHeight;
				//float random = Random.Range (0.001f, 0.01f);
//				if((yCoord + (int)mountainRadius)%j<=4)
//					heights[i,j] = Mathf.Max(heights[i,j],val Random.Range (0.001f, 0.01f));
//				else
				heights[i,j] = Mathf.Max(heights[i,j],val+ random);

				maps [i, j, 0] = 1.0f-i/(float)(2*mountainRadius);//1-i/2*(float)(xCoord+mountainRadius);;//0.0f;
				maps [i, j, 1] = 0.0f;
				maps [i, j, 2] = i/(float)(2*mountainRadius);//0.95f;
				maps [i, j, 3] = 0.0f;//1-i/2*(float)(xCoord+mountainRadius);//0.05f;
			}
		}

		//Debug.Log (heights [xCoord, yCoord]);
		tData.SetHeights(0,0,heights);
		tData.SetAlphamaps (0, 0, maps);
	}

	bool GenerateTree(Vector3 position){
//		TreeInstance newTree = new TreeInstance ();
//		newTree.prototypeIndex = 0;
//		newTree.color = new Color (1, 1, 1);
//		newTree.lightmapColor = new Color (1, 1, 1); 
//		newTree.heightScale = 1;
//		newTree.widthScale = 1;
//		newTree.position = new Vector3 (0.999f,0.0f,0.999f);
//		
//		treeList.Add (newTree);
//		//terrainObject.GetComponent<TerrainData> () .treeInstances = treeList.ToArray ();
//		terrainObject.GetComponent<Terrain> ().terrainData.treeInstances = treeList.ToArray ();
		TerrainData tData = terrainObject.GetComponent<Terrain> ().terrainData;

		float[,] heights = tData.GetHeights(0,0,tData.heightmapWidth,tData.heightmapHeight);
		int z = (int)(tData.heightmapWidth/terrainWidth*position.x);
		int x = (int)(tData.heightmapHeight/terrainLength*position.z);
		//Debug.Log (x);
		//Debug.Log (z);
		//Debug.Log (heights[x,z]);
		if (heights [x, z] * terrainHeight < 20.0f) {
			Instantiate (trees [Random.Range (0, 3)], new Vector3 (position.x, heights [x, z] * terrainHeight, position.z), Quaternion.identity);
			return true;
		} else
			return false;

	}

	void GeneratePond(int xCoord,int yCoord,float pondRadius){

		TerrainData tData = terrainObject.GetComponent<Terrain> ().terrainData;

		float[,] heights = tData.GetHeights(0,0,tData.heightmapWidth,tData.heightmapHeight);


		//float mountainRadius = Random.Range (30.0f, 60.0f);
		//xCoord = Random.Range ((int)mountainRadius+1,tData.heightmapWidth-(int)mountainRadius-1);
		//yCoord = Random.Range ((int)mountainRadius+1,tData.heightmapHeight-(int)mountainRadius-1);


		float pondDepth = Random.Range (distanceAboveSeaLevel/8.0f,distanceAboveSeaLevel/7.0f);



		float mrs = pondRadius * pondRadius;
		//float random = Random.Range (0.001f, 0.01f);
		for (int i = xCoord-(int)pondRadius; i < xCoord+(int)pondRadius; i++) {
			float random = Random.Range (0.001f, 0.01f);
			for (int j = yCoord-(int)pondRadius; j < yCoord + (int)pondRadius; j++) {
				float distance = (xCoord - i) * (xCoord - i) + (yCoord - j) * (yCoord - j);
				if (distance > mrs)
					continue;
				distance = Mathf.Sqrt (distance);
				float val = 1.0f - distance / pondRadius/10.0f;
				if (val >= 0.90f)
					val += Random.Range (0.02f,0.04f);
				val = val * pondDepth/terrainHeight+ distanceAboveSeaLevel/terrainHeight;
				//float random = Random.Range (0.001f, 0.01f);
				//				if((yCoord + (int)mountainRadius)%j<=4)
				//					heights[i,j] = Mathf.Max(heights[i,j],val Random.Range (0.001f, 0.01f));
				//				else
				heights[i,j] = Mathf.Min(heights[i,j],val+ random);


			}
		}

		//Debug.Log (heights [xCoord, yCoord]);
		tData.SetHeights(0,0,heights);

	}
}
