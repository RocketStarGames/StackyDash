using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class PerfectmESSAGE : MonoBehaviour
{
    private float Ydefault;

    // Start is called before the first frame update
    void Start()
    {
        Ydefault = transform.position.y;
    }



    public void ShowBonusMessage()
    {
        StartCoroutine("anim");
    }

    IEnumerator anim()
    {
        GetComponent<Text>().DOFade(1f, 1f);
        transform.DOMoveY(Ydefault + 100f, 2f);
        yield return new WaitForSecondsRealtime(0.98f);
        GetComponent<Text>().DOFade(0f, 1f);
        yield return new WaitForSecondsRealtime(1f);
        transform.position =new Vector3 (transform.position.x, Ydefault,transform.position.z);


    }
}
