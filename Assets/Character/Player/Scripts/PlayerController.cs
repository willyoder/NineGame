using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public Animator animator;
    public string movementState;
    [SerializeField] private float _speed = 3;

    void Start() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        Move();
    }


    private void Move() {

        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
        Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow);
        // Check if either Shift key is pressed (left or right)
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // Character is running.
            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            _rigidbody.velocity = (input.normalized * _speed) * 1.6f;

            if (isMoving) {
                animator.SetFloat("Speed", 3f);
            } else {
                animator.SetFloat("Speed", 0);
            }
            movementState = "Run";
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) 
        {
            // Character is crouching.
            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            _rigidbody.velocity = (input.normalized * _speed) * 0.5f;
            if (isMoving) {
                animator.SetFloat("Speed", 1f);
            } else {
                animator.SetFloat("Speed", 0);
            }
            movementState = "Crouch";
        }
        else if (!(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && !(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            // Character is walking.
            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            _rigidbody.velocity = (input.normalized * _speed) * 0.9f;
            if (isMoving) {
                animator.SetFloat("Speed", 2f);
            } else {
                animator.SetFloat("Speed", 0);
            }
            movementState = "Walk";
        } else {
            // Character is idle.
            animator.SetFloat("Speed", 0f);
            movementState = "Idle";
        }
    }
}
