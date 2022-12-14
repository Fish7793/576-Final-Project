using System;
using System.Linq;
using UnityEngine;

public class BooleanOpField : MonoBehaviour
{
    public BooleanOp Value { get; set; }

    public TMPro.TMP_Dropdown dropdown;

    public void Start()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(Enum.GetNames(typeof(BooleanOp)).ToList());
        dropdown.onValueChanged.AddListener((i) =>
        {
            Value = (BooleanOp)i;
        });
    }
}