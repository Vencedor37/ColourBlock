﻿using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
  public static int w = 9;
  public static int h = 18;
  public static Transform[,] grid = new Transform[w, h];

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

  public static Vector2 roundVec2(Vector2 v) {
    return new Vector2(Mathf.Round(v.x),
        Mathf.Round(v.y));
  }

	public static bool insideBorder(Vector2 pos) {
		return ((int)pos.x >= 0 &&
				    (int)pos.x < w &&
				    (int)pos.y >= 0);
	}

  // might not use this function, coming from tetris tutorial
	public static void deleteRow(int y) {
		for (int x = 0; x < w; ++x) {
			Destroy(grid[x, y].gameObject);
			grid[x, y] = null;
		}
	}

  // again, might not use this, from tetris tutorial
  public static void decreaseRow(int y) {
    for (int x = 0; x < w; ++x) {
      if (grid[x, y] != null) {
        // Move one towards bottom
        grid[x, y-1] = grid[x, y];
        grid[x, y] = null;

        // Update Block position
        grid[x, y-1].position += new Vector3(0, -1, 0);
      }
    }
  }

  public static BlockController getBlockAt(int x, int y)
  {
    if (grid[x, y] != null) {
      BlockController block = grid[x, y].GetComponent<BlockController>();
      return block;
    }
    return null;
  }


}
