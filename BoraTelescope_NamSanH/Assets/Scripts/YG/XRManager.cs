using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class XRManager : MonoBehaviour
{
    public class XR
    {
        public string LabelName;
        public int Label_X;
        public int Label_Y;
        public int LabelScale;
    }

    public string[] XRarr;
    public GameObject[] XRobject;
    // Start is called before the first frame update

    void Start()
    {
        string XRjson = File.ReadAllText(Application.dataPath + "/XRModeLabelPosition.json");
        if (XRjson.Contains("}"))
        {
            XRarr = XRjson.Split('}');

            for (int index = -1; index < XRarr.Length - 1; index++)
            {
                if (index < XRarr.Length - 2)
                {
                    XRarr[index + 1] = XRarr[index + 1] + "}";
                }
            }
        }

        for(int i=0; i<XRarr.Length; i++)
        {
            try
            {
                XR myxr = JsonUtility.FromJson<XR>(XRarr[i].ToString());
                XRobject[i].transform.localPosition = new Vector3(myxr.Label_X, myxr.Label_Y, 0);
                XRobject[i].transform.localScale = new Vector3(myxr.LabelScale, myxr.LabelScale, myxr.LabelScale);
            }
            catch
            {

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
