using UnityEngine;

/// <summary>
/// Clase que representa a la planta enemiga. Hereda de Enemy y define su comportamiento al ser derrotada.
/// </summary>
public class Plant : Enemy
{
    /// <summary>
    /// Lógica cuando la planta es golpeada por una bola de fuego.
    /// </summary>
    public override void HitFireball()
    {
        Dead(); // Suma puntos y muestra los puntos flotantes
        Destroy(transform.parent.gameObject); // Destruye toda la planta (incluyendo el padre)
    }

    /// <summary>
    /// Lógica cuando la planta es golpeada por una estrella.
    /// </summary>
    public override void HitStarman()
    {
        Dead(); // Suma puntos y muestra los puntos flotantes
        Destroy(transform.parent.gameObject); // Destruye toda la planta (incluyendo el padre)
    }

    /// <summary>
    /// Lógica cuando la planta es golpeada por una concha rodante.
    /// </summary>
    public override void HitRollingShell()
    {
        Dead(); // Suma puntos y muestra los puntos flotantes
        Destroy(transform.parent.gameObject); // Destruye toda la planta (incluyendo el padre)
    }
}
