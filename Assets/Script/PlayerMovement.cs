using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;

    public Rigidbody rb;

    Vector3 movement;

    public Animator animator;
    private bool isGrounded;
    public Collider col;
    public LayerMask layerMask;

    public static PlayerMovement instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            Debug.LogWarning("il y a plus d'une instance de PlayerMovement");
            return;
        }
        DontDestroyOnLoad(this);
        instance = this;
    }

    private void Update()
    {
        isGrounded = Physics.CheckBox(transform.position, new Vector3(col.bounds.extents.x, 0.1f, col.bounds.extents.z), transform.rotation, layerMask);
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        // Set Run direction animation
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.z);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Check if the character is not moving
        if (movement.sqrMagnitude == 0f)
        {
            // Get the last input direction released
            string lastReleasedKey = GetLastReleasedDirection();

            // Set Idle direction animation based on the last released key
            if (lastReleasedKey == "Left")
            {
                animator.SetBool("LeftDirection", true);
                animator.SetBool("RightDirection", false);
                animator.SetBool("DownDirection", false);
                animator.SetBool("UpDirection", false);
            }               
            else if (lastReleasedKey == "Right")
            {
                animator.SetBool("RightDirection", true);
                animator.SetBool("LeftDirection", false);
                animator.SetBool("DownDirection", false);
                animator.SetBool("UpDirection", false);
            }
            else if (lastReleasedKey == "Forward")
            {
                animator.SetBool("UpDirection", true);
                animator.SetBool("RightDirection", false);
                animator.SetBool("DownDirection", false);
                animator.SetBool("LeftDirection", false);
            }
            else if (lastReleasedKey == "Backward")
            {
                animator.SetBool("DownDirection", true);
                animator.SetBool("RightDirection", false);
                animator.SetBool("LeftDirection", false);
                animator.SetBool("UpDirection", false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (movement.x != 0 && movement.z !=0)
        {
            rb.MovePosition(rb.position + (movement / (float) 1.5f) * moveSpeed * Time.deltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
        }
    }

    private string GetLastReleasedDirection()
    {
        if (Input.GetKeyUp(KeyCode.Q))
            return "Left";
        else if (Input.GetKeyUp(KeyCode.D))
            return "Right";
        else if (Input.GetKeyUp(KeyCode.Z))
            return "Forward";
        else if (Input.GetKeyUp(KeyCode.S))
            return "Backward";

        return string.Empty;
    }

    public void StopMovement()
    {
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 0);
        animator.SetFloat("Speed", 0);
        animator.SetBool("UpDirection", true);
        enabled = false;
    }

    public bool GetisGrounded() => isGrounded;
    public Transform GetTransform() => transform;
}