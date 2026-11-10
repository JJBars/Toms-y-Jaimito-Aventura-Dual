using UnityEngine;

/// <summary>
/// Botón de presión que se activa cuando un jugador está pisándolo.
/// Puede notificar a un mecanismo vinculado (puerta, plataforma, etc.)
/// cuando cambia su estado de activado/desactivado.
/// </summary>
public class PressureButton : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Etiqueta de los personajes que pueden activar el botón")]
    [SerializeField] private string playerTag = "Player";

    [Tooltip("Objetos a notificar cuando el botón cambia de estado")]
    [SerializeField] private PressureButtonListener[] listeners;

    [Tooltip("Tiempo mínimo que debe mantenerse presionado (segundos)")]
    [SerializeField] private float holdTime = 0f;

    private bool isPressed;
    private float pressTimer;
    private GameObject pressingPlayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (pressingPlayer != null) return; // Ya hay alguien pisando

        pressingPlayer = other.gameObject;
        pressTimer = 0f;

        if (holdTime <= 0f)
        {
            SetPressed(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (pressingPlayer == null) return;

        if (holdTime > 0f)
        {
            pressTimer += Time.deltaTime;
            if (pressTimer >= holdTime && !isPressed)
            {
                SetPressed(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (other.gameObject != pressingPlayer) return;

        pressingPlayer = null;
        pressTimer = 0f;
        SetPressed(false);
    }

    private void SetPressed(bool pressed)
    {
        if (isPressed == pressed) return;
        isPressed = pressed;

        foreach (var listener in listeners)
        {
            listener?.OnButtonStateChanged(this, isPressed);
        }
    }

    public bool IsPressed => isPressed;
}

/// <summary>
/// Interfaz para objetos que escuchan cambios en un botón de presión.
/// </summary>
public interface PressureButtonListener
{
    void OnButtonStateChanged(PressureButton button, bool isPressed);
}
