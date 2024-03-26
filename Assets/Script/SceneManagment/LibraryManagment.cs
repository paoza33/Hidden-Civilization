using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LibraryManagment : MonoBehaviour
{
    private List<int> orderPlayer = new List<int>();
    private List<int> orderSolution = new List<int>();

    private List<int> solution1 = new List<int>{ 1, 2};
    private List<int> solution2 = new List<int> { 3,4};
    private List<int> solution3 = new List<int> { 5,6};

    private List<GameObject> symbols = new List<GameObject>();

    public List<GameObject> symbols1;
    public List<GameObject> symbols2;
    public List<GameObject> symbols3;

    public GameObject playerStart;
    public Transform spawnLibTwo;

    public BoxCollider[] collidersDesactivateState0;
    public BoxCollider[] collidersDesactivateState1;
    public GameObject[] objState0;
    public GameObject[] objState1;

    private int nbrInteractionState0; // déclanche un dialogue quand = en state0
    public Dialog dialogEndState0;
    public Dialog dialogBeginningState2;

    public GameObject exitWall;

    private int level = 0;

    [HideInInspector]
    public int state;

    public static LibraryManagment instance;

    private void Awake()
    {
        enabled = false;
        if(instance != null)
        {
            Debug.Log("il y a plus d'une instance de library managment");
            return;
        }
        instance = this;

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
                StartCoroutine(Fade());
        }
        else if(state == 1){    // nuit -> pas encore le médaillon
            foreach(BoxCollider coll in collidersDesactivateState1)
                coll.enabled = false;

            foreach(GameObject obj in objState1)
                obj.SetActive(true);
                StartCoroutine(Fade());
        }
        else if (state == 2){
            SettingsEngima();
            DialogOpen.instance.StartDialog(dialogBeginningState2);
            enabled = true;
        }
    }

    private void Update()
    {
        if(Input.GetButtonDown("Interact")){
            if(!DialogOpen.instance.DisplayNextSentences()){
                if (state == 2){ // start state2
                    StartCoroutine(Fade());
                    enabled = false;
                }
            }
        }
    }

    public void AddOrderPlayer(int symboleID, GameObject symbol)
    {
        if (orderPlayer.Count < orderSolution.Count) // on ajoute si liste pas compl�te
        {
            if(!orderPlayer.Contains(symboleID))
            {
                orderPlayer.Add(symboleID);
                symbol.GetComponent<FlickeringEmissive>().enabled = true;   // la valeur isReverse est d�j� sur true, donc il suffit juste de l'activer
            }
            else
            {
                return;
            }
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
        orderPlayer.Clear();
        StartCoroutine(DelayResetFlicker());
    }

    private IEnumerator DelayResetFlicker()
    {
        yield return new WaitForSeconds(0.25f);
        ResetFlickering();
    }

    private void LevelAccomplished()    // a faire : on supprime l'affichage des symboles une fois l'ordre trouv� -> donc trouver exactement le meme nombre de mots que de symboles
    {
        orderPlayer.Clear();
        if (level == 0)
        {
            level = 1;
            UpdateLevel(solution2, symbols2);
        }
        else if (level == 1)
        {
            level = 2;
            UpdateLevel(solution3, symbols3);
        }
        else if (level == 2)
        {
            Debug.Log("finished");
        }
    }

    private void SettingsEngima()
    {
        symbols.AddRange(symbols1);
        orderSolution.AddRange(solution1);
        ResetFlickering();
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
        if(nbrInteractionState0 == 5){
            StartCoroutine(StartDialogEndState0());
        }
    }

    private IEnumerator StartDialogEndState0(){
        yield return new WaitForSeconds(1f);
        exitWall.SetActive(false); // desactive le mur invisible pour quitter library
        DialogOpen.instance.StartDialog(dialogEndState0);
        enabled = true;
    }

    private IEnumerator Fade()
    {
        PlayerMovement.instance.StopMovement();
        yield return new WaitForSeconds(1f);
        Animator animator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
        PlayerMovement.instance.enabled = true;
        CameraMovement.instance.cameraFixX = false;
        CameraMovement.instance.cameraFixZ = false;
    }
}