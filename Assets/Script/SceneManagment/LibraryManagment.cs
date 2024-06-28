using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LibraryManagment : MonoBehaviour
{
    public Dialog startState0, startState1, startState2;
    private bool startingDialog = true;

    private List<int> orderPlayer = new List<int>();
    private List<int> orderSolution = new List<int>();

    private List<int> solution1 = new List<int>{ 2, 3, 1, 5, 4};
    private List<int> solution2 = new List<int> { 7, 9, 8, 6, 10};
    private List<int> solution3 = new List<int> { 14, 13, 12, 11};
    private List<int> solution4 = new List<int> { 16, 15, 20, 19, 17, 18};


    private List<GameObject> symbols = new List<GameObject>();

    public List<GameObject> symbols1, symbols2, symbols3, symbols4;

    public GameObject playerStart;
    public Transform spawnLibTwo;
    public Transform spawnState2;

    public BoxCollider[] collidersDesactivateState0;
    public BoxCollider[] collidersDesactivateState1;

    public GameObject[] allCab;
    public GameObject[] cabLevel0, cabLevel1, cabLevel2, cabLevel3;

    public GameObject[] objState0;
    public GameObject[] objState1;

    private int nbrInteractionState0; // déclanche un dialogue quand = en state0
    public Dialog dialogEndState0;
    public Dialog ending;

    public Dialog bookLevel0, bookLevel1, bookLevel2, bookLevel3;

    private int level = 0;

    private TextMeshProUGUI readBook;
    private TextMeshProUGUI textInteract;

    [HideInInspector]
    public int state;

    public int currentId;
    private GameObject currentSymbol;
    private BoxCollider currentBoxCol;

    public GameObject windows;

    private bool endingState0;

    [HideInInspector]
    public bool readingBook;

    public bool anotherInteraction;
    private bool isEnding;

    public AudioClip clip;

    private TextMeshProUGUI counterText;    // counter interaction cabinet in state 0

    public static LibraryManagment instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("il y a plus d'une instance de library managment");
            return;
        }
        instance = this;

        counterText = GameObject.FindGameObjectWithTag("Counter").GetComponent<TextMeshProUGUI>();
        readBook = GameObject.FindGameObjectWithTag("UIReadBook").GetComponent<TextMeshProUGUI>();
        textInteract = GameObject.FindGameObjectWithTag("UIInteract").GetComponent<TextMeshProUGUI>();

        SaveDataSpawn data = SaveDataManager.LoadDataSpawn();
        if (data.previousSceneName == "Library2")
            playerStart.transform.position = spawnLibTwo.position;

        SaveDataSceneState dataState = SaveDataManager.LoadDataSceneState();
        state = dataState.libraryState;

        if(state == 0)
        {
            foreach(BoxCollider coll in collidersDesactivateState0)
                coll.enabled = false;
            foreach(GameObject obj in objState0)
                obj.SetActive(true);
            DialogOpen.instance.StartDialog(startState0);
        }
        else if(state == 1){    // nuit -> pas encore le médaillon
            foreach(BoxCollider coll in collidersDesactivateState1)
                coll.enabled = false;

            foreach(GameObject obj in objState1)
                obj.SetActive(true);
            DialogOpen.instance.StartDialog(startState1);
        }
        else if (state == 2){
            foreach (GameObject obj in cabLevel1) { obj.GetComponent<BoxCollider>().enabled = false; }
            foreach (GameObject obj in cabLevel2) { obj.GetComponent<BoxCollider>().enabled = false; }
            foreach (GameObject obj in cabLevel3) { obj.GetComponent<BoxCollider>().enabled = false; }

            playerStart.transform.position = spawnState2.position;
            SettingsEngima();
            DialogOpen.instance.StartDialog(startState2);
            StartCoroutine(FadeState2());
        }
    }

    private void Update()
    {
        if(!anotherInteraction)
        {
            if (Input.GetButtonDown("ReadBook") && !readingBook && state == 2 && !isEnding && !startingDialog)
            {
                readingBook = true;
                readBook.enabled = false;
                textInteract.enabled = false;

                if(level == 0)
                    DialogOpen.instance.StartDialog(bookLevel0);
                else if(level == 1)
                    DialogOpen.instance.StartDialog(bookLevel1);
                else if(level == 2)
                    DialogOpen.instance.StartDialog(bookLevel2);
                else if(level == 3)
                    DialogOpen.instance.StartDialog(bookLevel3);

            }
            else if(endingState0 && Input.GetButtonDown("Interact")){
                if (!DialogOpen.instance.DisplayNextSentences())
                {
                    endingState0 = false;
                    counterText.enabled = false;
                }
            }

            else if (startingDialog && Input.GetButtonDown("Interact")){
                if(!DialogOpen.instance.DisplayNextSentences()){
                    startingDialog = false;
                    if(state == 2)
                    {

                    }   
                    else
                        StartCoroutine(Fade());
                }
            }
            else if(readingBook && Input.GetButtonDown("Interact"))
            {
                if (!DialogOpen.instance.DisplayNextSentences())
                {
                    readingBook = false;
                    readBook.enabled = true;
                }
            }
            else if(isEnding && Input.GetButtonDown("Interact"))
            {
                if (!DialogOpen.instance.DisplayNextSentences())
                {
                    PlayerMovement.instance.enabled = true;
                    CameraMovement.instance.cameraFixZ = false;
                }
            }
        }
    }

    public void AddOrderPlayer(int symboleID, GameObject symbol)
    {
        if (orderPlayer.Count < orderSolution.Count) // on ajoute si liste pas compl�te
        {
            orderPlayer.Add(symboleID);
            symbol.GetComponent<FlickeringEmissive>().enabled = true;   // la valeur isReverse est d�j� sur true, donc il suffit juste de l'activer
        }

        if ((orderPlayer.Count == orderSolution.Count) && orderPlayer.SequenceEqual(orderSolution))   // si l'ordre du joueur est correct
        {
            LevelAccomplished();
        }
        else if ((orderPlayer.Count == orderSolution.Count) && !orderPlayer.SequenceEqual(orderSolution))
        {
            LevelFailed();
        }
        else if(orderPlayer.Count > orderSolution.Count ) // cas qui n'est pas suppose arriver
        {
            Debug.Log("depassement non suppos� etre possible");
        }
    }

    private void LevelFailed()
    {
        if (level == 0)
        {
            foreach (GameObject obj in cabLevel0)
            {
                obj.GetComponent<BoxCollider>().enabled = true;
                obj.GetComponent<CabinetInteraction>().playerAlreadyInteract = false;
            }
        }
        if (level == 1)
        {
            foreach (GameObject obj in cabLevel1)
            {
                obj.GetComponent<BoxCollider>().enabled = true;
                obj.GetComponent<CabinetInteraction>().playerAlreadyInteract = false;
            }
        }
        if (level == 2)
        {
            foreach (GameObject obj in cabLevel2)
            {
                obj.GetComponent<BoxCollider>().enabled = true;
                obj.GetComponent<CabinetInteraction>().playerAlreadyInteract = false;
            }
        }
        if (level == 3)
        {
            foreach (GameObject obj in cabLevel3)
            {
                obj.GetComponent<BoxCollider>().enabled = true;
                obj.GetComponent<CabinetInteraction>().playerAlreadyInteract = false;
            }
        }

        orderPlayer.Clear();
        StartCoroutine(DelayResetFlicker());
    }

    private IEnumerator DelayResetFlicker()
    {
        yield return new WaitForSeconds(0.25f);
        ResetFlickering();
    }

    private void LevelAccomplished()
    {
        orderPlayer.Clear();
        if (level == 0)
        {
            foreach(GameObject obj in cabLevel0)
            {
                obj.GetComponent<BoxCollider>().enabled = false;
                obj.GetComponent<CabinetInteraction>().enabled = false;
            }
            foreach (GameObject obj in cabLevel1)
                obj.GetComponent<BoxCollider>().enabled = true;

            level = 1;
            UpdateLevel(solution2, symbols2);
        }
        else if (level == 1)
        {
            foreach (GameObject obj in cabLevel1)
            {
                obj.GetComponent<BoxCollider>().enabled = false;
                obj.GetComponent<CabinetInteraction>().enabled = false;
            }
            foreach (GameObject obj in cabLevel2)
                obj.GetComponent<BoxCollider>().enabled = true;
            level = 2;
            UpdateLevel(solution3, symbols3);
        }
        else if (level == 2)
        {
            foreach (GameObject obj in cabLevel2)
            {
                obj.GetComponent<BoxCollider>().enabled = false;
                obj.GetComponent<CabinetInteraction>().enabled = false;
            }
            foreach (GameObject obj in cabLevel3)
                obj.GetComponent<BoxCollider>().enabled = true;
            level = 3;
            UpdateLevel(solution4, symbols4);readBook.enabled = false;
        }
        else if (level == 3)
        {
            AudioManager.instance.StopCurrentSong();
            readBook.enabled = false;
            DialogOpen.instance.StartDialog(ending);
            isEnding = true;
        }
    }

    private void SettingsEngima()
    {
        symbols.AddRange(symbols1);
        orderSolution.AddRange(solution1);
        ResetFlickering();
    }

    public void UpdateCurrentCabinet(int id, GameObject symbol, BoxCollider col)
    {
        currentId = id;
        currentSymbol = symbol;
        currentBoxCol = col;
    }

    public void Choice(int choice)
    {
        DialogOpen.instance.EndDialog();
        if (choice == 0)
        {
            textInteract.enabled = false;
            currentBoxCol.enabled = false;
            AddOrderPlayer(currentId, currentSymbol);
        }
        else
        {
            textInteract.enabled = true;
            allCab[currentId -1].GetComponent<CabinetInteraction>().enabled = true;
            allCab[currentId - 1].GetComponent<CabinetInteraction>().playerAlreadyInteract = false;
        }

        readBook.enabled = true;
    }

    private void UpdateLevel(List<int> newSolution, List<GameObject> newSymbols)
    {
        orderSolution.Clear();
        orderSolution.AddRange(newSolution);

        SetSymbolsDesactive();

        symbols.Clear();
        symbols.AddRange(newSymbols);

        ResetFlickering();
    }

    private void SetSymbolsDesactive()
    {
        for (int i = 0; i < symbols.Count; i++)
        {
            symbols[i].SetActive(false);
        }
    }

    private void ResetFlickering()
    {
        for(int i =0; i<symbols.Count; i++)
        {
            symbols[i].GetComponent<FlickeringEmissive>().enabled = true;
        }
    }

    public void SetupState0(){
        nbrInteractionState0 +=1;
        counterText.text = nbrInteractionState0 + " / 5";

        if(nbrInteractionState0 == 5){
            StartCoroutine(StartDialogEndState0());
        }
    }

    private IEnumerator StartDialogEndState0(){
        yield return new WaitForSeconds(0.2f);
        windows.SetActive(true);
        endingState0 = true;
        DialogOpen.instance.StartDialog(dialogEndState0);
    }

    private IEnumerator Fade()
    {
        PlayerMovement.instance.StopMovement();
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlayThemeSong(clip);
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;
        yield return new WaitForSeconds(1f);
        if (state == 0)
            counterText.enabled = true;
    }

    private IEnumerator FadeState2()
    {
        PlayerMovement.instance.StopMovement();
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlayThemeSong(clip);
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        readBook.enabled = true;
    }

    private IEnumerator FadeEnding()
    {
        readBook.enabled = false;
        PlayerMovement.instance.StopMovement();
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(1f);

        SaveDataSceneState data = SaveDataManager.LoadDataSceneState();
        data.homeState = 3;
        SaveDataManager.SaveDataSceneState(data);
        SceneManager.LoadScene("Home");
        
    }
}