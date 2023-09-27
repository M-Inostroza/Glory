using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeManager : MonoBehaviour
{
    [SerializeField] GameObject _leftBlock;
    [SerializeField] GameObject _centerBlock;
    [SerializeField] GameObject _rightBlock;

    List<GameObject> blockList = new List<GameObject>();

    private void Start()
    {
        getBlocks();
        setRandomUpgrade();
    }
    private void setRandomUpgrade()
    {
        foreach (GameObject block in blockList)
        {
            block.transform.GetChild(Random.Range(0, blockList.Count -1)).gameObject.SetActive(true);
        }
    }
    void getBlocks()
    {
        blockList.Add(_leftBlock);
        blockList.Add(_centerBlock);
        blockList.Add(_rightBlock);
    }
}
