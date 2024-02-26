using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject toplamaButton, cikarmaButton, cikisButton, renkEslemeButton;

    void Start()
    {
        butonSonradanGel();   
    }

    public void GameLevel(string hangiIslem) //Toplama veya çýkarma seçimine göre sorularýn getirilmesini saðlar
    {
        PlayerPrefs.SetString("hangiIslem", hangiIslem);
        SceneManager.LoadScene("gameLevel");
    }

    public void RenkEsleme()
    {
        SceneManager.LoadScene("SampleScene"); //Renk eþleme sahnesine gidilmesini saðlar
    }

    public void oyundanCik()
    {
        Application.Quit();
    }

    void butonSonradanGel() //Butonlarýn fluudan görünüre doðru efektli bir þekilde açýlmasýný saðlar
    {
        toplamaButton.GetComponent<CanvasGroup>().DOFade(1, 0.8f);
        cikarmaButton.GetComponent<CanvasGroup>().DOFade(1, 0.8f);
        renkEslemeButton.GetComponent<CanvasGroup>().DOFade(1, 0.8f);
        cikisButton.GetComponent<CanvasGroup>().DOFade(1, 0.8f).SetDelay(0.5f);
    }
}
