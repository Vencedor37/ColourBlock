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
      Vector3 releasePosition = Input.mousePosition;
      columns.CheckInputColumn(releasePosition);
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
