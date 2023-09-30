using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class upgradeManager : MonoBehaviour
{
    [SerializeField] GameObject _leftBlock;
    [SerializeField] GameObject _centerBlock;
    [SerializeField] GameObject _rightBlock;

    Player _player;
    timeManager _timeManager;
    endManager _endManager;
    AudioManager _audioManager;

    List<GameObject> blockList = new List<GameObject>();

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _timeManager = FindObjectOfType<timeManager>();
        _endManager = FindObjectOfType<endManager>();
        _audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnEnable()
    {
        getBlocks();
        setRandomUpgrade();
    }
    private void setRandomUpgrade()
    {
        foreach (GameObject block in blockList)
        {
            block.transform.GetChild(Random.Range(0, block.transform.childCount)).gameObject.SetActive(true);
        }
    }
    void getBlocks()
    {
        blockList.Add(_leftBlock);
        blockList.Add(_centerBlock);
        blockList.Add(_rightBlock);
    }
    public void buttonFeedback(bool hasStars, Transform button)
    {
        if (hasStars)
        {
            _audioManager.Play("Upgrade_Click");
            button.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f).OnComplete(() => button.gameObject.SetActive(false));
        } else
        {
            _audioManager.Play("Upgrade_Click_NO");
        }
    }

    // Upgrades
    public void reduceDodgeCooldown()
    {
        if (_endManager.GetStars() >= 2)
        {
            _timeManager.dodgeFactorCD -= 0.2f;
            _endManager.reduceStars(2);
            _endManager.updateStarUI();
            buttonFeedback(true, _rightBlock.transform);
        } else
        {
            buttonFeedback(false, _rightBlock.transform);
        }
    }

    public void reduceATKCooldown()
    {
        if (_endManager.GetStars() >= 2)
        {
            _timeManager.attackFactorCD -= 0.2f;
            _endManager.reduceStars(2);
            _endManager.updateStarUI();
            buttonFeedback(true, _rightBlock.transform);
        } else
        {
            buttonFeedback(false, _rightBlock.transform);
        }
    }

    public void incrementATK()
    {
        if (_endManager.GetStars() >= 3)
        {
            _player.NativeDamage++;
            _endManager.reduceStars(3);
            _endManager.updateStarUI();
            buttonFeedback(true, _centerBlock.transform);
        }
        else
        {
            buttonFeedback(false, _centerBlock.transform);
        }
    }
    public void incrementSpeed()
    {
        if (_endManager.GetStars() >= 4)
        {
            _player.incrementBaseSpeed(2);
            _endManager.reduceStars(4);
            _endManager.updateStarUI();
            buttonFeedback(true, _centerBlock.transform);
        }
        else
        {
            buttonFeedback(false, _centerBlock.transform);
        }
    }

    public void recoverHealth()
    {
        // calculate 30%
        float lifeBack = _player.GetMaxHP() * .3f;
        if (_endManager.GetStars() >= 1)
        {
            _player.SetCurrentHP(_player.GetCurrentHP() + (int)lifeBack);
            _endManager.reduceStars(1);
            _endManager.updateStarUI();
            buttonFeedback(true, _leftBlock.transform);
        }
        else
        {
            buttonFeedback(false, _leftBlock.transform);
        }
    }
    public void incrementMaxShield()
    {
        if (_endManager.GetStars() >= 2)
        {
            _player.setMaxShield(_player.GetMaxShield() + 1);
            _endManager.reduceStars(2);
            _endManager.updateStarUI();
            buttonFeedback(true, _leftBlock.transform);
        }
        else
        {
            buttonFeedback(false, _leftBlock.transform);
        }
    }
}
