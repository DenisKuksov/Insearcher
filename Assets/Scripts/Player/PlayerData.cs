using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData : MonoBehaviour
{
    public UnityEvent PointsChanged;

    private float _points = 1f;
    public float Points
    {
        get
        {
            return _points;
        }
        set
        {
            _points = value;
            PointsChanged.Invoke();
        }
    }

    [Range(0f,1f)]
    public float Strictness;
    [Range(1f, 4f)]
    public float Steapness;

    public float Mass;
    public float Sensitivity;
    public float MoveSpeed;
    public float ConsumingSpeed;
    public float DrawConusmingLineSpeed;
    public AnimationCurve LineDrawingCurve;

    public int MaxConsumingAmount;
    public int NowConsuming;

    public bool isMoving;

    public SpriteRenderer PlayerSpriteRenderer;

    public ColorWorker CW;
    public ColorMath CM;
    public List<Food> InConsuming;

    public Color32 ColorWithFullConsumed;

    private void Start()
    {
        PlayerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        CW = new ColorWorker();
        CM = new ColorMath();
    }

}
