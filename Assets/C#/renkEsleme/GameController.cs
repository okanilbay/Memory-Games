using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Sprite bgImage;

    public Sprite[] puzzles;

    public List <Sprite> gamePuzzles = new List<Sprite>();

    public List<Button> btns = new List<Button>();

    private bool firsGuess, secondGuess;

    private int countGuesses, countCorrectGuesses, gameGuesses, firstGuessIndex, secondGuessIndex;

    private string firstGuessPuzzle, secondGuessPuzzle;

    [SerializeField]
    private GameObject sonucPanel;

    private void Awake()
    {
        sonucPanel.GetComponent<RectTransform>().localScale = Vector3.zero; //sonuç panelinin ekrandan gizlenmesi saðlanýr.
        puzzles = Resources.LoadAll<Sprite>("Resimler/colorpalet"); //renklerin içerisinde olduðu klasör tanýtýlýr.
    }
    private void Start() //Oyun baþladýðýnda metotlarýn çaðrýlmasý
    {
        GetButtons ();
        AddListeners ();
        AddGamePuzzles ();
        Shuffle (gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
    }
    void GetButtons() //Renklerin içerisinde bulunduðu resimler parçalanýr.
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        for (int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

    void AddGamePuzzles() //Renkler butonlarýn içerisine daðýtýlýr.
    {
        int looper = btns.Count;
        int index = 0;
        for (int i = 0; i < looper; i++)
        { 
            if (index == looper / 2 )
            {
                index = 0;
            }
            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    void AddListeners() // Buton týklandýðýnda içerisindeki resmi hafýzaya alýr.
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }
        
    public void PickAPuzzle() //týklanan iki butonun içerisindeki renklerin ayný olup olmadýðý kontrol edilir.
    {

        if (!firsGuess)
        {
            firsGuess = true;

            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;

            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }
        else if (!secondGuess)
        {
            secondGuess = true;

            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;

            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            countGuesses++;

            StartCoroutine(CheckIfThePuzzlesMatch());
        }

    }
    IEnumerator CheckIfThePuzzlesMatch() //Tüm eþleþmelerin yapýlýp yapýlmadýðýný kontrol edilir.
    {
        yield return new WaitForSeconds(1f);

        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(.3f);

            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

            OyunBitti();
        }
        else
        {
            yield return new WaitForSeconds(.4f);
            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }

        yield return new WaitForSeconds(.4f);

        firsGuess = secondGuess = false;
    }

    void OyunBitti()
    {
        countCorrectGuesses++;

        if (countCorrectGuesses == gameGuesses)
        {
            sonucPanel.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);
            //Sonuç panelini oyun bittikten sonra görünmesini saðlar
        }
    }

  

    void Shuffle(List<Sprite> list) //Renklerin her seferinde farklý gelmesi için karýþtýrýlýr.
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

   
}
