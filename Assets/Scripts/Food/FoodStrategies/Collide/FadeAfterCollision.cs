using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAfterCollision : FoodCollide
{
    [SerializeField] int _reward;
    [SerializeField] AnimationCurve _fadeCurve;
    [SerializeField] float _speed;


    public override void Collide(Food food)
    {
        StartCoroutine(FadeToZero(food));
    }

    private IEnumerator FadeToZero(Food food)
    {
        food.IsSpawned = false;
        float t = 1f;
        Vector3 startScale = food.transform.localScale;

        while (t > 0f)
        {
            food.transform.localScale = _fadeCurve.Evaluate(t) * startScale;
            t -= Time.deltaTime * _speed;
            yield return 0;
        }

        food.transform.localScale = Vector3.forward;
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + _reward);
        Debug.Log(PlayerPrefs.GetInt("Coins", 0));
    }

}
