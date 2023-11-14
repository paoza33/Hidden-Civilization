using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBeggining : MonoBehaviour
{
    public Dialog dialog;

    // Start is called before the first frame update
    void Start()
    {
        DialogOpen.instance.StartDialog(dialog);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Interact")){
            if(!DialogOpen.instance.DisplayNextSentences()){
                enabled = false;
            }
        }
    }
}
