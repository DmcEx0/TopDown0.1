using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamageble
{
    [SerializeField] private int _startHealth;
    [SerializeField] private int _reward;
    [SerializeField] private int _damage;

    private Player _target;
    private float _currentHealth;

    public int Reward => _reward;
    public Player Target => _target;

    public event UnityAction<Enemy> Dying;

    public void Init(Player target)
    {
        _target = target;
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
            Debug.Log("������������� �������� �����");
    }

    private void Start()
    {
        SetDefaultState();
    }

    private void SetDefaultState()
    {
        _currentHealth = _startHealth;
    }
}
