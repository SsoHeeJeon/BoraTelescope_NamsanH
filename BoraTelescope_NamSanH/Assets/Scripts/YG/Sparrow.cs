using Amazon.IoTSiteWise.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

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
    public Vector3 Pos1;
    public Vector3 Pos2;

    public Image shadow;

    [SerializeField]
    private Button btn;
    private void Start()
    {
        anim = GetComponent<Animator>();
        Pos1 = transform.position + new Vector3(5 * transform.localScale.x, 5 * transform.localScale.y, 5 * transform.localScale.z);
        Pos2 = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("state " + state);
        switch (state)
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
        transform.LookAt(Pos1);
        transform.position = Vector3.Lerp(transform.position, Pos1, Time.deltaTime*0.5f);
        float dist = Vector3.Distance(transform.position, Pos1);
        transform.position = new Vector3(transform.position.x, transform.position.y, 500);
        print(dist);
        if(dist < 1600)
        {
            state = State.Back;
        }
        
        if(dist > 2300)
        {
            shadow.color = new Color(1, 1, 1, (dist - 2200)/174);
        } else if(dist <= 2300)
        {
            shadow.color = new Color(1, 1, 1, 0);
        }
    }

    private void UpdateBack()
    {
        transform.LookAt(Pos2);
        transform.position = Vector3.Lerp(transform.position, Pos2, Time.deltaTime * 0.5f);
        float dist = Vector3.Distance(transform.position, Pos2);
        transform.position = new Vector3(transform.position.x, transform.position.y, 500);
        print(dist);
        if (dist<100)
        {
            transform.position = Pos2;
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            anim.CrossFade("IdleEat", 1f);
            Invoke("GoIdle", 10f);
            state = State.Eat;
        }


        if (dist < 500 && dist > 100)
        {
            shadow.color = new Color(1,1,1,400/(200+dist)*(1/dist)*100);
        }
        else if (dist <= 100)
        {
            shadow.color = new Color(1, 1, 1, 1);
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
        //shadow.SetActive(false);
        btn.enabled = false;
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
        btn.enabled = true;
        state = State.Idle;
        //shadow.SetActive(true);
    }
}
