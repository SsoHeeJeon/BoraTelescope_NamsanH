using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObj : MonoBehaviour
{
    public GameObject obj;
    public GameObject obj_360;

    Vector3 obj_rot;
    Vector3 this_rot;


    // Start is called before the first frame update
    void Start()
    {
        obj_rot = this.transform.rotation.eulerAngles;
        this_rot = this.transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(this.transform.rotation.eulerAngles.y + " / " + this.transform.localRotation.eulerAngles.y);
        this.transform.rotation = Quaternion.Euler(obj_rot.x, obj_rot.y, obj_rot.z);
        this.transform.localRotation = Quaternion.Euler(this_rot.x, 109.7f, this_rot.z);
        obj.transform.rotation = Quaternion.Euler(0,obj_360.transform.rotation.y,0);
        //obj.transform.rotation = Quaternion.Euler(obj_rot.x, obj_rot.y, obj_rot.z);
    }
}
