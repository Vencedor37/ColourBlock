using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour {
  public bool isNew = false;
  private bool isActive = false;
  public ColumnManager columnManager;
  private BlockManager blockManager;
  private bool matchesColumn = false;
  private float moveAmount;
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
    float downwardMovement = moveAmount * Time.fixedDeltaTime * 30;
    Vector3 newPosition = transform.position;
    Vector2 hitFloor = CheckForBlocksBelow(downwardMovement);
    if (hitFloor != Vector2.zero) {
      newPosition = hitFloor;
      newPosition.y += transform.localScale.y;
      PlaceBlock();
    }
    newPosition.y -= downwardMovement;
    transform.position = newPosition;
  }

  public bool getIsActive()
  {
    return isActive;
  }

  private Vector2 CheckForBlocksBelow(float downwardMovement)
  {
    float blockFloor = transform.position.y - transform.localScale.y;
    Vector2 start = new Vector2(transform.position.x, blockFloor);
    Vector2 finish = new Vector2(transform.position.x, blockFloor-downwardMovement);
    int layerMask = 1 << PlacedBlockLayer;
    RaycastHit2D castResults = Physics2D.Linecast(start, finish, layerMask);
    if (castResults) {
      return castResults.point;
    }
    return Vector2.zero;
  }

  public void AttemptMoveToColumn(ColumnController column)
  {
    if (!CheckForBlockInColumn(column)) {
      SnapToColumn(column);
    }
  }

  private bool CheckForBlockInColumn(ColumnController column)
  {
    Vector2 start = new Vector2(column.getCentreX(), transform.position.y);
    Vector2 finish = new Vector2(column.getLeftX(), transform.position.y);
    int layerMask = 1 << PlacedBlockLayer;
    RaycastHit2D castResults = Physics2D.Linecast(start, finish, layerMask);
    if (castResults) {
      return true;
    } else {
      return false;
    }
  }

  public void ResizeToFitColumn()
  {
    if (!matchesColumn && columnManager.getColumnWidth() != 0) {
      float amount = columnManager.getColumnWidth();
      Vector3 scale = transform.localScale;
      scale.x = amount;
      scale.y = amount;
      transform.localScale = scale;
      matchesColumn = true;
      moveAmount = amount;
    }
  }

  public void MoveToTop()
  {
    Vector3 newPosition = transform.position;
    newPosition.y = columnManager.getGameAreaTop();
    transform.position = newPosition;
  }

  public void SnapToColumn(bool useCentre = false)
  {
    ColumnController snapColumn;
    if (useCentre) {
      int index = columnManager.getColumns().Count/2;
      snapColumn = columnManager.getColumns()[index];
    } else {
      snapColumn = columnManager.CheckPointColumn(transform.position);
    }
    Vector3 position = transform.position;
    position.x = snapColumn.getCentreX() - columnManager.columnBorderOffset;
    transform.position = position;
  }

  public void SnapToColumn(ColumnController snapColumn)
  {
    Vector3 position = transform.position;
    position.x = snapColumn.getCentreX() - columnManager.columnBorderOffset;
    transform.position = position;
  }

  public void ActivateBlock()
  {
    columnManager = blockManager.columnManager;
    ResizeToFitColumn();
    MoveToTop();
    SnapToColumn(true);
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

}
