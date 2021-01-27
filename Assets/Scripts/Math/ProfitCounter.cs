using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfitCounter : MonoBehaviour
{
    ColorWorker _cw;

    [SerializeField] int _minColorValue;
    [SerializeField] int _maxColorValue;

    void Start ()
    {
        _cw = new ColorWorker();
        print(_cw.CalculateInversedColor(new Color32(100, 90, 120, 255)));
	}

    public int CalculateProfit(Color32 chosenColor, Color32 ourColor)
    {
        int distance = _cw.CalculateColorDistance(chosenColor, ourColor);
        return 3 * (_maxColorValue - _minColorValue) - distance;

    }
	
}
