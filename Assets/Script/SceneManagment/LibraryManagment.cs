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

    private int level = 0;

    [HideInInspector]
    public int state;

    public static LibraryManagment instance;

    private void Awake()
    {
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

    }

    private void Start()
    {
        StartCoroutine(Fade());
        if(state == 1)
            SettingsEngima();
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