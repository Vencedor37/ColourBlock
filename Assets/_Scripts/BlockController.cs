using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockController : MonoBehaviour {
  public ColorController colorController;
  public BlockColor blockColor;
  public bool isNew = false;
  private bool isActive = false;
  public ColumnManager columnManager;
  private BlockManager blockManager;
  private float moveAmount = 1;
  private float moveTimer = 0;
  private int PlacedBlockLayer = 9;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
    if (isNew) {
      ActivateBlock();
      isNew = false;
    }
	}

  void FixedUpdate () {
    if (isActive) {
      moveTimer += Time.deltaTime;
      if (moveTimer >= blockManager.getMoveAtTime()) {
        MoveDown();
        moveTimer = 0;
      }
    }
  }

  void MoveDown () {
    float downwardMovement = moveAmount;
    Vector3 newPosition = transform.position;
    newPosition.y -= downwardMovement;
    if (IsValidGridPosition(newPosition)) {
      transform.position = newPosition;
      UpdateGrid();
    }
    else {
      PlaceBlock();
    }
  }

  public bool getIsActive()
  {
    return isActive;
  }

  public void AttemptMoveToColumn(int columnIndex)
  {
    if (!CheckForBlockInColumn(columnIndex)) {
      SnapToColumn(columnIndex);
      UpdateGrid();
    }
  }

  private bool CheckForBlockInColumn(int columnIndex)
  {
    return !IsValidGridPosition(new Vector2(columnIndex, transform.position.y));
  }

  public void MoveToTop()
  {
    Vector3 newPosition = transform.position;
    newPosition.y = columnManager.getGameAreaTop();
    transform.position = newPosition;
  }

  public void SnapToColumn(int columnIndex)
  {
    Vector3 position = transform.position;
    position.x = columnIndex;
    transform.position = Grid.roundVec2(position);
  }

  public void ActivateBlock()
  {
    columnManager = blockManager.columnManager;
    colorController = blockManager.colorController;
    int colorIndex = Random.Range(0,6);
    UpdateColor(colorIndex);
    MoveToTop();
    SnapToColumn(Grid.w/2);
    isActive = true;
    gameObject.tag = "ActiveBlock";
  }

  private void UpdateColor(int index)
  {
    blockColor = (BlockColor)index;
    foreach (Transform child in transform) {
      child.GetComponent<SpriteRenderer>().color = colorController.getColor(blockColor);
    }
  }

  public void PlaceBlock()
  {
    isActive = false;
    gameObject.tag = "Untagged";
    gameObject.layer = PlacedBlockLayer;
    getAdjoiningBlocks();

  }

  public void setBlockManager(BlockManager value)
  {
    blockManager = value;
  }

  private bool IsValidGridPosition(Vector2 v)
  {
    v = Grid.roundVec2(v);
    if (!Grid.insideBorder(v))
      return false;

    if (Grid.grid[(int)v.x, (int)v.y] != null &&
        Grid.grid[(int)v.x, (int)v.y] != transform)
      return false;

    return true;
  }

  private void UpdateGrid()
  {
    // Remove self from grid at old position
    for (int y = 0; y < Grid.h; y++)
      for (int x = 0; x < Grid.w; x++) {
        Transform cell = Grid.grid[x,y];
        if (cell != null && cell == transform)
          Grid.grid[x, y] = null;
      }

    // Add self to grid at new position
    Vector2 v = Grid.roundVec2(transform.position);
    Grid.grid[(int)v.x, (int)v.y] = transform;
  }

  private List<AdjoiningBlock> getAdjoiningBlocks()
  {
    List<AdjoiningBlock> adjoining = new List<AdjoiningBlock>();
    Vector2 position = Grid.roundVec2(transform.position);
    BlockController leftBlock = null;
    BlockController rightBlock = null;
    BlockController bottomBlock = null;
    int x = (int) position.x;
    int y = (int) position.y;

    if (x > 0) {
      leftBlock = Grid.getBlockAt(x-1, y);
      if (leftBlock) {
        adjoining.Add(new AdjoiningBlock(leftBlock, BlockPosition.LEFT));
      }
    }
    if (x < Grid.w - 1) {
      rightBlock = Grid.getBlockAt(x+1, y);
      if (rightBlock) {
        adjoining.Add(new AdjoiningBlock(rightBlock, BlockPosition.RIGHT));
      }
    }
    if (y > 0) {
      bottomBlock = Grid.getBlockAt(x, y-1);
      if (bottomBlock) {
        adjoining.Add(new AdjoiningBlock(bottomBlock, BlockPosition.BELOW));
      }
    }
    foreach (AdjoiningBlock adjoiningBlock in adjoining) {
      if (!colorController.IsPairPermitted(this, adjoiningBlock.block)) {
        TurnGrey();
      }
    }
    return adjoining;
  }

  private void TurnGrey()
  {
    int colorIndex = 6;
    UpdateColor(colorIndex);
  }


  private class AdjoiningBlock {
    public BlockController block;
    public BlockPosition blockPosition;

    public AdjoiningBlock(BlockController block, BlockPosition blockPosition)
    {
      this.block = block;
      this.blockPosition = blockPosition;
    }
  }

}


public enum BlockPosition { LEFT, RIGHT, BELOW };
