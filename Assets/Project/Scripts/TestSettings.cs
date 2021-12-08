using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Bit34Games.Unity.Update;


public class TestSettings : MonoBehaviour
{
    //  MEMBERS
    public UpdateTimeTypes TimeType{get; private set;}
#pragma warning disable 649
    //      For Editor
    [SerializeField] private Text   _settingLabel;
    [SerializeField] private Button _settingButton1;
    [SerializeField] private Button _settingButton2;
    [SerializeField] private Button _settingButton3;
#pragma warning restore 649
    //      Internal
    private List<Button> _buttons;

    //  METHODS
    public void TestStarted(TestBase test)
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].interactable = false;
        }

        TestBase[] tests = GetComponentsInChildren<TestBase>();
        for (int i = 0; i < tests.Length; i++)
        {
            if (tests[i] != test)
            {
                tests[i].LockTest(true);
            }
        }
    }

    public void TestEnded()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].interactable = true;
        }

        TestBase[] tests = GetComponentsInChildren<TestBase>();
        for (int i = 0; i < tests.Length; i++)
        {
            tests[i].LockTest(false);
        }
    }

    private void Awake()
    {
        _settingLabel.text = "Time : ";

        _buttons = new List<Button>();
        SetButton(_settingButton1, "Utc",               UseUtc);
        SetButton(_settingButton2, "Unity Unscaled",    UseUnityUnscaled);
        SetButton(_settingButton3, "Unity Scaled(0.5)", UseUnityScaled);

        UseUtc();
        _settingButton1.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
    }

    private void SetButton(Button button, string label, Action onClick)
    {
        button.GetComponentInChildren<Text>().text = label;
        button.onClick.AddListener( ()=>
        {
            onClick();
            for (int i = 0; i < _buttons.Count; i++)
            {
                if (_buttons[i] == button)
                {
                    _buttons[i].GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
                }
                else
                {
                    _buttons[i].GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
                }
            }
        });
        _buttons.Add(button);
    }

    private void UseUtc()
    {
        TimeType = UpdateTimeTypes.Utc;
        Time.timeScale = 1.0f;
    }

    private void UseUnityUnscaled()
    {
        TimeType = UpdateTimeTypes.UnityUnscaled;
        Time.timeScale = 1.0f;
    }

    private void UseUnityScaled()
    {
        TimeType = UpdateTimeTypes.UnityScaled;
        Time.timeScale = 0.5f;
    }
}