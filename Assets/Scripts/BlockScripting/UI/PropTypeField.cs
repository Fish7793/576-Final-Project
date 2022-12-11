using System;
using System.Linq;
using UnityEngine;

public class PropTypeField : MonoBehaviour
{
    public PropType Value { get; set; }

    public TMPro.TMP_Dropdown dropdown;

    public void Start()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(Enum.GetNames(typeof(PropType)).ToList());
        dropdown.onValueChanged.AddListener((i) =>
        {
            Value = (PropType)i;
        });
    }
}
