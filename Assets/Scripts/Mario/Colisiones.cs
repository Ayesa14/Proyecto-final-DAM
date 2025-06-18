using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Colisiones : MonoBehaviour
{
    public bool isGrounded;               // Indica si Mario está en el suelo
    public Transform groundCheck;         // Transform para comprobar el suelo
    public float groundCheckRadius;       // Radio para la comprobación de suelo
    public LayerMask groundLayer;         // Máscara de capa para el suelo
    BoxCollider2D col2D;                  // Referencia al BoxCollider2D
    Mario mario;                          // Referencia al script Mario
    Mover mover;                          // Referencia al script Mover

    /// <summary>
    /// Inicializa referencias a componentes.
    /// </summary>
    private void Awake(){
        col2D = GetComponent<BoxCollider2D>();
        mario = GetComponent<Mario>();
        mover = GetComponent<Mover>();
    }
  
    /// <summary>
    /// Comprueba si Mario está en el suelo usando raycasts en los pies.
    /// </summary>
    public bool Grounded(){
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Vector2 footLeft = new Vector2(col2D.bounds.center.x - col2D.bounds.extents.x, col2D.bounds.center.y);
        Vector2 footRight = new Vector2(col2D.bounds.center.x + col2D.bounds.extents.x, col2D.bounds.center.y);
                        
        Debug.DrawRay(footLeft, Vector2.down*col2D.bounds.extents.y*1.5f, Color.magenta);
        Debug.DrawRay(footRight, Vector2.down*col2D.bounds.extents.y*1.5f, Color.magenta);

        // Lanza rayos hacia abajo desde ambos pies para detectar el suelo
        if(Physics2D.Raycast(footLeft, Vector2.down, col2D.bounds.extents.y * 1.25f, groundLayer)){
            isGrounded = true;
        } else if(Physics2D.Raycast(footRight, Vector2.down, col2D.bounds.extents.y * 1.25f, groundLayer)){
            isGrounded = true;
        } else {
            isGrounded = false;
        }
        return isGrounded;
    }

    /// <summary>
    /// En cada FixedUpdate, actualiza el estado de isGrounded usando un círculo en groundCheck.
    /// </summary>
    private void FixedUpdate(){
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    /// <summary>
    /// Detecta colisiones físicas con enemigos.
    /// Si Mario es invencible, derrota al enemigo; si no, recibe daño.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (mario.isInvincible)
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.HitStarman();
            }
            else
            {
                mario.Hit();
            }
        }
    }

    /// <summary>
    /// Cambia la capa del jugador y sus hijos a "PlayerDead" al morir.
    /// </summary>
    public void Dead()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDead");
        foreach (Transform t in transform)
        {
            t.gameObject.layer = LayerMask.NameToLayer("PlayerDead");
        }
    }

    /// <summary>
    /// Restaura la capa del jugador y sus hijos a "Player" al reaparecer.
    /// </summary>
    public void Respawn(){
        gameObject.layer = LayerMask.NameToLayer("Player");
        foreach (Transform t in transform)
        {
            t.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    /// <summary>
    /// Cambia la capa del jugador y su primer hijo a "OnlyGround" si está herido, o la restaura si no.
    /// </summary>
    public void HurtCollision(bool activate)
    {
        if (activate)
        {
            gameObject.layer = LayerMask.NameToLayer("OnlyGround");
            transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("OnlyGround");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    /// <summary>
    /// Detecta colisiones por trigger con enemigos.
    /// Si Mario es invencible, derrota al enemigo.
    /// Si no, si es una planta, recibe daño; si no, pisa al enemigo y rebota.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (mario.isInvincible)
            {
                enemy.HitStarman();
            }
            else
            {
                if (collision.CompareTag("Plant"))
                {
                    mario.Hit();
                }
                else
                {
                    enemy.Stomped(transform);
                    mover.BounceUp();
                }
            }
        }
    }
}
