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

    private int level = 0;

    public static LibraryManagment instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("il y a plus d'une instance de library managment");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        symbols.AddRange(symbols1);
        for(int i = 0; i < symbols.Count; i++)
        {
            symbols[i].SetActive(true);
        }
        orderSolution.AddRange(solution1);
    }

    public void AddOrderPlayer(int symboleID)
    {
        if (orderPlayer.Count < orderSolution.Count) // on ajoute si liste pas complète
        {
            if(!orderPlayer.Contains(symboleID))
            {
                orderPlayer.Add(symboleID);
            }
            else
            {
                Debug.Log("élément deja mis");
            }
            
        }
        Debug.Log(orderPlayer[0]);
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
            Debug.Log("depassement non suppose etre possible");
        }
    }

    private void LevelFailed()
    {
        Debug.Log("failed level " + level);
        orderPlayer.Clear();
        for (int i = 0; i < symbols.Count; i++)
        {
            symbols[i].GetComponent<FlickeringEmissive>().enabled = true;
            symbols[i].GetComponent<FlickeringEmissive>().isReverse = true;
        }
    }

    private void LevelAccomplished()    // a faire : on supprime l'affichage des symboles une fois l'ordre trouvé -> donc trouver exactement le meme nombre de mots que de symboles
    {
        Debug.Log("accomplished lvl " + level);
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
            //gérer l'ending
        }
    }

    private void UpdateLevel(List<int> newSolution, List<GameObject> newSymbols)
    {
        orderSolution.Clear();
        orderSolution.AddRange(newSolution);

        for (int i = 0; i < symbols.Count; i++)
        {
            symbols[i].GetComponent<FlickeringEmissive>().enabled = true;
            symbols[i].GetComponent<FlickeringEmissive>().isReverse = true;
        }
        SetSymbolsActive(false);

        symbols.Clear();
        symbols.AddRange(newSymbols);

        SetSymbolsActive(true);
    }

    private void SetSymbolsActive(bool isActive)
    {
        for (int i = 0; i < symbols.Count; i++)
        {
            symbols[i].SetActive(isActive);
            symbols[i].GetComponent<FlickeringEmissive>().enabled = true;
            symbols[i].GetComponent<FlickeringEmissive>().isReverse = true;
        }
    }
}
