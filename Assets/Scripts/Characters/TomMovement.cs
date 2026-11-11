using UnityEngine;

/// <summary>
/// Movimiento de Tom (el gato).
/// Más pesado y lento que Jaimito, pero puede activar mecanismos grandes.
/// Controles: WASD
/// </summary>
public class TomMovement : CharacterMovement
{
    protected override void Awake()
    {
        base.Awake();

        // Valores diferenciados para Tom
        moveSpeed = 4f;          // más lento
        jumpForce = 12f;         // impulso suficiente para superar plataformas
        body.gravityScale = 3.5f;// más pesado, cae más rápido
    }

    protected override void ReadInput()
    {
        if (Input.GetKey(KeyCode.A))
            horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            horizontalInput = 1f;

        if (Input.GetKeyDown(KeyCode.W))
            jumpRequested = true;
    }
}
