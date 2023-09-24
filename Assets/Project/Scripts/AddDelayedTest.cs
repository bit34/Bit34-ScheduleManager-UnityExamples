using System;
using UnityEngine;
using UnityEngine.UI;


public class AddDelayedTest : TestBase
{
    //  CONSTANTS
    private const int DELAY_SECONDS = 3;
    private const string START_LABEL = "Start : Calls every {0}. second";
    private const string STOP_LABEL  = "Stop  : Calls every {0}. second";
    private const string START_INFO  = "Started > Time : {0}";
    private const string UPDATE_INFO = "Updated > Time : {0}";
    private const string STOP_INFO   = "Stopped > Time : {0}";

    private const string PAUSE_LABEL  = "Pause";
    private const string RESUME_LABEL = "Resume";
    private const string PAUSE_INFO   = "Paused  > Time : {0}";
    private const string RESUME_INFO  = "Resumed > Time : {0}";


    //  MEMBERS
#pragma warning disable 649
    //      For Editor
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Text   _pauseButtonLabel;
#pragma warning restore 649
    private TimeSpan _delay;


    //  METHODS
    private void Awake()
    {
        InitializeTest(StartTestCallback, string.Format(START_LABEL, DELAY_SECONDS),
                       StopTestCallback,  string.Format(STOP_LABEL, DELAY_SECONDS),
                       LockTestCallback);

        _delay = TimeSpan.FromSeconds(DELAY_SECONDS);

        DisablePauseButton();
    }

    private void StartTestCallback()
    {
        Debug.Log(string.Format(START_INFO, GetTimeString()));

        ScheduleManager.AddInterval(this, Settings.TimeType, UpdateCallback, _delay);

        EnablePauseButton();
    }

    private void StopTestCallback()
    {
        Debug.Log(string.Format(STOP_INFO, GetTimeString()));

        ScheduleManager.Remove(this, UpdateCallback);

        DisablePauseButton();
    }

    private void LockTestCallback(bool state){}
    
    private void UpdateCallback(float timeDelta)
    {
        Debug.Log(string.Format(UPDATE_INFO, GetTimeString()));
    }

    private void EnablePauseButton()
    {
        _pauseButton.interactable = true;
        _pauseButton.onClick.AddListener(OnPauseButtonClick);
        _pauseButtonLabel.text = PAUSE_LABEL;
    }

    private void DisablePauseButton()
    {
        _pauseButton.interactable = false;
        _pauseButton.onClick.RemoveAllListeners();
        _pauseButtonLabel.text = PAUSE_LABEL;
    }

    private void OnPauseButtonClick()
    {
        Debug.Log(string.Format(PAUSE_INFO, GetTimeString()));

        ScheduleManager.PauseAllFrom(this);

        _pauseButton.onClick.RemoveAllListeners();
        _pauseButton.onClick.AddListener(OnResumeButtonClick);
        
        _pauseButtonLabel.text = RESUME_LABEL;
    }

    private void OnResumeButtonClick()
    {
        Debug.Log(string.Format(RESUME_INFO, GetTimeString()));

        ScheduleManager.ResumeAllFrom(this);

        _pauseButton.onClick.RemoveAllListeners();
        _pauseButton.onClick.AddListener(OnPauseButtonClick);

        _pauseButtonLabel.text = PAUSE_LABEL;
    }

}
