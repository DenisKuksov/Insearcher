using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    const float NORMAL_MASS = 100f;
    public float MaxMass; // but stores ints (float for valid calculation)
    public float NowMass; // but stores ints (float for valid calculation)
    public float ConsumedPersentage;
    [SerializeField] AnimationCurve _sizeCurve;

    public SpriteRenderer FoodSpriteRenderer;

    private Vector3 _scaleVector;
    private float _scale;
    private bool _isInConsumingProcess;

    public FoodSpawn SpawnFood;
    public FoodIdle IdleFood;
    public FoodConsume ConsumingFood;
    public FoodCollide CollidingFood;

    public bool IsSpawned;
    public bool NonConsumable;

    public float LifeTime { get; set; }

    public void Spawn()
    {
        ConsumedPersentage = 0f;
        _isInConsumingProcess = false;
        SpawnFood.Spawn(this);

        StartCoroutine(WaitUntilEndOfTheLifeTime());
    }

    public void Idle()
    {
        IdleFood.Idle(this);
    }

    public void Consume(PlayerData playerData)
    {
        _isInConsumingProcess = true;
        ConsumingFood.Consume(this, playerData);
    }

    private void Start()
    {
        NowMass = MaxMass;
        FoodSpriteRenderer = GetComponent<SpriteRenderer>();

        _scaleVector = Vector3.one;
    }

    private void Update()
    {
        if (IsSpawned)
        {
            _scale = _sizeCurve.Evaluate(NowMass / MaxMass) * MaxMass / NORMAL_MASS;
            _scaleVector.Set(_scale, _scale, 1f);
            transform.localScale = _scaleVector;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && IsSpawned)
        {
            CollidingFood.Collide(this);
        }
    }

    private IEnumerator WaitUntilEndOfTheLifeTime()
    {
        yield return new WaitForSeconds(LifeTime);
        yield return new WaitUntil(() => !_isInConsumingProcess);

        // fade out
        IsSpawned = false;

        float t = 1f;
        Color _setColor = FoodSpriteRenderer.color;

        while(t > 0f)
        {
            _setColor.a = _sizeCurve.Evaluate(t);
            FoodSpriteRenderer.color = _setColor;
            t -= Time.deltaTime;
            yield return 0;
        }

        gameObject.SetActive(false);

    }
}
