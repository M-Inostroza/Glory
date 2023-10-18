using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class cameraManager : MonoBehaviour
{
    private static MMFeedback chromaticBeat;
    private static Transform _transformRef;

    Animator myAnim;
    private void Start()
    {
        _transformRef = transform;
        chromaticBeat = FindObjectOfType<MMFeedback>();
        myAnim = GetComponent<Animator>();
    }
    public static void playChrome()
    {   
        chromaticBeat.Play(_transformRef.position);
    }
    public void deactivateAnimator()
    {
        myAnim.Play("Idle");
        myAnim.enabled = false;
    }
}
