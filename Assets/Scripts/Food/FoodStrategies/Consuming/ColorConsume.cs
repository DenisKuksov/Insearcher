using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorConsume : FoodConsume
{
    [SerializeField] AnimationCurve _consumeCurve;
    [SerializeField] float _speed;

    public override void Consume(Food food, PlayerData playerData)
    {
        StartCoroutine(ConsumeMe(food, playerData));
    }

    private IEnumerator ConsumeMe(Food me, PlayerData playerData)
    {

        float t = 0f;
        Color delta = playerData.CM.Substract( 
            playerData.CM.MiddleColorWithWeights
            (
                playerData.PlayerSpriteRenderer.color,
                me.FoodSpriteRenderer.color,
                me.NowMass / (me.NowMass + playerData.Mass)
            ),
            playerData.PlayerSpriteRenderer.color
            );

        float last = 0f;
        
        while(t < 1f)
        {
            me.ConsumedPersentage = _consumeCurve.Evaluate(t);
            me.NowMass = (1f - me.ConsumedPersentage) * me.MaxMass;

            t += Time.deltaTime * _speed;

            playerData.PlayerSpriteRenderer.color =
                playerData.CM.Add
                (
                    playerData.PlayerSpriteRenderer.color,
                    playerData.CM.Substract(playerData.CM.Multiply(delta, _consumeCurve.Evaluate(t)), playerData.CM.Multiply(delta, last))
                );

            last = _consumeCurve.Evaluate(t);

            yield return 0;
        }

        playerData.NowConsuming -= 1;
        playerData.InConsuming.Remove(me);
    }
}
