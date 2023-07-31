using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class cameraManager : MonoBehaviour
{
    [SerializeField] MMFeedback chromaticBeat;

    Animator myAnim;
    private void Start()
    {
        myAnim = GetComponent<Animator>();
    }
    public void playChrome()
    {   
        chromaticBeat.Play(transform.position);
    }
    public void deactivateAnimator()
    {
        myAnim.Play("Idle");
        myAnim.enabled = false;
    }
}
