using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image progressBar;
    public Color fullColor;
    public Color emptyColor;
    public float startFill = 1;

    private void Start()
    {
        if(progressBar == null)
        {
            Debug.LogError("No progressBar Image set on " + this.gameObject.name);
            Destroy(this);
        }

        SetProgressBar(startFill);
    }

    public void SetProgressBar(float percentage)
    {
        if (percentage > 1) percentage = 1;

        progressBar.fillAmount = percentage;
        Color progressColor = new Color(Mathf.Lerp(emptyColor.r, fullColor.r, percentage), Mathf.Lerp(emptyColor.g, fullColor.g, percentage), Mathf.Lerp(emptyColor.b, fullColor.b, percentage));
        progressBar.color = progressColor;
    }
}
