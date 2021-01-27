using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] AnimationCurve _fadeCurve;
    [SerializeField] float speed;

    public void FadeInPause()
    {
        _canvasGroup.gameObject.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine(PausePanelAlphaFader(0f, speed));
    }

    public void FadeFromPause()
    {
        StartCoroutine(PausePanelAlphaFader(1f, speed));
    }

    public void PlayAgain(Image fadeImage)
    {
        StartCoroutine(FadePanel(fadeImage, SceneManager.GetActiveScene().buildIndex));
        Time.timeScale = 1f;
    }

    public void GoToMenu(Image fadeImage)
    {
        StartCoroutine(FadePanel(fadeImage, 0));
        Time.timeScale = 1f;
    }

    private IEnumerator FadePanel(Image panel, int sceneIndex)
    {
        float t = 0f;
        Color c = panel.color;
        while(t <= 1f)
        {
            c.a = _fadeCurve.Evaluate(t);
            panel.color = c;
            t += Time.unscaledDeltaTime * speed;
            yield return 0;
        }

        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator PausePanelAlphaFader(float startTime, float speed)
    {
        System.Func<float, bool> predicat = null;
        float delta = 0f;

        if (startTime == 0f)
        {
            predicat = _startTime => _startTime < 1f;
            delta = 1f;
        }
        else if (startTime == 1f)
        {
            predicat = _startTime => _startTime > 0f;
            delta = -1f;
        }

        while (predicat(startTime))
        {
            _canvasGroup.alpha = _fadeCurve.Evaluate(startTime);
            startTime += delta * Time.unscaledDeltaTime * speed;
            yield return 0;
        }

        if (Mathf.Round(startTime) == 0f)
        {
            Time.timeScale = 1f;
            _canvasGroup.gameObject.SetActive(false);
        }
    }
}
