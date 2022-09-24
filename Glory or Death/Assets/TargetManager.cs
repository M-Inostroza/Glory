using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject target;

    private void OnEnable()
    {
        target = Instantiate<GameObject>(target);
    }
}
