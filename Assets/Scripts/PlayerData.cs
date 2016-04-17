using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class PlayerData
{
    public PlayerData()
    {
    }

    public PlayerData(PlayerData original)
    {
        MapPosition = original.MapPosition;
        MaxHealth = original.MaxHealth;
        CurrentHealth = original.CurrentHealth;
        BaseDamage = original.BaseDamage;
        PlayerTemplateName = original.PlayerTemplateName;
    }

    public Vector2 MapPosition;

    public float MaxHealth;

    public float CurrentHealth;

    public float BaseDamage;

    public string PlayerTemplateName;
}
