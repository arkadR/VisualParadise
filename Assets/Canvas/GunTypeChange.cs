using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunTypeChange : MonoBehaviour
{
    public Text guntype;

    public void ChangeGunType(string value)
    {
        guntype.text = value;
    }
}
