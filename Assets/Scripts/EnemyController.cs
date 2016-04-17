using System;
using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour, IUnitWithHealth {


    public float MaxHealth;
    public float BaseDamage;

    private float _normalizedHealth;
    private float _currentHealth;
    private Animator _animator;

    // Use this for initialization
    void Start ()
    {
        _currentHealth = MaxHealth;

        NormalizedHealth = _currentHealth / MaxHealth;

        _animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public float NormalizedHealth
    {
        get { return _normalizedHealth; }
        private set
        {
            if (_normalizedHealth != value)
            {
                _normalizedHealth = value;

                var handler = OnHealthChanged;

                if (handler != null)
                {
                    handler(this, new EventArgs());
                }
            }
        }
    }

    public event EventHandler<EventArgs> OnHealthChanged;

    public bool Defend(float rawDamage)
    {
        _animator.SetTrigger("EnemyDefend");

        _currentHealth -= rawDamage;

        NormalizedHealth = _currentHealth/MaxHealth;

        return _currentHealth > 0.0f;
    }

    public float Attack()
    {
        _animator.SetTrigger("EnemyAttack");

        return BaseDamage;
    }
}
