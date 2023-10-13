using System;

namespace Uniject
{
    /// <summary>
    /// An interface allowing Engine APIs to be stubbed out
    /// because unit tests do not have access to Engine APIs.
    /// </summary>
    internal interface IUtil
    {
        void RunOnMainThread(Action runnable);
        // Have the specified action called when the app is paused or resumed.
        // The parameter is true if paused, false if resuming.
        // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnApplicationPause.html
        void AddPauseListener(Action<bool> runnable);
    }
}
