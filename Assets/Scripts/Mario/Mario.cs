using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Mario : MonoBehaviour
{
    public GameObject stompBox;
    Mover mover;
    Colisiones colisiones;
    Animaciones animaciones;
    Rigidbody2D rb2D;
    public GameObject fireBallPrefab;
    public Transform shootPos;
    public bool isInvincible;
    public float invincibleTime;
    float invincibleTimer;

    public bool isHurt;
    public float hurtTime;
    float hurtTimer;
    public bool isCrouched;
    
    bool isDead;
    enum State { Default = 0, Super = 1, Fire = 2 }
    State currentState = State.Default;
    private void Awake()
    {
        mover = GetComponent<Mover>();
        colisiones = GetComponent<Colisiones>();
        animaciones = GetComponent<Animaciones>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        isCrouched = false;
        if (!isDead)
        {
            if (rb2D.linearVelocity.y < 0)
            {
                stompBox.SetActive(true);
            }
            else
            {
                stompBox.SetActive(false);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (colisiones.Grounded())
                {
                    isCrouched = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Shoot();
            }
            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer <= 0)
                {
                    isInvincible = false;
                    animaciones.InvincibleMode(false);
                }
            }
            if (isHurt)
            {
                hurtTimer -= Time.deltaTime;
                if (hurtTimer <= 0)
                {
                    EndHurt();
                }
            }
        }
        animaciones.Crouch(isCrouched);
    }
    public void Hit()
    {
        if (!isHurt)
        {
            if (currentState == State.Default)
            {
                Dead();
            }
            else
            {
                AudioManager.Instance.PlayPowerDown();
                Time.timeScale = 0;
                animaciones.Hit();
                StartHurt();
            }
        }
    }
    void StartHurt()
    {
        isHurt = true;
        animaciones.Hurt(true);
        hurtTimer = hurtTime;
        colisiones.HurtCollision(true);
    }
    void EndHurt()
    {
        isHurt = false;
        animaciones.Hurt(false);
        colisiones.HurtCollision(false);

    }
    public void Dead()
    {
        if (!isDead)
        {
            AudioManager.Instance.PlayDie();
            isDead = true;
            colisiones.Dead();
            mover.Dead();
            animaciones.Dead();
        }
    }
    void ChangeState(int newState)
    {
        currentState = (State)newState;
        animaciones.NewState(newState);
        Time.timeScale = 1;
    }
    public void CatchItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.MagicMushroom:
                AudioManager.Instance.PlayPowerUp();
                if (currentState == State.Default)
                {
                    animaciones.PowerUp();
                    Time.timeScale = 0;
                }
                break;

            case ItemType.Coin:
                AudioManager.Instance.PlayCoin();
                LevelManager.Instance.AddCoins();
                break;

            case ItemType.FireFlower:
                AudioManager.Instance.PlayPowerUp();
                if (currentState != State.Fire)
                {
                    animaciones.PowerUp();
                    Time.timeScale = 0;
                }
                break;

            case ItemType.Life:
                //life
                break;

            case ItemType.Star:
                AudioManager.Instance.PlayPowerUp();
                isInvincible = true;
                animaciones.InvincibleMode(true);
                invincibleTimer = invincibleTime;
                EndHurt();
                break;

            default:
                break;
        }
    }
    void Shoot()
    {
        if (currentState == State.Fire && !isCrouched)
        {
            AudioManager.Instance.PlayShoot();
            GameObject newFireBall = Instantiate(fireBallPrefab, shootPos.position, Quaternion.identity);
            newFireBall.GetComponent<FireBall>().direction = transform.localScale.x;
            animaciones.Shoot();
        }
    }
    public bool isBig()
    {
        return currentState != State.Default;
    }
    public void Goal()
    {
        AudioManager.Instance.PlayFlagPole();
        mover.DownFlagPole();
        LevelManager.Instance.LevelFinished();
    }
}
