using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUI : MonoBehaviour
{
    public Command data;
    public void Click()
    {
        GameManager.LoadCommand(data);
    }
}
