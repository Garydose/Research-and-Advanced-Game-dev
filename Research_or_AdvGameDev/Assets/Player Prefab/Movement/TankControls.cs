using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControls : MonoBehaviour
{

    //public float _speed = 10;
    public float _rotationSpeed = 180;
    //[SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private Vector3 moveDirection;
    private Vector3 velocity;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Animator animator;
    public GameObject Fireball = null;

    private Vector3 rotation;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        Move();
        // Cast fireball
        if (Input.GetKeyDown("r"))
            Instantiate(Fireball, transform);

    }

    private void Move()
    {
        this.rotation = new Vector3(0, Input.GetAxisRaw("Horizontal") * _rotationSpeed * Time.deltaTime, 0);
        this.transform.Rotate(this.rotation);
        //check to see if we are on the ground
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        //"stop" applying gravity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float moveZ = Input.GetAxis("Vertical");
        moveDirection = new Vector3(0, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            //WALK CASE
            Walk();
        }
        else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            //RUN CASE
            Run();
        }
        else if (moveDirection == Vector3.zero)
        {
            Idle();
        }
        /*if (isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }  */
        //handle player input based movement

        //handle gravity based movement
        velocity.y += gravity * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime);

    }

    private void Walk()
    {
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        //this.rotation = new Vector3(0, Input.GetAxisRaw("Horizontal") * _rotationSpeed * Time.deltaTime, 0);

        Vector3 move = new Vector3(0, 0, Input.GetAxisRaw("Vertical") * Time.deltaTime);
        move = this.transform.TransformDirection(move);
        _controller.Move(move * walkSpeed);
        //this.transform.Rotate(this.rotation);
    }
    private void Run()
    {
        Vector3 move = new Vector3(0, 0, Input.GetAxisRaw("Vertical") * Time.deltaTime);
        animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
        move = this.transform.TransformDirection(move);
        _controller.Move(move * runSpeed);
    }
    private void Idle()
    {
        animator.SetFloat("Speed", 0.0f, 0.1f, Time.deltaTime);
    }
}