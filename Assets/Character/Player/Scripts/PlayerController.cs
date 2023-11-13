using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using Cinemachine;
using CodeMonkey.Utils;
using JetBrains.Annotations;
using UnityEditor.SearchService;
using UnityEngine;
using V_ObjectSystem;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private FieldOfView fieldOfView;
    public Rigidbody2D _rigidbody;
    public Animator animator;
    public string movementState;
    [SerializeField] private float _speed = 3;
    public int staminaBarNum;
    public bool isRunning;

    void Start() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        
        Vector3 targetPosition = UtilsClass.GetMouseWorldPosition();
        Vector3 playerPosition = transform.position;
        Vector3 aimDir = (targetPosition - playerPosition).normalized;
        fieldOfView.SetAimDirection(aimDir);
        fieldOfView.SetOrigin(playerPosition);
        staminaBarNum = StaminaBarScript.instance.currentStamina;
        Move();
        
    }


    private void Move() {

        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
        Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow);
        // Check if either Shift key is pressed (left or right)
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (isMoving && staminaBarNum > 0) {
                // Character is running.
                isRunning = true;
                var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                _rigidbody.velocity = (input.normalized * _speed) * 1.8f;
                // Set anim speed
                animator.SetFloat("Speed", 3f);
                // This script should drain the stamina bar.
                StaminaBarScript.instance.UseStamina(2);
                movementState = "Run";
            }
            else if (isMoving && staminaBarNum == 0)
            {
                isRunning = false;
                // Forces you to walk.
                var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                _rigidbody.velocity = (input.normalized * _speed) * 0.9f;
                // Set anim speed
                animator.SetFloat("Speed", 2f);
                movementState = "Walking (Trying To Run)";
            }
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) 
        {
            isRunning = false;
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
        else if (!(Input.GetKey(KeyCode.LeftControl)) || !(Input.GetKey(KeyCode.RightControl)) && !(Input.GetKey(KeyCode.LeftShift)) || !(Input.GetKey(KeyCode.RightShift)))
        {
            isRunning = false;
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
            isRunning = false;
            // Character is idle.
            animator.SetFloat("Speed", 0f);
            movementState = "Idle";
        }
    }
}
