using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using AssetKits.ParticleImage;

public class TargetManager : MonoBehaviour
{
    public GameObject[] targets;
    public List<int> attackOrder = new List<int>();

    private BattleSystem BattleSystem;
    private Camera MainCamera;
    private Combat_UI combat_UI;
    private TutorialManager TutorialManager;
    private Player _player;

    [SerializeField] SpriteRenderer courtain;
    [SerializeField] GameObject vFeedback;

    [SerializeField] ParticleImage ATKstars;
    
    private void Start()
    {
        BattleSystem = FindObjectOfType<BattleSystem>();
        MainCamera = FindObjectOfType<Camera>();
        combat_UI = FindObjectOfType<Combat_UI>();
        TutorialManager = FindObjectOfType<TutorialManager>();
        _player = FindObjectOfType<Player>();
    }

    public void attack()
    {
        activateFeedback();
        if (!gameManager.isTutorial())
        {
            Combat_UI.move_UI_out();
            BattleSystem.targetHit = 0;
            BattleSystem.canBloomAttack = true;
            courtain.DOColor(new Color(0, 0, 0, .5f), 0.8f);
        } else
        {
            if (!TutorialManager.hasShownDetail_attack)
            {
                courtain.DOColor(new Color(0, 0, 0, .5f), 0.1f);
            }
            courtain.DOColor(new Color(0, 0, 0, .5f), 0.8f);
            
        }
        StartCoroutine(activateTargets());
        zoomCameraIn();
    }

    IEnumerator activateTargets()
    {
        if (gameManager.isTutorial())
        {
            if (!TutorialManager.hasShownDetail_attack)
            {
                TutorialManager.attackDetailTutorial(3);
            }
        }
        
        var targets = this.targets;
        
        for (int i = 0; i < 3; i++)
        {
            targets[i].transform.DOScale(1, 0.1f);
            targets[i].GetComponent<SpriteRenderer>().DOFade(1, 0);
            targets[i].SetActive(true);

            if (gameManager.isTutorial())
            {
                if (!TutorialManager.hasShownDetail_attack)
                {
                    targets[i].transform.GetChild(0).gameObject.SetActive(true);
                    targets[i].transform.GetChild(0).transform.DOScale(new Vector2(.5f, .5f), .2f);
                }
            }
            

            if (i == 0)
            {
                targets[i].transform.localPosition.Set(Random.Range(-1f, 1.5f), Random.Range(3f, 1.5f), 0);
            }
            else if (i == 1)
            {
                targets[i].transform.localPosition = new Vector3(Random.Range(-1.1f, 2.4f), Random.Range(1.3f, -0.4f), 0);
            }
            else if (i == 2)
            {
                targets[i].transform.localPosition = new Vector3(Random.Range(-1, 2.6f), Random.Range(-1.4f, -2.2f), 0);
            }
        }

        // Deactivates after timer (Mejorable)!!
        yield return new WaitForSeconds(1.2f);

        foreach (var target in targets)
        {
            target.transform.DOKill();
            target.transform.DOScale(0, 0.05f).OnComplete(() => target.SetActive(false));
        }

        courtain.DOColor(new Color(0, 0, 0, 0), .5f);
        Combat_UI.move_UI_in();
        if (gameManager.isTutorial() && !TutorialManager.hasShownDetail_attack && TutorialManager.GetNumberOfTries() != 0)
        {
            StartCoroutine(TutorialManager.toggleInput(0, 1, 3.2f));
        }
    }

    public void checkCritic()
    {
        if (BattleSystem.targetHit == 3 && !BattleSystem.GetDeadEnemy() && !gameManager.isTutorial())
        {
            combat_UI.showStars();
            ATKstars.Play();
            _player.incrementAdrenaline(_player.GetAdrenalineFactor());
        }
    }
    void activateFeedback()
    {
        vFeedback.SetActive(true);
        foreach (Transform child in vFeedback.transform)
        {
            child.transform.GetComponent<Image>().DOFade(.25f, 0);
            child.transform.DOScale(1.6f, 0.2f).SetEase(Ease.InBack);
        }
    }
    void zoomCameraIn()
    {
        MainCamera.transform.DOLocalMove(new Vector3(3.3f, -1, -10), 3f);
        MainCamera.DOFieldOfView(35, 3f);
    }

    // Getters and Setters
    public GameObject GetAttackFeedback()
    {
        return vFeedback;
    }
}
