using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class cameraManager : MonoBehaviour
{
    [SerializeField] MMFeedback chromaticBeat;

    public void playChrome()
    {
        chromaticBeat.Play(transform.position);
    }
}
