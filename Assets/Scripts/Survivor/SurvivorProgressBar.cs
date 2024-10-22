using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivorProgressBar : MonoBehaviour
{
    public Image progressBar;
    public Color startColor;
    public Color endColor;
    public float duration = 6;
    private float elapsedTime = 0;
    public bool startFull = false;

    private void Start()
    {
        SetUpProgressBar();
    }

    private void OnEnable()
    {
        SetUpProgressBar();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        float progress = Mathf.Clamp01(1f - (elapsedTime / duration));

        progressBar.fillAmount = progress;
        Debug.Log(progress);
        Color progressColor = new Color(Mathf.Lerp(endColor.r, startColor.r, progress), Mathf.Lerp(endColor.g, startColor.g, progress), Mathf.Lerp(endColor.b, startColor.b, progress));
        progressBar.color = progressColor;
        if(progress==0)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void SetUpProgressBar()
    {
        if (startFull)
        {
            progressBar.fillAmount = 0;
        }
        else
        {
            progressBar.fillAmount = 1;
        }
        elapsedTime = 0;
    }

}
