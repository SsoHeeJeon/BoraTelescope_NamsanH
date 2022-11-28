using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparrow : MonoBehaviour
{
    public enum State
    {
        Idle,
        PickWing,
        StrectchWing,
        TakeOff,
        Fly,
        Back,
        Hop,
        Eat,
    }
    public State state = 0;

    private Animator anim;
    public float speed;
    private Vector3 dir;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.PickWing:
                UpdatePickWing();
                break;
            case State.StrectchWing:
                UpdateStrectchWing();
                break;
            case State.TakeOff:
                UpdateTakeOff();
                break;
            case State.Fly:
                UpdateFly();
                break;
            case State.Hop:
                UpdateHop();
                break;
            case State.Back:
                UpdateBack();
                break;
            case State.Eat:
                UpdateEat();
                break;
        }
    }
    private void UpdateEat()
    {

    }

    private void UpdateFly()
    {
        //transform.LookAt(new Vector3(1.3f, 4, 0));
        transform.position = Vector3.Lerp(transform.position, new Vector3(1.3f, 4f, 0), Time.deltaTime*0.5f);
        float dist = Vector3.Distance(transform.position, new Vector3(1.3f, 4f, 0));
        if(dist < 1)
        {
            state = State.Back;
        }
    }

    private void UpdateBack()
    {
        //transform.LookAt(new Vector3(1, 0, 0));
        transform.position = Vector3.Lerp(transform.position, new Vector3(1f, 0, 0), Time.deltaTime * 0.5f);
        float dist = Vector3.Distance(transform.position, new Vector3(1f, 0, 0));
        if(dist<1)
        {
            anim.CrossFade("IdleEat", 1f);
            Invoke("GoIdle", 10f);
            state = State.Eat;
        }
    }


    private void UpdateHop()
    {
        //transform.LookAt(Vector3.Lerp(transform.forward, new Vector3(1,1,0), Time.deltaTime));
        transform.position += dir* speed * Time.deltaTime;
    }

    private void UpdateTakeOff()
    {

    }

    private void UpdateStrectchWing()
    {

    }   

    private void UpdatePickWing()
    {

    }

    private void UpdateIdle()
    {

    }

    public void OnClickBtn()
    {
        anim.CrossFade("IdlePickWing", 1f);
        state = State.PickWing;
    }

    private void StrectchWing()
    {
        anim.CrossFade("IdleStretchWings", 1f);
        state = State.StrectchWing;
    }

    private void TakeOff()
    {
    }

    private void Fly()
    {
        anim.CrossFade("Fly", 0.2f);
        state = State.Fly;
    }

    private void Hop()
    {
        Invoke("StrectchWing", 5f);
        dir = new Vector3(1, 1, 0);
        anim.CrossFade("Hop", 0.2f);
        state = State.Hop;
    }

    private void GoIdle()
    {
        anim.CrossFade("IdleLookAround", 0.2f);
        state = State.Idle;

    }
}
