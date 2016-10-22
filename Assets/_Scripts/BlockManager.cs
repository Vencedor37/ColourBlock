using UnityEngine;
using System.Collections;

public class BlockManager : MonoBehaviour {
  private BlockController activeBlock;
  private BlockController[] pool;
  private int currentIndex;
  public int poolCount;
  public Transform spawnPoint;
  public GameObject block;


	// Use this for initialization
	void Start () {
    currentIndex = 0;
    InitialisePool();

	}

	// Update is called once per frame
	void Update () {
    //SpawnBlock();

	}

  void SpawnBlock()
  {
    if (activeBlock == null || !activeBlock.getIsActive()) {
      activeBlock = pool[currentIndex];
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
      pool[i] = newObject.GetComponent<BlockController>();
    }
  }
}
