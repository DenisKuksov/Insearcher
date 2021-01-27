using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FoodConsume : MonoBehaviour
{
    public abstract void Consume(Food food, PlayerData playerData);
}
