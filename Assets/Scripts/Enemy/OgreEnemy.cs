using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreEnemy : Enemy
{
    [SerializeField] private int _damage;

    public int Damage => _damage;
}
