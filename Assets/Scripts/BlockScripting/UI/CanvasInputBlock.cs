

using UnityEngine;

public class CanvasInputBlock : CanvasBlockBase
{
    public BlockInputType inputType;
    public UnityEngine.UI.Dropdown typeDropdown;
    public MonoBehaviour inputField;

    public void Start()
    {
        
    }

    public void UpdateInput()
    {
        switch (inputType)
        {

        }
    }

    public override void Begin(CanvasGraph cg)
    {
        object value = null;

        if (inputField!= null)
        {
            switch (inputField)
            {
                case UnityEngine.UI.Toggle toggle:
                    break;
                case UnityEngine.UI.InputField inputField:
                    break;
            }
        }

        Block = new InputBlock() { value = new Value(value) };
    }
}

