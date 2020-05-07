using UnityEngine;

public class movementController : MonoBehaviour
{

    public Transform camera;
    public Rigidbody rb;

    public float sensitivity = 5f;
    public float cameraMinimumY = -60f;
    public float cameraMaximumY = 75f;
    public float rotationSmoothSpeed = 10f;

    public float walkSpeed = 9f;
    public float runSpeed = 14f;
    public float maxSpeed = 20f;
    public float crouchSpeed = 7f;
    public float jumpPower = 30;

    public float extraGravity = 45;

    float bodyRotationX;
    float camRotationY;
    Vector3 directionIntentX;
    Vector3 directionIntentY;
    float speed;

    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float fallMultiplier = 2.5f;

    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);

    private Vector3 playerScale;
    public bool crouching;
    private void Start()
    {
        playerScale = transform.localScale;
    }
    void Update()
    {
        BetterJump();
        LookRotation();
        Movement();
        ExtraGravity();
        GroundCheck();
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
    }
    void LookRotation()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //get camera and body rotation values
        bodyRotationX += Input.GetAxis("Mouse X") * sensitivity;
        camRotationY += Input.GetAxis("Mouse Y") * sensitivity;

        //clam camera rotation
        camRotationY = Mathf.Clamp(camRotationY, cameraMinimumY, cameraMaximumY);

        //create rotation targets and handle rotation of body
        Quaternion camTargetRotation = Quaternion.Euler(-camRotationY, 0, 0);
        Quaternion bodyTargetRotation = Quaternion.Euler(0, bodyRotationX, 0);

        // handle rotations
        transform.rotation = Quaternion.Lerp(transform.rotation, bodyTargetRotation, Time.deltaTime * rotationSmoothSpeed);

        camera.localRotation = Quaternion.Lerp(camera.localRotation, camTargetRotation, Time.deltaTime * rotationSmoothSpeed);
    }
    void StartCrouch()
    {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        crouching = true;
    }

    private void StopCrouch()
    {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        crouching = false;
    }
    void Movement()
    {
        directionIntentX = camera.right;
        directionIntentX.y = 0;

        directionIntentY = camera.forward;
        directionIntentY.y = 0;
        float multiplyer1 = 1f;
        float multiplyer2 = 1f;
        if (!isGrounded)
        {
            multiplyer1 = 1f;
            multiplyer2 = 0.5f;
        }
        Vector3 amountMoveX = directionIntentX * Input.GetAxis("Horizontal") * multiplyer2;
        Vector3 amountMoveY = directionIntentY * Input.GetAxis("Vertical") ;



        //change our characters velocity in this direction

        Vector3 vectorTest = Vector3.ClampMagnitude(amountMoveY + amountMoveX, 1f) * speed * multiplyer1;
        rb.velocity = new Vector3(vectorTest.x, 0f, vectorTest.z)+ Vector3.up * rb.velocity.y;

        //control our speed based on our movement state
        if (Input.GetKey(KeyCode.LeftShift) && !crouching)
        {
            speed = runSpeed;
        }
        else if (crouching)
        {
            speed = crouchSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
    }

    void ExtraGravity()
    {
        rb.AddForce(Vector3.down * extraGravity);
    }
    void GroundCheck()
    {
        RaycastHit groundHit;
        isGrounded = Physics.Raycast(transform.position, -transform.up, out groundHit, 1.25f);
    }
    void Jump()
    { 
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
    void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }
}
