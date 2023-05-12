using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Movement))]
public class PlayerCharacter : Pawn, IDamagable, IDetectable
{
    public Movement movement { get; private set; }
    public Rigidbody rb { get; private set; }
    public Animator animator { get; private set; }
    public PlayerInteractor interactor { get; private set; }
    public PlayerStats stats { get; private set; }
    public DialogueBox dialogueBox { get; private set; }
    public InformationLog informationLog { get; private set; }

    [field: SerializeField]
    public HealingTool healingTool { get; private set; }

    public MonoBehaviour mono => this;

    public Action onDestroy { get; set; }
    private void OnDestroy() => onDestroy?.Invoke();

    private Vector2 mouseDelta;

    [Range(0.01f, 2f)]
    [SerializeField] private float sensitivity;
    public Transform orientation;

    [Space]
    [SerializeField] private PIDBehaviour crystalPrefab;
    public Transform crystalTarget;

    public LUI.PlayerUI[] PlayerUIsPrefabs;

    public DialogueBox dialogueBoxPrefab;
    public InformationLog informationLogPrefab;

    private Vector2 mouseRotationVector = Vector2.zero;

    private void Awake()
    {
        //Initialising and Component Getting
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        animator = GetComponent<Animator>();
        interactor = GetComponent<PlayerInteractor>();

        stats = new PlayerStats(this, 100, 100, 100);

        movement.OnDash += PlayDashAnimation;

        InitChain();
    }

    private void PlayDashAnimation(float obj)
    {
        animator.CrossFade("Dash", .1f);
    }

    private void InitChain()
    {
        healingTool?.Init(this);
        movement.Init(this);
    }

    private void CreateUI()
    {
        foreach (LUI.PlayerUI pUI in PlayerUIsPrefabs)
        {
            LUI.PlayerUI tempPUI = (LUI.PlayerUI)LUI.UIHolder.AddElement(pUI);
            tempPUI.Init(this);
        }

        dialogueBox = (DialogueBox)LUI.UIHolder.AddElement(dialogueBoxPrefab);
        dialogueBox.Init(this);
        informationLog = (InformationLog)LUI.UIHolder.AddElement(informationLogPrefab);
        informationLog.Init(this);
    }

    private void Update()
    {
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetInteger("Movestate", (int)movement.movestate);
    }


    public override void Possess(PlayerController controller)
    {
        base.Possess(controller);

        controls.Gameplay.Look.performed += MouseLook;
        controls.Gameplay.Use.performed += Use;
        controls.Gameplay.Use.canceled += Use;

        movement.BindControls(controls);
        interactor?.Init(this);
        
        CreateUI();
    }

    private void Use(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        healingTool?.Use(context.ReadValue<float>() > .5f);
    }

    public override void UnPossess()
    {
        base.UnPossess();
        controls.Gameplay.Look.performed -= MouseLook;
    }

    private void MouseLook(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();

        mouseDelta *= sensitivity;
        mouseDelta.y = -mouseDelta.y;

        mouseRotationVector += mouseDelta;

        float yClamped = Mathf.Clamp(mouseRotationVector.y, -60f, 60f);
        mouseRotationVector.y = yClamped;

        rb.MoveRotation(Quaternion.Euler(0, mouseRotationVector.x, 0));
        orientation.localRotation = Quaternion.Euler(mouseRotationVector.y, 0, 0);

    }

    public void TakeDamage(Damage damage)
    {
        stats.AlterHealth(damage.amount);
    }

}
