using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Your solution must make use of the following fields. If these values are changed, even at runtime,
    /// the character controller should respect the new values and act as detailed in the Unity inspector.
    /// </summary>

    [SerializeField]
    private float m_jumpApexHeight;

    [SerializeField]
    private float m_jumpApexTime;

    [SerializeField]
    private float m_terminalVelocity;

    [SerializeField]
    private float m_coyoteTime;

    [SerializeField]
    private float m_jumpBufferTime;

    [SerializeField]
    private float m_accelerationTimeFromRest;

    [SerializeField]
    private float m_decelerationTimeToRest;

    [SerializeField]
    private float m_accelerationTimeFromQuickturn;

    [SerializeField]
    private float m_decelerationTimeFromQuickturn;

    [SerializeField]
    private float MaxHorizontalSpeed;

    public float MinJumpHeight;
    public float MaxHeldJump;
    Rigidbody2D m_rigidBody;

    
    Vector2 PlayerVector;
    public Vector2 direction;
    public float currentSpeed;
    float acceleration;

    public bool jumping = false;
    public bool ground;
    RaycastHit2D groundHit;
    RaycastHit2D ceilingHit;

    float groundTiming;
    float jumpHolding;
    public enum FacingDirection { Left, Right }

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        float gravity = 2 * m_jumpApexHeight / Mathf.Pow(m_jumpApexTime, 2);    
        if (jumping || (!IsGrounded() && !jumping))
        {
            float speed = m_rigidBody.velocity.y + (-gravity * Time.deltaTime);
            speed = Mathf.Clamp(speed, -m_terminalVelocity, speed);
            m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, speed);
            
        }
     
    }


    public void Update()
    {


        ground = IsGrounded();

        PlayerVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;


        //Horizontal Movement//
        if (Mathf.Abs(PlayerVector.x) > 0.5f)
        {
            
                acceleration = MaxHorizontalSpeed / m_accelerationTimeFromRest * Time.deltaTime;
                currentSpeed = currentSpeed + acceleration;
                currentSpeed = Mathf.Clamp(currentSpeed, 0, MaxHorizontalSpeed);
                m_rigidBody.velocity = new Vector2(PlayerVector.x * currentSpeed, m_rigidBody.velocity.y);
                direction = PlayerVector;
    
        }

        else if (Mathf.Abs(PlayerVector.x) < 0.5f )
        {
            acceleration = MaxHorizontalSpeed / m_decelerationTimeToRest * Time.deltaTime;
            currentSpeed = currentSpeed - acceleration;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, MaxHorizontalSpeed);
            m_rigidBody.velocity = new Vector2(direction.x * currentSpeed, m_rigidBody.velocity.y);
        }




        //Vertical Movement//
        if (IsGrounded()){
            jumping = false;
        }

            if (Input.GetButton("Jump"))
            {
                jumpHolding += Time.deltaTime;
            }
             
            if (Input.GetButtonUp("Jump"))
            {
           
                Jump(jumpHolding);

                jumpHolding = 0;
                   
                  if (!IsGrounded())
                   {
                     StartCoroutine(BufferJump());
                   }
            }
        




        //  ceilingHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - (transform.localScale.y / 2)), Vector2.up, m_rigidBody.velocity.y * Time.deltaTime, LayerMask.GetMask("Ground"));

        ceilingHit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y + (transform.localScale.y)), GetComponent<BoxCollider2D>().size, 0f, Vector2.up, m_rigidBody.velocity.y * Time.deltaTime, LayerMask.GetMask("Ground"));
        //Check Ceiling//
        if (ceilingHit.collider != null && !IsGrounded())
        {
            jumping = false;
            m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, 0f);
            SetGroundPosition(-ceilingHit.distance);    
        }
       
    }

    public void WalkingControl()
    {

    }

    public IEnumerator BufferJump()
    {
        float buffering = Time.time + m_jumpBufferTime;
        while(Time.time <= buffering)
        {
            Jump(jumpHolding);
            yield return new WaitForEndOfFrame();
        }
    }

    public void Jump(float timeHeld)
    {
        if (IsGrounded())
        {
            
            timeHeld = Mathf.Clamp(timeHeld, 0, MaxHeldJump);
            float percent = timeHeld / MaxHeldJump;
            float jumpPower = Mathf.Lerp(MinJumpHeight, m_jumpApexHeight, percent);
            m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, (2 * jumpPower) / m_jumpApexTime);
            jumping = true;
        }
    }


    public bool IsWalking()
    {
        if (Mathf.Abs(PlayerVector.normalized.x) > 0.1f)
        {       
            return true;
        }
        else return false;
        
      //  throw new System.NotImplementedException( "IsWalking in PlayerController has not been implemented." );
    }


    public bool IsGrounded()
    {

        // RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0f, Vector2.down, 0f, LayerMask.GetMask("Ground"));
        groundHit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y), GetComponent<BoxCollider2D>().size, 0f, Vector2.down, m_rigidBody.velocity.y * Time.deltaTime, LayerMask.GetMask("Ground")) ;
        RaycastHit2D testHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + (GetComponent<BoxCollider2D>().size.y)), Vector2.down, (GetComponent<BoxCollider2D>().size.y - 0.1f) + m_rigidBody.velocity.y * Time.deltaTime, LayerMask.GetMask("Ground"));
       
        if (groundHit.collider != null)
        {
           
            if (!jumping)
            {
                float clamp = testHit.distance - (GetComponent<BoxCollider2D>().size.y);
                if (testHit.distance > 0)
                {
                    SetGroundPosition(clamp);
                }
                if (m_rigidBody.velocity.y < 0)
                {
                    m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, 0f);                            
                }
            }
            
            groundTiming = Time.time + m_coyoteTime;
            //box cast out distance +- position;
            return true;
        }
       else if (Time.time <= groundTiming)
        {
            return true;
       }
    
        return false;
      //  throw new System.NotImplementedException( "IsGrounded in PlayerController has not been implemented." );
    }



    public void SetGroundPosition(float offset)
    {
        if (!jumping)
        {     
            m_rigidBody.position = new Vector2(m_rigidBody.position.x, m_rigidBody.position.y - offset);
            m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, 0);
        }
    }

    public FacingDirection GetFacingDirection()
    {
        if (direction.x < 0)
        {
            return FacingDirection.Left;
        }
        else
        {
            return FacingDirection.Right;
        }
      //  throw new System.NotImplementedException( "GetFacingDirection in PlayerController has not been implemented." );
    }

    // Add additional methods such as Start, Update, FixedUpdate, or whatever else you think is necessary, below.
}
