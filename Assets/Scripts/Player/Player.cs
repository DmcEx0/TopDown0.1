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
    private List<Enemy> _targets = new List<Enemy>();
    private Enemy _target;
    private Collider[] _targetBuffer = new Collider[20];

    public Enemy Target => _target;

    public event UnityAction<int> ChangeMoney;

    public void ApplyDamage(int value)
    {
        if (value >= 0)
        {
            _currentHealth -= value;

            if (_currentHealth <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        else
            Debug.Log("������������� �������� �����");
    }

    public void AddMoney(int money)
    {
        _money += money;
        ChangeMoney?.Invoke(_money);
    }

    private void Start()
    {
        _playerBehaviour = GetComponent<PlayerBehaviour>();
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
            //_playerBehaviour.StartDoShootCoroutine(_target);
        }
        else
        {
            _target = null;
            //_playerBehaviour.StopDoShootCoroutine();
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