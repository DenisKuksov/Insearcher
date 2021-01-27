using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnevenRectilinearMotion : MoveStrategy
{
    private Vector3 _startPoint;

    public override void MoveTo(Vector3 endPoint, Transform target, PlayerData playerData, int cost)
    {
        StartCoroutine(Move(endPoint, target, playerData));
        playerData.Points -= cost;
    }

    private IEnumerator Move(Vector3 endPoint, Transform target, PlayerData playerData)
    {
        playerData.isMoving = true;

        float t = 0f;
        _startPoint = target.transform.position;

        while (t < 1f)
        {
            target.transform.position = _startPoint + _curve.Evaluate(t) * (endPoint - _startPoint);
            t += Time.deltaTime * playerData.MoveSpeed; // Mathf.Sqrt(Mathf.Pow(endPoint.x - _startPoint.x, 2) + Mathf.Pow(endPoint.y - _startPoint.y, 2));
            yield return 0;
        }

        playerData.isMoving = false;
    }
}
