using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class KoiManager : MonoBehaviour
{
    [SerializeField]
    GameObject Koi;

    [SerializeField]
    GameObject Eat;

    [SerializeField]
    Koi[] Koiarr;




    public List<GameObject> EatList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("InsEat");
    }

    
    IEnumerator InsEat()
    {
        yield return new WaitForSeconds(3f);
        float x = Random.Range(-1.2f, 2.8f);
        float z = Random.Range(-1.2f, 2.8f);
        if (EatList.Count <= 5)
        {
            GameObject obj = Instantiate(Eat);
            EatList.Add(obj);
            obj.transform.parent = Koi.transform;
            obj.transform.localPosition = new Vector3(x, 0.44f, z);
        }
        StartCoroutine("InsEat");
    }

    public void OnClickBtn()
    {
        for(int i=0; i< Koiarr.Length; i++)
        {
            Koiarr[i].OnClickBtn();
        }
    }
}
