using UnityEngine;

public class TomMount : MonoBehaviour
{
    [Header("Montura")]
    [SerializeField] private JerryMovement jerry;
    [SerializeField] private KeyCode mountKey = KeyCode.E;
    [SerializeField] private float mountRange = 1.75f;
    [SerializeField] private float dismountGap = 0.1f;

    private Rigidbody2D tomBody;
    private Collider2D tomCollider;
    private Rigidbody2D jerryBody;
    private Collider2D jerryCollider;
    private Vector2 jerryColliderExtents;
    private bool jerryWasSimulated;
    private bool jerryMovementWasEnabled;
    private bool isMounted;
    private float dismountSide = 1f;

    public bool IsMounted => isMounted;

    private void Awake()
    {
        tomBody = GetComponent<Rigidbody2D>();
        tomCollider = GetComponent<Collider2D>();

        if (jerry == null)
        {
            Debug.LogError("Asigna el componente JerryMovement al sistema de montura de Tom.", this);
            enabled = false;
            return;
        }

        jerryBody = jerry.GetComponent<Rigidbody2D>();
        jerryCollider = jerry.GetComponent<Collider2D>();

        if (tomBody == null || tomCollider == null || jerryBody == null || jerryCollider == null)
        {
            Debug.LogError("Tom y Jaimito necesitan Rigidbody2D y Collider2D para usar la montura.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(mountKey))
        {
            if (isMounted)
                Dismount();
            else
                TryMount();
        }
    }

    private void LateUpdate()
    {
        if (isMounted)
            PlaceJerryOnTom();
    }

    private void TryMount()
    {
        if (Vector2.Distance(transform.position, jerry.transform.position) > mountRange)
            return;

        float side = jerry.transform.position.x - transform.position.x;
        dismountSide = Mathf.Abs(side) > 0.01f ? Mathf.Sign(side) : 1f;
        jerryColliderExtents = jerryCollider.bounds.extents;
        jerryWasSimulated = jerryBody.simulated;
        jerryMovementWasEnabled = jerry.enabled;

        jerry.enabled = false;
        jerryBody.linearVelocity = Vector2.zero;
        jerryBody.simulated = false;
        isMounted = true;
        PlaceJerryOnTom();
    }

    private void Dismount()
    {
        Bounds tomBounds = tomCollider.bounds;
        float x = tomBounds.center.x + dismountSide * (tomBounds.extents.x + jerryColliderExtents.x + dismountGap);
        float y = Mathf.Max(jerry.transform.position.y, tomBounds.min.y + jerryColliderExtents.y);
        Vector2 position = new Vector2(x, y);

        isMounted = false;
        jerryBody.simulated = jerryWasSimulated;
        jerryBody.position = position;
        jerryBody.linearVelocity = tomBody.linearVelocity;
        jerry.enabled = jerryMovementWasEnabled;
    }

    private void PlaceJerryOnTom()
    {
        Bounds tomBounds = tomCollider.bounds;
        float y = tomBounds.max.y + jerryColliderExtents.y;
        jerry.transform.position = new Vector3(tomBounds.center.x, y, jerry.transform.position.z);
    }

    private void OnDisable()
    {
        if (isMounted && jerryBody != null)
            Dismount();
    }
}
