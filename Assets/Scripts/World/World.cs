using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class World : MonoBehaviour
{
    [SerializeField] FoodFactory _foodFactory;
    [SerializeField] FoodFactory _coinsFactory;
    [SerializeField] RandomPointsGenerator _rpg;
    [SerializeField] float _generationZoneScale;
    [SerializeField] float _minSpawnRadius;
    [SerializeField] float _minSpawnRadiusToPlayer;
    [SerializeField] float _spawnRadius;

    [SerializeField] WorldData _worldData;
    [SerializeField] PlayerData _playerData;

    [SerializeField] int _startFoodAmount;

    private Vector3 _currentCenter;
    private Vector3 _generatedPos;

    private void Start()
    {
        Init();
        SpawnNewZone(_startFoodAmount, 0f, 0f);
        StartCoroutine(SpawnNewCycle());
    }

    public void SpawnNewZone(int foodAmount, float x, float y)
    {
        _rpg.SetCenter(x, y);

        for (int i = 0; i < foodAmount; i++)
        {
            Food f = _foodFactory.Create();
            //_generatedPos = _playerData.transform.position;

            //while (Mathf.Sqrt(Mathf.Pow(_generatedPos.x - _playerData.transform.position.x, 2f)
            //    + Mathf.Pow(_generatedPos.y - _playerData.transform.position.y, 2f)) < _playerData.transform.localScale.x + _minSpawnRadius)
            //{
            //    _generatedPos = _rpg.GeneratePointPosition();
            //}

            bool validPos = false;
            bool thisIterValid;
            int tryes = 0;

            Food[] alredySpawned = _worldData.AllConsumerableObjects.Where(food => food.gameObject.activeSelf).ToArray();

            while (!validPos && tryes < 64)
            {
                _generatedPos = _rpg.GeneratePointPosition();

                thisIterValid = true;
                foreach (var obj in alredySpawned)
                {
                    if (Mathf.Sqrt(Mathf.Pow(_generatedPos.x - obj.transform.position.x, 2f)
                + Mathf.Pow(_generatedPos.y - obj.transform.position.y, 2f)) < obj.transform.localScale.x + _minSpawnRadius)
                    {
                        thisIterValid = false;
                    }
                }

                validPos = thisIterValid && Mathf.Sqrt(
                    Mathf.Pow(_generatedPos.x - _playerData.transform.position.x, 2f) + 
                    Mathf.Pow(_generatedPos.y - _playerData.transform.position.y, 2f)) 
                    >= _playerData.transform.localScale.x + _minSpawnRadiusToPlayer;

                tryes += 1;
            }

            if (tryes < 64)
            {

                _worldData.AllConsumerableObjects.Add(f);
                f.transform.position = _generatedPos;

                f.gameObject.SetActive(true);
                f.Spawn();
            }
            else
                print("Too densely");
        }
    }

    private IEnumerator SpawnNewCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(_worldData.ObjectsSpawnDelay);
            SpawnNewZone(1, _playerData.transform.position.x, _playerData.transform.position.y);
            _worldData.ObjectsSpawnDelay = 0.25f + _worldData.AllConsumerableObjects.Where(f => f.IsSpawned && 
                Vector3.Distance(f.transform.position, _playerData.transform.position) < _spawnRadius).Count() / 32f;
        }
    }

    private void Init()
    {
        _currentCenter = Vector3.forward;

        _rpg.Height = _worldData.GenerationHeight * _generationZoneScale;
        _rpg.Width = _worldData.GenerationWidth * _generationZoneScale;
        _rpg.Init();
        _rpg.SetCenter(_currentCenter.x, _currentCenter.y);
    }

}
