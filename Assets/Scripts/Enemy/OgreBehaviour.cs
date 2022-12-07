
public class OgreBehaviour : EnemyBehaviour
{
    private OgreEnemy _ogreEnemy;

    public override void Shoot()
    {
        Enemy.Target.ApplyDamage(_ogreEnemy.Damage);
    }

    private void Awake()
    {
        _ogreEnemy = GetComponent<OgreEnemy>();
    }
}
