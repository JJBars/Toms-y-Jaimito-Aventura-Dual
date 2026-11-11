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
    [SerializeField] protected float groundCheckDistance = 0.12f;
    [Tooltip("Capa que se considera como suelo")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] private float jumpBufferTime = 0.15f;
    [SerializeField] private float coyoteTime = 0.12f;

    protected Rigidbody2D body;
    protected float horizontalInput;
    protected bool isGrounded;
    protected bool jumpRequested;
    private Collider2D characterCollider;
    private float jumpBufferCounter;
    private float coyoteTimeCounter;

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
        characterCollider = GetComponent<Collider2D>();

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
        jumpBufferCounter = Mathf.Max(0f, jumpBufferCounter - Time.deltaTime);

        // Cada personaje define sus propias teclas en ReadInput()
        ReadInput();
        if (jumpRequested)
        {
            jumpBufferCounter = jumpBufferTime;
            jumpRequested = false;
        }
    }

    protected virtual void FixedUpdate()
    {
        CheckGround();
        coyoteTimeCounter = isGrounded
            ? coyoteTime
            : Mathf.Max(0f, coyoteTimeCounter - Time.fixedDeltaTime);

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            Jump();
        }

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
        if (characterCollider == null)
        {
            isGrounded = groundCheck != null
                && Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            return;
        }

        Bounds bounds = characterCollider.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y + 0.03f);
        Vector2 sensorSize = new Vector2(Mathf.Max(bounds.size.x * 0.75f, 0.12f), 0.06f);
        isGrounded = Physics2D.BoxCast(
            origin,
            sensorSize,
            0f,
            Vector2.down,
            groundCheckDistance,
            groundLayer);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (characterCollider != null)
        {
            Bounds bounds = characterCollider.bounds;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(
                new Vector3(bounds.center.x, bounds.min.y + 0.03f - groundCheckDistance * 0.5f, bounds.center.z),
                new Vector3(Mathf.Max(bounds.size.x * 0.75f, 0.12f), 0.06f + groundCheckDistance, 0f));
        }
    }
}
