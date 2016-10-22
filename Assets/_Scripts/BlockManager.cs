using UnityEngine;
using System.Collections;

public class BlockManager : MonoBehaviour {
  public ColumnManager columnManager;
  private BlockController activeBlock;
  private BlockController[] pool;
  private int currentIndex;
  public int poolCount;
  public GameObject block;
  private bool readyToStart = false;
  public float moveAtTime = 1f;



	// Use this for initialization
	void Start () {
    currentIndex = 0;
    InitialisePool();
    readyToStart = true;

	}

	// Update is called once per frame
	void Update () {
    if (readyToStart) {
      SpawnBlock();
    }

	}

  void SpawnBlock()
  {
    if (currentIndex < pool.Length && (activeBlock == null || !activeBlock.getIsActive())) {
      activeBlock = pool[currentIndex];
      activeBlock.gameObject.SetActive(true);
      activeBlock.ActivateBlock();
      currentIndex ++;
    }
  }

  void InitialisePool()
  {
    pool = new BlockController[poolCount];
    for (int i = 0; i < poolCount; i++) {
      GameObject newObject = (GameObject)Instantiate(block, new Vector3(0, 0, 0), Quaternion.identity) ;
      newObject.transform.SetParent(GetComponent<Transform>(), false);
      newObject.gameObject.SetActive(false);
      BlockController blockController = newObject.GetComponent<BlockController>();
      blockController.setBlockManager(this);
      pool[i] = blockController;
    }
  }

  public float getMoveAtTime()
  {
    return moveAtTime;
  }
}
