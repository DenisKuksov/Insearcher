using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMath
{
    private Color _setColor;
    private Gradient _gradient;

    private GradientColorKey[] _colorKeys;
    private GradientAlphaKey[] _alphaKeys; 

    private readonly Color _BLACK;

    public ColorMath()
    {
        _setColor = new Color();
        _setColor.a = 1f;
        _gradient = new Gradient();

        _colorKeys = new GradientColorKey[2];
        _alphaKeys = new GradientAlphaKey[2];

        for (int i = 0; i < 2; i++)
        {
            _colorKeys[i].time = i;
            _alphaKeys[i].time = i;
            _alphaKeys[i].alpha = 1f;
        }

        _gradient.SetKeys(_colorKeys, _alphaKeys);

        _BLACK = new Color(0, 0, 0, 1f);
    }

    public Color MiddleColorWithWeights(Color color1, Color color2, float wayFromColor1)
    {
        _colorKeys[0].color = color1;
        _colorKeys[1].color = color2;

        _gradient.SetKeys(_colorKeys, _alphaKeys);
        
        return _gradient.Evaluate(wayFromColor1);
    }

    public Color MiddleColorWithWeights(Color[] colors, float[] weights)
    {
        float n = colors.Length;
        _setColor = _BLACK;

        float coef = 0;

        for (int i = 0; i < n; i++)
        {
            coef = weights[i] / n;

            _setColor.r += colors[i].r * coef;
            _setColor.g += colors[i].g * coef;
            _setColor.b += colors[i].b * coef;
        }

        return _setColor;
    }


    public Color Add(Color c1, Color c2)
    {
        _setColor.r = c1.r + c2.r;
        _setColor.g = c1.g + c2.g;
        _setColor.b = c1.b + c2.b;

        return _setColor;
    }

    public Color Substract(Color c1, Color c2)
    {
        _setColor.r = c1.r - c2.r;
        _setColor.g = c1.g - c2.g;
        _setColor.b = c1.b - c2.b;

        return _setColor;
    }

    public Color Multiply(Color c, float coef)
    {
        _setColor.r = c.r * coef;
        _setColor.g = c.g * coef;
        _setColor.b = c.b * coef;

        return _setColor;
    }

}
