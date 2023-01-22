using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogOpen : MonoBehaviour
{
    private Animator animatorDialog;
    private Text textNameCanvas;
    private Text textDialogCanvas;
    private Queue<string> sentences;
    public static DialogOpen instance;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Il y a plus d'une instance de DialogOpen");
            return;
        }
        instance = this;
        animatorDialog = GameObject.FindGameObjectWithTag("UIDialog").GetComponent<Animator>();
        textNameCanvas = GameObject.FindGameObjectWithTag("UINameDialog").GetComponent<Text>();
        textDialogCanvas = GameObject.FindGameObjectWithTag("UITextDialog").GetComponent <Text>();
        sentences = new Queue<string>();
    }

    public void StartDialog(Dialog _dialog)
    {
        animatorDialog.SetBool("isOpen", true);
        PlayerMovement.instance.enabled = false;
        textNameCanvas.text = _dialog.name;

        sentences.Clear();
        foreach (string sentence in _dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentences();
    }

    public bool DisplayNextSentences()
    {
        if(sentences.Count == 0) { 
            EndDialog();
            return false;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        return true;
    }
    IEnumerator TypeSentence(string sentence)
    {
        textDialogCanvas.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            textDialogCanvas.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }
    public void EndDialog()
    {
        animatorDialog.SetBool("isOpen", false);
        textNameCanvas.text = "";
        textDialogCanvas.text = "";
        PlayerMovement.instance.enabled = true;
    }
}
