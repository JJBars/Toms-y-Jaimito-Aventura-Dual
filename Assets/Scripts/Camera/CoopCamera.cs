using UnityEngine;

/// <summary>
/// Cámara cooperativa para 2 jugadores.
/// Sigue al punto medio entre Tom y Jaimito y hace zoom dinámico
/// según la distancia que los separa.
/// </summary>
public class CoopCamera : MonoBehaviour
{
    [Header("Objetivos a seguir")]
    [Tooltip("Transform de Tom")]
    [SerializeField] private Transform targetTom;

    [Tooltip("Transform de Jaimito")]
    [SerializeField] private Transform targetJerry;

    [Header("Configuración de cámara")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float minOrthographicSize = 5f;
    [SerializeField] private float maxOrthographicSize = 14f;
    [Tooltip("Cuánto se aleja la cámara por unidad de distancia entre jugadores")]
    [SerializeField] private float distanceMultiplier = 0.5f;
    [SerializeField] private float verticalOffset = 2f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
            cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (targetTom == null || targetJerry == null) return;

        // Punto medio entre ambos jugadores
        Vector3 midPoint = (targetTom.position + targetJerry.position) * 0.5f;
        midPoint.z = transform.position.z;
        midPoint.y += verticalOffset;

        // Movimiento suave hacia el punto medio
        Vector3 targetPosition = Vector3.Lerp(transform.position, midPoint, smoothSpeed * Time.deltaTime);
        transform.position = targetPosition;

        // Zoom dinámico: si se separan mucho, la cámara se aleja
        float distance = Vector3.Distance(targetTom.position, targetJerry.position);
        float targetSize = Mathf.Clamp(
            minOrthographicSize + distance * distanceMultiplier,
            minOrthographicSize,
            maxOrthographicSize
        );

        if (cam.orthographic)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, smoothSpeed * Time.deltaTime);
        }
    }
}
