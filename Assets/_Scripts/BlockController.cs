using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour {
  private bool isActive = false;
  private float size;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

  void FixedUpdate () {

  }

  void MoveDown () {

  }

  public bool getIsActive()
  {
    return isActive;
  }

  public void setSize(float value)
  {
    size = value;
  }
}
