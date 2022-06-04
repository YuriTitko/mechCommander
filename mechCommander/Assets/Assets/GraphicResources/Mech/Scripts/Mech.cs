using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private float movementSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;

    [SerializeField] private Transform cam;
    
    private Vector3 moveDirection;
    private Vector3 velocity; // we can also store our upDown movement into moveDirection, but it's better to have separated vars for both

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDist;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    [SerializeField] private bool isStuck;
    [SerializeField] private float stuckCheckDist;
    [SerializeField] private Transform head;

    [SerializeField] private float maxFuel = 4f;
    [SerializeField] private float curFuel;
    [SerializeField] private float thrustForce = 0.5f;
    [SerializeField] private Rigidbody rigid;
    // [SerializeField] private Transform groundedTransform;
    [SerializeField] private ParticleSystem effect01;
    [SerializeField] private ParticleSystem effect02;

    // [SerializeField] private float doubleTapTime = 1f;
    // [SerializeField] private float elapsedTime;
    // [SerializeField] private int pressCount;

    // REFERENCES
    private CharacterController controller;
    private Animator anim;

    private void Start()
    {
        effect01.Stop();
        effect02.Stop();
        // Cursor.visible = false; //makes cursor invisible
        // Cursor.lockState = CursorLockMode.Locked;
        curFuel = maxFuel;
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>(); // in children - let us find the animator component
    }

    private void Update()
    {
        Move(); //let move function initialize every frame
        // HandleRotationInput();
        
        if (PauseMenu.GamePaused == false)
        {
            HandleShootInput();
        }

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            Attack();
        }
    }

    
    
    // private void resetPressTimer() //reset the press count & timer
    // {
    // pressCount = 0;
    // elapsedTime = 0;
    // }

    private void Move()
    {
        isStuck = Physics.CheckSphere(head.position, stuckCheckDist, groundMask);
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDist, groundMask); 
        //transform.position - characters pivot
        //groundCheckDist - radius of sphere around this pivot
        //groundMask - Layer in Unity with objects which gonna make our character grounded

        if(isGrounded && velocity.y < 0) //stops gravity if we grounded
        {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical"); // when w pressed equals 1, when s equals 0
        float moveX = Input.GetAxis("Horizontal"); 

        moveDirection = new Vector3(moveX, 0f, moveZ).normalized; // to make sure that if we press two buttons at once we won't double speed
        //moveDirection = transform.TransformDirection(moveDirection); // from local to world

        if(moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // 
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // here we get turn angled smoothed
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * movementSpeed * Time.deltaTime); // Time.deltaTime - no matter how many frames do we have we will still move with the same amount
        }

        if(isGrounded) 
        {
            Landing();

            if(moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if(moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if(moveDirection == Vector3.zero)
            {
                Idle(); //!!MAYBE IDLE SHOULD BE THE FIRST ON IN IF STATEMENT TO GET CLEAR COMPILATION
            }
            
            //moveDirection *= movementSpeed;

            if(Input.GetKeyDown(KeyCode.Space))
            {
                // pressCount++;
                Jump();
                return;
                // if(pressCount > 0) // if they pressed at least once
                // {
                // elapsedTime += Time.deltaTime; // count the time passed

                //     if(elapsedTime > doubleTapTime) // if the time elapsed is greater than the time limit
                //     {
                //     resetPressTimer();
                //     }
                //     else if(pressCount == 2) // otherwise if the press count is 2
                //     {
                //     // double pressed within the time limit
                //     // do stuff
                //     curFuel -= Time.deltaTime;
                //     rigid.AddForce(rigid.transform.up * thrustForce, ForceMode.Impulse);
                //     effect01.Play();
                //     effect02.Play();
                //     resetPressTimer();
                //         if(isGrounded && curFuel < maxFuel)
                //         {
                //         curFuel += Time.deltaTime;
                //         effect01.Stop();
                //         effect02.Stop();
                //         }
                //         else
                //         {
                //         effect01.Stop();
                //         effect02.Stop(); 
                //         }
                //     }
                // }
            }

            if(Input.GetKey(KeyCode.LeftControl))
            {
                Crawl();
            }
            else if(isStuck)
            {
                Crawl();
            }
            else
            {
                anim.SetBool("Crawl", false);
                controller.height = 2f;
                controller.center = new Vector3(0f, 1f, 0.2f);
            }
            
            curFuel = Mathf.Min(maxFuel, curFuel + Time.deltaTime*100); //returns the smaller of two numbers
        }
        else
        {
            if(Input.GetKey(KeyCode.Space) && curFuel > 0f)  //was Input.GetAxis("Jump")
            {
                curFuel -= Time.deltaTime;
                velocity.y += Mathf.Sqrt(thrustForce * -1f * gravity);
                controller.Move(velocity * Time.deltaTime);
                effect01.Play();
                effect02.Play();
            }
            
            else
            {
                effect01.Stop();
                effect02.Stop();
                velocity.y += gravity * Time.deltaTime; //calculate gravity
                controller.Move(velocity * Time.deltaTime); // apply gravity to the character
            }  
            velocity.y += gravity * Time.deltaTime; //calculate gravity
            controller.Move(velocity * Time.deltaTime); // apply gravity to the character
        }
    }

    private void Idle() 
    {
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        movementSpeed = walkSpeed;
        anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Run()
    {
        movementSpeed = runSpeed;
        anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }

    private void CrawlIdle()
    {
        anim.SetBool("Crawl", true);
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }
    private void Crawl()
    {
        movementSpeed = walkSpeed;
        anim.SetBool("Crawl", true);
        anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        controller.height = 1.4f;
        controller.center = new Vector3(0f, 0.7f, 0.2f);
    }
    private void Jump()
    {
        anim.SetBool("Grounded", false);
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }
    
    private void Fall()
    {
        anim.SetBool("Grounded", false);
    }

    private void Landing()
    {
        anim.SetBool("Grounded", true);
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
    }

    //SHOOTING
    // private void HandleRotationInput()
    // {
    //     RaycastHit _hit;
    //     Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //     if(Physics.Raycast(_ray, out _hit))
    //     {
    //         transform.LookAt(new Vector3(_hit.point.x, transform.position.y, _hit.point.z));
    //     }
    // }
    private void HandleShootInput()
    {
        if(Input.GetButton("Fire1"))
        {
            Weapon.Instance.Shoot();
        }
    }
}