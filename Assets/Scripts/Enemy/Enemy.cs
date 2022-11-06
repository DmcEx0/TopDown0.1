using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour, IDamageble
{
    [SerializeField] private int _startHealth;
    [SerializeField] private int _reward;

    private Player _target;
    private float _currentHealth;
    private Transform _parent;

    public int Reward => _reward;
    public Player Target => _target;

    public event UnityAction<Enemy> Dying;

    public void Init(Player target, Transform parent)
    {
        _target = target;
        _parent = parent;
    }

    public void ApplyDamage(int value)
    {
        if (value >= 0)
        {
            _currentHealth -= value;

            if (_currentHealth <= 0)
            {
                Dying?.Invoke(this);
                gameObject.SetActive(false);
                SetDefaultState();
            }
        }
        else
            Debug.Log("Отрицательное значение урона");
    }

    private void Start()
    {
        _currentHealth = _startHealth;
    }

    private void SetDefaultState()
    {
        _currentHealth = _startHealth;
        gameObject.transform.position = _parent.transform.position;
    }
}
