using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GroundCheck : MonoBehaviour
{
    [SerializeField] private BoxCollider2D groundCollider;
    [SerializeField] private LayerMask groundLayers = ~0;

    private readonly HashSet<Collider2D> groundColliders = new HashSet<Collider2D>();

    public bool IsGrounded => groundColliders.Count > 0;

    // Indica si el suelo detectado corresponde al objeto indicado
    // (por ejemplo, si Jerry está apoyado sobre Tomás).
    public bool IsGroundedOn(GameObject other)
    {
        foreach (Collider2D col in groundColliders)
        {
            if (col != null && col.gameObject == other)
            {
                return true;
            }
        }

        return false;
    }

    void Awake()
    {
        if (groundCollider == null)
        {
            groundCollider = GetComponent<BoxCollider2D>();
        }

        if (groundCollider.gameObject != gameObject || !groundCollider.isTrigger)
        {
            Debug.LogError("GroundCheck requiere un BoxCollider2D trigger en el mismo objeto.", this);
            enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        AddGroundCollider(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        AddGroundCollider(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        groundColliders.Remove(other);
    }

    void OnDisable()
    {
        groundColliders.Clear();
    }

    private void AddGroundCollider(Collider2D other)
    {
        if (!other.isTrigger && (groundLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            groundColliders.Add(other);
        }
    }
}
