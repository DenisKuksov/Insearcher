using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private float _fadeInSpeed;
    [SerializeField] private AnimationCurve _fadeInCurve;

    [SerializeField] private float _fadeOutSpeed;
    [SerializeField] private AnimationCurve _fadeOutCurve;

    [SerializeField] private CanvasGroup[] _dialogs;

    void Start()
    {
        StartCoroutine(TutorialCoroutine());
    }
    
    IEnumerator ChangeDialogState(CanvasGroup cg, AnimationCurve curve, float speed, Action end = null)
    {
        for (float t = 0f; t < 1f; t += Time.deltaTime * speed)
        {
            cg.alpha = curve.Evaluate(t);
            yield return 0;
        }

        end?.Invoke();
    }

    IEnumerator TutorialCoroutine()
    {
        PlayerPrefs.SetInt("TutorialsFinished", 100500);
        StartCoroutine(ChangeDialogState(_dialogs[0], _fadeInCurve, _fadeInSpeed));
        yield return new WaitForSeconds(8f);
        StartCoroutine(ChangeDialogState(_dialogs[0], _fadeOutCurve, _fadeOutSpeed,
            () => StartCoroutine(ChangeDialogState(_dialogs[1], _fadeInCurve, _fadeInSpeed))));
        yield return new WaitForSeconds(10f);
        StartCoroutine(ChangeDialogState(_dialogs[1], _fadeOutCurve, _fadeOutSpeed,
            () => StartCoroutine(ChangeDialogState(_dialogs[2], _fadeInCurve, _fadeInSpeed))));
        yield return new WaitForSeconds(10f);
        StartCoroutine(ChangeDialogState(_dialogs[2], _fadeOutCurve, _fadeOutSpeed));

    }
}
