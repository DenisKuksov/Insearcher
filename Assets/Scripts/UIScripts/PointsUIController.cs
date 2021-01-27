using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsUIController : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;
    public Text _pointsText;
    [SerializeField] Image[] _colorDependent;

    [SerializeField] Text[] _rgbTexts;

    [SerializeField] float _delay;
    [SerializeField] float _speed;
    [SerializeField] AnimationCurve _changePointsCurve;

    private bool _coroutineStarted;
    private float _change;
    private float _last;

    private float _unrounded;

    private void Start()
    {
        _pointsText.text = _playerData.Points.ToString();
    }

    public void ChangePoints()
    {
        _change = _playerData.Points - _last;
        _last = _playerData.Points;


        StartCoroutine(SmoothPointsChange(_change));
    }

    private IEnumerator SmoothPointsChange(float change)
    {
        yield return new WaitForSeconds(_delay);

        float t = 0f;
        float last = 0f;

        while (t <= 1f)
        {
            _unrounded = (_unrounded + (_changePointsCurve.Evaluate(t) - last) * change);
            last = _changePointsCurve.Evaluate(t);
            t += Time.deltaTime * _speed;

            yield return 0;
        }
    }

    Color _setColor;
    private void LateUpdate()
    {
        _pointsText.text = Mathf.RoundToInt(_unrounded).ToString();
        _setColor = _playerData.PlayerSpriteRenderer.color;
        _pointsText.color = _setColor;

        foreach (var dependent in _colorDependent)
        {
            _setColor.a = dependent.color.a;
            dependent.color = _setColor;
        }

        _rgbTexts[0].text = Mathf.RoundToInt(_playerData.PlayerSpriteRenderer.color.r * 255f).ToString();
        _rgbTexts[1].text = Mathf.RoundToInt(_playerData.PlayerSpriteRenderer.color.g * 255f).ToString();
        _rgbTexts[2].text = Mathf.RoundToInt(_playerData.PlayerSpriteRenderer.color.b * 255f).ToString();
    }

}
