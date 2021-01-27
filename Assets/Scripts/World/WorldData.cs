using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldData : MonoBehaviour
{
    public List<Food> AllConsumerableObjects;
    public Vector3 CenterOfGeneration;

    public float GenerationHeight { get; private set; }
    public float GenerationWidth { get; private set; }

    public float ObjectsSpawnDelay { get; set; }
    public int SpawnedFoodAmount { get { return AllConsumerableObjects.Where(f => f.IsSpawned).Count(); } }

    private void Awake()
    {
        GenerationHeight = Camera.main.orthographicSize * 2f;
        GenerationWidth = GenerationHeight * Screen.width / Screen.height;
    }
}
