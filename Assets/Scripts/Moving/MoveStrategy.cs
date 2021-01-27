using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveStrategy : MonoBehaviour
{
    [SerializeField] protected AnimationCurve _curve;
    public abstract void MoveTo(Vector3 endPoint, Transform target, PlayerData playerData, int cost);
}
