using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class cameraManager : MonoBehaviour
{
    [SerializeField] Volume _volume;
    UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration;
    UnityEngine.Rendering.Universal.Bloom bloom;
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

    public void playBloom(int state)
    {
        // 1 Bright Bloom
        // 2 Red Bloom
        if (_volume == null)
        {
            Debug.LogError("Global volume is not assigned!");
            return;
        }

        // Try to get the bloom override from the global volume
        if (_volume.profile.TryGet(out bloom))
        {
            switch (state)
            {
                case 1: //FFFAB1
                    bloom.tint.Override(HexToColor("#FFFAB1"));
                    bloom.intensity.Override(10);
                    bloom.active = true;
                    DOTween.To(() => bloom.intensity.value,
                               x => bloom.intensity.Override(x),
                               0, 1.0f)
                        .SetEase(Ease.OutQuad);
                    break;
                case 2:
                    bloom.tint.Override(HexToColor("#DD2E03"));
                    bloom.intensity.Override(10);
                    bloom.active = true;
                    DOTween.To(() => bloom.intensity.value,
                               x => bloom.intensity.Override(x),
                               0, 1.0f)
                        .SetEase(Ease.OutQuad);
                    break;
                default:
                    break;
            }
        }
    }
    public void deactivateAnimator()
    {
        myAnim.Play("Idle");
        myAnim.enabled = false;
    }

    public static Color HexToColor(string hex)
    {
        Color color = Color.black;

        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }

        Debug.LogError("Invalid hex code: " + hex);
        return Color.white; // Default color in case of an error
    }
}
