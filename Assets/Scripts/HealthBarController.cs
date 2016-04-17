using System;
using UnityEngine;
using System.Collections;

public class HealthBarController : MonoBehaviour
{
    public PlayerBattleController Player;
    public EnemyController Enemy;

    private IUnitWithHealth _unit;

	// Use this for initialization
	void Start ()
	{
	    TrackEnemyOrPlayer();
	    UpdateHealthBar();

	    _unit.OnHealthChanged += HandleHealthChange;
	}

    private void TrackEnemyOrPlayer()
    {
        if (Player == null)
        {
            if (Enemy != null)
            {
                _unit = Enemy;
            }
            else
            {
                Debug.LogError("Health Bar not attached to Enemy or Player, will not function properly.");
            }
        }
        else
        {
            if (Enemy != null)
            {
                Debug.LogError("Health Bar should NOT be attached to both an Enemy and a Player. Defaulting to Player.");
            }

            _unit = Player;
        }
    }

    private void UpdateHealthBar()
    {
        transform.localScale = new Vector3(_unit.NormalizedHealth, 1f, 1f);
    }

    private void HandleHealthChange(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    // Update is called once per frame
	void Update () {
	
	}
}