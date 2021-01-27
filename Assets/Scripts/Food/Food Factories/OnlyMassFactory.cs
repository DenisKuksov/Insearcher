using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyMassFactory : FoodFactory
{
    private ColorWorker _cw;
    protected override void SetFoodParams(Food food)
    {
        food.LifeTime = Random.Range(64f, 128f);
        food.SpawnFood = SpawnVariations[0];
        food.ConsumingFood = ConsumeVariations[0];
        food.CollidingFood = CollideVariations[0];
        food.FoodSpriteRenderer.color = _cw.RandomizeColorBetween(50, 200);
        food.MaxMass = Random.Range(73, 110);
        food.NowMass = food.MaxMass;
    }

    public Food Create(Color c, float mass)
    {
        Food instance = _foodPool.Dequeue();
        instance.IsSpawned = false;
        _foodPool.Enqueue(instance);

        instance.LifeTime = Random.Range(64f, 128f);
        instance.FoodSpriteRenderer.color = c;
        instance.MaxMass = mass;
        instance.NowMass = mass;

        return instance;
    }

    private void Start()
    {
        _cw = new ColorWorker();
    }

}
