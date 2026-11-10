using UnityEngine;

/// <summary>
/// Clase base para el movimiento de los personajes.
/// Contiene la lógica compartida de movimiento horizontal, salto y detección de suelo.
/// Tom y Jaimito heredan de esta clase para evitar duplicar código.
/// </summary>
public abstract class CharacterMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float jumpForce = 7f;

    [Header("Detección de Suelo")]
    [Tooltip("Objeto hijo posicionado en los pies del personaje para detectar el suelo")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckRadius = 0.2f;
    [Tooltip("Capa que se considera como suelo")]
    [SerializeField] protected LayerMask groundLayer;

    protected Rigidbody2D body;
    protected float horizontalInput;
    protected bool isGrounded;
    protected bool jumpRequested;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        if (body == null)
        {
            body = gameObject.AddComponent<Rigidbody2D>();
        }

        body.gravityScale = 3f;
        body.freezeRotation = true;
        body.linearVelocity = Vector2.zero;

        // Si no se asignó un GroundCheck, crear uno automáticamente en los pies
        if (groundCheck == null)
        {
            GameObject go = new GameObject("GroundCheck");
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            groundCheck = go.transform;
        }
    }

    protected virtual void Update()
    {
        horizontalInput = 0f;

        // Cada personaje define sus propias teclas en ReadInput()
        ReadInput();

        // Ejecutar salto solicitado
        if (jumpRequested && isGrounded)
        {
            jumpRequested = false;
            Jump();
        }
    }

    protected virtual void FixedUpdate()
    {
        CheckGround();

        Vector2 velocity = body.linearVelocity;
        velocity.x = horizontalInput * moveSpeed;
        body.linearVelocity = velocity;
    }

    /// <summary>
    /// Cada subclase implementa la lectura de sus propias teclas.
    /// </summary>
    protected abstract void ReadInput();

    protected virtual void Jump()
    {
        Vector2 velocity = body.linearVelocity;
        velocity.y = jumpForce;
        body.linearVelocity = velocity;
        isGrounded = false;
    }

    /// <summary>
    /// Detección robusta de suelo mediante círculo en la posición del GroundCheck.
    /// Mucho más confiable que confiar en la posición Y de las colisiones.
    /// </summary>
    protected virtual void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
