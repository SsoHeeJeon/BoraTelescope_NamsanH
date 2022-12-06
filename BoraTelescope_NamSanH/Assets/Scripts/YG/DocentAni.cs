using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocentAni : MonoBehaviour
{
    // Start is called before the first frame update
    Animator Anim;

    public float NarrtionLen;

    [SerializeField]
    NamSanHMode namsan;

    public void AniHi()
    {
        if(Anim==null)
        {
            Anim = GetComponent<Animator>();
        }
        CancelInvoke("InvokeAnimation");
        Anim.CrossFade("Ani_Emotion_Hi_02", 0f);
    }

    public void HitoTalk()
    {
        namsan.Narration.Play();
        Anim.CrossFade("Ani_Talk_01", 0);
        Invoke("InvokeAnimation", NarrtionLen-0.5f);
    }

    void InvokeAnimation()
    {
        Anim.CrossFade("Ani_Idle_01", 0.2f);
    }
}
