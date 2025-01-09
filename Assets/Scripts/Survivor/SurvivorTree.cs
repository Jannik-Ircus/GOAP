using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivorTree : MonoBehaviour
{
    [SerializeField]
    private Image progressBar;
    [HideInInspector]
    public float progress = 0;
    public GameObject wood;
    public float choppingDuration = 4;
    private bool alreadyChopping = false;

    private void Start()
    {
        //StartCoroutine(ChopTree());

    }

    public void TreeAction()
    {
        if (this == null) return;
        if(!alreadyChopping)StartCoroutine(ChopTree());
    }

    public void PauseTreeAction()
    {
        StopAllCoroutines();
        alreadyChopping = false;
    }

    private IEnumerator ChopTree()
    {
        alreadyChopping = true;
        progress += 1/choppingDuration;
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
