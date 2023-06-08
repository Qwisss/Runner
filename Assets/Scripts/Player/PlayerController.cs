using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float laneOffset = 2.5f;
    [SerializeField] private float laneChangeSpeed = 15f;

    private float timeElapsed;
    private float lerpDuration = 0.5f;
    private float pointStart;
    private float pointFinish;
    private float lastVectorX;
    private float jumpPower = 15f;
    private float jumpGravity = -40;
    private float realGravity = -9.8f;
    

    Animator animator;
   
    Vector3 startGamePosition;
    Vector3 targetVelocity;

    bool isJumping = false;
    bool isMoving = false;
    bool isSlide = false;

    Coroutine movingCoroutine;

    Rigidbody rb;
    //Quaternion startGameRotation;


    private void Start()
    {
        laneOffset = MapGenerator.instance.laneOffset;
        FindObjectOfType<RoadGenerator>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        startGamePosition = transform.position;
        //startGameRotation = transform.rotation;
        SwipeSystem.instance.MoveEvent += MovePlayer;
        
        

    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) && pointFinish > -laneOffset)
        {
            MoveHorizontal(-laneChangeSpeed);
        }
        if (Input.GetKeyUp(KeyCode.D) && pointFinish < laneOffset)
        {
            MoveHorizontal(laneChangeSpeed);
        }             

        if (Input.GetKeyUp(KeyCode.W) && isJumping == false)
        {
            Jump();
            //animator.SetTrigger("Jump");
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetTrigger("Slide");
            
        }
    }

    private void MovePlayer(bool[] swipes)
    {
        if (swipes[(int)SwipeSystem.Direction.Left] ||Input.GetKeyUp(KeyCode.A) && pointFinish > -laneOffset)
        {
            MoveHorizontal(-laneChangeSpeed);
        }
        if (swipes[(int)SwipeSystem.Direction.Right] || Input.GetKeyUp(KeyCode.D) && pointFinish < laneOffset)
        {
            MoveHorizontal(laneChangeSpeed);
        }

        if (swipes[(int)SwipeSystem.Direction.Up] || Input.GetKeyUp(KeyCode.W) && isJumping == false)
        {
            Jump();
            //animator.SetTrigger("Jump");
        }
        if (swipes[(int)SwipeSystem.Direction.Down] || Input.GetKeyUp(KeyCode.S))
        {
            animator.SetTrigger("Slide");
            isSlide = true;
           
            

        }
    }
  

    private void Jump()
    {
        isJumping = true;
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        Physics.gravity = new Vector3(0, jumpGravity, 0);
        StartCoroutine(StopJumpCourutine());
    }

    IEnumerator StopJumpCourutine()
    {
        do
        {
            yield return new WaitForSeconds(0.02f);
        } while (rb.velocity.y != 0);
        isJumping = false;
        Physics.gravity = new Vector3(0, realGravity, 0);
    }



    private void MoveHorizontal(float speed)
    {
        animator.applyRootMotion = false; 
        pointStart = pointFinish;
        pointFinish += Mathf.Sign(speed) * laneOffset;

        if (isMoving) 
        { 
            StopCoroutine (movingCoroutine); isMoving = false; 
        }
        movingCoroutine = StartCoroutine(MoveCoroutine(speed));
            
    }

    IEnumerator MoveCoroutine(float vectorX)
    {
        isMoving = true;
        while (Mathf.Abs(pointStart - transform.position.x) < laneOffset)
        {
            yield return new WaitForFixedUpdate();

            rb.velocity = new Vector3(vectorX, rb.velocity.y, 0);
            lastVectorX = vectorX;
            float x = Mathf.Clamp(transform.position.x, Mathf.Min(pointStart, pointFinish), Mathf.Max(pointStart, pointFinish));
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(pointFinish, transform.position.y, transform.position.z);
        if (transform.position.y > 1)
        {
            rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.y);

        }
        isMoving = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ramp") 
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionZ;
        
        }
        if (other.gameObject.tag == "NotLose")
        {
            MoveHorizontal(-lastVectorX);
        }
        if (other.gameObject.tag == "Lose")
        {
            FindObjectOfType<GamePause>().RestartGame();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
       

    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "RampPlane")
        {
            if (rb.velocity.x == 0 && isJumping == false)
            {
                rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
            }
        }
    }
    public void StartRun()
    {
        animator.SetTrigger("Run");
       
    }
   
}
