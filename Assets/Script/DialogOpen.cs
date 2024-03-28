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
    private string currentSentence; // pour pouvoir afficher instantanement la phrase en cours
    private Button choice1;
    private Button choice2;

    private string sentenceChoice1;
    private string sentenceChoice2;

    private bool ifDialog = false;

    private bool sentenceComplete = true;
    
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
        choice1 = GameObject.FindGameObjectWithTag("UIChoice1").GetComponent<Button>();
        choice2 = GameObject.FindGameObjectWithTag("UIChoice2").GetComponent<Button>();
    }  
    
    
    public void StartDialog(Dialog _dialog) // chercher a afficher les boutons seulement a la derniere phrase
    {
        PlayerMovement.instance.StopMovement();
        textNameCanvas.text = _dialog.name;
        if(_dialog.choices.Length == 2) // cette solution n'est pas viable si on travail en groupe.
        {
            ifDialog = true;
            choice1.GetComponentInChildren<Text>().text = _dialog.choices[0];
            choice2.GetComponentInChildren<Text>().text = _dialog.choices[1];

            sentenceChoice1 = _dialog.sentencesChoice1;
            sentenceChoice2 = _dialog.sentencesChoice2;
        }
        else
        {
            choice1.gameObject.SetActive(false);
            choice2.gameObject.SetActive(false);
            ifDialog = false;
        }

        animatorDialog.SetBool("isOpen", true);

        sentences.Clear();
        foreach (string sentence in _dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentences();
    }

    public bool DisplayNextSentences()
    {
        if(!sentenceComplete){
            StopAllCoroutines();
            textDialogCanvas.text = currentSentence;
            sentenceComplete = true;
        }
        else if(sentences.Count == 0 && !ifDialog) { 
            EndDialog();
            return false;
        }
        else if (sentences.Count == 0 && ifDialog)
        {
            if (!choice1.gameObject.activeSelf)    // on affiche les boutons lorsque c'est la derni�re phrase
            {
                choice1.gameObject.SetActive(true);
                choice2.gameObject.SetActive(true);
            }
        }
        else 
        {
            string sentence = sentences.Dequeue();
            currentSentence = sentence;
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
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
        sentenceComplete = false;
        foreach (string word in words)
        {
            textDialogCanvas.text += word + " ";
            yield return new WaitForSeconds(0.05f);
        }
        sentenceComplete = true;
    }

    public void EndDialog()
    {
        animatorDialog.SetBool("isOpen", false);
        textNameCanvas.text = "";
        textDialogCanvas.text = "";
        PlayerMovement.instance.enabled = true;
    }

    public void PlayerMakeAChoice(int choice)
    {
        if(choice == 1)
        {
            sentences.Enqueue(sentenceChoice1);
        }
        else if (choice == 2)
        {
            sentences.Enqueue(sentenceChoice2);
        }

        choice1.gameObject.SetActive(false);
        choice2.gameObject.SetActive(false);

        ifDialog = false;

        DisplayNextSentences();
    }

    private void DisplayButton()
    {

    }
}
