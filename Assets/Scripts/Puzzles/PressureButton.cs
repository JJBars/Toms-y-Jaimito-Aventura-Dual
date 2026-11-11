using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Botón de presión que se activa cuando un jugador está pisándolo.
/// Puede notificar a un mecanismo vinculado (puerta, plataforma, etc.)
/// cuando cambia su estado de activado/desactivado.
/// </summary>
public class PressureButton : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Etiquetas de los personajes que pueden activar el botón")]
    [SerializeField] private string[] playerTags = { "Tom", "Jerry" };

    [Tooltip("Objetos a notificar cuando el botón cambia de estado")]
    [SerializeField] private MonoBehaviour[] listeners;

    [Tooltip("Tiempo mínimo que debe mantenerse presionado (segundos)")]
    [SerializeField] private float holdTime = 0f;

    private bool isPressed;
    private float pressTimer;
    private readonly HashSet<Collider2D> playerColliders = new HashSet<Collider2D>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsPlayer(other)) return;
        if (!playerColliders.Add(other)) return;

        if (playerColliders.Count == 1)
        {
            pressTimer = 0f;

            if (holdTime <= 0f)
                SetPressed(true);
        }
    }

    private void Update()
    {
        if (holdTime > 0f && playerColliders.Count > 0 && !isPressed)
        {
            pressTimer += Time.deltaTime;
            if (pressTimer >= holdTime)
                SetPressed(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsPlayer(other) || !playerColliders.Remove(other)) return;

        if (playerColliders.Count == 0)
        {
            pressTimer = 0f;
            SetPressed(false);
        }
    }

    private bool IsPlayer(Collider2D other)
    {
        if (playerTags == null) return false;

        GameObject player = other.attachedRigidbody != null
            ? other.attachedRigidbody.gameObject
            : other.gameObject;

        foreach (string tag in playerTags)
        {
            if (!string.IsNullOrEmpty(tag) && player.CompareTag(tag))
                return true;
        }

        return false;
    }

    private void SetPressed(bool pressed)
    {
        if (isPressed == pressed) return;
        isPressed = pressed;

        if (listeners != null)
        {
            foreach (MonoBehaviour listener in listeners)
            {
                if (listener is PressureButtonListener pressureButtonListener)
                    pressureButtonListener.OnButtonStateChanged(this, isPressed);
            }
        }

        StateChanged?.Invoke(this, isPressed);
    }

    public bool IsPressed => isPressed;
    public event System.Action<PressureButton, bool> StateChanged;
}

/// <summary>
/// Interfaz para objetos que escuchan cambios en un botón de presión.
/// </summary>
public interface PressureButtonListener
{
    void OnButtonStateChanged(PressureButton button, bool isPressed);
}
