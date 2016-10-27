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
    GameObject activeBlocks = GameObject.FindWithTag("ActiveBlock");
    if (isInputReleased()) {
      if (activeBlocks != null) {
        ContainerController container = activeBlocks.GetComponent<ContainerController>();
        if (container != null) {
          int moveToColumn = columns.CheckPointColumnIndex(Input.mousePosition);
          if (moveToColumn != -1) {
            container.AttemptMoveToColumn(moveToColumn);
          }
        }
      }
    }
    if (Input.GetMouseButtonUp(1))
    {
      ContainerController container = activeBlocks.GetComponent<ContainerController>();
      container.SwapColors();
    }

    if (Input.GetKeyUp("space")) {
      activeBlocks.GetComponent<ContainerController>().setMoveAtTime(0.00001f);
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
