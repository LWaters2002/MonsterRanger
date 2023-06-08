using System;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

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

    public CinemachineVirtualCamera cmVcam;

    public MonoBehaviour mono => this;

    public Action onDestroy { get; set; }
    private void OnDestroy() => onDestroy?.Invoke();

    private Vector2 mouseDelta;

    [Range(0.01f, 2f)]
    [SerializeField] private float sensitivity;
    public Transform orientation;

    public Transform spawnTransform;

    [Space]
    [SerializeField] private PIDBehaviour crystalPrefab;
    public Transform crystalTarget;

    public LUI.PlayerUI[] PlayerUIsPrefabs;

    public DialogueBox dialogueBoxPrefab;
    public InformationLog informationLogPrefab;

    private Vector2 mouseRotationVector = Vector2.zero;

    [Header("Cheeky Last Minute Additons")]
    public CinemachineVirtualCamera endCamera;
    public GameObject endUI;

    private void Awake()
    {
        //Initialising and Component Getting
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        animator = GetComponent<Animator>();
        interactor = GetComponent<PlayerInteractor>();

        stats = new PlayerStats(this, 100, 100, 100);
        stats.OnDeath += Death;
        movement.OnDash += PlayDashAnimation;
        movement.OnSprint += OnSprint;
        movement.OnJump += () => animator.SetTrigger("Jump");


        InitChain();
    }

    private void OnSprint(bool isSprint)
    {
        if (isSprint)
        {
            DOTween.To(() => cmVcam.m_Lens.FieldOfView, x => cmVcam.m_Lens.FieldOfView = x, 90f, 1f).SetEase(Ease.OutSine);
        }
        else
        {
            DOTween.To(() => cmVcam.m_Lens.FieldOfView, x => cmVcam.m_Lens.FieldOfView = x, 80f, 1f).SetEase(Ease.OutSine);
        }
    }

    public void Death()
    {
        // rb.MovePosition(spawnTransform.position);
        // stats.AlterHealth(100);
        // stats.AlterStamina(100);

        endCamera.Priority = 1000;
        endUI.SetActive(true);
        GameManager.Get().PlayCutscene(null);

        Invoke("RestartGame", 7f);
    }

    private void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SC_World");
    }


    private void PlayDashAnimation(float obj)
    {
        animator.CrossFade("Dash", .1f, -1);
        Sequence sequence = DOTween.Sequence();

        sequence.Append(
        DOTween.To(() => cmVcam.m_Lens.FieldOfView, x => cmVcam.m_Lens.FieldOfView = x, 85f, .4f).SetEase(Ease.OutSine)
        );

        sequence.Append(
        DOTween.To(() => cmVcam.m_Lens.FieldOfView, x => cmVcam.m_Lens.FieldOfView = x, 80f, .4f).SetEase(Ease.OutSine)
        );

        sequence.Play();

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
        Vector2 lateralVelocity = new Vector2(rb.velocity.x, rb.velocity.z);
        animator.SetFloat("Speed", lateralVelocity.magnitude);
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
