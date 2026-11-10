using UnityEngine;

/// <summary>
/// Meta del nivel. Se activa cuando ambos jugadores (Tom y Jaimito)
/// están dentro del área al mismo tiempo, notificando al GameManager
/// que el nivel se ha completado.
/// </summary>
public class LevelGoal : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Etiqueta de Tom")]
    [SerializeField] private string tomTag = "Tom";

    [Tooltip("Etiqueta de Jaimito")]
    [SerializeField] private string jerryTag = "Jerry";

    private bool tomInside;
    private bool jerryInside;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tomTag))
            tomInside = true;
        else if (other.CompareTag(jerryTag))
            jerryInside = true;

        CheckCompletion();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(tomTag))
            tomInside = false;
        else if (other.CompareTag(jerryTag))
            jerryInside = false;
    }

    private void CheckCompletion()
    {
        if (tomInside && jerryInside)
        {
            Debug.Log("¡Ambos jugadores llegaron a la meta!");
            GameManager.Instance?.CompleteLevel();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        var collider = GetComponent<Collider2D>();
        if (collider != null)
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
    }
}
