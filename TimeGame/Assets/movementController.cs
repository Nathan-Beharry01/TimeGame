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
    public float maxWalkSpeed = 20f;
    public float maxRunSpeed = 25f;
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

    void Update()
    {
        LookRotation();
        Movement();
        ExtraGravity();
        GroundCheck();
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
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

    void Movement()
    {
        directionIntentX = camera.right;
        directionIntentX.y = 0;

        directionIntentY = camera.forward;
        directionIntentY.y = 0;
        float multiplyer1 = 1f;
        if (!isGrounded)
        {
            multiplyer1 = 0.5f;
        }

        //change our characters velocity in this direction
        rb.velocity = directionIntentY * Input.GetAxis("Vertical") * speed * multiplyer1 + directionIntentX * Input.GetAxis("Horizontal") * speed * multiplyer1 + Vector3.up * rb.velocity.y;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        

       
        //control our spped based on our movement state
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
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
}
