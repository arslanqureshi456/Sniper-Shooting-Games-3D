using System;
using System.Collections;
using Firebase.Analytics;
using UnityEngine;

namespace TheKnights.FirebasePlugin
{

    public class FirebaseInitializer : MonoBehaviour
    {
        public static FirebaseInitializer Instance;

        private bool _isGameLoaderInitialized;
        
        private IEnumerator _delayedCoroutine;

        public bool _isFirebaseInitialized;


        public bool IsFirebaseInitialized
        {
            get => _isFirebaseInitialized;
            private set => _isGameLoaderInitialized = value;
        }

        private void Awake()
        {
           Instance = this;
        }
        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(3);
            _delayedCoroutine = DelayedLocalAction(() =>
            {
                InitializeGameLoader(true);
            }, 3f);

            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    var app = Firebase.FirebaseApp.DefaultInstance;
                    
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                    FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertySignUpMethod, "Google");
                    FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                    InitializeFirebase(true);
                    //Debug.LogError("Firebase Initialized Successfully . . .");
                }
                else
                {
                    InitializeFirebase(false);
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }

        public bool IsInitialized()
        {
            return _isGameLoaderInitialized;
        }

        private void InitializeGameLoader(bool flag)
        {
            if (flag && _delayedCoroutine != null) StopCoroutine(_delayedCoroutine);
            _isGameLoaderInitialized = flag;
        }

        private void InitializeFirebase(bool flag) => _isFirebaseInitialized = flag;

        private IEnumerator DelayedLocalAction(Action action, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            action?.Invoke();
            if (_delayedCoroutine == null) yield break;
            StopCoroutine(_delayedCoroutine);
        }
    }
}