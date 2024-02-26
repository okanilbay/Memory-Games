using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class gameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject KareNesne;

    [SerializeField]
    private Transform karelerPaneli;

    private GameObject[] karelerDizisi = new GameObject[25];

    [SerializeField]
    private Transform soruPanel;

    [SerializeField]
    private Text soruText;

    [SerializeField]
    private Sprite[] kareSprites;

    [SerializeField]
    private GameObject sonucPanel;

    List<int> kareDegerleriListesi = new List<int>();

    int birinciSayi, ikinciSayi, kacinciSoru, butonDegeri, dogruSonuc, kalanCan;
    bool butonaBasilsinmi;
    string hangiIslem, zorlukDerecesi;

    kalanCanlarManager kalanCanlarManager;
    puanManager puanManager;

    GameObject gecerliKare;

    private void Awake()
    {
        kalanCan = 3;

        sonucPanel.GetComponent<RectTransform>().localScale = Vector3.zero; //Sonuç panelinin gizlenmesini saðlar

        kalanCanlarManager=Object.FindObjectOfType<kalanCanlarManager>(); //Kalan canlar için açýlan scripti çaðýrýr.
        puanManager = Object.FindObjectOfType<puanManager>(); // Puanlama için açýlan scripti çaðýrýr.

        kalanCanlarManager.kalanCanlariKontrolEt(kalanCan);
    }

    void Start()
    {
        butonaBasilsinmi = false;

        hangiIslem = PlayerPrefs.GetString("hangiIslem"); //Toplama veya çýkarma butonlarýndan seçilene göre soru üretmesini saðlar.

        soruPanel.GetComponent<RectTransform>().localScale= Vector3.zero; //soru panelinin açýlýþta görünmez olmasýný saðlar.

        KaraleriOlustur();
    }


    public void KaraleriOlustur()
    {
        for (int i = 0; i < 25; i++) // 25 tane cevap karesi oluþturmayý saðlar.
        {
            GameObject kare = Instantiate(KareNesne, karelerPaneli);

            kare.transform.GetChild(1).GetComponent<Image>().sprite = kareSprites[Random.Range(0,kareSprites.Length)]; 
            //karelerin arkasýna resim eklemeyi saðlar

            kare.transform.GetComponent<Button>().onClick.AddListener(() => ButonaBasildi()); 
            //kareler oluþturulurken soru paneli gelmeden týklanmalarý engellenir.
            karelerDizisi[i] = kare;
        }
        KareDegerleriYazdir();

        StartCoroutine(DoFadeRoutine());
        
        Invoke("SoruPaneliAc",1f); // Kareler oluþturulduktan sonra sorunun ekrana gelmesini saðlar.

    }

    void KareDegerleriYazdir() //karelerin içerisine rastgele sayýlar atanmasýný saðlar.
    {
        foreach (var kare in karelerDizisi)
        {
            int rndDeger = Random.Range(15, 30);

            kareDegerleriListesi.Add(rndDeger);

            kare.transform.GetChild(0).GetComponent<Text>().text = rndDeger.ToString();
        }
    }

    void ButonaBasildi()
    {
        if (butonaBasilsinmi)
        {
            butonDegeri = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text);
            // Týklanan butonun içindeki deðeri deðiþkende saklanýyor.

            gecerliKare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject; 
            // doðru karenin seçilip seçilmediði kontrul ediliyor.

            sonucuKontrolEt();
        }
        
    }

    void sonucuKontrolEt()
    {
        if (butonDegeri == dogruSonuc)
        {
            gecerliKare.transform.GetChild(1).GetComponent<Image>().enabled = true; 
            //doðru cevap seçildiðinde arkasýndaki resmin görünmesini saðladýk

            gecerliKare.transform.GetChild(0).GetComponent<Text>().text = ""; 
            //Doðru cevaptan sonra resmin üstünde sayýnýn kaybolmasýný saðladýk

            gecerliKare.transform.GetComponent<Button>().interactable = false;
            //doðru cevaplanan kareye tekrar týklanmamasýný saðladýk

            puanManager.puaniArttir(zorlukDerecesi); //Cevapladýðý soruya göre puan verilmesi

            kareDegerleriListesi.RemoveAt(kacinciSoru); //Verdiði soruyu tekrar vermemesi için

            if (kareDegerleriListesi.Count > 0)
            {
                SoruPaneliAc(); //Bir soruyu yanýtladýktan sonra yenisinin gelmesi için
            }
            else
            {
                OyunBitti();
            }
            
        }
        else
        {
            kalanCan--; // Hatalý cevap verilince canlarýn eksiltilmesini saðlar.
            kalanCanlarManager.kalanCanlariKontrolEt(kalanCan);
        }

        if (kalanCan <= 0) // Canlar bittikten sonra oyun bitti ekranýnýn açýlmasýný saðlar.
        {
            OyunBitti();
        }
    }

    void OyunBitti()
    {
        butonaBasilsinmi = false;
        sonucPanel.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack); 
        //Sonuç panelini oyun bittikten sonra görünmesini saðlar
    }

    IEnumerator DoFadeRoutine() //kareler oluþturulurken sýrayla efekt görünümünde ekrane gelmelerini saðlar
    {
        foreach (var kare in karelerDizisi)
        {
            kare.GetComponent<CanvasGroup>().DOFade(1, 0.2f); 

            yield return new WaitForSeconds(0.07f);
        }
    }

    void SoruPaneliAc() //Soru panelinin kareler oluþtuktan sonra ekrane gelmesini saðlar
    {
        SoruyuSor();
        butonaBasilsinmi = true;
        soruPanel.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }

    public void SoruyuSor() //sorularýn oluþturulmasýný saðlar ve zorluk dereceleri belirlenir.
    {
        switch (hangiIslem)
        {
            case "toplama":

                birinciSayi = Random.Range(2, 15);
                kacinciSoru = Random.Range(0, kareDegerleriListesi.Count);

                dogruSonuc = kareDegerleriListesi[kacinciSoru];
                ikinciSayi = kareDegerleriListesi[kacinciSoru] - birinciSayi;

                if (ikinciSayi <= 10 )
                {
                    zorlukDerecesi = "kolay";
                }
                else if (ikinciSayi > 10 && ikinciSayi <= 20)
                {
                    zorlukDerecesi = "orta";
                }
                else
                {
                    zorlukDerecesi = "zor";
                }

                soruText.text = ikinciSayi.ToString() + " + " + birinciSayi.ToString();

                break;

            case "cikarma":

                birinciSayi = Random.Range(2, 15);
                kacinciSoru = Random.Range(0, kareDegerleriListesi.Count);

                dogruSonuc = kareDegerleriListesi[kacinciSoru];
                ikinciSayi = kareDegerleriListesi[kacinciSoru] + birinciSayi;

                if (ikinciSayi <= 20)
                {
                    zorlukDerecesi = "kolay";
                }
                else if (ikinciSayi > 20 && ikinciSayi <= 35)
                {
                    zorlukDerecesi = "orta";
                }
                else
                {
                    zorlukDerecesi = "zor";
                }

                soruText.text = ikinciSayi.ToString() + " - " + birinciSayi.ToString();

                break;
        }
    }
}
