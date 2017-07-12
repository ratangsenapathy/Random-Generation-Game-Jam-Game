﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

	public float terrainWidth;
	public float terrainHeight;
	public float terrainLength;
	public float distanceAboveSeaLevel;
	public int heightMapResolution;

	private SplatPrototype[] terrainTexture;
	private GameObject terrainObject;
	// Use this for initialization
	void Start () {

		InitializeTerrainTextures ();
		CreateTerrain ();
		GenerateMountain ();
		//GenerateMountain ();
		//GenerateMountain ();
	}

	void InitializeTerrainTextures(){
		
		terrainTexture = new SplatPrototype[2]; 
		terrainTexture[0] = new SplatPrototype();
		terrainTexture[0].texture = (Texture2D)Resources.Load("Standard Assets/Environment/TerrainAssets/SurfaceTextures/GrassHillAlbedo");
		terrainTexture[1] = new SplatPrototype();
		terrainTexture[1].texture = (Texture2D)Resources.Load("Standard Assets/Environment/TerrainAssets/SurfaceTextures/SandAlbedo");

	}
	
	void CreateTerrain(){
		
		TerrainData tData = new TerrainData();
		tData.heightmapResolution = heightMapResolution;
		tData.size = new Vector3(terrainWidth,terrainHeight,terrainLength);

		tData.splatPrototypes = terrainTexture;

		float[,] heights = tData.GetHeights(0,0,tData.heightmapWidth,tData.heightmapHeight );

		for (int i = 0; i < tData.heightmapWidth; i++) 
		{
			for (int j = 0; j < tData.heightmapHeight; j++)
			{
				if(i==tData.heightmapWidth-1 || j==tData.heightmapHeight-1 || i==0 || j==0)
					heights [i, j] = 0.0f;
				else if(i==tData.heightmapWidth-2 || j==tData.heightmapHeight-2 || i==1 || j==1)
					heights [i, j] = 3/terrainHeight;
				else if(i==tData.heightmapWidth-3 || j==tData.heightmapHeight-3 || i==2 || j==2)
					heights [i, j] = 5/terrainHeight;
				else if(i==tData.heightmapWidth-4 || j==tData.heightmapHeight-4 || i==3 || j==3)
					heights [i, j] = 8/terrainHeight;
				else
					heights [i, j] = distanceAboveSeaLevel/terrainHeight;
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

	void GenerateMountain(){

		TerrainData tData = terrainObject.GetComponent<Terrain> ().terrainData;

		float[,] heights = tData.GetHeights(0,0,tData.heightmapWidth,tData.heightmapHeight);


		int xCoord = Random.Range (30,tData.heightmapWidth-30);
		int yCoord = Random.Range (30,tData.heightmapHeight-30);
		Debug.Log (tData.heightmapWidth);
		Debug.Log (tData.heightmapHeight);
		Debug.Log (xCoord);
		Debug.Log (yCoord);

		float mountainAltitude = Random.Range (terrainHeight/3.0f,terrainHeight);
//		heights [xCoord, yCoord] = (Mathf.Log(mountainAltitude,2)/10.0f);
//		heights [xCoord-1, yCoord] = (Mathf.Log(mountainAltitude/2,2)/10.0f);
//		heights [xCoord-2, yCoord] = (Mathf.Log(mountainAltitude/4,2)/10.0f);
//		heights [xCoord-3, yCoord] = (Mathf.Log(mountainAltitude/8,2)/10.0f);
//		int mountainSlope = 30;
//		float maxDistanceSq = mountainSlope*mountainSlope*2; 
//		for (int i = xCoord - mountainSlope; i < xCoord + mountainSlope; i++) {
//			for (int j = yCoord - mountainSlope; j < yCoord + mountainSlope; j++) {
//				float distSq = (xCoord - i) * (xCoord - i) + (yCoord - j) * (yCoord - j);
//				float distFraction = distSq / maxDistanceSq;
//				heights [i, j] = 1.0f/Mathf.Exp (distFraction)/Mathf.Exp(1.0f);
//			}
//		}

//		for (int i = xCoord; i < xCoord + 50; i++) {
//			float x = (i-xCoord) / 50.0f;
//			heights [i, yCoord] = 1.0f-(Mathf.Exp(10*x) - 1.0f)/(Mathf.Exp(10*1) - 1.0f);
//		}

		float maxDistance = Mathf.Sqrt(30 * 30 * 2);
		for (int i = xCoord-30; i < xCoord+30; i++) {
			for (int j = yCoord-30; j < yCoord + 30; j++) {
				float distance = (xCoord - i) * (xCoord - i) + (yCoord - j) * (yCoord - j);
				distance = Mathf.Sqrt (distance);
				float val = 1.0f - distance / maxDistance;
				val = val * mountainAltitude/terrainHeight+ distanceAboveSeaLevel/terrainHeight;
				heights[i,j] = val;
			}
		}


		//Debug.Log (heights [xCoord, yCoord]);
		tData.SetHeights(0,0,heights);
	}
}