using UnityEngine;
using System.Collections;

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
    int colorIndex = Random.Range(0,5);
    blockColor = (BlockColor)colorIndex;
    GetComponent<SpriteRenderer>().color = colorController.getColor(blockColor);
    MoveToTop();
    SnapToColumn(Grid.w/2);
    isActive = true;
    gameObject.tag = "ActiveBlock";
  }

  public void PlaceBlock()
  {
    isActive = false;
    gameObject.tag = "Untagged";
    gameObject.layer = PlacedBlockLayer;
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

}
