using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IDamageble
{
    [SerializeField] private int _health;
    [SerializeField] private int _money;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _rotationSpeed;

    private PlayerBehaviour _playerBehaviour;
    private int _currentHealth;
    private Enemy _target;
    private List<Enemy> _targets = new List<Enemy>();
    private Collider[] _targetBuffer = new Collider[20];

    public Enemy Target => _target;

    public event UnityAction<int> MoneyChanged;
    public event UnityAction<int, int> HealthChanged;

    public void ApplyDamage(int value)
    {
        if (value >= 0)
        {
            _currentHealth -= value;
            HealthChanged?.Invoke(_currentHealth, _health);

            if (_currentHealth <= 0)
            {
                Debug.Log("Player DIE");
                Destroy(gameObject);
            }
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
        _playerBehaviour = GetComponent<PlayerBehaviour>();
        _currentHealth = _health;
    }

    private void Update()
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
                if(Vector3.Distance(transform.position, _targets[i].transform.position) < Vector3.Distance(transform.position, _target.transform.position))
                    _target = _targets[i];
            }

            Vector3 direction = _target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);

            _playerBehaviour.Shoot(_target);
        }
        else
            _target = null;
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
