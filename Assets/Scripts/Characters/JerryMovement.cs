using UnityEngine;

/// <summary>
/// Movimiento de Jaimito (el ratón).
/// Más rápido y ágil que Tom, salta más alto y cabe en espacios pequeños.
/// Controles: Flechas del teclado
/// </summary>
public class JerryMovement : CharacterMovement
{
    protected override void Awake()
    {
        base.Awake();

        // Valores diferenciados para Jaimito
        moveSpeed = 6f;          // más rápido
        jumpForce = 9f;          // salta más alto
        body.gravityScale = 2.5f;// más ligero, cae más suave
    }

    protected override void ReadInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.RightArrow))
            horizontalInput = 1f;

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            jumpRequested = true;
    }
}
