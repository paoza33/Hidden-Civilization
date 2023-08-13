using UnityEngine;

//pour qu'il puisse être utilisé dans d'autre script
[System.Serializable]
public class Dialog
{
    public string name;

    // "TextArea" Create a scrollable textview in the inspector.
    [TextArea(3, 10)]
    public string[] sentences;
}
