using System.Collections.Generic;
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

    private readonly HashSet<Collider2D> tomColliders = new HashSet<Collider2D>();
    private readonly HashSet<Collider2D> jerryColliders = new HashSet<Collider2D>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tomTag))
            tomColliders.Add(other);
        else if (other.CompareTag(jerryTag))
            jerryColliders.Add(other);

        CheckCompletion();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(tomTag))
            tomColliders.Remove(other);
        else if (other.CompareTag(jerryTag))
            jerryColliders.Remove(other);
    }

    private void CheckCompletion()
    {
        if (tomColliders.Count > 0 && jerryColliders.Count > 0)
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
