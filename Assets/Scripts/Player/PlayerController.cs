using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] private float _speed;
    //[SerializeField] private CharacterController _characterController;

    [SerializeField] private float laneOffset = 2.5f;
    [SerializeField] private float laneChangeSpeed = 15f;

    Animator animator;

    Vector3 targetPosition;
    Vector3 startGamePosition;

    Quaternion startGameRotation;


    private void Start()
    {
        FindObjectOfType<RoadGenerator>();
        animator = GetComponent<Animator>();
        startGamePosition = transform.position;
        startGameRotation = transform.rotation;
        targetPosition = transform.position;

    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) && targetPosition.x < -laneOffset)
        { 
            targetPosition = new Vector3(targetPosition.x - laneOffset, transform.position.y, transform.position.z);      
        }
        if (Input.GetKeyUp(KeyCode.D) && targetPosition.x < laneOffset)
        {
            targetPosition = new Vector3(targetPosition.x + laneOffset, transform.position.y, transform.position.z);
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime);

        if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetTrigger("Jump");
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetTrigger("Slide");
        }
    }

    public void StartRun()
    {
        animator.SetTrigger("Run");
       
    }
}
