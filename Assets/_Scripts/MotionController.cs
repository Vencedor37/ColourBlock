using UnityEngine;
using System.Collections;

public class MotionController : MonoBehaviour {
  public ColumnManager columns;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
  void Update () {
    if (isInputReleased()) {
      GameObject activeBlock = GameObject.FindWithTag("ActiveBlock");
      if (activeBlock != null) {
        ColumnController moveToColumn = columns.CheckPointColumn(Input.mousePosition);
        if (moveToColumn != null) {
          activeBlock.GetComponent<BlockController>().SnapToColumn(moveToColumn);
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
