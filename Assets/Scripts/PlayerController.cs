using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    static public float Hp;
    static public float Damage;

    [SerializeField] float hp;
    [SerializeField] float damage;

    [Header("PlayerMove")]
    public float moveSpeed;
    public float sprintSpeed;

    private CharacterController characterController;
    private Vector2 inputDirection = Vector2.zero;
    private float targetRotation;
    private Vector3 moveDirection = Vector3.zero;

    [Header("PlayerDash")]
    public float dashVelocity;
    public float drag;
    public float dashSpeed = 20f;
    public float dashTime = 0.25f;
    public float dashCoolDown = 1f;

    private float _dashCoolDown = 0;

    [Header("PlayerJump")]
    public float jumpHeight;
    public float gravity = -9.87f;

    private float jumpVelocity = 0;

    [Header("PlayerCamera")]
    public GameObject playerCam;
    [Range(0.01f, 5f)]
    public float mouseSpeed = 1;
    public float maxPitch = 40f;
    public float minPitch = -40f;

    private Vector2 mouseVec = Vector2.zero;
    private float camTargetPitch = 0;
    private float camTargetYaw = 0;

    [Header("AttackAndSkill")]
    public GameObject attackBullet;
    public GameObject firePoint;
    public float attackDelayTime = 0.7f;

    private float _attackDelayTime;

    [Header("CheckGround")]
    public Vector3 groundOffSet = Vector3.zero;
    public float sphereRadius = 0.35f;
    public LayerMask groundLayer;

    private bool isGrounded;

    private void Awake()
    {
        Hp = hp;
        Damage = damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어 밑에 땅이 있으면 true값 반환
        CheckGround();

        //점프
        Jump();

        //이동
        Move();

        //대쉬
        Dash();

        //공격
        Skill();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void Move()
    {
        inputDirection.x = Input.GetAxis("Horizontal");
        inputDirection.y = Input.GetAxis("Vertical");

        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
        if (inputDirection == Vector2.zero) speed = 0f;

        Vector3 moveVector = new Vector3(inputDirection.x, 0, inputDirection.y).normalized;
        transform.eulerAngles = new Vector3(0, playerCam.transform.eulerAngles.y, 0);

        targetRotation = Mathf.Atan2(moveVector.x, moveVector.z) * Mathf.Rad2Deg +
                playerCam.transform.eulerAngles.y;

        moveDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
        characterController.Move(moveDirection * speed * Time.deltaTime +
            new Vector3(0, jumpVelocity, 0) * Time.deltaTime);
    }

    private void Jump()
    {
        if(jumpVelocity <= 0 && isGrounded) jumpVelocity = 0;

        if(Input.GetKey(KeyCode.Space) && isGrounded)
        {
            jumpVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        jumpVelocity += gravity * Time.deltaTime;
    }

    private void Dash()
    {
        if (_dashCoolDown <= 0) _dashCoolDown = 0;
        if (dashVelocity <= 0 ) dashVelocity = 0;

        if (Input.GetKeyDown(KeyCode.LeftControl) && _dashCoolDown <= 0)
        {
            _dashCoolDown = dashCoolDown;
            dashVelocity = dashSpeed * dashTime;
            //_dashTime -= Time.deltaTime;
        }

        _dashCoolDown -= Time.deltaTime;
        dashVelocity -= drag * Time.deltaTime;

        characterController.Move(moveDirection * dashVelocity * Time.deltaTime);
    }

    private void CameraRotation()
    {
        mouseVec.x = Input.GetAxis("Mouse X") * mouseSpeed;
        mouseVec.y = Input.GetAxis("Mouse Y") * mouseSpeed;

        if (mouseVec != Vector2.zero)
        {
            camTargetPitch += -mouseVec.y;
            camTargetYaw += mouseVec.x;
        }

        camTargetYaw = Mathf.Clamp(camTargetYaw, float.MinValue, float.MaxValue);
        camTargetPitch = Mathf.Clamp(camTargetPitch, minPitch, maxPitch);

        playerCam.transform.eulerAngles = new Vector3(camTargetPitch, camTargetYaw, 0);
    }

    private void Skill()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        Vector3 hitPosition = ray.origin + ray.direction * 200f;

        Vector3 bulletPosition = firePoint.transform.position;

        if(Input.GetKeyDown(KeyCode.Mouse0) && _attackDelayTime == 0)
        {
            _attackDelayTime = attackDelayTime;

            if(Physics.Raycast(ray.origin, ray.direction * 1000f, out hit))
            {
                if(5f < hit.distance)
                    hitPosition = hit.point;
            }
            SkillManager.Attack(attackBullet, bulletPosition, hitPosition);
        }

        DelayTime();
    }

    private void DelayTime()
    {
        if(_attackDelayTime > 0) _attackDelayTime -= Time.deltaTime;
        else _attackDelayTime = 0;
    }

    private void CheckGround()
    {
        Vector3 spherePosition = transform.position + groundOffSet;
        isGrounded = Physics.CheckSphere(spherePosition, sphereRadius, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        Debug.DrawRay(ray.origin, ray.direction * 2000f, Color.red);
    }
}
