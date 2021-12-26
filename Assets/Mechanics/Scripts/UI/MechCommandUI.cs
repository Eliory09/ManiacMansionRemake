using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechCommandUI : MonoBehaviour
{
    public MechCommand data;
    public void Click()
    {
        MechGameManager.LoadCommand(data);
    }
}
