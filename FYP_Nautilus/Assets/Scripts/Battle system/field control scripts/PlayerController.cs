using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public TextMeshProUGUI depthText;

    public GameObject headLight;

    public bool underwater = false;
    public bool inBattle = false;
    public float speed;
    float walkSpeed = 12;
    float swimSpeed = 10;

    float rbDrag_swim = 20;
    public float rbDrag_walk = 0.1f;
    public float jumpForce;

    public CameraController cameraController;
    public PlayerInteractionController playerInteractionController;

    private float h;
    private float v;
    private float up;
    private float down;

    Animator anim;
    Rigidbody rb;

    public bool onGround = true;
    //bool jumpPressing = false;

    public float jumpTimer = 0;

    LayerMask floorMask;

    BoxCollider waterBox;

    //interaction in field
    public bool canMove;

    private void Start()
    {
        waterBox = GameObject.FindGameObjectWithTag("Water").GetComponent<BoxCollider>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask("Floor");
        rb.drag = rbDrag_walk;
        inBattle = false;
    }

    private void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        up = Input.GetAxis("Jump");
        down = Input.GetAxis("Sink");
    }

    private void FixedUpdate()
    {
        jumpTimer += Time.deltaTime;
        if (!inBattle && canMove)
        {
            //check player touched the ground or not
            if (!onGround && jumpTimer >= 0.25f)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position - new Vector3(0, 1, 0), 0.025f, floorMask, QueryTriggerInteraction.Ignore);
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].CompareTag("Floor"))
                    {
                        onGround = true;
                        speed = walkSpeed;
                        rb.drag = rbDrag_walk;
                        rb.useGravity = true;
                        break;
                    }                    
                }
            }

            //set animator bool : walking 
            if (h != 0 || v != 0)
            {
                anim.SetBool("Moving", true);
            }
            else
            {
                anim.SetBool("Moving", false);
            }

            //movement
            if (underwater)
            {
                //underwater movement: 

                //get camera rotation (forward & right)    
                Vector3 forward = cameraController.target.forward;
                Vector3 right = cameraController.target.right;
                forward.Normalize();
                right.Normalize();
                //move base on this direction
                Vector3 moveDirection = forward * v + right * h;
                //get movement direction
                //set rotation
                if (h != 0 || v != 0)
                {
                    //if moving
                    transform.rotation = Quaternion.LookRotation(moveDirection);
                }
                else
                {
                    //if not moving, reset all rotations except y-axis (facing direction)
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                }
                //move player object
                moveDirection += new Vector3(0, -down, 0) + new Vector3(0, up, 0);
                moveDirection.Normalize();
                moveDirection = moveDirection * (Time.deltaTime * speed);
                transform.position += moveDirection;
                //if(up > 0)
                //{
                //    rb.constraints = RigidbodyConstraints.FreezePositionY;
                //    rb.constraints = RigidbodyConstraints.FreezeRotationX;
                //    rb.constraints = RigidbodyConstraints.FreezeRotationY;
                //    rb.constraints = RigidbodyConstraints.FreezeRotationZ;
                //}
                //else
                //{
                //    rb.constraints = RigidbodyConstraints.None;
                //    rb.constraints = RigidbodyConstraints.FreezeRotationX;
                //    rb.constraints = RigidbodyConstraints.FreezeRotationY;
                //    rb.constraints = RigidbodyConstraints.FreezeRotationZ;
                //}
            }
            else
            {
                Vector3 forward = cameraController.target.forward;
                forward.y = 0;
                Vector3 right = cameraController.target.right;
                right.y = 0;
                forward.Normalize();
                right.Normalize();
                Vector3 moveDirection = forward * v + right * h;
                moveDirection = moveDirection.normalized;
                Vector3 movement = moveDirection * (Time.deltaTime * speed);
                if (h != 0 || v != 0)
                {
                    transform.rotation = Quaternion.LookRotation(moveDirection);
                    transform.Translate(movement, Space.World);
                }
                
                if (onGround && Input.GetButtonDown("Jump"))
                {                    
                    jumpTimer = 0;
                    onGround = false;
                    rb.velocity += Vector3.up * jumpForce;
                    speed = walkSpeed / 1.5f;
                }
                if (rb.velocity.y > 0)
                {
                    rb.velocity -= new Vector3(0, jumpForce, 0) * Time.deltaTime;
                }
                if (rb.velocity.y < 0)
                {
                    rb.velocity -= new Vector3(0, jumpForce * 2, 0) * Time.deltaTime;
                }
            }

        }
        //set depth text
        if(depthText != null)
        {
            depthText.text = "Current Depth: " + Mathf.CeilToInt(Mathf.Abs(transform.position.y));
        }
        

        if(waterBox.bounds.Contains(gameObject.transform.position))
        {
            depthText.transform.parent.gameObject.SetActive(true);
            underwater = true;
            anim.SetBool("Underwater", true);
            rb.drag = rbDrag_swim;
            speed = swimSpeed;
            onGround = false;
        }
        else
        {
            depthText.transform.parent.gameObject.SetActive(false);
            if (onGround)
            {
                speed = walkSpeed;
            }
            else
            {
                speed = walkSpeed / 1.5f;
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Floor"))
        {
            underwater = false;
            anim.SetBool("Underwater", false);
            rb.drag = rbDrag_walk;
        }
        //if (other.CompareTag("Water"))
        //{
        //    headLight.SetActive(true);
        //    underwater = true;
        //    anim.SetBool("Underwater", true);
        //    rb.drag = rbDrag_swim;
        //    speed = swimSpeed;
        //    onGround = false;
        //}
        //else if (other.CompareTag("Floor"))
        //{
        //    headLight.SetActive(false);
        //    rb.drag = rbDrag_walk;
        //    if(onGround)
        //    {
        //        speed = walkSpeed;
        //    }
        //    else
        //    {
        //        speed = walkSpeed / 1.5f;
        //    }
        //    anim.SetBool("Underwater", false);
        //    underwater = false;
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Water"))
        //{
        //    //underwater = false;            
        //    rb.drag = rbDrag_walk;
        //    speed = walkSpeed;
        //    rb.velocity += new Vector3(0, rb.velocity.y, 0);
        //}
    }

    
}

