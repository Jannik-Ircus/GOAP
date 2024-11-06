using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivorTree : MonoBehaviour
{
    [SerializeField]
    private Image progressBar;
    private float progress = 0;
    public GameObject wood;

    private void Start()
    {
        StartCoroutine(ChopTree());

    }

    private IEnumerator ChopTree()
    {
        progress += 0.25f;
        progressBar.fillAmount = progress;

        yield return new WaitForSeconds(1);

        if (progress < 1)
        {
            StartCoroutine(ChopTree());
        }
        else
        {
            StartCoroutine(SpawnWood());
        }

    }

    private IEnumerator SpawnWood()
    {
        //this.gameObject.SetActive(false);
        Instantiate(wood, transform.position, transform.rotation);
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }
}
