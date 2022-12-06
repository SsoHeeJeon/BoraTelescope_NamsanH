using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Crane : MonoBehaviour
{

    [SerializeField]
    private Animator anim;

    public float speed;
    public float Min_Pan;
    public float Max_Pan;
    public enum State
    {
        Seat,
        Idle,
        WalkReady,
        GlideUp,
        GlideForward,
        GlideDown,
        Walk,
        Land,
        Groomingstart,
        Grooming,
        GroomingEnd,
    }
    public State state = 0;

    public Vector3 StartPos;
    [SerializeField] Button btnl;
    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        StartPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Seat:
                UpdateSeat();
                break;
            case State.Idle:
                UpdateIdle();
                break;
            case State.WalkReady:
                UpdateWalkReady();
                break;
            case State.GlideUp:
                UpdateGlideUp();
                break;
            case State.GlideForward:
                UpdateGlideForward();
                break;
            case State.Walk:
                UpdateWalk();
                break;
            case State.GlideDown:
                UpdateGlideDown();
                break;
            case State.Land:
                UpdateLand();
                break;
            case State.Groomingstart:
                UpdateGroomingStart();
                break;
            case State.Grooming:
                UpdateGrooming();
                break;
            case State.GroomingEnd:
                UpdateGroomingEnd();
                break;

        }
    }

    private void UpdateSeat()
    {

    }

    private void UpdateGroomingEnd()
    {

    }

    private void UpdateGrooming()
    {

    }

    private void UpdateGroomingStart()
    {

    }

    private void UpdateLand()
    {

    }

    private void UpdateGlideDown()
    {
        speed -= Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, StartPos, speed*Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, 500);
    }

    private void UpdateWalk()
    {

    }

    private void UpdateGlideUp()
    {
        speed += Time.deltaTime;
        Vector3 dir = new Vector3(0, 1, 1);
        transform.position += dir * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, 500);
    }

    bool LeftCheck;
    bool RightCheck;
    private void UpdateGlideForward()
    {
        float dist = Vector3.Distance(transform.position, StartPos);
        Vector3 dir = transform.forward;
        if(transform.position.x<Min_Pan)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            LeftCheck = true;
        }
        else if(transform.position.x > Max_Pan)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
            RightCheck = true;
        }
        print(dist);
        if(LeftCheck && RightCheck && dist<350)
        {
            LeftCheck = false;
            RightCheck = false;
            GlideDown();
        }
        transform.position += dir *speed* Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, 500);
    }

    private void UpdateWalkReady()
    {

    }

    private void UpdateIdle()
    {

    }

    private void IdleEnd()
    {
        if(state == State.Idle)
        {
            anim.CrossFade("GroomingStart", 0.2f);
            state = State.Groomingstart;
        }
        else if(state == State.WalkReady)
        {
            anim.CrossFade("IdleToWalk", 0.2f);
            state = State.Walk;
        }
    }

    private void Grooming()
    {
        anim.CrossFade("Grooming", 0.2f);
        state = State.Grooming;
    }

    private void GroomingEnd()
    {
        anim.CrossFade("GroomingEnd", 0.2f);
        state = State.GroomingEnd;
    }

    private void Idle()
    {
        anim.CrossFade("Idle", 0.2f);
        state = State.WalkReady;
    }

    private void GlideUp()
    {
        anim.CrossFade("Flap1Up", 1f);
        state = State.GlideUp;
    }

    private void GlideFoward()
    {
        speed *= 3;
        anim.CrossFade("Flap1Left", 1f);
        state = State.GlideForward;
    }

    private void Walk()
    {
        anim.CrossFade("RunForward", 1f);
        state = State.Walk;
    }

    private void GlideDown()
    {
        speed /= 3;
        anim.CrossFade("Flap1Down", 0.5f);
        state = State.GlideDown;
    }

    private void Land()
    {
        anim.CrossFade("Landing", 1f);
        state = State.Land;
    }

    private void SitDown()
    {
        anim.CrossFade("SitDown", 1f);
        state = State.Seat;
        btnl.enabled = true;
    }

    public void OnClickBtn()
    {
        btnl.enabled = false;
        anim.CrossFade("Rebirth", 1f);
    }

    private void StartIdle()
    {
        anim.CrossFade("Idle", 1f);
        state = State.Idle;
    }
}
