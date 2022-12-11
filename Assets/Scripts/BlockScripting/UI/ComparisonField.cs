using System;
using System.Linq;
using UnityEngine;

public class ComparisonField : MonoBehaviour
{
    public Comparison Value { get; set; }

    public TMPro.TMP_Dropdown dropdown;

    public void Start()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(Enum.GetNames(typeof(Comparison)).ToList());
        dropdown.onValueChanged.AddListener((i) =>
        {
            Value = (Comparison)i;
        });
    }
}