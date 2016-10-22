using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorController : MonoBehaviour {
  public Color redColor;
  public Color blueColor;
  public Color yellowColor;
  public Color greenColor;
  public Color purpleColor;
  public Color orangeColor;
  private BlockColor[] redAllowedBeside;
  private BlockColor[] blueAllowedBeside;
  private BlockColor[] yellowAllowedBeside;
  private BlockColor[] greenAllowedBeside;
  private BlockColor[] purpleAllowedBeside;
  private BlockColor[] orangeAllowedBeside;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

  public Color getColor(BlockColor blockColor)
  {
    switch (blockColor)
    {
      case BlockColor.RED:
        return redColor;
      case BlockColor.BLUE:
        return blueColor;
      case BlockColor.YELLOW:
        return yellowColor;
      case BlockColor.GREEN:
        return greenColor;
      case BlockColor.PURPLE:
        return purpleColor;
      case BlockColor.ORANGE:
        return orangeColor;
      default:
        return Color.white;
    }
  }
}
public enum BlockColor {RED,BLUE,YELLOW,GREEN,PURPLE,ORANGE};
