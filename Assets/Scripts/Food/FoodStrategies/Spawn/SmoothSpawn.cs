using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothSpawn : FoodSpawn
{
    [SerializeField] private AnimationCurve _spawnCurve;
    [SerializeField] private float _speed;

    private Vector3 setVector;

    public override void Spawn(Food food)
    {
        StartCoroutine(SmoothSpawnEnumerator(food));
    }

    private IEnumerator SmoothSpawnEnumerator(Food food)
    {
        float t = 0f;
        Vector3 startScale = food.transform.localScale;

        while (t < 1f)
        {
            food.transform.localScale = startScale * _spawnCurve.Evaluate(t);
            t += Time.deltaTime * _speed;
            yield return 0;
        }

        StartCoroutine(TransitToStartScale(food));
    }

    private IEnumerator TransitToStartScale(Food food)
    {
        float t = 0f;
        Vector3 startScale = food.transform.localScale;
        setVector.Set(food.MaxMass / 100f, food.MaxMass / 100f, 1f);
        Vector3 delta = setVector - startScale;

        while(t < 1f)
        {
            food.transform.localScale = startScale + delta * _spawnCurve.Evaluate(t);
            t += Time.deltaTime * _speed;
            yield return 0;
        }
        food.IsSpawned = true;
    }


}
