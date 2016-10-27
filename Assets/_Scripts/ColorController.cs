using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ColorController : MonoBehaviour {
  public Color redColor;
  public Color blueColor;
  public Color yellowColor;
  public Color greenColor;
  public Color purpleColor;
  public Color orangeColor;
  public Color greyColor;
  public Color rainbowColor;
  public BlockColor[] redAllowedBeside;
  public BlockColor[] blueAllowedBeside;
  public BlockColor[] yellowAllowedBeside;
  public BlockColor[] greenAllowedBeside;
  public BlockColor[] purpleAllowedBeside;
  public BlockColor[] orangeAllowedBeside;
  public BlockColor[] spawnableColors;
  private BlockColor[] greyAllowedBeside;
  private BlockColor[] allColors;
  private BlockColor[] rainbowAllowedBeside;

  public static int GREY_INDEX = 6;

	// Use this for initialization
	void Start () {
    allColors = new BlockColor[System.Enum.GetNames(typeof(BlockColor)).Length];
    for (int i = 0; i < allColors.Length; i++) {
      allColors[i] = (BlockColor)i;
    }

    greyAllowedBeside   = allColors;
    rainbowAllowedBeside = allColors;
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
      case BlockColor.GREY:
        return greyColor;
      default:
        return Color.white;
    }
  }

  public BlockColor[] GetPermittedColors(BlockColor currentColor)
  {
    switch (currentColor)
    {
      case BlockColor.RED:
        return redAllowedBeside;
      case BlockColor.BLUE:
        return blueAllowedBeside;
      case BlockColor.YELLOW:
        return yellowAllowedBeside;
      case BlockColor.GREEN:
        return greenAllowedBeside;
      case BlockColor.PURPLE:
        return purpleAllowedBeside;
      case BlockColor.ORANGE:
        return orangeAllowedBeside;
      case BlockColor.GREY:
        return greyAllowedBeside;
      case BlockColor.RAINBOW:
        return rainbowAllowedBeside;
      default:
        return null;
    }
  }

  public bool IsPairPermitted(BlockController currentBlock, BlockController adjoiningBlock)
  {
    return CheckColorPair(currentBlock.blockColor, adjoiningBlock.blockColor);
  }

  private bool CheckColorPair(BlockColor current, BlockColor adjoining)
  {
    switch (current)
    {
      case BlockColor.RED:
        return redAllowedBeside.Contains(adjoining);
      case BlockColor.BLUE:
        return blueAllowedBeside.Contains(adjoining);
      case BlockColor.YELLOW:
        return yellowAllowedBeside.Contains(adjoining);
      case BlockColor.GREEN:
        return greenAllowedBeside.Contains(adjoining);
      case BlockColor.PURPLE:
        return purpleAllowedBeside.Contains(adjoining);
      case BlockColor.ORANGE:
        return orangeAllowedBeside.Contains(adjoining);
      case BlockColor.RAINBOW:
        return rainbowAllowedBeside.Contains(adjoining);
      default:
        return false;
    }
  }

  public BlockColor[] GetSpawnableColors()
  {
    return spawnableColors;
  }

  public List<BlockColor> GetPermittedColors(List<BlockColor> currentColors)
  {
    if (currentColors.Count == 0) {
      return new List<BlockColor>(spawnableColors);
    }

    List<BlockColor> permittedColors = new List<BlockColor>();
    for (int i = 0; i < currentColors.Count; i++) {
      BlockColor[] colors = GetPermittedColors(currentColors[i]);
      if (i == 0) {
        foreach (BlockColor color in colors) {
          permittedColors.Add(color);
        }
      } else {
        foreach (BlockColor storedColor in permittedColors) {
          if (!colors.Contains(storedColor)) {
            permittedColors.Remove(storedColor);
          }
        }
      }
    }
    permittedColors.Remove(BlockColor.GREY);
    foreach (BlockColor color in currentColors) {
      permittedColors.Remove(color);
    }
    return permittedColors;
  }
}
public enum BlockColor {RED,BLUE,YELLOW,GREEN,PURPLE,ORANGE, GREY, UNDEFINED, RAINBOW};
