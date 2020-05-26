using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagInventory : MonoBehaviour
{
    public void MoveBagUI()
    {
        this.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,0);
    }
}
