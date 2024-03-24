using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class dirtToss : MonoBehaviour
{
    private bool speedReduced;
    public bool IsDirty { get; private set; }

    [SerializeField]
    private Camera mainCam;

    private Player Player;
    private Enemy Enemy;
    private SpriteRenderer dirtTexture;
    private TutorialManager TutorialManager;

    // Dirt texture opacity
    private float maxOpacity = 0.8f;
    private float opacity;

    // Scratch's speed threshold
    float scratchSpeedThreshold = 10000f;

    // Percentage of maximum opacity to reduce per scratch
    float opacityReductionPerScratch = 0.003f;
    private Vector3 prevMousePos;

    [SerializeField] Slider _dirtFeedback;
    [SerializeField] Image _sliderFill;
    [SerializeField] Image _sliderContainer;

    private void Start()
    {
        dirtTexture = GetComponent<SpriteRenderer>();
        Player = FindObjectOfType<Player>();
        Enemy = FindObjectOfType<Enemy>();
        TutorialManager = FindObjectOfType<TutorialManager>();
    }

    private void OnEnable()
    {
        if (!gameManager.isTutorial())
        {
            CombatManager.move_Inputs_out();
        }
        FadeFeedback(true);
        mainCam.DOShakePosition(0.3f, 0.3f, 20, 10);
        speedReduced = false;
        IsDirty = true;
        opacity = maxOpacity;
        StartCoroutine(deactivateTimer(10));
    }

    private void Update()
    {
        makeItDirt();
        UpdateFeedback();
    }
    private void OnDisable()
    {
        TutorialManager._canClick = true;
        if (gameManager.isTutorial())
        {
            TutorialManager.fadeTimer(1);
            if (TutorialManager._hasPlayedTutorial)
            {
                TutorialManager.showAllInput(1);
            }
        }
        Player.incrementBaseSpeed(1000);
        speedReduced = false;
    }
    private void makeItDirt()
    {
        dirtTexture.color = new Color(1f, 1f, 1f, opacity);
        transform.DOMoveY(transform.position.y - 0.005f, 0.2f);

        if (!speedReduced)
        {
            Player.reduceBaseSpeed(1000);
            speedReduced = true;
        }
        
        if (opacity > 0f && Input.GetMouseButton(0) && IsDirty && !BattleSystem.IsPaused)
        {
            // Mouse position to world coordinates
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Detect if the mouse is over the dirt texture
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

            // Check if the collider belongs to the dirt texture
            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                // Calculate the scratch speed based on the delta of the mouse input
                float scratchSpeed = Input.GetMouseButton(0) ? (Input.mousePosition - prevMousePos).magnitude / Time.deltaTime : 0f;
                prevMousePos = Input.mousePosition;

                // Check if the scratch speed is greater than the threshold
                if (scratchSpeed >= scratchSpeedThreshold)
                {
                    float opacityReductionPercentage = scratchSpeed / scratchSpeedThreshold * opacityReductionPerScratch;

                    opacity -= maxOpacity * opacityReductionPercentage;
                }
            }
            if (opacity <= 0)
            {
                if (gameManager.isTutorial())
                {
                    TutorialManager.dirtDetailTutorial(2);
                }
                IsDirty = false;
                FadeFeedback(false);
                gameObject.SetActive(false);
                CombatManager.move_Inputs_in();
            }
        }
    }

    void UpdateFeedback()
    {
        _dirtFeedback.value = opacity * 100;
    }

    void FadeFeedback(bool inOut)
    {
        if (inOut)
        {
            _dirtFeedback.gameObject.SetActive(true);
            _sliderContainer.DOFade(1, 0.2f);
            _sliderFill.DOFade(1, 0.3f);
        } else
        {
            _sliderFill.DOFade(0, 0.3f);
            _sliderContainer.DOFade(0, 0.2f).OnComplete(()=> _dirtFeedback.gameObject.SetActive(false));
        }
    }

    IEnumerator deactivateTimer(int time)
    {
        yield return new WaitForSeconds(time);
        DOTween.To(() => opacity, x => opacity = x, 0, 0.5f);
        IsDirty = false;
        if (gameManager.isTutorial())
        {
            TutorialManager.dirtDetailTutorial(2);
        }
        yield return new WaitForSeconds(0.5f);
        CombatManager.move_Inputs_in();
        gameObject.SetActive(false);
    }
}
