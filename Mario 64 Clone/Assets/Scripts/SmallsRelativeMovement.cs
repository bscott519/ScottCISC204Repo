using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallsRelativeMovement : MonoBehaviour
{
   [SerializeField] Transform target;

    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;

    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -20.0f;
    public float minFall = -1.5f;

    // ref to main player obj
    public GameObject player; 

    // smalls speed boost
    public float timer = 1.0f;
    public float boostAmount = 2.0f; 
    public bool boosted = false; 

    private Animator animator;

    private float vertSpeed;

    private ControllerColliderHit contact;

    private CharacterController charController;


    void Start()
    {
        vertSpeed = minFall;
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        float distanceToPlayer = 2.0f;
        bool isNear = distance <= distanceToPlayer;

        // jump if near player
        if (isNear) {
            if (charController.isGrounded) {
              vertSpeed = jumpSpeed;
            } else {
              vertSpeed += gravity * 5 * Time.deltaTime;
              if (vertSpeed < terminalVelocity) {
                vertSpeed = terminalVelocity;
                movement.y = vertSpeed;
                movement *= Time.deltaTime;
                charController.Move(movement);
                distance = Vector3.Distance(transform.position, player.transform.position);
                isNear = distance <= distanceToPlayer;  
              }
            }
        }

        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0)
        {
            Vector3 right = target.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up);
            movement = (right * horInput) + (forward * vertInput);
            movement *= moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation,
                direction, rotSpeed * Time.deltaTime);
        }

        bool hitGround = false;
        RaycastHit hit;
        if (vertSpeed < 0 &&
            Physics.Raycast(transform.position, Vector3.down, out hit)) {
            float check =
                (charController.height + charController.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);

        // boost smalls speed after 1 sec
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            timer = 1.0f;
            boosted = true;
        }

        if (timer > 0 && boosted)
        {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0 && boosted)
        {
            moveSpeed += boostAmount;
            boosted = false;
        }

        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpSpeed;
            }
            else
            {
                vertSpeed = minFall;
                animator.SetBool("Jumping", false);
            }
        }
        else
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }

            if (contact != null)
            {
                animator.SetBool("Jumping", true);
            }

            if (charController.isGrounded)
            {
                if (Vector3.Dot(movement, contact.normal) < 0)
                {
                    movement = contact.normal * moveSpeed;
                }
                else
                {
                    movement += contact.normal * moveSpeed;
                }
            }
        }

        movement.y = vertSpeed;

        movement *= Time.deltaTime;
        charController.Move(movement);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        contact = hit;
    }
}
