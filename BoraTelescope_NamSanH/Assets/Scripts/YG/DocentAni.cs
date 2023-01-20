using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocentAni : MonoBehaviour
{
    // Start is called before the first frame update
    Animator Anim;

    public float NarrtionLen;
    public AudioClip AdClip;
    [SerializeField]
    NamSanHMode namsan;


    private void Start()
    {
        Anim = GetComponent<Animator>();
    }

    public void AniHi()
    {
        CancelInvoke("InvokeAnimation");
        Anim.CrossFade("Ani_Emotion_Hi_02", 0f);
    }

    void InvokeAnimation()
    {
        Anim.CrossFade("SinBu_Cloth_Ani_Idle_Talk_v2022_re-noMouse", 0.2f);
    }

    public void CancelTalk()
    {
        CancelInvoke("InvokeAnimation");
        InvokeAnimation();
    }

    public void Talk()
    {
        Anim.CrossFade("SinBu_Cloth_Ani_Idle_Talk_v2022", 0.2f);
        Invoke("InvokeAnimation", NarrtionLen - 0.5f);
    }
}
