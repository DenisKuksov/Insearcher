using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitAfterCollision : FoodCollide
{
    [SerializeField] AnimationCurve _fadeCurve;
    [SerializeField] AnimationCurve _moveCurve;
    [SerializeField] ParticleSystem _particles;
    [SerializeField] float _speed;

    [SerializeField] float _minR;
    [SerializeField] float _maxR;

    [SerializeField] int _minAmount;
    [SerializeField] int _maxAmount;

    [SerializeField] OnlyMassFactory _massFactory;
    [SerializeField] Food _foodPrefab;

    [SerializeField] int _minSplitMass;

    public override void Collide(Food food)
    {
        if (food.NowMass >= _minSplitMass)
        {
            SplitCrash(food);
        }

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
    }

    private void SplitCrash(Food food)
    {
        int amount = Random.Range(_minAmount, _maxAmount);
        for (int i = 0; i < amount; i++)
        {
            food.IsSpawned = false;

            Food inst = _massFactory.Create(food.FoodSpriteRenderer.color, food.MaxMass / amount);

            inst.SpawnFood = food.SpawnFood;
            inst.CollidingFood = food.CollidingFood;
            inst.ConsumingFood = food.ConsumingFood;

            inst.transform.position = food.transform.position;
            inst.gameObject.SetActive(true);
            inst.Spawn();

            StartCoroutine(MoveRandomly(inst));
        }
    }

    private IEnumerator MoveRandomly(Food food)
    {
        float a = food.transform.position.x;
        float b = food.transform.position.y;

        float r = Random.Range(_minR, _maxR);
        float angle = Random.value * Mathf.PI * 2f;

        float x = Mathf.Cos(angle) * r + a;
        float y = Mathf.Sin(angle) * r + b;

        Vector3 target = new Vector3(x, y, 1f);
        Vector3 start = food.transform.position;
        float t = 0f;
        while (t < 1f)
        {
            food.transform.position = start + _moveCurve.Evaluate(t) * (target - start);
            t += Time.deltaTime * _speed;
            yield return 0;
        }

        food.IsSpawned = true;

    }

}
