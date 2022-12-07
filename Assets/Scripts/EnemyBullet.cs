using UnityEngine;

public class EnemyBullet : Bullet
{
    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            TurnOffBullet();
            player.ApplyDamage(Damage);
        }
    }
}
