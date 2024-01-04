using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class cameraManager : MonoBehaviour
{
    [SerializeField] Volume _volume;
    UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration;
    Animator myAnim;
    private void Start()
    {
        myAnim = GetComponent<Animator>();
    }
    public void playChrome()
    {
        if (_volume == null)
        {
            Debug.LogError("Global volume is not assigned!");
            return;
        }

        // Try to get the ChromaticAberration override from the global volume
        if (_volume.profile.TryGet(out chromaticAberration))
        {
            // Modify chromatic aberration properties
            chromaticAberration.intensity.Override(1);
            chromaticAberration.active = true;
            DOTween.To(() => chromaticAberration.intensity.value,
                       x => chromaticAberration.intensity.Override(x),
                       0, 1.0f)
                .SetEase(Ease.OutQuad);
        }
        else
        {
            Debug.LogError("Chromatic Aberration not found in the global volume!");
        }
    }
    public void deactivateAnimator()
    {
        myAnim.Play("Idle");
        myAnim.enabled = false;
    }
}
