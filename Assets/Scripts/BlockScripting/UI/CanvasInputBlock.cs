using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CanvasInputBlock : CanvasBlockBase
{
    public BlockInputType inputType;
    public Transform fieldRoot;
    public TMPro.TMP_Dropdown typeDropdown;
    public MonoBehaviour inputField;

    public void Start()
    {
        typeDropdown.onValueChanged.AddListener((x) => 
        {
            inputType = (BlockInputType)x;
            UpdateInput(); 
        });

        var names = Enum.GetNames(typeof(BlockInputType));
        typeDropdown.ClearOptions();
        typeDropdown.AddOptions(names.ToList());
    }

    public static string ToNumber(string s)
    {
        var res = Regex.Match(s, "(-?\\d+(\\.\\d+)?)|(-?\\.\\d+)").Value.Trim();
        return String.IsNullOrEmpty(res) ? "0" : res;
    }

    public void UpdateInput()
    {
        foreach (Transform child in fieldRoot)
            Destroy(child.gameObject);

        switch (inputType)
        {
            case BlockInputType.None:
                break;
            case BlockInputType.Float:
                var if1 = Instantiate(GameManager.prefabs["V1Field"], fieldRoot).GetComponent<V3Field>();
                if1.transform.SetParent(fieldRoot);
                inputField = if1;
                break;
            case BlockInputType.Vector2:
                var if2 = Instantiate(GameManager.prefabs["V2Field"], fieldRoot).GetComponent<V3Field>();
                if2.transform.SetParent(fieldRoot);
                inputField = if2;
                break;
            case BlockInputType.Vector3:
                var if3 = Instantiate(GameManager.prefabs["V3Field"], fieldRoot).GetComponent<V3Field>();
                if3.transform.SetParent(fieldRoot);
                inputField = if3;
                break;
            case BlockInputType.Boolean:
                var check = Instantiate(GameManager.prefabs["BToggle"], fieldRoot).GetComponent<Toggle>();
                check.transform.SetParent(fieldRoot);
                inputField = check;
                break;
            case BlockInputType.PropType:
                var prp = Instantiate(GameManager.prefabs["BPropType"], fieldRoot).GetComponent<PropTypeField>();
                prp.transform.SetParent(fieldRoot);
                inputField = prp;
                break;

        }
    }

    public override void Begin(CanvasGraph cg)
    {
        object value = null;

        if (inputField != null)
        {
            switch (inputField)
            {
                case Toggle toggle:
                    value = toggle.isOn;
                    break;
                case TMPro.TMP_InputField inputField:
                    value = float.Parse(ToNumber(inputField.text));
                    break;
                case V3Field v3field:
                    value = v3field.Value;
                    break;
                case PropTypeField propTypeField:
                    value = propTypeField.Value;
                    break;
            }
        }

        Block = new InputBlock() { value = new Value(value) };
    }
}

