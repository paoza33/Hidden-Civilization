using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogOpen : MonoBehaviour
{
    private Animator animatorDialog;
    private TextMeshProUGUI textNameCanvas;
    private TextMeshProUGUI textDialogCanvas;
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
        textNameCanvas = GameObject.FindGameObjectWithTag("UINameDialog").GetComponent<TextMeshProUGUI>();
        textDialogCanvas = GameObject.FindGameObjectWithTag("UITextDialog").GetComponent <TextMeshProUGUI>();
        sentences = new Queue<string>();
    }

    public void StartDialog(Dialog _dialog)
    {
        animatorDialog.SetBool("isOpen", true);
        PlayerMovement.instance.StopMovement();
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
    /*IEnumerator TypeSentence(string sentence)     // Letter by letter
    {
        textDialogCanvas.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textDialogCanvas.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }*/

    IEnumerator TypeSentence(string sentence)       // Word by word (work with sprite)
    {
        textDialogCanvas.text = "";
        string[] words = sentence.Split(' ');
        foreach (string word in words)
        {
            textDialogCanvas.text += word + " ";
            yield return new WaitForSeconds(0.05f);
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
