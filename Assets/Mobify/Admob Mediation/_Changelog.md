Admob Mediation V_2.5.1 (12 Oct 23)
========================
Addition:
* AdInspector support added instead of Mediation Test Suite for verification of Admob ID's and Adapters working.
* isTestingID support added in every Ad Type calling.

Remove: 
* Remove support of Mediation Test Suite.

----------------------------------:|:---------------------------------

Admob Mediation V_2.5.0 (20 Sep 23)
========================
Addition:
* Add Support "Native Ads"
* Add Supported Script "NativeAdFetcher.cs" for fetching data (image,text) of native Ad for showing custom UI panel areas.
* Add Sample UI panel "nativeAd".
* Add "ANR Supervisor.dll" & "native-release.arr" file in Plugins/Android with file support.

Bug Resolve: 
* Automatic LoadingAdBG hide after certain time period if full screen ad not shown due to some reason.

----------------------------------:|:---------------------------------

Admob Mediation V_2.4.4 (18 July 23)
========================
Bug Resolve: 
* Minor issues resolve regarding Interstitial

----------------------------------:|:---------------------------------

Admob Mediation V_2.4.3 (13 July 23)
========================
Bug Resolve:
* Resolve AppOpen show on other full screen Ads
* Exception handles on Rewarded Ad FailedToShow handler

Change: 
* Change plugin path Assets/Mobify/Admob Mediation 
* Use Only 3 Interstitial (Interstitial, Interstitial_Static, Interstitial_without_FB).
* Every Interstitial calling (LoadInterstitial, IsInterstitialLoaded, ShowInterstitial) must required InterstitialType into parameter.

Addition:
* ANRSupervisor support added. But did not work until ANRSupervisor.java class to import in project.

----------------------------------:|:---------------------------------
Admob Mediation V_2.4.2 (26 May 23)
========================
Addition:
* Interstitial for Facebook added. This is Individual and Separate from other Interstitials. You can use this interstitial by using
  - LoadInterstitialFacebook()
  - IsInterstitialFacebookLoaded()
  - ShowInterstitialFacebook(float afterAdTimeScale = 1)
* Analytics enable/disable Boolean added in Memory Settings for low memory event send to Firebase.

Change:
* Script Define Symbols updated for Firebase Analytics.
* AudioListener last state managed.

----------------------------------:|:---------------------------------

Admob Mediation V_2.4.1 (24 May 23)
========================
Addition:
* Editor Properties Division (for better look management)

Change:
* If Memoty Threshold not checked, then no memory threshold applied.
* Update IsBannerLoaded(BannerType banner, bool isShowAfterLoad false)  
  and LoadBanner(BannerType banner, bool isShowAfterLoad = false) functions.

----------------------------------:|:---------------------------------

Admob Mediation V_2.3 (04 Apr 23)
======================
Addition:
* IsBannerLoaded(BannerType banner) function added.
* LoadBanner(BannerType banner) function added. 
* Script Define Symbols added as a script name
* "Small Banner GamePlay" added as a 2nd small banner.
* Banner Destroy & Reload Functionality added on specific time (Optional)
* Script Define Symbols added as a script name

Change:
* Modify Action<BannerType, AdPosition> afterLoadingBannerShowAction
* Modify AfterLoadingBGBannerToShow() & ShowLoadingAdBG() function
* Minor dealy of calling GetReward() function.

Bug Resolve:
* More than 1 banner shown at a same screen when
   - Combination of "Banner" click & "App Open" Ad on ApplicationPause.
   - Rewarded loaded but failed to load.

----------------------------------:|:---------------------------------

Admob Mediation V_2.2 (20 Mar 23)
======================
Addition:
* Banner availability can check by IsBannerLoaded(BannerType)

Change:
* Memory Threshold individually added for Small Banner & Large Banner in Editor Property.
 
----------------------------------:|:---------------------------------

Admob Mediation V_2.1 (08 Mar 23)
======================
Addition:
* Send "MemoryInfo" event to Firebase on every memory check
* AfterLoadingBGBannerToShow() added for banner manage after Ads.
* Support for IOS and OSXEditor

Change:
* Initilization call only once.
* ActionPerformIenumerator change waitForSeconds to WaitForSecondsRealtime

----------------------------------:|:---------------------------------

Admob Mediation V_2.0 (07 Mar 23)
======================
Addition:
* Support 4 Type of Banners (Small , Smart , Rectangular , Adaptive)
* Support 3 Type of Interstitial (Resession , interstitial , static)
* Support 2 Type of Rewarded (Rewarded Video , Rewarded Interstitial)
* Support 1 Type of App Open Ad
* Support "Debug.Log(msg)" for Request/Response every Ads related call.
* Support "Low Memory" handling by individual Ad types & modifiable.
* Editor Property for properly understnading of AD ID's & Memory details.
* Support for IOS

Networks:
* AdColony
* IronSource
* Unity
* Vungle

----------------------------------:|:---------------------------------
