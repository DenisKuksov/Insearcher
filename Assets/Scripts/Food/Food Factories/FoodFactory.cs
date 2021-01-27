using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FoodFactory : MonoBehaviour
{

    [SerializeField] protected FoodSpawn[] SpawnVariations;
    [SerializeField] protected FoodIdle[] IdleVariations;
    [SerializeField] protected FoodConsume[] ConsumeVariations;
    [SerializeField] protected FoodCollide[] CollideVariations;

    [SerializeField] Food _foodPrefab;
    [SerializeField] int _poolAmount;
    protected Queue<Food> _foodPool;

    private void Awake()
    {
        InitPool();
    }

    private void InitPool()
    {
        _foodPool = new Queue<Food>(_poolAmount);

        Food instance = null;

        for (int i = 0; i < _poolAmount; i++)
        {
            instance = Instantiate(_foodPrefab); 
            instance.gameObject.SetActive(false);
            _foodPool.Enqueue(instance);
        }
    }

    public void AddToPool(Food food)
    {
        _foodPool.Enqueue(food);
    }


    public Food Create()
    {
        Food instance = _foodPool.Dequeue();
        instance.IsSpawned = false;
        SetFoodParams(instance);
        _foodPool.Enqueue(instance);

        return instance;
    }

    protected abstract void SetFoodParams(Food food);
}
