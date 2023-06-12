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

    private Animator _animator;
    
    private Vector3 _startGamePosition;
    private Vector3 _targetVelocity;

    private bool _isJumping = false;
    private bool _isMoving = false;
    private bool _isSliding = false;
    private bool _isStarted = false;
    
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
        _animator = GetComponent<Animator>();
        _startGamePosition = transform.position;
        //startGameRotation = transform.rotation;
        /*SwipeSystem.instance.MoveEvent += MovePlayer;*/          
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) && _pointFinish > -_laneOffset && _isStarted)
        {
            MoveHorizontal(-_laneChangeSpeed);
        }
        if (Input.GetKeyUp(KeyCode.D) && _pointFinish < _laneOffset && _isStarted)
        {
            MoveHorizontal(_laneChangeSpeed);
        }             

        if (Input.GetKeyUp(KeyCode.W) && _isJumping == false && _isStarted)
        {
            _animator.SetTrigger("Jump");
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.S) && _isStarted)
        {
            if (!_isSliding)
            {
                _animator.SetTrigger("Slide");
                _isSliding = true;                            
            }
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
        _isJumping = true;
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
        _isJumping = false;
        Physics.gravity = new Vector3(0, _realGravity, 0);
    }



    private void MoveHorizontal(float speed)
    {
        _animator.applyRootMotion = false; 
        _pointStart = _pointFinish;
        _pointFinish += Mathf.Sign(speed) * _laneOffset;

        if (_isMoving) 
        { 
            StopCoroutine (movingCoroutine); _isMoving = false; 
        }
        movingCoroutine = StartCoroutine(MoveCoroutine(speed));            
    }

    IEnumerator MoveCoroutine(float vectorX)
    {
        _isMoving = true;
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
        _isMoving = false;
    }

    public void OnSlideAnimationComplete()
    {       
        _isSliding = false;       
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
        if (other.gameObject.tag == "TopLose" & _isSliding == false) 
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
            if (rigidBody.velocity.x == 0 && _isJumping == false)
            {
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, -10, rigidBody.velocity.z);
            }
        }
    }
    public void StartRun()
    {
        if (setTirggerRun == 0)
        {
            _animator.SetTrigger("FastRun");

        }
        if (setTirggerRun == 1)
        {
            _animator.SetTrigger("DrunkRun");                      
        }
        if (setTirggerRun == 2)
        {          
            _animator.SetTrigger("InjuredRun");           
        }
        _isStarted = true;
        
    }
    
}
   

