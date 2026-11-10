using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JerryMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D body;
    private float horizontalInput;
    private bool isGrounded;
    private bool jumpRequested;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        Vector2 velocity = body.linearVelocity;
        velocity.x = horizontalInput * moveSpeed;

        if (jumpRequested && isGrounded)
        {
            velocity.y = jumpForce;
            isGrounded = false;
        }

        body.linearVelocity = velocity;
        jumpRequested = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.position.y < transform.position.y && body.linearVelocity.y <= 0f)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
