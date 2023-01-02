using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Handle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] GameObject handle;
    [SerializeField] Scrollbar scrollbar;
    public void OnPointerDown(PointerEventData eventData)
    {
        handle.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.SetActive(false);    
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        handle.SetActive(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        handle.SetActive(false);
    }


    //float dist;
    //float Beforedist;
    //public void On()
    //{
    //    Beforedist = dist;
    //    dist = scrollbar.value;
    //    float result = Mathf.Abs(dist- Beforedist);
    //    print(result);
    //    if(result<0.0001f)
    //    {
    //        handle.SetActive(false);
    //    }
    //    else
    //    {
    //        handle.SetActive(true);
    //    }
    //}

}
