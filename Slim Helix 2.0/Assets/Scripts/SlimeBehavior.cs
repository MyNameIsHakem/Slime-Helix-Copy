using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehavior : MonoBehaviour
{
    public GameObject DeathEffect;
    public TerrainGenerator Tg;
    public MeshRenderer mesh;

    public SlimeType Type;
    public int Lives;
    public bool Attack;
    [Tooltip("How Much Time it takes to charge the attack In Seconds")]
    public float ChargeAttack;
    [Tooltip("How Much till the next attack (should give the animation the time to finish)")]
    public float CoolDown;    
    public Vector3 SenseField ,AttackBoxOffset ,AttackField;
    public LayerMask PlayerLayer;

    [Header("Hurt Animation")]
    [Tooltip("How Much I will retract the slime Y scale when hurt")]
    public float HurtValue;
    [Tooltip("How Much Time the hurt animation will last (where i retract the Y scale)")]
    [Range(0f, 1f)]
    public float HurtTime;

    [Header("Idle Breathing Values")] [Range(0f , 5f)]
    [Tooltip("How much i will take from the original scale then grow it back")]
    public float SubstractedScale;
    [Tooltip("How much the Scale Changes Each")]
    [Range(0f, 3f)]
    public float ChangeSpeed;
    private Vector3 OriScale;
    private int InOutValue;
    private bool IsHurt;

    private BoxCollider col;
    private Animator anim;
    private bool Invulnerable;

    void Start()
    {
        InOutValue = -1;
        Invulnerable = false;
        IsHurt = false;
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider>();
        OriScale = transform.localScale;
    }

    void Update()
    {
        if (Attack)
        {
            var col = Physics.OverlapBox(transform.position, SenseField, Quaternion.Euler(transform.eulerAngles + Vector3.right * 90), PlayerLayer);

            if (col.Length > 0)
            {
                if (col[0].transform.position.x - transform.position.x > 0)
                {
                    Debug.Log("I Will attack the player (From the Right)");
                    anim.SetTrigger("Right Attack");
                }
                else
                {
                    Debug.Log("I Will attack the player (From the Left)");
                    anim.SetTrigger("Left Attack");
                }


                Attack = false;
                StartCoroutine(PrepareAttack());                
            }
        }

        #region Code to Make the Slime Retract then Grow Back to simulate Bearthing (Inactive Now)
        /*
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Slime Idle") && !IsHurt)
        {
            transform.localScale += new Vector3(1, 0, 1) * Time.deltaTime * ChangeSpeed * InOutValue;

            if (transform.localScale.x <= OriScale.x - SubstractedScale)
            {
                //now it will grow
                InOutValue = 1;
            }
            else if (transform.localScale.x >= OriScale.x)
            {
                //now it will Smallen
                InOutValue = -1;
            }
        }
        */
        #endregion

    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawCube(transform.position + transform.up * AttackBoxOffset.z + transform.forward * AttackBoxOffset.y, AttackField);
    //}

    IEnumerator PrepareAttack()
    {
        yield return new WaitForSeconds(ChargeAttack);

        Collider[] col = Physics.OverlapBox(transform.position + transform.up * AttackBoxOffset.z + transform.forward * AttackBoxOffset.y
                             , AttackField, new Quaternion(), PlayerLayer);

        Debug.Log("I Can attack the player is : " + (col.Length > 0));

        if (col.Length > 0)
        {
            col[0].GetComponent<PlayerInteractions>().Hurt();
        }

        //Till the animation finishes
        yield return new WaitForSeconds(CoolDown);

        Attack = true;
    }

    public void Hurt(int Damage)
    {
        if (!Invulnerable)
        {
            Invulnerable = true;
            Lives -= Damage;

            if (Lives <= 0)
            {
                col.enabled = false;

                Instantiate(DeathEffect, transform.position, new Quaternion());

                Tg.CheckIfDestroy();

                Destroy(transform.parent.gameObject);
            }
            else
            {
                StartCoroutine(HurtAnim());
            }
        }
        
    }

    IEnumerator HurtAnim()
    {
        IsHurt = true;

        transform.localScale += Vector3.back * HurtValue;

        yield return new WaitForSeconds(HurtTime/2);

        Invulnerable = false;

        yield return new WaitForSeconds(HurtTime/2);

        transform.localScale += Vector3.forward * HurtValue;

        IsHurt = false;
    }

    public void ChangeType(Slimes slime)
    {
        Type = slime.Types;
        Lives = slime.Lives;
        Attack = slime.Attack;
        mesh.material = slime.Mat;
        DeathEffect = slime.DeathEffect;       
    }

}

public enum SlimeType { RedSlime , GreenSlime , BlueSlime}
