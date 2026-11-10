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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
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

    private void BlockPlayer(Collider2D player)
    {
        if (spriteRenderer != null)
            spriteRenderer.color = blockedColor;

        if (blockEffect != null)
            Instantiate(blockEffect, player.transform.position, Quaternion.identity);

        // Empujar al jugador fuera del túnel (suavemente)
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 pushDirection = (player.transform.position - transform.position).normalized;
            rb.AddForce(pushDirection * 5f, ForceMode2D.Impulse);
        }

        Debug.Log($"{player.name} es demasiado grande para este túnel.");
    }

    private void AllowPlayer()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = new Color(0.5f, 1f, 0.5f, 0.5f);
    }
}
