using UnityEngine;

public class JerryMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GroundCheck groundCheck;

    public float salto = 7f;
    private Rigidbody2D body;
    private RiderStack rider;
    private float horizontalInput;
    private bool jumpHeld;
    private bool jumpStarted;
    private bool hasLeftGround;

    private bool IsRiding => rider != null && rider.IsRiding;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        rider = GetComponent<RiderStack>();

        if (groundCheck == null)
        {
            Debug.LogError("JerryMove necesita una referencia a GroundCheck.", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRiding)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
        }

        jumpHeld = Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKeyUp(KeyCode.UpArrow) && body != null && body.linearVelocity.y > 0f)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, 0f);
        }
    }

    void FixedUpdate()
    {
        if (body == null || groundCheck == null || IsRiding)
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
