using UnityEngine;

// Coloca este objeto en el punto medio entre los dos jugadores
// y asignarlo como Follow Target de la CinemachineCamera.
public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Transform tom;
    [SerializeField] private Transform jerry;
    [SerializeField, Min(0f)] private float smoothingTime = 0.25f;

    private Vector3 velocity;

    void Start()
    {
        if (tom == null)
        {
            TomMove t = FindFirstObjectByType<TomMove>();
            if (t != null)
            {
                tom = t.transform;
            }
        }

        if (jerry == null)
        {
            JerryMove j = FindFirstObjectByType<JerryMove>();
            if (j != null)
            {
                jerry = j.transform;
            }
        }

        if (tom == null || jerry == null)
        {
            Debug.LogWarning("CameraTarget no encuentra a los dos jugadores en la escena.", this);
        }
    }

    void LateUpdate()
    {
        if (tom == null || jerry == null)
        {
            return;
        }

        Vector3 objetivo = (tom.position + jerry.position) * 0.5f;
        objetivo.z = transform.position.z;

        if (smoothingTime <= 0f)
        {
            transform.position = objetivo;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, objetivo, ref velocity, smoothingTime);
        }
    }
}
