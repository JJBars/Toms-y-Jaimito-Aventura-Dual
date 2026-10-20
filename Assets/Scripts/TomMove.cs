using UnityEngine;

public class TomMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GroundCheck groundCheck;

    public float salto = 5f;
    private Rigidbody2D body;
    private float horizontalInput;
    private bool jumpHeld;
    private bool jumpStarted;
    private bool hasLeftGround;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();

        if (groundCheck == null)
        {
            Debug.LogError("TomMove necesita una referencia a GroundCheck.", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }

        jumpHeld = Input.GetButton("Jump");

        if (Input.GetButtonUp("Jump") && body != null && body.linearVelocity.y > 0f)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, 0f);
        }

    }

    void FixedUpdate()
    {
        if (body == null || groundCheck == null)
        {
            return;
        }

        body.linearVelocity = new Vector2(horizontalInput * moveSpeed, body.linearVelocity.y);

        if (!groundCheck.IsGrounded)
        {
            if (jumpStarted)
            {
                hasLeftGround = true;
            }

            return;
        }

        if (jumpStarted && hasLeftGround)
        {
            jumpStarted = false;
            hasLeftGround = false;
        }

        if (jumpHeld && !jumpStarted)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, salto);
            jumpStarted = true;
        }
    }
}
