using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using AssetKits.ParticleImage;

public class TargetManager : MonoBehaviour
{
    /*
     - 0 -> attack head
     - 1 -> attack mid
     - 2 -> attack bottom
    */

    public GameObject[] targets;
    public List<int> attackOrder = new List<int>();

    private BattleSystem BattleSystem;
    private Camera MainCamera;
    private Combat_UI combat_UI;

    [SerializeField] SpriteRenderer courtain;
    [SerializeField] GameObject vFeedback;

    [SerializeField] ParticleImage ATKstars;
    
    private void Start()
    {
        BattleSystem = FindObjectOfType<BattleSystem>();
        MainCamera = FindObjectOfType<Camera>();
        combat_UI = FindObjectOfType<Combat_UI>();
    }

    public void attack()
    {
        combat_UI.move_UI_out();
        activateFeedback();

        BattleSystem.targetHit = 0;

        courtain.DOColor(new Color(0, 0, 0, .5f), 1f);
        StartCoroutine(activateTargets());
        zoomCameraIn();
    }

    IEnumerator activateTargets()
    {
        var targets = this.targets;
        
        for (int i = 0; i < 3; i++)
        {
            targets[i].transform.DOScale(1, 0.1f);
            targets[i].GetComponent<SpriteRenderer>().DOFade(1, 0);
            targets[i].SetActive(true);

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
        yield return new WaitForSeconds(1.5f);

        foreach (var target in targets)
        {
            target.transform.DOKill();
            target.transform.DOScale(0, 0.05f).OnComplete(() => target.SetActive(false));
        }

        courtain.DOColor(new Color(0, 0, 0, 0), .5f);
        combat_UI.move_UI_in();
    }

    public void checkCritic()
    {
        if (BattleSystem.targetHit == 3)
        {
            combat_UI.showStars();
            ATKstars.Play();
        }
    }
    void activateFeedback()
    {
        vFeedback.SetActive(true);
        foreach (Transform child in vFeedback.transform)
        {
            child.transform.GetComponent<Image>().DOFade(.25f, 0);
            child.transform.DOScale(1, 0.3f).SetEase(Ease.InBack);
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
