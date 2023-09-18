using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedyPi : MonoBehaviour
{
    public KMNeedyModule module;
    public KMSelectable moduleSelectable;

    public TextMesh digitLengthText;
    public TextMesh inputText;

    public KMSelectable[] buttons; // 0-9

    private string pi = "14159265358979323846";

    private int from;
    private int to;
    private string correctAnswer = "";

    private string input = "";

    bool isActivated = false;
    bool isSelected = false;

    static int moduleIdCounter = 1;
#pragma warning disable 0414
    int moduleId;
#pragma warning disable 0414

    private void Start()
    {
        module.OnNeedyActivation += OnNeedyActivation;
        module.OnNeedyDeactivation += OnNeedyDeactivation;
        module.OnTimerExpired += OnTimerExpired;

        moduleId = moduleIdCounter++;

        buttons[0].OnInteract += delegate () { buttonPress(0); return false; };
        buttons[1].OnInteract += delegate () { buttonPress(1); return false; };
        buttons[2].OnInteract += delegate () { buttonPress(2); return false; };
        buttons[3].OnInteract += delegate () { buttonPress(3); return false; };
        buttons[4].OnInteract += delegate () { buttonPress(4); return false; };
        buttons[5].OnInteract += delegate () { buttonPress(5); return false; };
        buttons[6].OnInteract += delegate () { buttonPress(6); return false; };
        buttons[7].OnInteract += delegate () { buttonPress(7); return false; };
        buttons[8].OnInteract += delegate () { buttonPress(8); return false; };
        buttons[9].OnInteract += delegate () { buttonPress(9); return false; };

        moduleSelectable.OnFocus += delegate () { isSelected = true; };
        moduleSelectable.OnDefocus += delegate () { isSelected = false; };
    }

    private void Update()
    {
        if(isSelected)
        {
            if (Input.GetKeyDown(KeyCode.Keypad0)) buttonPress(0);
            if (Input.GetKeyDown(KeyCode.Keypad1)) buttonPress(1);
            if (Input.GetKeyDown(KeyCode.Keypad2)) buttonPress(2);
            if (Input.GetKeyDown(KeyCode.Keypad3)) buttonPress(3);
            if (Input.GetKeyDown(KeyCode.Keypad4)) buttonPress(4);
            if (Input.GetKeyDown(KeyCode.Keypad5)) buttonPress(5);
            if (Input.GetKeyDown(KeyCode.Keypad6)) buttonPress(6);
            if (Input.GetKeyDown(KeyCode.Keypad7)) buttonPress(7);
            if (Input.GetKeyDown(KeyCode.Keypad8)) buttonPress(8);
            if (Input.GetKeyDown(KeyCode.Keypad9)) buttonPress(9);
        }
    }

    protected void OnNeedyActivation()
    {
        correctAnswer = "";

        from = Random.Range(1, 16);
        to = from + 4;

        for (int i = from - 1; i < to; i++)
        {
            correctAnswer += pi[i];
        }

        Debug.LogFormat("[Needy Pi #{0}] From range {1} to {2}, expecting {3}", moduleId, from, to, correctAnswer);

        digitLengthText.text = $"Digits: {from}-{to}";

        input = "";
        inputText.text = input;

        isActivated = true;
    }

    protected void OnNeedyDeactivation()
    {
        input = "";
        correctAnswer = "";
        isActivated = false;
    }

    protected void OnTimerExpired()
    {
        module.HandleStrike();
        Debug.LogFormat("[Needy Pi #{0}] Time ran out! Strike!", moduleId);
    }

    private void buttonPress(int buttonNumber)
    {
        if(isActivated)
        {
            if (buttonNumber.ToString() == correctAnswer[input.Length].ToString())
            {
                input += buttonNumber.ToString();
                inputText.text = input;
            }
            else
            {
                module.HandleStrike();
                Debug.LogFormat("[Needy Pi #{0}] Wrong! Inputted digit at position {1}: {2}, while expected {3}.", moduleId, input.Length + 1, buttonNumber.ToString(), correctAnswer[input.Length].ToString());
            }

            if (input.Length == correctAnswer.Length)
            {
                module.HandlePass();
                Debug.LogFormat("[Needy Pi #{0}] Successfully inputted the correct digits of pi!", moduleId);
                input = "";
                correctAnswer = "";
            }
        }
    }

#pragma warning disable 0414
    readonly private string TwitchHelpMessage = "Input digits using \"!{0} <digits>\". Ex. \"!{0} 14159\".";
#pragma warning restore 0414

    private IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant();

        if ((command.Length == 0) || (command.Length > 5))
        {
            yield return "sendtocharerror Please input a max 5 digit number.";
        } else
        {
            for (int i = 0; i < command.Length; i++)
            {
                if ((!(char.IsDigit(command[i]))) || (command[i] - '0' < 0))
                {
                    yield return $"sendtocharerror Position {i} was a \"{command[i]}\", which is not a digit... (previous digits, if any, were inputted)";
                }
                else
                {
                    yield return null;
                    yield return new WaitForSeconds(0.2f);
                    buttonPress((int)command[i] - '0');
                }
            }
        }
    }
}