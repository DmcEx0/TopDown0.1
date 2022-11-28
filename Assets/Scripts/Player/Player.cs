using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IDamageble
{
    [SerializeField] private int _damage;
    [SerializeField] private int _health;
    [SerializeField] private int _money;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _attackRange;

    private int _currentHealth;
    private Enemy _target;
    private List<Enemy> _targets = new List<Enemy>();
    private Collider[] _targetBuffer = new Collider[20];

    public bool IsTargetReceived { get; private set; }
    public Enemy Target => _target;
    public int Damage => _damage;

    public event UnityAction<int> MoneyChanged;
    public event UnityAction<int, int> HealthChanged;

    public void ApplyDamage(int value)
    {
        if (value >= 0)
        {
            _currentHealth -= value;
            HealthChanged?.Invoke(_currentHealth, _health);

            if (_currentHealth <= 0)
                Destroy(gameObject);
        }
        else
            Debug.Log("Отрицательное значение урона");
    }

    public void AddMoney(int money)
    {
        _money += money;
        MoneyChanged?.Invoke(_money);
    }

    private void Start()
    {
        _currentHealth = _health;
        IsTargetReceived = false;
    }

    private void Update()
    {
        SetNearestTarget();
    }

    private void SetNearestTarget()
    {
        float permissibleError = 0.3f;
        float attackRange = _attackRange - permissibleError;

        int hits = Physics.OverlapSphereNonAlloc(transform.position, attackRange, _targetBuffer, _layerMask);

        if (hits > 0)
        {
            for (int i = 0; i < hits; i++)
            {
                _target = _targetBuffer[i].GetComponent<Enemy>();

                if (_targets.Contains(_target) == false)
                    _targets.Add(_target);
            }

            for (int i = 0; i < _targets.Count; i++)
            {
                if (Vector3.Distance(transform.position, _targets[i].transform.position) < Vector3.Distance(transform.position, _target.transform.position) && _targets[i].isActiveAndEnabled)
                    _target = _targets[i];
            }

            IsTargetReceived = true;
        }
        else
        {
            IsTargetReceived = false;
            _target = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 position = transform.position;
        Gizmos.DrawWireSphere(position, _attackRange);

        if (_target != null)
            Gizmos.DrawLine(position, _target.transform.position);
    }
}
