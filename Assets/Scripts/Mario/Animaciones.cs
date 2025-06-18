using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script que gestiona las animaciones de Mario mediante parámetros en el Animator.
/// Proporciona métodos para cambiar estados, triggers y parámetros de animación.
/// </summary>
public class Animaciones : MonoBehaviour
{
    Animator animator; // Referencia al componente Animator

    /// <summary>
    /// Inicializa la referencia al Animator.
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Cambia el parámetro "Grounded" según si Mario está en el suelo.
    /// </summary>
    public void Grounded(bool isGrounded)
    {
        animator.SetBool("Grounded", isGrounded);
    }

    /// <summary>
    /// Cambia el parámetro "Velocity_X" según la velocidad horizontal de Mario.
    /// </summary>
    public void Velocity(float velocityX)
    {
        animator.SetFloat("Velocity_X", Mathf.Abs(velocityX));
    }

    /// <summary>
    /// Cambia el parámetro "Jumping" según si Mario está saltando.
    /// </summary>
    public void Jumping(bool isJumping)
    {
        animator.SetBool("Jumping", isJumping);
    }

    /// <summary>
    /// Cambia el parámetro "Skid" según si Mario está derrapando.
    /// </summary>
    public void Skid(bool isSkidding)
    {
        animator.SetBool("Skid", isSkidding);
    }

    /// <summary>
    /// Activa el trigger "Dead" para la animación de muerte.
    /// </summary>
    public void Dead()
    {
        animator.SetTrigger("Dead");
    }

    /// <summary>
    /// Cambia el parámetro "State" para el estado general de Mario (pequeño, grande, fuego, etc).
    /// </summary>
    public void NewState(int state)
    {
        animator.SetInteger("State", state);
    }

    /// <summary>
    /// Activa el trigger "PowerUp" para la animación de obtener un power-up.
    /// </summary>
    public void PowerUp()
    {
        animator.SetTrigger("PowerUp");
    }

    /// <summary>
    /// Activa el trigger "Hit" para la animación de recibir daño.
    /// </summary>
    public void Hit()
    {
        animator.SetTrigger("Hit");
    }

    /// <summary>
    /// Activa el trigger "Shoot" para la animación de disparar.
    /// </summary>
    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }

    /// <summary>
    /// Cambia el parámetro "Invincible" según si Mario está en modo invencible.
    /// </summary>
    public void InvincibleMode(bool activate)
    {
        animator.SetBool("Invincible", activate);
    }

    /// <summary>
    /// Cambia el parámetro "Hurt" según si Mario está herido.
    /// </summary>
    public void Hurt(bool activate)
    {
        animator.SetBool("Hurt", activate);
    }

    /// <summary>
    /// Cambia el parámetro "Crouched" según si Mario está agachado.
    /// </summary>
    public void Crouch(bool activate)
    {
        animator.SetBool("Crouched", activate);
    }

    /// <summary>
    /// Cambia el parámetro "Climb" según si Mario está trepando.
    /// </summary>
    public void Climb(bool activate)
    {
        animator.SetBool("Climb", activate);
    }

    /// <summary>
    /// Pausa la animación (velocidad 0).
    /// </summary>
    public void Pause()
    {
        animator.speed = 0;
    }

    /// <summary>
    /// Continúa la animación (velocidad 1).
    /// </summary>
    public void Continue()
    {
        animator.speed = 1;
    }

    /// <summary>
    /// Restaura todos los parámetros y triggers del Animator a sus valores por defecto.
    /// </summary>
    public void Reset()
    {
        animator.SetBool("Grounded", false);
        animator.SetFloat("Velocity_X", 0);
        animator.SetBool("Jumping", false);
        animator.SetBool("Skid", false);
        animator.SetBool("Invincible", false);
        animator.SetBool("Hurt", false);
        animator.SetBool("Crouched", false);
        animator.SetBool("Climb", false);

        animator.ResetTrigger("Dead");
        animator.ResetTrigger("PowerUp");
        animator.ResetTrigger("Hit");
        animator.ResetTrigger("Shoot");

        animator.SetInteger("State", 0);
        animator.Play("States"); // Reproduce la animación base
    }
}