using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeManager : MonoBehaviour
{
    [SerializeField] GameObject _leftBlock;
    [SerializeField] GameObject _centerBlock;
    [SerializeField] GameObject _rightBlock;

    Player _player;
    timeManager _timeManager;
    endManager _endManager;

    List<GameObject> blockList = new List<GameObject>();

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _timeManager = FindObjectOfType<timeManager>();
        _endManager = FindObjectOfType<endManager>();

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

    // Upgrades
    public void reduceDodgeCooldown()
    {
        if (_endManager.GetStars() >= 2)
        {
            _timeManager.dodgeFactorCD -= 0.2f;
            _endManager.reduceStars(2);
            _endManager.updateStarUI();
        } else
        {
            Debug.Log("not enough stars");
        }
    }
    public void reduceATKCooldown()
    {
        if (_endManager.GetStars() >= 2)
        {
            _timeManager.attackFactorCD -= 0.2f;
            _endManager.reduceStars(2);
            _endManager.updateStarUI();
        } else
        {
            Debug.Log("not enough stars");
        }
    }
}
