using UnityEngine;

/// <summary>
/// Clase que representa al enemigo Koopa. Hereda de Enemy y define su comportamiento especial.
/// </summary>
public class Koopa : Enemy
{
    bool isHidden; // Indica si el Koopa está oculto en su caparazón
    public float maxStoppedTime; // Tiempo máximo que puede estar detenido antes de salir del caparazón
    float stoppedTimer; // Temporizador para el tiempo detenido
    public float rollingSpeed; // Velocidad al rodar en caparazón
    public bool isRolling; // Indica si el Koopa está rodando

    /// <summary>
    /// Actualización por frame. Controla el temporizador cuando está oculto y detenido.
    /// </summary>
    protected override void Update()
    {
        base.Update();
        // Si está oculto y no se mueve, suma tiempo
        if (isHidden && rb2D.linearVelocity.x == 0f)
        {
            stoppedTimer += Time.deltaTime;
            // Si supera el tiempo máximo, vuelve a caminar
            if (stoppedTimer >= maxStoppedTime)
            {
                ResetMove();
            }
        }
    }

    /// <summary>
    /// Lógica cuando el Koopa es pisado por el jugador.
    /// </summary>
    /// <param name="player">Transform del jugador que lo pisa.</param>
    public override void Stomped(Transform player)
    {
        AudioManager.Instance.PlayStomp();
        isRolling = false;
        if (!isHidden)
        {
            // Si no está oculto, se oculta en el caparazón
            isHidden = true;
            animator.SetBool("Hidden", isHidden);
            autoMovement.PauseMovement();
        }
        else
        {
            // Si ya está oculto
            if (Mathf.Abs(rb2D.linearVelocity.x) > 0f)
            {
                // Si está rodando, se detiene
                autoMovement.PauseMovement();
            }
            else
            {
                // Si está parado, comienza a rodar en dirección opuesta al jugador
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

        // Configura el comportamiento de destrucción fuera de cámara
        DestroyOutCamera destroyOutCamera = GetComponent<DestroyOutCamera>();
        if(isRolling){
            destroyOutCamera.onlyBack = false;
        }
        else {
            destroyOutCamera.onlyBack = true;
        }

        // Cambia la capa para evitar colisiones con el jugador temporalmente
        gameObject.layer = LayerMask.NameToLayer("OnlyGround");
        Invoke("ResetLayer", 0.1f); // Restaura la capa después de un corto tiempo
        stoppedTimer = 0; // Reinicia el temporizador
    }

    /// <summary>
    /// Lógica cuando el Koopa es golpeado por una concha rodante.
    /// </summary>
    public override void HitRollingShell()
    {
        if (!isRolling)
        {
            // Si no está rodando, muere volteado
            FlipDie();
        }
        else
        {
            // Si está rodando, cambia de dirección
            autoMovement.ChangeDirection();
        }
    }

    /// <summary>
    /// Restaura la capa original del Koopa.
    /// </summary>
    void ResetLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    /// <summary>
    /// Hace que el Koopa salga del caparazón y vuelva a caminar.
    /// </summary>
    void ResetMove()
    {
        autoMovement.ContinueMovement();
        isHidden = false;
        animator.SetBool("Hidden", isHidden);
        stoppedTimer = 0;
    }

    /// <summary>
    /// Lógica de colisión. Si está rodando y choca con otro enemigo, lo golpea.
    /// </summary>
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
