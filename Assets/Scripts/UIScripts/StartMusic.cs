using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
    private static bool _firstTime = true;
    void Start()
    {
        if (_firstTime)
        {
            AudioManager.instance.Play("Music");
            _firstTime = false;
        }
    }

}
