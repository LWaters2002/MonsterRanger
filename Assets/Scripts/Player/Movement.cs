using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

using Cinemachine;


public class Movement : MonoBehaviour
{
    [Header("Required")]
    public LayerMask groundMask;
    public Transform groundCheck;
    public CinemachineVirtualCamera virtualCamera;

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

    [Header("Wall Jump")]
    public float wallJumpForce;
    public UnityEvent OnWallJump;

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
    public System.Action<float> OnDash;
    public System.Action<bool> OnSprint;
    public System.Action OnJump;


    private Stat _externalMultiplier;
    private CinemachineBasicMultiChannelPerlin _virtualNoise;

    //private settings
    public void Init(PlayerCharacter player)
    {
        _player = player;
        ChangeMoveState(Movestate.grounded);

        player.healingTool.OnCharge += movementMultiplier;
        GameManager.Get().OnCutsceneChange += _ => _moveIn = Vector2.zero;

        _externalMultiplier = new Stat(1.0f);

        _virtualNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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
        controls.Gameplay.Dash.canceled += StopSprint;
        controls.Gameplay.Jump.performed += Jump;
    }

    private void StopSprint(InputAction.CallbackContext obj)
    {
        _isSprinting = false;
        OnSprint?.Invoke(false);
    }

    private void Dash(InputAction.CallbackContext ctx)
    {
        //Cursed
        string interactionType = ctx.interaction.ToString();
        interactionType = interactionType.Substring(37);

        if (interactionType == "TapInteraction")
        {
            if (_dashTimer < dashCooldown) return;
            if (!_player.stats.ConsumeStamina(dashCost)) return;

            _dashTimer = 0;

            Vector3 moveDirection = _moveIn.y * transform.forward + _moveIn.x * transform.right;

            if (_moveIn == Vector2.zero) moveDirection = transform.forward;

            moveDirection = moveDirection.normalized;

            _player.rb.AddForce(moveDirection * dashForce, ForceMode.VelocityChange);
            OnDash?.Invoke(dashCooldown);
        }
        else if (interactionType == "HoldInteraction")
        {
            if (!_isSprinting) OnSprint?.Invoke(true);
            _isSprinting = true;
        }
    }

    private void FixedUpdate()
    {
        if (!_player) return;

        GroundCheck();

        ApplyGravity();

        ApplyMovement();
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


    private void ApplyMovement()
    {
        Vector3 moveDirection = _moveIn.y * transform.forward + _moveIn.x * transform.right;
        moveDirection = moveDirection.normalized;

        Vector3 projectDirection = Vector3.ProjectOnPlane(moveDirection, _slopeNormal);

        float multiplier = (_isSprinting) ? sprintMultiplier : 1f;

        float externalModifier = Mathf.Clamp(_externalMultiplier.Value, 0f, 100f);

        _player.rb.AddForce(projectDirection * _speed * multiplier * externalModifier, ForceMode.Acceleration);
    }

    private void GroundCheck()
    {
        if (Physics.SphereCast(groundCheck.position, .1f, Vector3.down, out RaycastHit hit, 1f, groundMask))
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
      //          _player.animator?.CrossFade("Landed", .2f, -1);
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
                jumpDirectionalForce = wallJumpForce * (_player.orientation.forward * _moveIn.y + _player.orientation.right * _moveIn.x).normalized;
                OnWallJump?.Invoke();
                break;
        }
        OnJump?.Invoke();
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
