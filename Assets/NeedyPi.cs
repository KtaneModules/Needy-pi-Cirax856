using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedyPi : MonoBehaviour
{
    public KMNeedyModule module;

    public TextMesh digitLengthText;
    public TextMesh inputText;

    public KMSelectable[] buttons; // 0-9

    private string pi = "14159265358979323846";

    private int from;
    private int to;
    private string correctAnswer = "";

    private string input = "";

    bool isActivated = false;

    static int moduleIdCounter = 1;
#pragma warning disable 0414
    int moduleId;
#pragma warning disable 0414

    private void Start()
    {
        GetComponent<KMNeedyModule>().OnNeedyActivation += OnNeedyActivation;
        GetComponent<KMNeedyModule>().OnNeedyDeactivation += OnNeedyDeactivation;

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
    }

    protected void OnNeedyActivation()
    {
        from = Random.Range(1, 16);
        to = from + 4;

        for (int i = from - 1; i < to; i++)
        {
            correctAnswer += pi[i];
        }

        Debug.LogFormat("[Needy Pi {0}] From range {1} to {2}, expecting {3}", moduleId, from, to, correctAnswer);

        digitLengthText.text = $"Digits: {from}-{to}";

        input = "";
        inputText.text = input;

        isActivated = true;
    }

    protected void OnNeedyDeactivation()
    {
        correctAnswer = "";
        isActivated = false;
    }

    protected void OnTimerExpired()
    {
        module.HandleStrike();
        Debug.LogFormat("[Needy Pi {0}] Time ran out! Strike!", moduleId);
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
                Debug.LogFormat("[Needy Pi {0}] Wrong! Inputted {1}, while expected {2}.", moduleId, input, correctAnswer);
            }

            if (input.Length == correctAnswer.Length)
            {
                module.HandlePass();
                Debug.LogFormat("[Needy Pi {0}] Successfully inputted the correct digits of pi!", moduleId);
            }
        }
    }
}