
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{

	public GameObject BasicCube;
	public GameObject Ground;
	public GameObject PickUpRed;
	public GameObject PickUpGreen;
	public GameObject PickUpBlue;
	public GameObject Player;
    public GameObject MainCamera;

    public int width;
	public int height;
	[Range(0, 100)]
	public int fillPercent;

	public string seed;
	public bool useRandomSeed;

	GameObject[,] mBasicCube;
	GameObject mGround;
	GameObject mPickUpRed;
	GameObject mPickUpGreen;
	GameObject mPickUpBlue;
	GameObject mPlayer;
    GameObject mMainCamera;

	// Use this for initialization
	void Start()
	{
		InitializeMap();
		GenerateMap();
	}

	// Debug feature to test new maps by left clicking
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
			GenerateMap();
	}

    // Generates map by changing the active state of mBasicCubes then centers player
    void GenerateMap()
	{
		RandomFillMap();
		for (int i = 0; i < 5; i++)
		{
			SmoothMap();
		}

		mPlayer.transform.position = new Vector3(.5f, 1.5f, .5f);
	}

    // Initialize all GameObjects by creating them and placing them in the scene
	void InitializeMap()
	{
		// Creates the ground to scale with the map size
		mGround = Instantiate(Ground);
		mGround.name = "New Ground";
		mGround.transform.localScale += new Vector3((float)width / 10 - 1, 0, (float)height / 10 - 1);

		// Creates a [,] to hold all of the mBasicCubes
		mBasicCube = new GameObject[width, height];

		// Creates a mBasicCube at every grid location in relation to map size
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				mBasicCube[x, y] = Instantiate(BasicCube, new Vector3(x + -width / 2 + .5f, .5f, y + -height / 2 + .5f), Quaternion.identity);
				mBasicCube[x, y].name = "Basic Cube: " + x + ", " + y;
			}
		}

		// Creates a Player
		mPlayer = Instantiate(Player);
		mPlayer.name = "Player 1";

        // Creates Main Camera
        mMainCamera = Instantiate(MainCamera);
    }

    // Creates pseudo random static by changing the active state of mBasicCubes
	void RandomFillMap()
	{
		System.Random prng = new System.Random();
		if (useRandomSeed == false)
			prng = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
					mBasicCube[x, y].SetActive(true);
				else if (prng.Next(0, 100) < fillPercent)
					mBasicCube[x, y].SetActive(true);
				else
					mBasicCube[x, y].SetActive(false);
			}
		}
	}

    // Changes the map from static to cave-like maps
	void SmoothMap()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				int wallCount = GetWallCount(x, y);
				if (wallCount > 4)
				{
					mBasicCube[x, y].SetActive(true);
				}
				else if (wallCount < 4)
				{
					mBasicCube[x, y].SetActive(false);
				}
			}
		}
	}

    // Destroys all gameobjects created from 'InitializeMap'
	void DestroyMap()
	{

	}

    // Identifies the 'active' surrounding wall count of a given 'mBasicCube'
	int GetWallCount(int x, int y)
	{
		int wallCount = 0;

		for (int mX = x - 1; mX <= x + 1; mX++)
		{
			for (int mY = y - 1; mY <= y + 1; mY++)
			{
				if (mX >= 0 && mX < width && mY >= 0 && mY < height)
				{
					if (mX != x || mY != y)
					{
						if (mBasicCube[mX, mY].activeSelf)
							wallCount++;
					}
				}
				else
				{
					wallCount++;
				}
			}
		}
		return wallCount;
	}

}

