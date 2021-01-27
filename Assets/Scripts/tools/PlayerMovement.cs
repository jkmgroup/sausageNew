using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Tools/Player movement")]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
  [Tooltip("Player move speed.")]
  [SerializeField]
  private float moveSpeed = 6.0f;

  [Tooltip("Player rotation speed.")]
  [SerializeField]
  private float rotationSpeed = 180.0f;

  [Tooltip("Gravity.")]
  [SerializeField]
  private float gravity = -9.8f;

  [Tooltip("Minimum speed to go down after jump.")]
  [SerializeField]
  private float terminalVelocity = -20.0f;

  [Tooltip("Fall in every step.")]
  [SerializeField]
  private float minFall = -1.5f;

  [Tooltip("Jump speed/power.")]
  [SerializeField]
  private float jumpSpeed = 6.0f;


  private CharacterController _characterController;
  private ControllerColliderHit _contact;
  private float _vertSpeed = 0.0f;

  void Start()
  {
    _characterController = GetComponent<CharacterController>();
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
  }

  void Update()
  {
    UpdateRotation();
    UpdateMove();
  }

  private void UpdateRotation()
  {
    var horInput = Input.GetAxis("Mouse X") * rotationSpeed;
    if (horInput == 0)
      return;
    var angle = transform.rotation.eulerAngles;
    angle.y += horInput * Time.deltaTime;
    transform.rotation = Quaternion.Euler(angle);
  }

  private void UpdateMove()
  {
    var vertInput = Input.GetAxis("Vertical");
    var horInput = Input.GetAxis("Horizontal");

    var movement = transform.forward * moveSpeed * vertInput;
    movement += transform.right * moveSpeed * horInput;

    var hitGround = false;
    RaycastHit hit;
    if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
    {
      if (!hit.collider.isTrigger)
      {
        float check = (_characterController.height + _characterController.radius) / 1.9f;
        hitGround = hit.distance <= check;  
      }
    }

    if (hitGround)
    {
      if (Input.GetButtonDown("Jump"))
        _vertSpeed = jumpSpeed;
      else
        _vertSpeed = minFall;
    }
    else
    {
      _vertSpeed += gravity * 5 * Time.deltaTime;
      if (_vertSpeed < terminalVelocity)
        _vertSpeed = terminalVelocity;

      if (_characterController.isGrounded)
      {
        if (_contact == null)
          Debug.LogError("Something wrong! _contact can be null");
        else
        { 
          if (Vector3.Dot(movement, _contact.normal) < 0)
            movement = _contact.normal * moveSpeed;
          else
            movement += _contact.normal * moveSpeed;
        }
      }
    }
    movement.y = _vertSpeed;

    _characterController.Move(movement * Time.deltaTime);
  }


  void OnControllerColliderHit(ControllerColliderHit hit)
  {
    _contact = hit;
  }

  [ExecuteInEditMode]
  private void OnValidate()
  {
    if (moveSpeed <= 0.0f) 
    {
      Debug.LogWarning("moveSpeed in PlayerMovement (" + name + ") must be more then 0.0f. Value was changed to 5.0f!");
      moveSpeed = 5.0f;
    }

    if (rotationSpeed <= 0)
    {
      Debug.LogWarning("rotationSpeed in PlayerMovement (" + name + ") must be more then 0.0f. Value was changed to 180.0f!");
      rotationSpeed = 180.0f;
    }

    if (jumpSpeed <= 0)
    {
      Debug.LogWarning("jumpSpeed in PlayerMovement (" + name + ") must be more then 0.0f. Value was changed to 5.0f!");
      jumpSpeed = 6.0f;
    }

    if (gravity >= 0)
    {
      Debug.LogWarning("gravity in PlayerMovement (" + name + ") must be less then 0.0f. Value was changed to -9.8f!");
      gravity = -9.8f;
    }
    if (terminalVelocity >= 0)
    {
      Debug.LogWarning("terminalVelocity in PlayerMovement (" + name + ") must be less then 0.0f. Value was changed to -20.0f!");
      terminalVelocity = -20.0f;
    }

    if (minFall >= 0)
    {
      Debug.LogWarning("minFall in PlayerMovement (" + name + ") must be less then 0.0f. Value was changed to -1.5f!");
      minFall = -1.5f;
    }
  }
}
