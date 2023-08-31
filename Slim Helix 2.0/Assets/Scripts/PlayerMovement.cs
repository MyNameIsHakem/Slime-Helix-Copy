using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public AudioSource BounsSound;
    public float JumpPower;    
    public LayerMask EnemyLayer; 
    public float GroundedCheckerRadius;    

    private LayerMask GroundLayer;
    private Collider[] EnemyDetected;
    private Rigidbody rb;
    private Animator anim;    

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        GroundLayer = LayerMask.NameToLayer("Ground");
    } 

    void Update()
    {
        EnemyDetected = Physics.OverlapSphere(transform.position, GroundedCheckerRadius, EnemyLayer);

        if(EnemyDetected.Length > 0 && rb.velocity.y <= 0)
        {
            BounsSound.Play();
            anim.SetTrigger("Bouns");
            rb.velocity = Vector3.up * JumpPower;
        }

        //Debug.DrawLine(GroundedCheckerRadius + transform.position, GroundedCheckerRadius + transform.position + Vector3.down * YOffset);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == GroundLayer)
        {
            BounsSound.Play();
            anim.SetTrigger("Bouns");
            rb.velocity = Vector3.up * JumpPower;
        }
    }  

}
