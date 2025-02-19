using Amazon.IoTThingsGraph.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koi : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    [SerializeField]
    KoiManager koimanager;

    public float speed;
    [SerializeField]
    private float rotspeed;
    private float rot;
    private Vector3 dir;
    public float defaultspeed;

    public GameObject Eat;

    public Vector3 StartPos;

    public enum State
    {
        Move,
        Idle,
        EatMove,
        Eat,
        Click,
    }
    public State state = 0;

    private void Start()
    {
        //defaultspeed = speed;
        anim = GetComponent<Animator>();
        //StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Move:
                UpdateMove();
                break;
            case State.EatMove:
                UpdateEatMove();
                break;
            case State.Eat:
                UpdateEat();
                break;
            case State.Click:
                UpdateClick();
                break;

        }
        //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, -75, 90);
    }

    private void UpdateClick()
    {
        transform.LookAt(new Vector3(1, transform.localPosition.y, 1));
        float dist = Vector3.Distance(transform.localPosition, new Vector3(1, transform.localPosition.y, 1));

        if(dist>0.5f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(1, transform.localPosition.y, 1), Time.deltaTime*0.5f);
        }
        else
        {
            int x = UnityEngine.Random.Range(7, 10);
            Invoke("GoIdle", x);
        }
    }

    void UpdateIdle()
    {
        float dist = Vector3.Distance(transform.position, StartPos);
        transform.LookAt(StartPos);
        dir = transform.forward;
        transform.position = Vector3.Lerp(transform.position, StartPos, Time.deltaTime/2);
        //transform.position += dir * speed * Time.deltaTime;
        if (dist<0.5f)
        {
            state = State.Move;
        }
    }

    void UpdateMove()
    {
        dir = transform.forward;
        transform.position += dir * speed * Time.deltaTime;
        rot += rotspeed * Time.deltaTime;
        if (state == State.Move)
        {
            transform.rotation = Quaternion.Euler(rot, -75, 90);
            //float dist = Vector3.Distance(StartPos, transform.position);
            //if(dist>300)
            //{
            //    state = State.Idle;
            //}
        }
    }

    void UpdateEatMove()
    {
        if (Eat != null)
        {
            transform.LookAt(Eat.transform.position);
            dir = transform.forward;
            //transform.position += dir * speed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, Eat.transform.position, Time.deltaTime);
            float dist = Vector3.Distance(transform.localPosition, Eat.transform.localPosition);
            if (dist < 0.5f)
            {
                anim.CrossFade("Slow Swim", 0.2f);
                speed = 0;
                Invoke("DestroyEat", 5f);
                state = State.Eat;
            }
        }
        else
        {
            anim.CrossFade("Speed Swim", 0.2f);
            speed = defaultspeed;
            state = State.Idle;
        }
    }

    void UpdateEat()
    {
        //transform.rotation = Quaternion.Euler(10, transform.eulerAngles.y, transform.eulerAngles.z);
        //if(Eat!=null)
        //{
        //    if(Eat.transform.localPosition.y!=0.05f)
        //    {
        //        Eat.transform.localPosition = new Vector3(Eat.transform.localPosition.x, 0.05f, Eat.transform.localPosition.z);
        //    }
        //    else
        //    {
        //        Eat.transform.localPosition = new Vector3(Eat.transform.localPosition.x, 0, Eat.transform.localPosition.z);
        //    }
        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        //if(state!=State.Click)
        //{
        //    if(other.gameObject.name.Contains("eat"))
        //    {
        //        state = State.EatMove;
        //        other.gameObject.transform.localPosition = new Vector3(other.gameObject.transform.localPosition.x, transform.localPosition.y, other.gameObject.transform.localPosition.z);
        //        transform.LookAt(new Vector3(other.gameObject.transform.localPosition.x, transform.localPosition.y, other.gameObject.transform.localPosition.z));
        //        Eat = other.gameObject;
        //    }
        //}
    }

    void DestroyEat()
    {
        if(state!=State.Click)
        {
            if(Eat!=null)
            {
                koimanager.EatList.Remove(Eat);
            }
            Destroy(Eat);
            anim.CrossFade("Speed Swim", 0.2f);
            speed = defaultspeed;
            state = State.Idle;
        }
    }

    public void OnClickBtn()
    {
        state = State.Click;
    }

    void GoIdle()
    {
        speed = defaultspeed;
        state = State.Idle;
        CancelInvoke("GoIdle");
    }
}
