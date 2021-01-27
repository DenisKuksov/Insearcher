using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] SpriteRenderer _spriteRenderer;

    [SerializeField] AnimationCurve _moveCurve;
    [SerializeField] float _moveSpeed;

    [SerializeField] float _standartScale;

    public bool InShot { get; private set; }

    public Color Color
    {
        get { return _spriteRenderer.color; }
        set
        {
            var main = _particleSystem.main;
            main.startColor = value;

            _spriteRenderer.color = value;
        }
    }

    public void ThrowIn(Transform target, Vector3 startPos, System.Action afterFinishing)
    {
            StartCoroutine(SmoothlyThrowIn(target, startPos, afterFinishing));
    }

    private IEnumerator SmoothlyThrowIn(Transform target, Vector3 startPos, System.Action afterFinishing)
    {
        _spriteRenderer.enabled = true;
        InShot = true;

        transform.localScale = Vector3.one * _standartScale;
        _particleSystem.Pause();
        transform.position = startPos;
        _particleSystem.Play();

        float t = 0f;
        Vector3 delta = target.position - startPos;

        while (t <= 1f)
        {
            transform.position = startPos + delta * _moveCurve.Evaluate(t);
            t += Time.deltaTime * _moveSpeed;
            yield return 0;
        }

        _spriteRenderer.enabled = false;
        InShot = false;
        afterFinishing();
    }
}
