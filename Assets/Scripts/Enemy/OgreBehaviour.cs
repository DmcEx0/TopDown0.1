using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreBehaviour : EnemyBehaviour
{
    private OgreEnemy _ogreEnemy;

    private void Awake()
    {
        _ogreEnemy = GetComponent<OgreEnemy>();
    }

    public void OgreAttacked()
    {
        Enemy.Target.ApplyDamage(_ogreEnemy.Damage);
    }
}
