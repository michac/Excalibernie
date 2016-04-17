using System;
using UnityEngine;
using System.Collections;

public class PlayerBattleController : MonoBehaviour, IUnitWithHealth {

    private float _normalizedHealth;
    
    private PlayerData _player;
    private Animator _animator;

    void Awake()
    { 
        _animator = GetComponent<Animator>();

        var controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldController>();

        BindToData(controller.Player);
    }

    private void BindToData(PlayerData player)
    {
        _player = player;
    }

    // Use this for initialization
	void Start ()
	{
	    NormalizedHealth = _player.CurrentHealth / _player.MaxHealth;
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

    public float Attack()
    {
        _animator.SetTrigger("PlayerAttack");
        
        return _player.BaseDamage;
    }

    public bool Defend(float rawDamage)
    {
        _animator.SetTrigger("PlayerDefend");
        
        _player.CurrentHealth = Mathf.Clamp(_player.CurrentHealth - rawDamage, 0f, _player.MaxHealth);

        NormalizedHealth = _player.CurrentHealth / _player.MaxHealth;

        return _player.CurrentHealth >= 1.0f;
    }

    public void Heal(float amount)
    {
        _player.CurrentHealth = Mathf.Clamp(amount + _player.CurrentHealth, 0f, _player.MaxHealth);

        NormalizedHealth = _player.CurrentHealth / _player.MaxHealth;
    }

    public float SuperAttack()
    {
        _animator.SetTrigger("PlayerSuperAttack");

        return _player.BaseDamage*3;
    }
}
