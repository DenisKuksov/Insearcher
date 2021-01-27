using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeController : MonoBehaviour
{
    [System.Serializable]
    enum GameMode { Classic, Hardcore, Meditation }

    [SerializeField] GameMode _gameMode;

    [SerializeField] PlayerControl _playerControl;
    [SerializeField] PlayerData _playerData;
    [SerializeField] PointsUIController _pointsUIController;

    [SerializeField] CanvasGroup _losePanel;
    [SerializeField] Image[] _colorDependent;
    [SerializeField] AnimationCurve _fadeCurve;

    private float _exhaustionAcceleration;

    public void SetClassic()
    {
        _playerControl.StartPointsAmount = 64;
        _exhaustionAcceleration = 1f;
        StartCoroutine(Hunger(0.9f));
        StartCoroutine(ClassicEnding());
    }

    public void SetHardcore()
    {
        _playerControl.StartPointsAmount = 72;
        _exhaustionAcceleration = 0.997f;
        StartCoroutine(Hunger(1f));
        _playerControl.MoveCost = 8;
        StartCoroutine(ClassicEnding());
    }

    private IEnumerator Hunger(float delay)
    {
        while (true)
        {
            delay *= _exhaustionAcceleration;
            yield return new WaitForSeconds(delay);
            _playerData.Points--;
        }
    }

    private IEnumerator ClassicEnding()
    {
        bool lost = false;
        yield return new WaitUntil(() => int.Parse(_pointsUIController._pointsText.text) <= 0f && !lost && _playerData.Points <= 0f);
        lost = true;
        SetUpLosePanel();
    }

    private void SetUpLosePanel()
    {
        foreach (var e in _colorDependent)
        {
            e.color = _playerData.PlayerSpriteRenderer.color;
        }

        _losePanel.gameObject.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine(FadeInLosePanel(1f));
    }

    private IEnumerator FadeInLosePanel(float speed) 
    {
        float t = 0f;

        while (t <= 1f)
        {
            _losePanel.alpha = _fadeCurve.Evaluate(t);
            t += Time.unscaledDeltaTime * speed;
            yield return 0;
        }
    }



    private void Awake()
    {
        if (_gameMode == GameMode.Classic)
            SetClassic();
        else if (_gameMode == GameMode.Hardcore)
            SetHardcore();

    }

}
