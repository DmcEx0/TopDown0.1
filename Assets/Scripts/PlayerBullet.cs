using UnityEngine;

public class PlayerBullet : Bullet
{
    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            TurnOffBullet();
            enemy.ApplyDamage(Damage);
        }
    }
}
