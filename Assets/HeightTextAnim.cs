using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeightTextAnim : MonoBehaviour
{
    float DefaultScale;
    public Color Golden;
    float targetFade;

    void Start()
    {
        DefaultScale = transform.localScale.x;
        transform.DOScale(DefaultScale * 5f, 0f);

        //transform.GetChild(0).GetComponent<Text>().DOFade(0, 0);
        //transform.GetChild(1).GetComponent<Text>().DOFade(0, 0);

        transform.GetComponent<CanvasGroup>().DOFade(0, 0);

    }
    public void GoldenColor()
    {
        transform.GetChild(0).GetComponent<Text>().DOColor(Golden, 0f);
        transform.GetComponent<Image>().DOColor(Golden, 0f);
    }

    IEnumerator AnimText()
    {
        float DefaultScoreMultipliedScale = Camera.main.GetComponent<Global>().ScoreHandler.transform.GetChild(1).transform.localScale.x;
        Camera.main.GetComponent<Global>().ScoreHandler.transform.GetChild(1).transform.DOScale(DefaultScale * 5f, 0f);

        transform.DOScale(DefaultScale, 0.3f);
        //transform.GetChild(0).GetComponent<Text>().DOFade(1f, 0.3f);
        //transform.GetChild(1).GetComponent<Text>().DOFade(1f, 0.3f);
        transform.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
        transform.GetComponent<Image>().DOFade(1f, 0.3f);

        yield return new WaitForSecondsRealtime(0.5f);
        Camera.main.GetComponent<Global>().ScoreHandler.transform.GetChild(0).GetComponent<Image>().DOFade(targetFade, 0.2f);
        Camera.main.GetComponent<Global>().ScoreHandler.transform.GetChild(1).transform.DOScale(DefaultScale, 0.3f);
        Camera.main.GetComponent<Global>().ScoreHandler.transform.GetChild(1).GetComponent<Text>().DOFade(1, 0.3f);




    }
    public void Anim(int Height, float TargetFadeForTheBarAboveScore)
    {
        targetFade = TargetFadeForTheBarAboveScore;
        transform.GetChild(0).GetComponent<Text>().text = Height.ToString() + "M";

        StartCoroutine("AnimText");
    }

}
