using UnityEngine;

// Permite que Jerry se monte sobre la cabeza de Tomás.
// Mientras está montado se sincroniza su posición con la cabeza
// de Tomás y se apaga su Rigidbody2D, de modo que Tomás lo carga
// sin colisiones ni rebotes, y puede saltar con él encima.
[RequireComponent(typeof(Rigidbody2D))]
public class RiderStack : MonoBehaviour
{
    [Header("Referencias (se buscan solas si se dejan vacias)")]
    [SerializeField] private Transform tom;
    [SerializeField] private CapsuleCollider2D tomCollider;
    [SerializeField] private Rigidbody2D tomBody;
    [SerializeField] private GroundCheck groundCheck;

    [Header("Ajustes")]
    [SerializeField] private float shuffleSpeed = 1.5f;
    [SerializeField] private float maxShuffleOffset = 0.35f;
    [SerializeField] private float jumpOffSpeed = 7f;
    [SerializeField] private float remountDelay = 0.15f;
    [SerializeField] private float feetSink = 0.02f;
    [SerializeField] private float topTolerance = 0.15f;

    private Rigidbody2D riderBody;
    private Collider2D riderCollider;
    private Vector3 rideOffset;
    private float shuffle;
    private float remountTimer;

    public bool IsRiding { get; private set; }

    void Awake()
    {
        riderBody = GetComponent<Rigidbody2D>();
        riderCollider = GetComponent<Collider2D>();

        if (groundCheck == null)
        {
            groundCheck = GetComponent<GroundCheck>();
        }
    }

    void Start()
    {
        if (tom == null)
        {
            TomMove t = FindFirstObjectByType<TomMove>();
            if (t != null)
            {
                tom = t.transform;
                tomCollider = t.GetComponent<CapsuleCollider2D>();
                tomBody = t.GetComponent<Rigidbody2D>();
            }
        }

        if (tom == null || tomCollider == null || groundCheck == null)
        {
            Debug.LogWarning("RiderStack no encuentra a Tomás (o a su GroundCheck).", this);
        }
    }

    void Update()
    {
        if (remountTimer > 0f)
        {
            remountTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (tom == null || tomCollider == null || groundCheck == null ||
            riderBody == null || riderCollider == null)
        {
            return;
        }

        if (IsRiding)
        {
            UpdateRiding();
            return;
        }

        if (remountTimer > 0f)
        {
            return;
        }

        // Montarse al aterrizar sobre la cabeza de Tomás
        if (groundCheck.IsGroundedOn(tom.gameObject) &&
            riderCollider.bounds.min.y >= tomCollider.bounds.max.y - topTolerance)
        {
            Mount();
        }
    }

    void LateUpdate()
    {
        if (IsRiding && tom != null)
        {
            Vector3 posicion = tom.TransformPoint(rideOffset);
            posicion.x += shuffle;
            transform.position = posicion;
        }
    }

    private void Mount()
    {
        Bounds tomBounds = tomCollider.bounds;
        Bounds riderBounds = riderCollider.bounds;

        // Posición mundial donde irían los pies de Jerry (sobre la cabeza de Tomás)
        Vector3 objetivo = tomBounds.center;
        objetivo.y = tomBounds.max.y - feetSink + riderBounds.extents.y;

        rideOffset = tom.InverseTransformPoint(objetivo);
        shuffle = 0f;
        IsRiding = true;

        riderBody.linearVelocity = Vector2.zero;
        riderBody.angularVelocity = 0f;
        riderBody.simulated = false; // sin física: Tomás lo lleva enganchado

        transform.position = tom.TransformPoint(rideOffset);
    }

    private void UpdateRiding()
    {
        // Desplazarse sobre los hombros de Tomás
        float input = 0f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            input = -1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            input = 1f;
        }

        shuffle += input * shuffleSpeed * Time.fixedDeltaTime;

        // Se pasa del borde: se cae
        if (Mathf.Abs(shuffle) > maxShuffleOffset)
        {
            Unmount(false);
            return;
        }

        // Saltar para desmontar
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Unmount(true);
        }
    }

    private void Unmount(bool jump)
    {
        IsRiding = false;
        remountTimer = remountDelay;

        riderBody.simulated = true;

        // Hereda el impulso de Tomás para que el desmontaje se sienta natural
        Vector2 velocidad = tomBody != null ? tomBody.linearVelocity : Vector2.zero;
        if (jump)
        {
            velocidad += Vector2.up * jumpOffSpeed;
        }

        riderBody.linearVelocity = velocidad;
    }
}
