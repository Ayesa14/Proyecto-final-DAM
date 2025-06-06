using UnityEngine;

public class Koopa : Enemy
{
    bool isHidden;
    public float maxStoppedTime;
    float stoppedTimer;
    public float rollingSpeed;
    public bool isRolling;
    protected override void Update()
    {
        base.Update();
        if (isHidden && rb2D.linearVelocity.x == 0f)
        {
            stoppedTimer += Time.deltaTime;
            if (stoppedTimer >= maxStoppedTime)
            {
                ResetMove();
            }
        }
    }
    public override void Stomped(Transform player)
    {
        AudioManager.Instance.PlayStomp();
        isRolling = false;
        if (!isHidden)
        {
            isHidden = true;
            animator.SetBool("Hidden", isHidden);
            autoMovement.PauseMovement();
        }
        else
        {
            if (Mathf.Abs(rb2D.linearVelocity.x) > 0f)
            {
                autoMovement.PauseMovement();
            }
            else
            {
                if (player.position.x < transform.position.x)
                {
                    autoMovement.speed = rollingSpeed;
                }
                else
                {
                    autoMovement.speed = -rollingSpeed;
                }
                autoMovement.ContinueMovement(new Vector2(autoMovement.speed, 0f));
                isRolling = true;
            }
        }
        gameObject.layer = LayerMask.NameToLayer("OnlyGround");
        Invoke("ResetLayer", 0.1f);
        stoppedTimer = 0;
    }
    public override void HitRollingShell()
    {
        if (!isRolling)
        {
            FlipDie();
        }
        else
        {
            autoMovement.ChangeDirection();
        }
    }
    void ResetLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }
    void ResetMove()
    {
        autoMovement.ContinueMovement();
        isHidden = false;
        animator.SetBool("Hidden", isHidden);
        stoppedTimer = 0;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isRolling)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().HitRollingShell();
            }
        }
    }
}
