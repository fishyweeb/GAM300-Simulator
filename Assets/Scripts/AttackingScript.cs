using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingScript : MonoBehaviour
{
    [SerializeField]
    protected Animator anim;
    public int noOfClicks = 0;
    protected ThirdPersonController controller;
    public string skillName;
    private void Start()
    {
        if (controller == null)
        {
            controller = GetComponent<ThirdPersonController>();
        }
    }
    // Update is called once per frame
    public void ResetClicks()
    {
        noOfClicks = 0;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
        if (Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger(skillName);
        }

        //temp fix
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Hit2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Hit1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("ClosePhone") && !anim.GetCurrentAnimatorStateInfo(0).IsName("OpenPhone"))
        {
            GetComponent<ThirdPersonControllerRB>().disableMovement = false;
        }
        else
        {
            GetComponent<ThirdPersonControllerRB>().disableMovement = true;
        }
    }
    void OnClick()
    {
        noOfClicks++;
        if (noOfClicks == 1)
        {
            anim.SetBool("Hit1", true);
            anim.SetBool("Hit2", false);
        }
        if (noOfClicks >= 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.3f && anim.GetCurrentAnimatorStateInfo(0).IsName("Hit1"))
        {
            anim.SetBool("Hit2", true);
            anim.SetBool("Hit1", false);
            ResetClicks();
        }
        if (noOfClicks >= 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.3f && anim.GetCurrentAnimatorStateInfo(0).IsName("Hit2"))
        {
            anim.SetBool("Hit3", true);
            anim.SetBool("Hit2", false);
        }
    }
}
