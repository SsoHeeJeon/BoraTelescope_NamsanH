using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Squirrel : MonoBehaviour
{

    enum State {
        Sleep,
        Idle,
        Jump,
        Catch,
        Eat,
        Sit,
        Idle2,
    }
    State state = 0;

    Animator anim;

    public GameObject shadow;

    // Start is called before the first frame update
    [SerializeField] Button btn;
    [SerializeField] GameObject Peanut;
    void Start()
    {
        anim = GetComponent<Animator>();
        Invoke("PauseAnimator", 1f);
    }

    private void PauseAnimator()
    {
        shadow.SetActive(true);
        anim.enabled = false;
        btn.enabled = true;
    }

    public void OnClickBtn()
    {
        shadow.SetActive(false);
        anim.enabled = true;
        btn.enabled= false;
    }

    private void Idle()
    {
        anim.CrossFade("Armature_squirrel_Idle_sit2", 0.2f);
        state = State.Idle;
    }

    private void Catch()
    {
        anim.CrossFade("Armature_squirrel_Attack2", 0.2f);
        state = State.Catch;
    }

    private void Eat()
    {
        Peanut.SetActive(true);
        anim.CrossFade("Armature_squirrel_Eat", 1f);
        Invoke("Jump", 3f);
        state = State.Eat;
    }


    private void Jump()
    {
        Peanut.SetActive(false);
        anim.CrossFade("Armature_squirrel_Jump", 0.2f);
        Invoke("Idle2", 5f);
        state = State.Jump;
    }

    private void Idle2()
    {
        anim.CrossFade("Armature_squirrel_Idle_2", 0.2f);
        state=State.Idle2;
        Invoke("Sleep", 3f);
    }

    private void Sleep()
    {
        anim.CrossFade("Armature_squirrel_Sleep", 0.2f);
        Invoke("PauseAnimator", 1f);
        state = State.Sleep;
    }
}
