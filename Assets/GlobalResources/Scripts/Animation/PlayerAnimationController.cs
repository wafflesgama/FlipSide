using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    public PlayerMovementController playerMovementController;
    public InputManager inputManager;

    public CharacterController characterController;
    // public Rigidbody body;
    public string speedParameter;
    public string landParameter;
    public string jumpParameter;
    public string AttackParameter;
    public string EquipWeaponParameter;
    public string RestoreAttackComboParameter;
    public string lockSpeedParameterX;
    public string lockSpeedParameterY;
    public string lockStateParameter;
    public float speedFactor = 2;
    public float speedLerpFactor = 2;
    public float chestLeanLerpFactor = 2;
    public float headLeanLerpFactor = 2;
    public float lockSpeedLerpFactor = 2;

    public Transform chestLeanTarget;
    public Transform headLeanTarget;

    Animator animator;
    RuntimeAnimatorController baseAnimatorController;
    float speedVal, lockXValue, lockYValue;
    int attackLvl = 0;


    void Start()
    {
        animator = GetComponent<Animator>();
        baseAnimatorController=animator.runtimeAnimatorController;
        playerMovementController.onJumped += Animate_Jump;
        playerMovementController.onLanded += Animate_Land;
    }

    private void OnDestroy()
    {
        playerMovementController.onJumped -= Animate_Jump;
        playerMovementController.onLanded -= Animate_Land;
    }

    // Update is called once per frame
    void Update()
    {
        speedVal = Mathf.Lerp(speedVal, Vector3.Scale(characterController.velocity* speedFactor, new Vector3(1,0,1)).magnitude, Time.deltaTime * speedLerpFactor);
        animator.SetFloat(speedParameter, speedVal);
        lockXValue = Mathf.Lerp(lockXValue, inputManager.input_move.value.x * speedVal, Time.deltaTime * lockSpeedLerpFactor);
        lockYValue = Mathf.Lerp(lockYValue, inputManager.input_move.value.y * speedVal, Time.deltaTime * lockSpeedLerpFactor);
        animator.SetFloat(lockSpeedParameterX, lockXValue);
        animator.SetFloat(lockSpeedParameterY, lockYValue);

        //ProcedurallyAnimate_ChestLean();
        //ProcedurallyAnimate_HeadLean();
    }


    public void ProcedurallyAnimate_ChestLean()
    {
        chestLeanTarget.forward = Vector3.Lerp(chestLeanTarget.forward, transform.forward, Time.deltaTime * chestLeanLerpFactor);
        // leanTarget.rotation=Quaternion.Lerp(leanTarget.rotation, Quaternion.LookRotation(transform.forward),Time.deltaTime*speedLerpFactor);
    }

    public void ProcedurallyAnimate_HeadLean()
    {
        headLeanTarget.forward = Vector3.Lerp(headLeanTarget.forward, transform.forward, Time.deltaTime * headLeanLerpFactor);
        // leanTarget.rotation=Quaternion.Lerp(leanTarget.rotation, Quaternion.LookRotation(transform.forward),Time.deltaTime*speedLerpFactor);
    }

    public void SwitchAnimator(RuntimeAnimatorController newAnimator)
    {
        animator.runtimeAnimatorController = newAnimator ?? baseAnimatorController;
    }

    public void Animate_EquipWeapon()
    {
        animator.SetTrigger(EquipWeaponParameter);
    }


    public void Animate_Attack(int lvl, bool canRestoreAttackCombo)
    {
        animator.SetInteger(AttackParameter, lvl);
        if (canRestoreAttackCombo) animator.SetTrigger(RestoreAttackComboParameter);
    }


    public void Animate_Jump()
    {
       var jumpValue= animator.GetBool(jumpParameter);
       Debug.LogError("jumpValue "+jumpValue);
        animator.SetTrigger(jumpParameter);
    }
    public void Animate_Land()
    {
        animator.ResetTrigger(landParameter);
        animator.SetTrigger(landParameter);
    }
}
