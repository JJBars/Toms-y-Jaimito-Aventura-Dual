using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Transform tom;
    [SerializeField] private Transform jerry;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float verticalOffset = 2f;

    private void Awake()
    {
        if (tom != null && jerry != null)
            transform.position = GetTargetPosition();
    }

    private void LateUpdate()
    {
        if (tom == null || jerry == null) return;

        Vector3 targetPosition = GetTargetPosition();
        float blend = 1f - Mathf.Exp(-smoothSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPosition, blend);
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 position = (tom.position + jerry.position) * 0.5f;
        position.y += verticalOffset;
        position.z = transform.position.z;
        return position;
    }
}
