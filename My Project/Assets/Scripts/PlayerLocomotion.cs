using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{

    Transform cameraObject;
    InputHandler inputHandler;
    Vector3 moveDirection;


    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public AnimatorHandler animatorHandler;


    public new Rigidbody rigidbody;
    public GameObject normalCamrea;

    [Header("Stat")]
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float rotationSpeed = 10;


    void Start()
    {
        //Get Components
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        cameraObject = Camera.main.transform;
        myTransform = transform;
        animatorHandler.Initialized();
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        inputHandler.TickInput(delta);
        handleMovement(delta);
        handleRollingAndSprint(delta);
    }

    #region Movement

    Vector3 normalVector;
    Vector3 targetPosition;

    public void handleMovement(float delta)
    {
        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = moveSpeed;
        moveDirection *= speed;

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);
        if (animatorHandler.canRotate)
        {
            handleRotation(delta);
        }

    }

    private void handleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir += cameraObject.right * inputHandler.horizontal;

        targetDir.Normalize();
        targetDir.y = 0;

        if(targetDir == Vector3.zero)
        {
            targetDir = myTransform.forward;
        }

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

        myTransform.rotation = targetRotation;

    }

    public void handleRollingAndSprint(float delta)
    {
        if (animatorHandler.anim.GetBool("isInteracting"))
        {
            Debug.Log("is interacting");
            return;
        }
        if (inputHandler.rollFlag)
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if(inputHandler.moveAmount > 0)
            {
                animatorHandler.PlayTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
            else
            {
                animatorHandler.PlayTargetAnimation("StepBack", true);
            }
        }
    }

    #endregion



}
