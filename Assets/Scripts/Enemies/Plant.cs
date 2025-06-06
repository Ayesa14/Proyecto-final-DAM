using UnityEngine;

public class Plant : Enemy
{
    public override void HitFireball()
    {
        Dead();
        Destroy(transform.parent.gameObject);
    }
    public override void HitStarman()
    {
        Dead();
        Destroy(transform.parent.gameObject);
    }
    public override void HitRollingShell()
    {
        Dead();
        Destroy(transform.parent.gameObject);

    }
}
