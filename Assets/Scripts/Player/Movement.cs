using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

public class Movement : MonoBehaviour
{
    [Header("Required")]
    public LayerMask groundMask;
    public Transform groundCheck;

    [Header("Jump")]
    public float jumpForce;
    public float jumpDirForce;

    [Header("Ground")]
    public float maxVelocity;
    public float walkSpeed;
    public float sprintMultiplier;
    public float groundDrag;

    [Header("Air")]
    public float airSpeed;
    public float airDrag;

    [Header("World")]
    public float gravity;

    [Header("Climbing")]
    public float maxClimbTime;
    public float maxWallDistance;
    public float wallInactiveTime;
    public float climbSpeed;
    public float climbDrag;

    private float _climbTime = 0f;
    private float _wallInactiveTimer = 0f;
    private bool _climbing;

    [Space]
    public LayerMask climbMask;
    public Transform climbAnchor;


    [Header("Climb Jump")]
    public float climbJumpForce;
    public UnityEvent onClimbJump;

    private PlayerCharacter _player;
    private Vector2 _moveIn;

    private Vector3 _slopeNormal;
    public Movestate movestate { get; private set; }

    [Header("Stamina Costs")]
    public float sprintCost;
    public float jumpCost;

    [Header("Dashing")]
    public float dashCost;
    public float dashForce;
    public float dashCooldown;

    private float _dashTimer; 

    //Private settings
    private float _speed;
    private float _drag;

    private bool _isSprinting;

    //Values for debugging
    private float _velocityMagnitude;

    private System.Action<Movestate> OnStateChange;
    public System.Action<float> OnDash; //Cooldown

    private Stat _externalMultiplier;

    //private settings
    public void Init(PlayerCharacter player)
    {
        _player = player;
        ChangeMoveState(Movestate.grounded);

        player.healingTool.OnCharge += movementMultiplier;

        _externalMultiplier = new Stat(1.0f);
    }

    private void movementMultiplier(bool start)
    {
        if (start)
        {
            _externalMultiplier.AddModifier(_player.healingTool.speedModifier);
        }
        else
        {
            _externalMultiplier.RemoveModifier(_player.healingTool.speedModifier);
        }
    }

    public void AddMovementMultiplier(StatModifier modifier, float duration = -1)
    {
        if (duration <= 0)
        {
            _externalMultiplier.AddModifier(modifier);
            return;
        }

        StartCoroutine(AddAndRemoveMovementMultiplierStatModifier(modifier, duration));
    }

    private IEnumerator AddAndRemoveMovementMultiplierStatModifier(StatModifier modifier, float duration)
    {
        _externalMultiplier.AddModifier(modifier);

        yield return new WaitForSeconds(duration);

        _externalMultiplier.RemoveModifier(modifier);
    }

    public void BindControls(Controls controls)
    {
        controls.Gameplay.Move.performed += SetMovementInfo;
        controls.Gameplay.Dash.performed += Dash;
        controls.Gameplay.Jump.performed += Jump;
        controls.Gameplay.Sprint.performed += _ => _isSprinting = true;
        controls.Gameplay.Sprint.canceled += _ => _isSprinting = false;
    }

    private void Dash(InputAction.CallbackContext ctx)
    {
        if (_dashTimer < dashCooldown) return;
        if (!_player.stats.ConsumeStamina(dashCost)) return;

        _dashTimer = 0;

        Vector3 moveDirection = _moveIn.y * transform.forward + _moveIn.x * transform.right;
        moveDirection = moveDirection.normalized;

        _player.rb.AddForce(moveDirection * dashForce, ForceMode.VelocityChange);
        OnDash?.Invoke(dashCooldown);
    }



    private void FixedUpdate()
    {
        if (!_player) return;

        GroundCheck();

        ApplyGravity();

        ApplyMovement();

        CheckWallClimb();
    }

    private void Update()
    {
        if (_dashTimer < dashCooldown) _dashTimer += Time.deltaTime;

        if (!_isSprinting) return;
        if (!_player.stats.ConsumeStamina(sprintCost * Time.deltaTime))
        {
            _isSprinting = false;
        }

    }

    private void ApplyGravity()
    {
        _player.rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration); // Additional Gravity for feel purpose.
    }

    private void CheckWallClimb()
    {
        if (movestate == Movestate.grounded) return;
        if (_wallInactiveTimer > wallInactiveTime) return;

        bool wallCheck = Physics.CheckBox(climbAnchor.position, new Vector3(maxWallDistance, .1f, maxWallDistance), Quaternion.identity, climbMask);

        if (wallCheck)
        {
            ChangeMoveState(Movestate.climbing);

            if (_isSprinting)
            {
                _wallInactiveTimer = 0f;
            }
        }

        if ((wallCheck && !_isSprinting))
            _wallInactiveTimer += Time.fixedDeltaTime;

        if (_wallInactiveTimer > wallInactiveTime || !wallCheck)
            ChangeMoveState(Movestate.aerial);

    }

    private void ApplyMovement()
    {
        Vector3 moveDirection = _moveIn.y * transform.forward + _moveIn.x * transform.right;
        moveDirection = moveDirection.normalized;

        Vector3 projectDirection = Vector3.ProjectOnPlane(moveDirection, _slopeNormal);

        float multiplier = (_isSprinting) ? sprintMultiplier : 1f;

        _player.rb.AddForce(projectDirection * _speed * multiplier * _externalMultiplier.Value, ForceMode.Acceleration);
    }

    private void GroundCheck()
    {
        if (Physics.SphereCast(groundCheck.position, .2f, Vector3.down, out RaycastHit hit, 1f, groundMask))
        {
            _slopeNormal = hit.normal;

            ChangeMoveState(Movestate.grounded);

            return;
        }

        _slopeNormal = Vector3.up;

        if (movestate == Movestate.grounded)
            ChangeMoveState(Movestate.aerial);
    }

    private void ChangeMoveState(Movestate state)
    {
        if (movestate == state) return;

        switch (state)
        {
            case (Movestate.none):
                _speed = 0f;
                _drag = 0f;
                break;
            case (Movestate.grounded):
                _speed = walkSpeed;
                _drag = groundDrag;
                _climbTime = 0f;
                _wallInactiveTimer = 0f;
                _player.rb.useGravity = true;
                break;
            case (Movestate.aerial):
                _speed = airSpeed;
                _drag = airDrag;
                _player.rb.useGravity = true;
                break;
            case (Movestate.climbing):
                _speed = climbSpeed;
                _drag = climbDrag;
                _climbTime = 0f;
                _wallInactiveTimer = 0f;
                _player.rb.useGravity = false;
                break;
        }

        movestate = state;

        if (_player.rb)
            _player.rb.drag = _drag;
    }

    private void SetMovementInfo(InputAction.CallbackContext context)
    {
        _moveIn = context.ReadValue<Vector2>();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (movestate == Movestate.aerial || movestate == Movestate.none) return;

        if (!_player.stats.ConsumeStamina(jumpCost)) return;

        Vector3 jumpDirectionalForce = Vector3.zero;

        switch (movestate)
        {
            case Movestate.grounded:
                jumpDirectionalForce = Vector3.up * jumpForce + jumpDirForce * (transform.forward * _moveIn.y + transform.right * _moveIn.x).normalized;
                break;
            case Movestate.climbing:
                jumpDirectionalForce = climbJumpForce * (_player.orientation.forward * _moveIn.y + _player.orientation.right * _moveIn.x).normalized;
                onClimbJump?.Invoke();
                break;
        }

        _player.rb.AddForce(jumpDirectionalForce, ForceMode.VelocityChange);
    }

}

public enum Movestate
{
    none,
    grounded,
    aerial,
    climbing
}
