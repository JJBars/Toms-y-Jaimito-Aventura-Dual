using UnityEngine;

/// <summary>
/// Túnel pequeño que solo permite pasar a Jaimito (el ratón).
/// Tom (el gato) es demasiado grande y no puede atravesarlo.
/// Se usa un collider de activación que filtra por etiqueta o tamaño.
/// </summary>
public class Tunnel : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Etiqueta del personaje que puede pasar por el túnel (ej: 'Jerry')")]
    [SerializeField] private string allowedTag = "Jerry";

    [Tooltip("Etiqueta del personaje bloqueado (ej: 'Tom')")]
    [SerializeField] private string blockedTag = "Tom";

    [Tooltip("Efecto visual al bloquear (opcional)")]
    [SerializeField] private GameObject blockEffect;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Color blockedColor = new Color(1f, 0.5f, 0.5f, 0.5f);
    private Collider2D tunnelCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tunnelCollider = GetComponent<Collider2D>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        if (tunnelCollider == null || !tunnelCollider.isTrigger)
            Debug.LogError("Tunnel requires a trigger Collider2D on the same object.", this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(allowedTag) && !other.CompareTag(blockedTag)) return;

        if (other.CompareTag(blockedTag))
        {
            // Tom intenta entrar: bloquear y empujar suavemente
            BlockPlayer(other);
        }
        else
        {
            // Jaimito puede pasar: indicar visualmente
            AllowPlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(allowedTag) || other.CompareTag(blockedTag))
        {
            if (spriteRenderer != null)
                spriteRenderer.color = originalColor;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(blockedTag))
            KeepPlayerOutside(other);
    }

    private void BlockPlayer(Collider2D player)
    {
        if (spriteRenderer != null)
            spriteRenderer.color = blockedColor;

        if (blockEffect != null)
            Instantiate(blockEffect, player.transform.position, Quaternion.identity);

        Debug.Log($"{player.name} es demasiado grande para este túnel.");
    }

    private void KeepPlayerOutside(Collider2D player)
    {
        Rigidbody2D rb = player.attachedRigidbody;
        if (rb == null || tunnelCollider == null) return;

        Bounds playerBounds = player.bounds;
        Bounds tunnelBounds = tunnelCollider.bounds;
        float side = playerBounds.center.x < tunnelBounds.center.x ? -1f : 1f;
        float outsideX = side < 0f
            ? tunnelBounds.min.x - playerBounds.extents.x
            : tunnelBounds.max.x + playerBounds.extents.x;

        rb.position = new Vector2(outsideX, rb.position.y);
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    private void AllowPlayer()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = new Color(0.5f, 1f, 0.5f, 0.5f);
    }
}
