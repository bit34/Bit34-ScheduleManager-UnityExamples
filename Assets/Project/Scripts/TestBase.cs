using System;
using UnityEngine;
using UnityEngine.UI;
using Com.Bit34Games.Time.Unity;
using Com.Bit34Games.Time.Constants;
using Com.Bit34Games.Time.Utilities;


public class TestBase : MonoBehaviour
{
    //  MEMBERS
    public bool             TestActive       { get; private set; }
    public int              TestFrameCounter { get; private set; }
    public TestSettings     Settings         { get; private set; }
    public TimeManager      Manager          { get{ return _timeForUnity.Manager; } }
#pragma warning disable 649
    //      For Editor
    [SerializeField] protected TimeForUnity _timeForUnity;
    [SerializeField] private   Button      _testButton;
    [SerializeField] private   Text        _testButtonLabel;
#pragma warning restore 649
    //      Internal
    private   string       _startLabel;
    private   string       _stopLabel;
    private   Action       _startAction;
    private   Action       _stopAction;
    private   Action<bool> _lockAction;


    //  METHODS
    public void LockTest(bool state)
    {
        _testButton.interactable = !state;
        _lockAction(state);
    }

    protected void InitializeTest(Action startAction, string startLabel, Action stopAction, string stopLabel, Action<bool> lockAction)
    {
        Settings = GetComponentInParent<TestSettings>();

        _startLabel  = startLabel;
        _stopLabel   = stopLabel;

        _startAction = startAction;
        _stopAction  = stopAction;
        _lockAction  = lockAction;

        _testButtonLabel.text = _startLabel;
        _testButton.onClick.AddListener(OnStartClick);
    }

    private void OnStartClick()
    {
        _testButton.onClick.RemoveListener(OnStartClick);
        _testButton.onClick.AddListener(OnStopClick);
        _testButtonLabel.text = _stopLabel;

        Settings.TestStarted(this);

        TestActive = true;
        _startAction();
    }

    private void OnStopClick()
    {
        _testButton.onClick.RemoveListener(OnStopClick);
        _testButton.onClick.AddListener(OnStartClick);
        _testButtonLabel.text = _startLabel;

        Settings.TestEnded();

        TestActive = false;
        TestFrameCounter = 0;
        _stopAction();
    }
    
    private void Update()
    {
        if (TestActive)
        {
            TestFrameCounter++;
        }
    }

    protected string GetTimeString()
    {
        TimeSpan elapsedTime = TimeSpan.FromSeconds(Time.unscaledTime);
        string   elapsed     = string.Format("[Elapsed:{0:00}:{1:00}:{2:00}:{3:D2}]", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds, elapsedTime.Milliseconds);
        
        TimeTypes timeType = Settings.TimeType;
        DateTime  timeNow  = Manager.Time.GetNow(timeType);
        string    time     = String.Format("[" + timeType + ":{0:00}:{1:00}:{2:00}:{3:D2}", timeNow.Hour, timeNow.Minute, timeNow.Second, timeNow.Millisecond);

        return elapsed + time;
    }

    protected void CancelTest()
    {
        if (TestActive)
        {
            OnStopClick();
        }
    }

}
