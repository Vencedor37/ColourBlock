using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockController : MonoBehaviour {
  public ColorController colorController;
  public BlockColor blockColor = BlockColor.UNDEFINED;
  public List<BlockController> adjoiningPartners;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
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
    return adjoining;
  }

  public void CheckForViolation()
  {
    List<AdjoiningBlock> adjoining = getAdjoiningBlocks();
    foreach (AdjoiningBlock adjoiningBlock in adjoining) {
      if (!colorController.IsPairPermitted(this, adjoiningBlock.block)) {
        TurnGrey();
      }
    }
  }

  public void SelectRandomPermittedColor()
  {
    List<BlockColor> currentColors = new List<BlockColor>();
    foreach (BlockController adjoiningPartner in adjoiningPartners) {
      if (adjoiningPartner.blockColor != BlockColor.UNDEFINED) {
        currentColors.Add(adjoiningPartner.blockColor);
      }
    }
    List<BlockColor> permittedColors = colorController.GetPermittedColors(currentColors);
    int index = Random.Range(0, permittedColors.Count);
    UpdateColor(permittedColors[index]);
  }

  private void TurnGrey()
  {
    UpdateColor(ColorController.GREY_INDEX);
  }

  private void UpdateColor(int index)
  {
    blockColor = (BlockColor)index;
    foreach (Transform child in transform) {
      child.GetComponent<SpriteRenderer>().color = colorController.getColor(blockColor);
    }
  }

  public void UpdateColor(BlockColor newColor)
  {
    blockColor = newColor;
    foreach (Transform child in transform) {
      child.GetComponent<SpriteRenderer>().color = colorController.getColor(blockColor);
    }
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
