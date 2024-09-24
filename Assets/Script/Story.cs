using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/Story")]
public class Story : ScriptableObject
{
    [TextArea]
    public string story;
    public string hint;
    public string part;
    public Exit[] exits;

}
