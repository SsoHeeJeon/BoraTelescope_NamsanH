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
        Eat,
        Sit,
    }
    State state = 0;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        print(11);
        anim = GetComponent<Animator>();
        Invoke("PauseAnimator", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PauseAnimator()
    {
        anim.enabled = false;
    }

    public void OnClickBtn()
    {
        anim.enabled = true;
    }

    private void Idle()
    {
        anim.CrossFade("Armature_squirrel_Idle_sit2", 0.2f);
        state = State.Idle;
    }

    private void Eat()
    {
        anim.CrossFade("Armature_squirrel_Eat", 1f);
        Invoke("Jump", 5f);
        state = State.Eat;
    }

    private void Jump()
    {
        anim.CrossFade("Armature_squirrel_Jump", 0.2f);
        Invoke("Sleep", 5f);
        state = State.Jump;
    }

    private void Sleep()
    {
        anim.CrossFade("Armature_squirrel_Sleep", 0.2f);
        Invoke("PauseAnimator", 1f);
        state = State.Sleep;
    }
}
