using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class dirtToss : MonoBehaviour
{
    private bool isDirty, speedReduced;

    // How much the base speed will be reduced
    private float speedDebuff = 3f;

    [SerializeField]
    private Camera mainCam;

    private Player player;
    private Input_Manager Input_Manager;

    // Maximum opacity of the dirt texture
    public float maxOpacity = 0.5f;

    // Speed threshold for scratch detection
    public float scratchSpeedThreshold = 10000f;

    // Percentage of maximum opacity to reduce per scratch
    public float opacityReductionPerScratch = 0.05f;

    private SpriteRenderer dirtTexture;
    private float opacity;

    private Vector3 prevMousePos;

    private void Start()
    {
        Input_Manager = FindObjectOfType<Input_Manager>();
        dirtTexture = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        mainCam.DOShakePosition(0.3f, 0.3f, 20, 10).OnComplete(() => Input_Manager.SetCanClick(false));
        speedReduced = false;
        isDirty = true;
        opacity = maxOpacity;
    }

    private void Update()
    {
        makeItDirt();
    }
    private void OnDisable()
    {
        player.baseSpeed += speedDebuff;
        speedReduced = false;
    }
    private void makeItDirt()
    {
        dirtTexture.color = new Color(1f, 1f, 1f, opacity);
        transform.DOMoveY(transform.position.y - 0.005f, 0.2f);

        if (!speedReduced)
        {
            player.baseSpeed -= speedDebuff;
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
                Input_Manager.SetCanClick(true);
            }
        }
    }
}
