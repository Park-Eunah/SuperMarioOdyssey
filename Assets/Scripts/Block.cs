using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Animator anim;

    public GameObject emptyBlock;
    //private bool isAnimation = false;

    public PlayerMove playerMove;

    private bool isQuestion = false;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (gameObject.name.Contains("Question"))
        {
            isQuestion = true;
            print("Question " + isQuestion) ;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChenckAnimation();
    }

    private void ChenckAnimation()
    {
        //if (isAnimation)
        //{
        //print(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("ReactionHipDrop"))
        {
            print("reactionHipDrop");
            if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                print("normalizedTime 1");
                print("Question " + isQuestion);
                anim.SetBool("isDown", false);
                if (isQuestion)
                {
                    print("question " + playerMove.isHipdrop);
                    if (playerMove.isHipdrop)
                    {
                        GameManager.instance.ActiveGetCoin(transform, 1);  
                        count++;
                        if(count > 10)
                        {
                            emptyBlock.SetActive(true);
                            gameObject.SetActive(false);
                            //GameManager.instance.
                        }
                        anim.SetBool("isDown", true);
                    }
                }
                else
                {
                    emptyBlock.SetActive(true);
                    gameObject.SetActive(false);
                    GameManager.instance.ActiveGetCoin(transform, 1);
                }
                    //isAnimation = false;
                    //anim.SetBool("isUp", false);
            }

        }
        //}
    }

    public void SetAnimDown()
    {
        anim.SetBool("isDown", true);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        print(playerMove.isHipdrop);
    //        if (playerMove.isHipdrop)
    //        {
    //            anim.SetBool("isDown", true);
    //        }
    //    }
    //}
}


