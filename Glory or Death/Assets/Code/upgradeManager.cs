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
    Combat_UI _combat_UI;

    List<GameObject> blockList = new List<GameObject>();

    private bool upgradedSomehthing;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _timeManager = FindObjectOfType<timeManager>();
        _endManager = FindObjectOfType<endManager>();
        _audioManager = FindObjectOfType<AudioManager>();
        _combat_UI = FindObjectOfType<Combat_UI>();
    }

    private void OnEnable()
    {
        getBlocks();
        setRandomUpgrade();
    }

    private void OnDisable()
    {
        upgradedSomehthing = false;
    }

    private int previousRandom = -1;
    

    private void setRandomUpgrade()
    {
        foreach (GameObject block in blockList)
        {
            foreach (Transform upgrade in block.transform)
            {
                upgrade.gameObject.SetActive(false);
            }

            int childCount = block.transform.childCount;
            int random;

            do
            {
                random = Random.Range(0, childCount);
            } while (random == previousRandom);

            previousRandom = random;

            block.transform.GetChild(random).gameObject.SetActive(true);
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

    // Upgrades right block
    public void reduceDodgeCooldown()
    {
        if (_endManager.GetStars() >= 2)
        {
            upgradedSomehthing = true;
            _timeManager.dodgeFactorCD -= 0.2f;
            _endManager.reduceStars(2);
            _endManager.updateStarUI();
            buttonFeedback(true, _rightBlock.transform.GetChild(0).transform);
        } else
        {
            buttonFeedback(false, _rightBlock.transform.GetChild(0).transform);
        }
    }
    public void reduceATKCooldown()
    {
        if (_endManager.GetStars() >= 2)
        {
            upgradedSomehthing = true;
            _timeManager.attackFactorCD -= 0.2f;
            _endManager.reduceStars(2);
            _endManager.updateStarUI();
            buttonFeedback(true, _rightBlock.transform.GetChild(1).transform);
        } else
        {
            buttonFeedback(false, _rightBlock.transform.GetChild(1).transform);
        }
    }
    public void adrenalineEarning()
    {
        if (_endManager.GetStars() >= 3)
        {
            upgradedSomehthing = true;
            _player.SetAdrenalineFactor(_player.GetAdrenalineFactor() + 1);
            _endManager.reduceStars(3);
            _endManager.updateStarUI();
            buttonFeedback(true, _rightBlock.transform.GetChild(2).transform);
        }
        else
        {
            buttonFeedback(false, _rightBlock.transform.GetChild(2).transform);
        }
    }
    public void reduceFocusCost()
    {
        if (_endManager.GetStars() >= 2)
        {
            upgradedSomehthing = true;
            _timeManager.CostFC = _timeManager.CostFC -= 5;
            _timeManager.setActionCost();
            _endManager.reduceStars(2);
            _endManager.updateStarUI();
            buttonFeedback(true, _rightBlock.transform.GetChild(3).transform);
        }
        else
        {
            buttonFeedback(false, _rightBlock.transform.GetChild(3).transform);
        }
    }

    // Upgrades center block
    public void incrementATK()
    {
        if (_endManager.GetStars() >= 3)
        {
            upgradedSomehthing = true;
            _player.NativeDamage++;
            _endManager.reduceStars(3);
            _endManager.updateStarUI();
            buttonFeedback(true, _centerBlock.transform.GetChild(0).transform);
        }
        else
        {
            buttonFeedback(false, _centerBlock.transform.GetChild(0).transform);
        }
    }
    public void incrementSpeed()
    {
        if (_endManager.GetStars() >= 4)
        {
            upgradedSomehthing = true;
            _player.incrementBaseSpeed(.4f);
            _endManager.reduceStars(4);
            _endManager.updateStarUI();
            buttonFeedback(true, _centerBlock.transform.GetChild(1).transform);
        }
        else
        {
            buttonFeedback(false, _centerBlock.transform.GetChild(1).transform);
        }
    }

    public void shieldEarning()
    {
        if (_endManager.GetStars() >= 3)
        {
            upgradedSomehthing = true;
            _player.SetShieldFactor(_player.GetShieldFactor() + 1);
            _endManager.reduceStars(3);
            _endManager.updateStarUI();
            buttonFeedback(true, _centerBlock.transform.GetChild(2).transform);
        }
        else
        {
            buttonFeedback(false, _centerBlock.transform.GetChild(2).transform);
        }
    }

    // Upgrades left block
    public void recoverHealth()
    {
        // calculate 30%
        float lifeBack = _player.GetMaxHP() * .3f;
        if (_endManager.GetStars() >= 3)
        {
            upgradedSomehthing = true;
            _player.SetCurrentHP(_player.GetCurrentHP() + (int)lifeBack);
            _endManager.reduceStars(3);
            _endManager.updateStarUI();
            buttonFeedback(true, _leftBlock.transform.GetChild(0).transform);
        }
        else
        {
            buttonFeedback(false, _leftBlock.transform.GetChild(0).transform);
        }
    }
    public void incrementMaxShield()
    {
        if (_endManager.GetStars() >= 2)
        {
            upgradedSomehthing = true;
            _combat_UI.updateShieldBar();
            _player.setMaxShield(_player.GetMaxShield() + 1);
            _endManager.reduceStars(2);
            _endManager.updateStarUI();
            buttonFeedback(true, _leftBlock.transform.GetChild(1).transform);
        }
        else
        {
            buttonFeedback(false, _leftBlock.transform.GetChild(1).transform);
        }
    }

    public void incrementBlockSpeed()
    {
        if (_endManager.GetStars() >= 2)
        {
            upgradedSomehthing = true;
            _combat_UI.updateShieldBar();
            CounterManager.SetRotationSpeed(CounterManager.GetRotationSpeed() + 1);
            _endManager.reduceStars(2);
            _endManager.updateStarUI();
            buttonFeedback(true, _leftBlock.transform.GetChild(2).transform);
        }
        else
        {
            buttonFeedback(false, _leftBlock.transform.GetChild(2).transform);
        }
    }

    // Randomizer
    public void randomizeUpgrades()
    {
        if (_endManager.GetStars() >= 1 && !upgradedSomehthing)
        {
            _audioManager.Play("Randomize");
            _endManager.reduceStars(1);
            _endManager.updateStarUI();
            setRandomUpgrade();
        } else
        {
            _audioManager.Play("Shield_metal_4");
        }
    }
}
