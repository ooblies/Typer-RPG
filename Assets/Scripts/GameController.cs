using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text displayText;
    // Start is called before the first frame update
    void Start()
    {
        updateCurrentString("");
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalBuffs.Instance.friendlyTypingFreezeDuration <= 0)
        {
            if (Input.anyKeyDown)
            {
                string strDown = Input.inputString;

                Regex r = new Regex("^[a-zA-Z0-9]*$");
                if (r.IsMatch(strDown) || strDown == " ")
                {
                    updateCurrentString(GlobalVars.Instance.CurrentTypedString += strDown);
                }
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                updateCurrentString("");
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                updateCurrentString(GlobalVars.Instance.CurrentTypedString.Substring(0, GlobalVars.Instance.CurrentTypedString.Length - 1));
            }
        }
        
        
        if (GlobalVars.Instance.CurrentTypedString != displayText.text)
        {
            updateCurrentString(GlobalVars.Instance.CurrentTypedString);
        }
    }

    void updateCurrentString(string c) {
        GlobalVars.Instance.CurrentTypedString = c;
        displayText.text = GlobalVars.Instance.CurrentTypedString;
    }
}
