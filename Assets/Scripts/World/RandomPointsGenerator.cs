using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPointsGenerator : MonoBehaviour
{
    [HideInInspector] public float Width;
    [HideInInspector] public float Height;

    private float _a;
    private float _b;

    private float _h;
    private float _k;

    private float _rand_x;
    private float _min_y;
    private float _max_y;
    private float _rand_y_in_rect;

    private Vector3 _generatedPos;

    public void Init()
    {
        _a = Height / 2f;
        _b = Width  / 2f;
    }

    public void SetCenter(float h, float k)
    {
        _h = h;
        _k = k;
    }

    public Vector3 GeneratePointPosition()
    {
        bool found = false;
        while (!found)
        {
            _rand_x = Random.Range(_h - _b, _h + _b);
            _min_y = -_a / _b * Mathf.Sqrt(Mathf.Pow(_b, 2f) - Mathf.Pow(_rand_x - _h, 2f)); // + k but for better code made after defining *
            _max_y = -_min_y + _k;
            _min_y += _k; // *

            _rand_y_in_rect = Random.Range(_k - _a, _k + _a);

            if (_rand_y_in_rect >= _min_y && _rand_y_in_rect <= _max_y)
            {
                _generatedPos.Set(_rand_x, _rand_y_in_rect, 1f);
                found = true;
            }
        }

        return _generatedPos;
    }
}
 