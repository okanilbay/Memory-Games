using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sonucPanel : MonoBehaviour
{
    public void OyunaYenidenBasla()
    {
        SceneManager.LoadScene("gameLevel"); //Game sahnesine gitmesini saðlar
    }
    public void OyunaYenidenBasla2()
    {
        SceneManager.LoadScene("SampleScene"); //Renk Eþleme sahnesine gitmesini saðlar
    }

    public void AnaMenuyeDon()
    {
        SceneManager.LoadScene("menuLevel"); //Anamenü sahnesine gitmeyi saðlar
    }
}
