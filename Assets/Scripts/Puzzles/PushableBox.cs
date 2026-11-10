using UnityEngine;

/// <summary>
• Caja empujable para puzles de plataforma.
• Tom puede empujar cajas pesadas (más lentas).
• Jaimito puede empujar cajas ligeras (más rápidas).
/// </summary>
public class PushableBox : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Tipo de caja: determina qué personaje la empuja mejor")]
    [SerializeField] private BoxType boxType = BoxType.Normal;

    [Tooltip("Velocidad base de empuje")]
    [SerializeField] private float basePushSpeed = 3f;

    [Tooltip("Multiplicador de velocidad según el personaje")]
    [SerializeField] private float tomPushMultiplier = 0.8f;
    [SerializeField] private float jerryPushMultiplier = 1.2f;

    private Rigidbody2D body;
    private bool isBeingPushed;
    private GameObject pusher;

    public enum BoxType
    {
        Normal,      // Ambos pueden empujar
        Heavy,       // Solo Tom empuja bien
        Light        // Solo Jaimito empuja bien
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        if (body == null)
            body = gameObject.AddComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Dynamic;
        body.freezeRotation = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject pusherCandidate = collision.gameObject;

        if (!pusherCandidate.CompareTag("Tom") && !pusherCandidate.CompareTag("Jerry"))
            return;

        // Determinar si este personaje puede empujar esta caja eficientemente
        bool canPushWell = boxType switch
        {
            BoxType.Heavy => pusherCandidate.CompareTag("Tom"),
            BoxType.Light => pusherCandidate.CompareTag("Jerry"),
            _ => true
        };

        if (!canPushWell) return;

        pusher = pusherCandidate;
        isBeingPushed = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == pusher)
        {
            pusher = null;
            isBeingPushed = false;
        }
    }

    private void FixedUpdate()
    {
        if (!isBeingPushed || pusher == null) return;

        // La caja se mueve ligeramente hacia donde empuja el jugador
        Vector2 pushDirection = (pusher.transform.position - transform.position).normalized;
        pushDirection.y = 0f; // Solo empuje horizontal

        float multiplier = pusher.CompareTag("Tom") ? tomPushMultiplier : jerryPushMultiplier;
        Vector2 targetVelocity = pushDirection * basePushSpeed * multiplier;

        body.linearVelocity = new Vector2(targetVelocity.x, body.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = boxType == BoxType.Heavy ? Color.red : (boxType == BoxType.Light ? Color.green : Color.white);
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}
