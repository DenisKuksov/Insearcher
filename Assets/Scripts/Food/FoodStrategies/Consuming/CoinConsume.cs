using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinConsume : FoodConsume
{
    [SerializeField] int _reward;
    [SerializeField] AnimationCurve _consumeCurve;
    [SerializeField] float _speed;

    public override void Consume(Food food, PlayerData playerData)
    {
        StartCoroutine(ConsumeMe(food, playerData));
    }

    private IEnumerator ConsumeMe(Food me, PlayerData playerData)
    {

        float t = 0f;

        while (t < 1f)
        {
            me.ConsumedPersentage = _consumeCurve.Evaluate(t);
            me.NowMass = (1f - me.ConsumedPersentage) * me.MaxMass;

            t += Time.deltaTime * _speed;
            yield return 0;
        }

        playerData.NowConsuming -= 1;
        playerData.InConsuming.Remove(me);

        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + _reward);
        Debug.Log(PlayerPrefs.GetInt("Coins", 0));
    }
}
