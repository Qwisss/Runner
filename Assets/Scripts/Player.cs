using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private CharacterController _characterController;

    Animator animator;

    Vector3 targetPosition;
    Vector3 startGamePosition;
    Quaternion startGameRotation;
   

    private void Start()
    {
        animator = GetComponent<Animator>();
        startGamePosition = transform.position;
        startGameRotation = transform.rotation;
        targetPosition = transform.position;

    }
    private void Update()
    {
        _characterController.Move(Vector3.right * _speed * Input.GetAxis("Horizontal") * Time.deltaTime);

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
