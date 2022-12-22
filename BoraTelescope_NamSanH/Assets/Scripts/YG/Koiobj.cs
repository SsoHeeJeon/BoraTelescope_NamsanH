using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koiobj : MonoBehaviour
{
    [SerializeField]
    GameObject Obj1;
    [SerializeField]
    GameObject Obj2;
    // Start is called before the first frame update
    
    public void ObjOn()
    {
        Obj1.SetActive(true);
        Obj2.SetActive(true);
    }

    public void ObjOff()
    {
        Obj1.SetActive(false);
        Obj2.SetActive(false);
    }
}
