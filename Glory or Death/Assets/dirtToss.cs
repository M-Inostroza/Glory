using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class dirtToss : MonoBehaviour
{
    bool isDirty;
    bool speedReduced;

    [SerializeField]
    private Camera mainCam;

    [SerializeField]
    private BattleSystem BS;
    private Player player;

    // Maximum opacity of the dirt texture
    public float maxOpacity = 0.5f;

    // Speed threshold for scratch detection
    public float scratchSpeedThreshold = 10000f;

    // Percentage of maximum opacity to reduce per scratch
    public float opacityReductionPerScratch = 0.05f;

    private SpriteRenderer spriteRenderer;
    private float opacity;

    private Vector3 prevMousePos;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        mainCam.DOShakePosition(0.3f, 0.3f, 20, 10);
        speedReduced = false;
        isDirty = true;
        opacity = maxOpacity;
        BS.SetCanClick(false);
    }

    private void Update()
    {
        makeItDirt();
    }
    private void OnDisable()
    {
        player.baseSpeed += 2;
        speedReduced = false;
    }
    private void makeItDirt()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, opacity);
        

        if (!speedReduced)
        {
            player.baseSpeed -= 2;
            speedReduced = true;
        }
        // Check if the left mouse button is pressed and over the dirt texture
        if (opacity > 0f && Input.GetMouseButton(0) && isDirty)
        {
            // Convert the mouse position to world coordinates
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Detect if the mouse is over the dirt texture by checking for a collider at the mouse position
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
                isDirty = false;
                gameObject.SetActive(false);
                BS.SetCanClick(true);
            }
        }
    }
}
