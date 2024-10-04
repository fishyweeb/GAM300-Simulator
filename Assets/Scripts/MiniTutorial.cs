using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MiniTutorial : MonoBehaviour
{
    int step = 0;
    [SerializeField]
    GameObject hello;
    [SerializeField]
    GameObject move;
    [SerializeField]
    GameObject dash;
    [SerializeField]
    GameObject jump;
    [SerializeField]
    GameObject attack;
    [SerializeField]
    GameObject hackmode;
    [SerializeField]
    GameObject hackobject;
    [SerializeField]
    GameObject useskill;
    [SerializeField]
    GameObject pauseGame;
    Animator animator;
    private static MiniTutorial _instance;

    public static MiniTutorial Instance
    {
        get
        {
            return _instance;
        }
    }
    IEnumerator Enter(int value)
    {
        yield return new WaitForSeconds(1.0f);
        if (value == 0)
        {
            hello.SetActive(false);
            move.SetActive(true);
        }
        else if (value == 1)
        {
            move.SetActive(false);
            dash.SetActive(true);
        }
        else if (value == 2)
        {
            dash.SetActive(false);
            jump.SetActive(true);
        }
        else if (value == 3)
        {
            jump.SetActive(false);
            attack.SetActive(true);
        }
        else if (value == 4)
        {
            attack.SetActive(false);
            hackmode.SetActive(true);
        }
        else if (value == 5)
        {
            hackmode.SetActive(false);
            hackobject.SetActive(true);
        }
        else if (value == 6)
        {
            hackobject.SetActive(false);
            useskill.SetActive(true);
        }
        else if (value == 7)
        {
            useskill.SetActive(false);
            pauseGame.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Enter");
    }
    public void CompleteStep(int value)
    {
        if (value == 0)
        {
            if (step == 0)
            {
                animator.SetTrigger("Exit");
                step = 1;
                StartCoroutine(Enter(value));
            }
        }
        else if (value == 1)
        {
            if (step == 1)
            {
                animator.SetTrigger("Exit");
                step = 2;
                StartCoroutine(Enter(value));
            }
        }
        else if (value == 2)
        {
            if (step == 2)
            {
                animator.SetTrigger("Exit");
                step = 3;
                StartCoroutine(Enter(value));
            }
        }
        else if (value == 3)
        {
            if (step == 3)
            {
                animator.SetTrigger("Exit");
                step = 4;
                StartCoroutine(Enter(value));
            }
        }
        else if (value == 4)
        {
            if (step == 4)
            {
                animator.SetTrigger("Exit");
                step = 5;
                StartCoroutine(Enter(value));
            }
        }
        else if (value == 5)
        {
            if (step == 5)
            {
                animator.SetTrigger("Exit");
                step = 6;
                StartCoroutine(Enter(value));
            }
        }
        else if (value == 6)
        {
            if (step == 6)
            {
                animator.SetTrigger("Exit");
                step = 7;
                StartCoroutine(Enter(value));
            }
        }
        else if (value == 7)
        {
            if (step == 7)
            {
                animator.SetTrigger("Exit");
                step = 8;
                StartCoroutine(Enter(value));
            }
        }
    }
    private void Awake()
    {
        _instance = this;
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        StartCoroutine(Hello());
    }
    IEnumerator Hello()
    {
        hello.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        CompleteStep(0);
    }

    void Update()
    {
    }
}
