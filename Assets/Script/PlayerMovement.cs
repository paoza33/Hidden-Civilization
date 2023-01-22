using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    private Vector3 velocity = new Vector3(0,1,0);
    private float horizontalMovement;
    private float verticalMovement;
    private float vAxis;
    private float hAxis;
    private bool inputLeft, inputRight, inputForward, inputBackward, inputV, inputH = false;
    public float moveSpeed;
    public float jumpForce;
    public Animator animator;
    private bool isJump;
    private bool canJump;
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
    
    void Update()
    {

        if (Input.GetButtonDown("Backward")) { inputBackward = true; }
        if (Input.GetButtonUp("Backward")) { inputBackward = false; }

        if (Input.GetButtonDown("Forward")) { inputForward = true; }
        if (Input.GetButtonUp("Forward")) { inputForward = false; }

        if (Input.GetButtonDown("Left")) { inputLeft = true; }
        if (Input.GetButtonUp("Left")) { inputLeft = false; }

        if (Input.GetButtonDown("Right")) { inputRight = true; }
        if (Input.GetButtonUp("Right")) { inputRight = false; }

        inputV = inputForward || inputBackward;
        inputH = inputLeft || inputRight;


        if (inputBackward) { vAxis = -1; }
        else if (inputForward) { vAxis = 1; }

        if (inputLeft) { hAxis = -1; }
        if (inputRight) { hAxis = 1; }

        horizontalMovement = hAxis * moveSpeed * Time.fixedDeltaTime;
        verticalMovement = vAxis * moveSpeed * Time.fixedDeltaTime;
        if (Input.GetButtonDown("Jump")) { isJump = true; }
        animator.SetBool("InputPressed", inputLeft || inputRight || inputForward || inputBackward);
    }

    private void FixedUpdate()
    {
        // isGrounded is true if the CheckBox had a collision with object with this layerMask
        isGrounded = Physics.CheckBox(transform.position, new Vector3(col.bounds.extents.x, 0.1f, col.bounds.extents.z), transform.rotation, layerMask);
        AddPlayerMovement();
        PlayerRotation(hAxis, vAxis);
        if (isJump && isGrounded && canJump)
        { // Jump
            Vector3 vel = rb.velocity;
            vel.y += (float)100.0 * Time.fixedDeltaTime;
            rb.velocity = vel;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            isJump = false;
        }
    }

    /// <summary>
    /// Add force to the Player.
    /// </summary>
    private void AddPlayerMovement()
    {
        Vector3 movement = new Vector3(horizontalMovement, rb.velocity.y, verticalMovement);
        if (inputV && inputH)
        {
            movement = new Vector3(horizontalMovement / 1.3f, rb.velocity.y, verticalMovement / 1.3f);
        }
        else if (inputV && !inputH)
        {
            movement = new Vector3(0, rb.velocity.y, verticalMovement);
        }
        else if (inputH && !inputV)
        {
            movement = new Vector3(horizontalMovement, rb.velocity.y, 0);
        }
        else
        {
            movement.x = 0;
            movement.z = 0;
        }
        rb.velocity = Vector3.SmoothDamp(rb.velocity, movement, ref velocity, 0);
    }

    /// <summary>
    /// Make a rotation to the Player. The degres of rotation depend of the value of parameters.
    /// </summary>
    /// <param name="_horizontal"></param>
    /// <param name="_vertical"></param>
    private void PlayerRotation(float _horizontal, float _vertical) //Première solution mais à optimiser
    {
        if (_horizontal < 0 && inputH)
        {
            if(_vertical < 0 && inputV)
            {
                Vector3 rotation = new Vector3(0,-135,0);
                transform.rotation = Quaternion.Euler(rotation);
            }
            else if(_vertical > 0 && inputV)
            {
                Vector3 rotation = new Vector3(0, -45, 0);
                transform.rotation = Quaternion.Euler(rotation);
            }
            else
            {
                Vector3 rotation = new Vector3(0, -90, 0);
                transform.rotation = Quaternion.Euler(rotation);
            }
        }
        else if(_horizontal > 0 && inputH)
        {
            if (_vertical < 0 && inputV)
            {
                Vector3 rotation = new Vector3(0, 135, 0);
                transform.rotation = Quaternion.Euler(rotation);
            }
            else if (_vertical > 0 && inputV)
            {
                Vector3 rotation = new Vector3(0, 45, 0);
                transform.rotation = Quaternion.Euler(rotation);
            }
            else
            {
                Vector3 rotation = new Vector3(0, 90, 0);
                transform.rotation = Quaternion.Euler(rotation);
            }
        }
        else
        {
            if(_vertical > 0 && inputV)
            {
                Vector3 rotation = new Vector3(0, 0, 0);
                transform.rotation = Quaternion.Euler(rotation);
            }
            else if(_vertical < 0 && inputV)
            {
                Vector3 rotation = new Vector3(0, 180, 0);
                transform.rotation = Quaternion.Euler(rotation);
            }
        }
    }
    public bool GetisGrounded() => isGrounded;
    public void SetCanJump(bool _canJump) { canJump = _canJump; }
    public Transform GetTransform() => transform;
}
