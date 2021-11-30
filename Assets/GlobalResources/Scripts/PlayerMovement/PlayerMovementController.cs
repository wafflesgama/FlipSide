using System.Collections;
using UnityEngine;

public delegate void MovementAction();


[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("External references")]
    public InputManager inputManager;
    public AimController aimController;
    public Transform playerMesh;

    [Header("Basic Movement Walk/Sprint")]
    public float walkSpeed = 2;
    public float maxWalkSpeed = 7.5f;
    public AnimationCurve movementSpeedCurve;
    public float sprintSpeedFactor = 5;
    public LayerMask layerMask;
    public float castMaxDistance;


    [Header("Jump")]
    public float maxJumpSpeed;
    public int jumpForceTicks;
    public float jumpAcceleration;

    [Header("Character Rotation")]
    public float lerpRotation;

    [Header("Freeze Movement")]
    public float freezeDecelFactor = 2;

    [Header("Exposed Values")]
    public Vector3 groundNormal;



    //Inspector Hidden parameters
    public Vector3 playerHorizontalDirection { get; private set; }
    public Vector3 playerTurningDirection { get; private set; }
    public Vector3 upLeanedVector { get; private set; }
    public event MovementAction onJumped;
    public event MovementAction onLanded;
    public event MovementAction onFalling;


    CharacterController characterController;
    Vector3 acceleration = Vector3.zero;
    Vector3 dampedAcceleration = Vector3.zero;
    Vector3 lastVelocity = Vector3.zero;
    Vector3 playerSpeed = Vector3.zero;

    bool isSprinting, isCheckingLand, isTurningLocked, isMovementLocked, isJumpingForce;
    int jumpTickCounter = 0;

    public void LockTurning(bool locked = true) => isTurningLocked = locked;
    public void LockMovement(bool locked = true) => isMovementLocked = locked;


    void Start()
    {
        groundNormal = Vector3.up;
        inputManager.input_jump.Onpressed += Jump;
        characterController = GetComponent<CharacterController>();
    }

    private void OnDestroy()
    {
        inputManager.input_jump.Onpressed -= Jump;
    }

    void Update()
    {
        GetGroundNormal();
        CalculateDirection();
        CheckIfJumpLanded();

        if (!isTurningLocked)
           StandardTurning();
           
        HandleGravity(ref playerSpeed);
        JumpForce(ref playerSpeed);
        Move(ref playerSpeed);

        characterController.Move(playerSpeed * Time.deltaTime);
    }

    void GetGroundNormal()
    {
        //if (!characterController.isGrounded) return;

        RaycastHit hit;
        var hasHit = Physics.Raycast(transform.position, Vector3.down, out hit, castMaxDistance, layerMask);
        //Debug.DrawRay(transform.position, Vector3.down, Color.red);
        if (!hasHit) return;
        //Debug.DrawRay(hit.point, hit.normal,Color.blue);
        groundNormal = hit.normal;
    }

    void CalculateDirection()
    {
        var frontalDirection = aimController.aimTarget.forward.normalized * inputManager.input_move.value.y;
        var sideDirection = aimController.aimTarget.right.normalized * inputManager.input_move.value.x;
        playerHorizontalDirection = Vector3.Scale((frontalDirection + sideDirection), new Vector3(1, 0, 1)).normalized;
       
        var frontalTurnDirection = aimController.aimTarget.forward.normalized * Mathf.Abs(inputManager.input_move.value.y);
        var sideTurnDirection = aimController.aimTarget.right.normalized * (inputManager.input_move.value.y<0 ? -inputManager.input_move.value.x: inputManager.input_move.value.x);
        playerTurningDirection = Vector3.Scale((frontalTurnDirection + sideTurnDirection), new Vector3(1, 0, 1)).normalized;
    }

    void HandleGravity(ref Vector3 currentVelocity)
    {
        if (characterController.isGrounded)
            currentVelocity.y = 0;

        currentVelocity.y += Physics.gravity.y * Time.deltaTime;
    }

    void JumpForce(ref Vector3 currentSpeed)
    {
        if (jumpTickCounter <= 0) return;

        jumpTickCounter--;
        currentSpeed += Vector3.up * jumpAcceleration;
    }




    void StandardTurning()
    {
        if (inputManager.input_move.value.y == 0 && inputManager.input_move.value.x == 0) return;

        //var absDirection = new Vector3(playerHorizontalDirection.x, playerHorizontalDirection.y, Mathf.Abs(playerHorizontalDirection.z)).normalized;
        var absDirection = playerTurningDirection.normalized;
        //if (inputManager.input_move.value.y < 0)
        //    absDirection.x *= -1;

        //var calculatedForwardVector = Vector3.Lerp(playerMesh.forward, playerHorizontalDirection.normalized, Time.deltaTime * lerpRotation);
        playerMesh.localEulerAngles = Quaternion.Lerp(playerMesh.rotation, Quaternion.LookRotation(absDirection), Time.deltaTime * lerpRotation).eulerAngles;
    }


    void Move(ref Vector3 currentSpeed)
    {
        if (!characterController.isGrounded) return;

        if (isMovementLocked)
        {
            bool xSpeedSign = currentSpeed.x > 0;
            bool zSpeedSign = currentSpeed.z > 0;

            var xDecelValue = xSpeedSign ? -1 : 1;
            var zDecelValue = zSpeedSign ? -1 : 1;

            //Case speed already zero no decel value
            if (currentSpeed.x == 0) xDecelValue = 0;
            if (currentSpeed.z == 0) zDecelValue = 0;

            currentSpeed += new Vector3(xDecelValue, 0, zDecelValue) * freezeDecelFactor;
            //currentSpeed += Vector3.ProjectOnPlane(new Vector3(xDecelValue, 0, zDecelValue) * freezeDecelFactor,);

            //In case decel inverted speed bring it to zero
            if (xSpeedSign && currentSpeed.x < 0) currentSpeed.x = 0;
            if (zSpeedSign && currentSpeed.z < 0) currentSpeed.z = 0;
            return;
        }

        var maxSpeed = inputManager.input_sprint.value > 0 ? maxWalkSpeed * sprintSpeedFactor : maxWalkSpeed;
        var maxWalkForce = inputManager.input_sprint.value > 0 ? walkSpeed * sprintSpeedFactor : walkSpeed;

        float speedFactor = walkSpeed;
        var clampedSpeed = Vector3.Scale(characterController.velocity, new Vector3(1, 0, 1)).magnitude;
        if (clampedSpeed > maxSpeed)
            clampedSpeed = maxSpeed;


        speedFactor = movementSpeedCurve.Evaluate(clampedSpeed / maxSpeed) * maxWalkForce;

        var inputFactor = inputManager.input_move.value.normalized.magnitude;

        var characterSpeed = playerHorizontalDirection * speedFactor * inputFactor;

        currentSpeed = Vector3.ProjectOnPlane(new Vector3(characterSpeed.x, currentSpeed.y, characterSpeed.z), groundNormal);
    }

    void Jump()
    {
        if (!characterController.isGrounded)
        {
            return;

        }
        onJumped.Invoke();
        LockMovement();
        LockTurning();
        jumpTickCounter = jumpForceTicks;
        isJumpingForce = true;
        StartCoroutine(StartCheckingLand());
    }

    IEnumerator StartCheckingLand()
    {
        yield return new WaitForSeconds(.1f);
        isCheckingLand = true;
    }

    void CheckIfJumpLanded()
    {
        if (!isCheckingLand || !characterController.isGrounded) return;

        isCheckingLand = false;
        LockMovement(false);
        LockTurning(false);
        onLanded.Invoke();
    }
}
