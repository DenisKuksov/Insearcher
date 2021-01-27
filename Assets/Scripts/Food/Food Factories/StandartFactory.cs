using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartFactory : FoodFactory
{
    [Range(0,1f)]
    [SerializeField] private float _coinChance;

    [SerializeField] private Sprite _circleSprite;
    [SerializeField] private Sprite _coinSprite;


    private ColorWorker _cw;
    protected override void SetFoodParams(Food food)
    {
        if (_coinChance < Random.value)
        {
            food.LifeTime = Random.Range(64f, 128f);
            food.SpawnFood = SpawnVariations[0];
            food.ConsumingFood = ConsumeVariations[0];
            food.CollidingFood = CollideVariations[0];
            food.FoodSpriteRenderer.color = _cw.RandomizeColorBetween(50, 200);
            food.MaxMass = Random.Range(73, 110);
            food.NowMass = food.MaxMass;
            food.FoodSpriteRenderer.sprite = _circleSprite;
            food.NonConsumable = false;
        }
        else
        {
            Debug.Log(SpawnVariations);
            food.LifeTime = Random.Range(64f, 128f);
            food.SpawnFood = SpawnVariations[0];
            food.ConsumingFood = ConsumeVariations[1];
            food.CollidingFood = CollideVariations[1];
            food.FoodSpriteRenderer.color = _cw.RandomizeColorBetween(50, 200);
            food.MaxMass = Random.Range(73, 110);
            food.NowMass = food.MaxMass;
            food.FoodSpriteRenderer.sprite = _coinSprite;
            food.NonConsumable = true;
        }
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
