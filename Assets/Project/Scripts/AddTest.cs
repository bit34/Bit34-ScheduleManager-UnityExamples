using UnityEngine;
using Com.Bit34Games.Unity.Update;


public class AddTest : TestBase
{
    //  CONSTANTS
    private const string START_LABEL = "Start : Calls every frame";
    private const string STOP_LABEL  = "Stop : Calls every frame";
    private const string START_INFO  = "Started > Frame : {0}";
    private const string UPDATE_INFO = "Updated > Frame : {0}";
    private const string STOP_INFO   = "Stopped > Frame : {0}";

    //  METHODS
    private void Awake()
    {
        InitializeTest(StartTestCallback, START_LABEL,
                       StopTestCallback,  STOP_LABEL,
                       LockTestCallback);
    }

    private void StartTestCallback()
    {
        Debug.Log(string.Format(START_INFO, Time.frameCount));

        UpdateManager.Add(UpdateCallback, this, null, Settings.TimeType);
    }

    private void StopTestCallback()
    {
        Debug.Log(string.Format(STOP_INFO, Time.frameCount));

        UpdateManager.Remove(UpdateCallback);
    }

    private void LockTestCallback(bool state){}

    private void UpdateCallback()
    {
        Debug.Log(string.Format(UPDATE_INFO, Time.frameCount));
    }

}
