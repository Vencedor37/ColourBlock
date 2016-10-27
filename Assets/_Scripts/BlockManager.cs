using UnityEngine;
using System.Collections;

public class BlockManager : MonoBehaviour {
  public ColumnManager columnManager;
  public ColorController colorController;
  private ContainerController activeBlock;
  private ContainerController[] pool;
  private int currentIndex;
  public int poolCount;
  public GameObject objectType;
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
      activeBlock.setMoveAtTime(moveAtTime);
      activeBlock.ActivateBlocks();
      currentIndex ++;
    }
  }

  void InitialisePool()
  {
    pool = new ContainerController[poolCount];
    for (int i = 0; i < poolCount; i++) {
      GameObject newObject = (GameObject)Instantiate(objectType, new Vector3(0, 0, 0), Quaternion.identity) ;
      newObject.transform.SetParent(GetComponent<Transform>(), false);
      newObject.gameObject.SetActive(false);
      ContainerController container = newObject.GetComponent<ContainerController>();
      container.setBlockManager(this);
      pool[i] = container;
    }
  }

  public float getMoveAtTime()
  {
    return moveAtTime;
  }
}
