using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Loading : MonoBehaviour
{
    public Slider loadingSlider;
    public Text loadingText;

    private float currentValue = 0;

    void Start()
    {
        
       

    }
    void Update()
    {
        currentValue += Time.deltaTime / 3; // adjust deltaTime to control the speed of the slider
        loadingSlider.value = Mathf.Clamp01(currentValue);
        loadingText.text = "Loading... " + (int)(loadingSlider.value * 100) + "%";

        if (loadingSlider.value >= 1)
        {

            SceneManager.LoadScene(1);
        }
    }

    

}

