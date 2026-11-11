using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Puerta que se abre cuando se activan los botones de presión vinculados.
/// Soporta requerir uno o dos botones activos simultáneamente.
/// </summary>
public class Door : MonoBehaviour, PressureButtonListener
{
    [Header("Configuración")]
    [Tooltip("Número de botones que deben estar activos para abrir la puerta")]
    [SerializeField] private int requiredButtons = 2;

    [Tooltip("Velocidad de apertura/cierre (unidades por segundo)")]
    [SerializeField] private float openSpeed = 3f;

    [Tooltip("Distancia que se desplaza la puerta al abrirse (en Y)")]
    [SerializeField] private float openDistance = 2f;

    [Tooltip("Sprite de puerta cerrada (opcional)")]
    [SerializeField] private Sprite closedSprite;

    [Tooltip("Sprite de puerta abierta (opcional)")]
    [SerializeField] private Sprite openSprite;

    private readonly HashSet<PressureButton> activeButtons = new HashSet<PressureButton>();
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen;
    private SpriteRenderer spriteRenderer;
    private float targetY;
    private bool puzzleOpen;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        closedPosition = transform.position;
        openPosition = transform.position + Vector3.up * openDistance;
        targetY = closedPosition.y;
    }

    public void OnButtonStateChanged(PressureButton button, bool isPressed)
    {
        if (isPressed)
            activeButtons.Add(button);
        else
            activeButtons.Remove(button);

        UpdateDoorState();
    }

    public void SetPuzzleOpen(bool open)
    {
        puzzleOpen = open;
        UpdateDoorState();
    }

    private void UpdateDoorState()
    {
        bool shouldBeOpen = puzzleOpen || activeButtons.Count >= requiredButtons;
        if (shouldBeOpen != isOpen)
        {
            isOpen = shouldBeOpen;
            targetY = isOpen ? openPosition.y : closedPosition.y;

            Sprite nextSprite = isOpen ? openSprite : closedSprite;
            if (spriteRenderer != null && nextSprite != null)
                spriteRenderer.sprite = nextSprite;
        }
    }

    private void Update()
    {
        float currentY = Mathf.MoveTowards(transform.position.y, targetY, openSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + Vector3.up * openDistance * 0.5f,
            new Vector3(transform.localScale.x, openDistance, transform.localScale.z));
    }
}
