using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    public float moveSpeed = 5f;
    bool facingRight = true;

    // ANIMATION STATES ----------------------
    private string CURRENT_STATE = "";
    const string idle = "Idle";
    const string idle_equip = "Idle_Equip";
    const string running = "Run";
    const string running_equip = "Run_Equip";
    const string equip = "Equip";
    const string dequip = "Dequip";

    bool IDLE = false;
    bool IDLE_EQUIP = false;
    bool RUNNING = false;
    bool RUNNING_EQUIP = false;
    bool EQUIP = false;
    // ---------------------------------------

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

        CURRENT_STATE = idle;
        IDLE = true;
        ChangeAnimationState(idle);
    }

    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += move * Time.deltaTime * moveSpeed;

        Movement(move.x);
        StartCoroutine(hammer());
    }

    void Movement(float x)
    {
        if (x != 0 && !EQUIP)
        {
            ChangeAnimationState(running);
            FlipSprite(x);
            RUNNING = true;
        }
        else if(x != 0 && EQUIP) 
        {
            ChangeAnimationState(running_equip);
            FlipSprite(x);
            RUNNING = true;
        }
        else if (x == 0 && IDLE && !EQUIP && !IDLE_EQUIP)
        {
            ChangeAnimationState(idle);
            RUNNING = false;
        }
        else if (x == 0 && IDLE_EQUIP && EQUIP && !IDLE)
        {
            ChangeAnimationState(idle_equip);
            RUNNING = false;
        }

    }

    IEnumerator hammer()
    {
        if(!RUNNING)
        {
            if (Input.GetMouseButtonDown(1))
            {
                EQUIP = true;
                if (EQUIP && !IDLE_EQUIP && IDLE)
                {
                    ChangeAnimationState(equip);

                    yield return new WaitForSeconds(0.65f);
                    IDLE_EQUIP = true;
                    IDLE = false;
                }   
                /*
                if(AnimationDone(equip)) 
                {
                    IDLE_EQUIP = true;
                    IDLE = false;
                    Debug.Log("equip anim done");
                } */
            }

            if (Input.GetMouseButtonDown(0))
            {
                EQUIP = false;
                if (!EQUIP && IDLE_EQUIP && !IDLE)
                {
                    ChangeAnimationState(dequip);

                    yield return new WaitForSeconds(0.65f);
                    IDLE_EQUIP = false;
                    IDLE = true;
                }  
                /*
                if(AnimationDone(dequip)) 
                {
                    IDLE_EQUIP = false;
                    IDLE = true;
                    Debug.Log("dequip anim done");
                } */
            }
        }
    }

    void FlipSprite(float x)
    {
        if (x > 0 && !facingRight || x < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void ChangeAnimationState(string NEW_STATE)
    {
        if(CURRENT_STATE == NEW_STATE) return;

        animator.Play(NEW_STATE);

        CURRENT_STATE = NEW_STATE;
    }

    bool AnimationDone(string anim) 
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(anim);
    }
}
