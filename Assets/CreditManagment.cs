using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManagment : MonoBehaviour
{
    private void Awake()
    {
        Animator credit = GameObject.FindGameObjectWithTag("Credit").GetComponent<Animator>();
        credit.SetTrigger("CreditIn");
    }
}
