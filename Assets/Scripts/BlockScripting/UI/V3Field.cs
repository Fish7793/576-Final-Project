using UnityEngine;
using UnityEngine.UI;

public class V3Field : MonoBehaviour
{
    public Vector3 Value 
    { 
        get 
        {
            return new Vector3(
                xField != null ? float.Parse(xField.text) : 0, 
                yField != null ? float.Parse(yField.text) : 0, 
                zField != null ? float.Parse(zField.text) : 0
            );
        }  
    }

    public TMPro.TMP_InputField xField;
    public TMPro.TMP_InputField yField;
    public TMPro.TMP_InputField zField;

    public void Start()
    {
        if (xField != null)
        {
            xField.onValueChanged.AddListener((s) => { xField.text = CanvasInputBlock.ToNumber(s); });
            xField.text = "0";
        }

        if (yField != null)
        {
            yField.onValueChanged.AddListener((s) => { yField.text = CanvasInputBlock.ToNumber(s); });
            yField.text = "0";
        }

        if (zField != null)
        {
            zField.onValueChanged.AddListener((s) => { zField.text = CanvasInputBlock.ToNumber(s); });
            zField.text = "0";
        }
    }
}