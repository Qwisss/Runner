using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    
    private float _laneChangeSpeed = 15f;
    private float _laneOffset = 2.5f;   
    private float _pointStart;
    private float _pointFinish;
    private float _lastVectorX;
    private float _jumpPower = 15f;
    private float _jumpGravity = -40f;
    private float _realGravity = -9.8f;   
    private int setTirggerRun;

    Animator animator;
   
    Vector3 startGamePosition;
    Vector3 targetVelocity;

    bool isJumping = false;
    bool isMoving = false;
    //bool isSlide = false;
    bool isStarted = false;
   

    Coroutine movingCoroutine;

    Rigidbody rigidBody;
    //Quaternion startGameRotation;

    private void Awake()
    {
        setTirggerRun = DataHolder.runIndexForPlayerController;       
    }

    private void Start()
    {    
        _laneOffset = MapGenerator.instance.laneOffset;
        FindObjectOfType<RoadGenerator>();
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        startGamePosition = transform.position;
        //startGameRotation = transform.rotation;
        /*SwipeSystem.instance.MoveEvent += MovePlayer;*/
        
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) && _pointFinish > -_laneOffset && isStarted)
        {
            MoveHorizontal(-_laneChangeSpeed);
        }
        if (Input.GetKeyUp(KeyCode.D) && _pointFinish < _laneOffset && isStarted)
        {
            MoveHorizontal(_laneChangeSpeed);
        }             

        if (Input.GetKeyUp(KeyCode.W) && isJumping == false && isStarted)
        {
            Jump();
            animator.SetTrigger("Jump");
        }
        if (Input.GetKeyUp(KeyCode.S) && isStarted)
        {
            animator.SetTrigger("Slide");
            
        }
    }
   

   /* private void MovePlayer(bool[] swipes)
    {
        if (swipes[(int)SwipeSystem.Direction.Left] ||Input.GetKeyUp(KeyCode.A) && _pointFinish > -_laneOffset)
        {
            MoveHorizontal(-_laneChangeSpeed);
        }
        if (swipes[(int)SwipeSystem.Direction.Right] || Input.GetKeyUp(KeyCode.D) && _pointFinish < _laneOffset)
        {
            MoveHorizontal(_laneChangeSpeed);
        }

        if (swipes[(int)SwipeSystem.Direction.Up] || Input.GetKeyUp(KeyCode.W) && isJumping == false )
        {
            Jump();
            animator.SetTrigger("Jump");
        }
        if (swipes[(int)SwipeSystem.Direction.Down] || Input.GetKeyUp(KeyCode.S))
        {
            animator.SetTrigger("Slide");
            //isSlide = true;
           
        }
    }*/
  

    private void Jump()
    {
        isJumping = true;
        rigidBody.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        Physics.gravity = new Vector3(0, _jumpGravity, 0);
        StartCoroutine(StopJumpCourutine());
    }

    IEnumerator StopJumpCourutine()
    {
        do
        {
            yield return new WaitForSeconds(0.02f);
        } while (rigidBody.velocity.y != 0);
        isJumping = false;
        Physics.gravity = new Vector3(0, _realGravity, 0);
    }



    private void MoveHorizontal(float speed)
    {
        animator.applyRootMotion = false; 
        _pointStart = _pointFinish;
        _pointFinish += Mathf.Sign(speed) * _laneOffset;

        if (isMoving) 
        { 
            StopCoroutine (movingCoroutine); isMoving = false; 
        }
        movingCoroutine = StartCoroutine(MoveCoroutine(speed));            
    }

    IEnumerator MoveCoroutine(float vectorX)
    {
        isMoving = true;
        while (Mathf.Abs(_pointStart - transform.position.x) < _laneOffset)
        {
            yield return new WaitForFixedUpdate();
            rigidBody.velocity = new Vector3(vectorX, rigidBody.velocity.y, 0);
            _lastVectorX = vectorX;
            float x = Mathf.Clamp(transform.position.x, Mathf.Min(_pointStart, _pointFinish), Mathf.Max(_pointStart, _pointFinish));
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
        rigidBody.velocity = Vector3.zero;
        transform.position = new Vector3(_pointFinish, transform.position.y, transform.position.z);
        if (transform.position.y > 1)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, -10, rigidBody.velocity.y);

        }
        isMoving = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ramp") 
        {
            rigidBody.constraints |= RigidbodyConstraints.FreezePositionZ;
        
        }
        if (other.gameObject.tag == "NotLose")
        {
            MoveHorizontal(-_lastVectorX);
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
            rigidBody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "RampPlane")
        {
            if (rigidBody.velocity.x == 0 && isJumping == false)
            {
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, -10, rigidBody.velocity.z);
            }
        }
    }
    public void StartRun()
    {
        if (setTirggerRun == 0)
        {
            animator.SetTrigger("FastRun");

        }
        if (setTirggerRun == 1)
        {
            animator.SetTrigger("DrunkRun");                      
        }
        if (setTirggerRun == 2)
        {          
            animator.SetTrigger("InjuredRun");           
        }
        isStarted = true;
    }
}
   

