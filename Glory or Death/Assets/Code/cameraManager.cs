using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class cameraManager : MonoBehaviour
{
    [SerializeField] MMFeedback chromaticBeat;

    timeManager timeManager;
    private void Start()
    {
        timeManager = FindObjectOfType<timeManager>();
    }
    public void playChrome()
    {   
        chromaticBeat.Play(transform.position);
        chromaticBeat.Play(transform.position);
    }
}
