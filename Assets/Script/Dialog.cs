using UnityEngine;

//pour qu'il puisse être utilisé dans d'autre script
[System.Serializable]
public class Dialog
{
    public string name;

    public string[] choices;

    public string sentencesChoice1;
    public string sentencesChoice2;

    // "TextArea" Create a scrollable textview in the inspector.
    [TextArea(3, 10)]
    public string[] sentences;
}
