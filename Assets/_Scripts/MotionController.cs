using UnityEngine;
using System.Collections;

public class MotionController : MonoBehaviour {
  public ColumnManager columns;
  public BlockManager blockManager;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
  void Update () {
    if (isInputReleased()) {
      GameObject activeBlock = GameObject.FindWithTag("ActiveBlock");
      if (activeBlock != null) {
        BlockController blockController = activeBlock.GetComponent<BlockController>();
        if (activeBlock != null) {
          ColumnController moveToColumn = columns.CheckPointColumn(Input.mousePosition);
          if (moveToColumn != null) {
            blockController.AttemptMoveToColumn(moveToColumn);
          }
        }
      }
    }
  }

  private bool leftPressed () {
    return false;
  }

  bool isInputReleased()
  {
    if (Input.GetMouseButtonUp(0)) {
      return true;
    }
    return false;
  }

}
