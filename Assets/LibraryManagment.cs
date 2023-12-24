using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LibraryManagment : MonoBehaviour
{
    public List<int> orderSolution;
    private List<int> orderPlayer;

    private void AddOrder(int i)
    {
        if(orderPlayer.Count > orderSolution.Count){ Debug.Log("Size of player list must not be larger than size of solution list");}
        else if (orderPlayer.Count < orderSolution.Count){
            orderPlayer.Add(i);
        }
        else if ((orderPlayer.Count == orderSolution.Count) && (orderPlayer.SequenceEqual(orderSolution))){
            // mettre l'evenement une fois que le joueur trouve la solution
        }
    }

    private bool OrderVerification(List<int> tab)
    {
        
        return false;
    }
}
