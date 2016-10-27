using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContainerController : MonoBehaviour {
  private bool isActive = false;
  private float moveAmount = 1;
  private float moveTimer = 0;
  private float moveAtTime;
  private int PlacedBlockLayer = 9;
  [HideInInspector] public ColorController colorController;
  [HideInInspector] public ColumnManager columnManager;
  private BlockManager blockManager;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

  void FixedUpdate () {

    if (isActive) {
      moveTimer += Time.deltaTime;
      if (moveTimer >= moveAtTime) {
        MoveDown();
        moveTimer = 0;
      }
    }
  }

  void MoveDown () {
    float downwardMovement = moveAmount;
    List<Vector3> newPositions = new List<Vector3>();
    bool validMovement = true;
    foreach (Transform child in transform) {
      Vector3 newPosition = child.position;
      newPosition.y -= downwardMovement;
      if (IsValidGridPosition(newPosition)) {
        newPositions.Add(newPosition);
      } else {
        validMovement = false;
      }
    }

    if (validMovement) {
      int index = 0;
      foreach (Transform child in transform) {
        child.position = newPositions[index];
        index ++;
      }
      UpdateGrid();
    } else {
      PlaceBlock();
    }

  }

  public void ActivateBlocks()
  {
    columnManager = blockManager.columnManager;
    colorController = blockManager.colorController;
    UpdateColor();
    MoveToTop();
    SnapToColumn(Grid.w/2);
    isActive = true;
    gameObject.tag = "ActiveBlock";
  }


  public void MoveToTop()
  {
    Vector3 newPosition = transform.position;
    newPosition.y = columnManager.getGameAreaTop();
    transform.position = newPosition;
  }

  private bool CheckForBlockInColumn(int columnIndex)
  {
    foreach (Transform child in transform) {
      if (!IsValidGridPosition(new Vector2(columnIndex, child.position.y))) {
        return true;
      }
    }
    return false;
  }

  public bool getIsActive()
  {
    return isActive;
  }

  private void UpdateColor()
  {
    int counter = 0;
    foreach (Transform child in transform) {
      BlockController block = child.GetComponent<BlockController>();
      if (block != null) {
        counter ++;
        block.colorController = colorController;
        block.SelectRandomPermittedColor();
      }
    }
  }

  // the isActive lives on the container, but I think most of the other stuff
  // needs to live on the Block, as these don't care what container they are in
  // after being dropped
  public void PlaceBlock()
  {
    isActive = false;
    gameObject.tag = "Untagged";
    gameObject.layer = PlacedBlockLayer;
    UpdateGrid();
    CheckAdjoiningBlocks();
  }

  public void AttemptMoveToColumn(int columnIndex)
  {
    if (!CheckForBlockInColumn(columnIndex)) {
      SnapToColumn(columnIndex);
      UpdateGrid();
    }
  }

  public void SnapToColumn(int columnIndex)
  {
    foreach (Transform child in transform) {
      Vector3 position = child.position;
      position.x = columnIndex;
      child.position = Grid.roundVec2(position);
    }
  }

  // I think this is working now
  private void UpdateGrid()
  {
    // Remove self from grid at old position
    for (int y = 0; y < Grid.h; y++)
      for (int x = 0; x < Grid.w; x++) {
        Transform cell = Grid.grid[x,y];
        foreach (Transform child in transform) {
          if (cell != null && cell == child)
            Grid.grid[x,y] = null;
        }
      }

    // Add self to grid at new position
    foreach (Transform child in transform) {
      Vector2 v = Grid.roundVec2(child.position);
      Grid.grid[(int)v.x, (int)v.y] = child;
    }
  }

  private bool IsValidGridPosition(Vector2 v)
  {
    v = Grid.roundVec2(v);
    if (!Grid.insideBorder(v))
      return false;

    if (Grid.grid[(int)v.x, (int)v.y] != null &&
        Grid.grid[(int)v.x, (int)v.y].parent != transform) {
      return false;
    }

    return true;
  }

  public void setBlockManager(BlockManager value)
  {
    blockManager = value;
  }

  private void CheckAdjoiningBlocks()
  {
    foreach (Transform child in transform) {
      BlockController block = child.GetComponent<BlockController>();
      if (block != null) {
        block.CheckForViolation();
      }
    }
  }

  public void SwapColors()
  {
    int index = 0;
    BlockController previousBlock = null;
    foreach (Transform child in transform) {
      BlockController block = child.GetComponent<BlockController>();
      if (block != null) {
        if (index == 0) {
          previousBlock = block;  
        } else {
          BlockColor color = block.blockColor;
          block.UpdateColor(previousBlock.blockColor);
          previousBlock.UpdateColor(color);
          previousBlock = block;  
        }
        index ++;
      }
    }
  }

  public void setMoveAtTime(float amount) {
    moveAtTime = amount;
  }
}
