
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    bool isEnabled = false;
    public AudioSource backgroundMusicSouce;
    public AudioSource otherAudioSource;
    public AudioSource thunderAudioSource;
    public AudioSource enemykillAudioSource;
    public AudioSource backAudioSource;
    public AudioClip
        backButtonClip,
        storeButtonClip,
        playButtonClip,
        loadoutStartButtonClip,
        //letsGoClip,
        whatsYourName,
        thankYou,
        welcomeBack,
        youreWelcome,
        normalClick,
        normalClick2,
        newBack,
        nameTyping,
       // outOfAmmo,
        fire,
       // attack,
        awsome,
      //  greatJob,
       // reload,
        nadeTakeCover,
        gameOver,
       // fireTakeCover,
        nadeSound,
      //  operationSuccessful,
        mainMenu,
        jumpSound,
        deathSoundSniper,
        lastenemybulletsound,
        mainmenuBackground,
        selectionBackground,
        assaultBackground,
        sniperBackground,
        coverStrikeBackground,
        tdmBackground,
        ffaBackground,
        brBackground,
        levelclick,
        modechange,
        levelselect,
        loadoutselect,
        nextlevel,
        objectiveok;

    public AudioClip[] outOfAmmo, attack, greatJob, reload, fireTakeCover, operationSuccessful;

    public AudioClip[] EnemyDeathSound;

    public void Awake()
    {
        instance = this;
    }

    public void ChangeBackgroundVolume(float value)
    {
        backgroundMusicSouce.volume = value;
    }

    public void ChangeSoundFxVolume(float value)
    {
        otherAudioSource.volume = value;
        backAudioSource.volume = value;
    }
    void Update()
    {
        if(isEnabled)
        {
#if UNITY_EDITOR
            print("changing volume");
#endif
            thunderAudioSource.volume = Mathf.Lerp(thunderAudioSource.volume, 0, 0.04f);
        }
    }
    public void BackButtonClick()
    {
        if (otherAudioSource.isActiveAndEnabled)
            otherAudioSource.PlayOneShot(backButtonClip);
    }

    public void StoreButtonClick()
    {
        if (otherAudioSource.isActiveAndEnabled)
            otherAudioSource.PlayOneShot(storeButtonClip);
    }

    public void PlayButtonClick()
    {
        if (otherAudioSource.isActiveAndEnabled)
            otherAudioSource.PlayOneShot(playButtonClip);
    }

    public void PlayLoadoutStartButtonClick()
    {
        if (otherAudioSource.isActiveAndEnabled)
            otherAudioSource.PlayOneShot(loadoutStartButtonClip);
    }

    //public void LetsGoButtonClick()
    //{
    //    otherAudioSource.PlayOneShot(letsGoClip);
    //}


    public void LoginButtonClick()
    {
        if (otherAudioSource.isActiveAndEnabled)
            otherAudioSource.PlayOneShot(whatsYourName);
    }

    public void ThankYouClick()
    {
        if (otherAudioSource.isActiveAndEnabled)
            otherAudioSource.PlayOneShot(thankYou);
    }

    public void WelcomeBackClick()
    {
        if (otherAudioSource.isActiveAndEnabled)
            otherAudioSource.PlayOneShot(welcomeBack);
    }

    public void YoureWelcomeClick()
    {
        if(otherAudioSource.isActiveAndEnabled)
        otherAudioSource.PlayOneShot(youreWelcome);
    }
    public void TypeName()
    {
        if (otherAudioSource.isActiveAndEnabled)
            otherAudioSource.PlayOneShot(nameTyping);
    }
    public void NormalClick()
    {
        if (otherAudioSource.isActiveAndEnabled)
            otherAudioSource.PlayOneShot(normalClick);
    }

    public void NormalClick2()
    {
        if (otherAudioSource.isActiveAndEnabled)
            otherAudioSource.PlayOneShot(normalClick2);
    }
    public void BackClickNew()
    {
        if (otherAudioSource.isActiveAndEnabled)
            backAudioSource.PlayOneShot(newBack);
    }
    public void PlayThunder()
    {
        isEnabled = false;
        thunderAudioSource.volume = 1;
        thunderAudioSource.pitch = Random.Range(0.8f, 1.2f);
        if(!thunderAudioSource.isPlaying)
            thunderAudioSource.Play();
    }
    public void StopThunder()
    {
        isEnabled = true;
    }
    public void PlayOutOfAmmo()
    {
        backAudioSource.volume = 1.0f;
        backAudioSource.PlayOneShot(outOfAmmo[Random.Range(0 , outOfAmmo.Length)]);
    }
    public void PlayAttack()
    {
        backAudioSource.volume = 1.0f;
        backAudioSource.PlayOneShot(attack[Random.Range(0,attack.Length)]);
    }
    public void PlayFire()
    {
        backAudioSource.volume = 1.0f;
        backAudioSource.PlayOneShot(fire);
    }
    public void PlayAwsome()
    {
        backAudioSource.volume = 1.0f;
        backAudioSource.PlayOneShot(awsome);
    }
    public void PlayGreatJob()
    {
        backAudioSource.volume = 1.0f;
        backAudioSource.PlayOneShot(greatJob[Random.Range(0, greatJob.Length)]);
    }
    public void PlayReload()
    {
        backAudioSource.volume = 1.0f;
        backAudioSource.PlayOneShot(reload[Random.Range(0, reload.Length)]);
    }
    public void PlayNadeTakeCover()
    {
        backAudioSource.volume = 1.0f;
        backAudioSource.PlayOneShot(nadeTakeCover);
    }
    public void PlayGameOver()
    {
        backAudioSource.volume = 1.0f;
        backAudioSource.PlayOneShot(gameOver);
    }
    public void PlayFireTakeCover()
    {
        backAudioSource.volume = 1.0f;
        backAudioSource.PlayOneShot(fireTakeCover[Random.Range(0, fireTakeCover.Length)]);
    }
    public void PlayNadeSoundForScene()
    {
        backAudioSource.volume = 1.0f;
        backAudioSource.PlayOneShot(nadeSound);
    }
    public void PlayOperationSuccessful()
    {
        backAudioSource.volume = 1.0f;
        backAudioSource.PlayOneShot(operationSuccessful[Random.Range(0, operationSuccessful.Length)]);
    }
    public void PlayJumpSound()
    {
        backAudioSource.volume = 1.0f;
        backAudioSource.PlayOneShot(jumpSound);
    }
    public void PlayMainMenu()
    {
        backgroundMusicSouce.clip = mainMenu;
       backgroundMusicSouce.Play();
    }
    public void PlayLastEnemyDeathSound()
    {
        backAudioSource.volume = 0.5f;
        backAudioSource.PlayOneShot(EnemyDeathSound[Random.Range(0 , EnemyDeathSound.Length)]);
    }
    public void PlayDeathSoundSniper()
    {
        backAudioSource.volume = 0.5f;
        backAudioSource.PlayOneShot(deathSoundSniper);
    }
    public void DisableSoundsForAds()
    {
        backgroundMusicSouce.gameObject.SetActive(false);
        otherAudioSource.gameObject.SetActive(false);
    }
    public void EnableSoundsAfterAds()
    {
        otherAudioSource.volume = SaveManager.Instance.state.soundFxVolume;
        backgroundMusicSouce.gameObject.SetActive(true);
        otherAudioSource.gameObject.SetActive(true);
    }
    public void PlayMainMenuThemeSound()
    {
        if (backgroundMusicSouce.clip == mainmenuBackground)
            return;
        backgroundMusicSouce.clip = mainmenuBackground;
        backgroundMusicSouce.Play();
        backgroundMusicSouce.loop = true;
        //backgroundMusicSouce.volume = 0.3f;
    }
    public void PlaySelectionThemeSound()
    {
        if (backgroundMusicSouce.clip == selectionBackground)
            return;
        backgroundMusicSouce.clip = selectionBackground;
        backgroundMusicSouce.Play();
    }

    public void PlayAssaultBGM()
    {
        if (backgroundMusicSouce.clip == assaultBackground)
            return;
        backgroundMusicSouce.clip = assaultBackground;
        backgroundMusicSouce.Play();
        backgroundMusicSouce.volume = 0.3f;
    }

    public void PlaySniperBGM()
    {
        if (backgroundMusicSouce.clip == sniperBackground)
            return;
        backgroundMusicSouce.clip = sniperBackground;
        backgroundMusicSouce.Play();
    }

    public void PlayCoverStrikeBGM()
    {
        if (backgroundMusicSouce.clip == coverStrikeBackground)
            return;
        backgroundMusicSouce.clip = coverStrikeBackground;
        backgroundMusicSouce.Play();
    }

    public void PlayTDMBGM()
    {
        if (backgroundMusicSouce.clip == tdmBackground)
            return;
        backgroundMusicSouce.clip = tdmBackground;
        backgroundMusicSouce.Play();
    }

    public void PlayFFABGM()
    {
        if (backgroundMusicSouce.clip == ffaBackground)
            return;
        backgroundMusicSouce.clip = ffaBackground;
        backgroundMusicSouce.Play();
    }

    public void PlayBRBGM()
    {
        if (backgroundMusicSouce.clip == brBackground)
            return;
        backgroundMusicSouce.clip = brBackground;
        backgroundMusicSouce.Play();
    }

    public void StopBRBGM()
    {
        backgroundMusicSouce.Stop();
    }

    public void LastKillBulletSound()
    {
        enemykillAudioSource.PlayOneShot(lastenemybulletsound);
        otherAudioSource.volume = 0.15f;
    }

    public void LevelClickSound()
    {
        if (otherAudioSource.isActiveAndEnabled)
            otherAudioSource.PlayOneShot(levelclick);
    } 
    public void ModeChangeSound()
    {
        if (otherAudioSource.isActiveAndEnabled)
            otherAudioSource.PlayOneShot(modechange);
    }

    public void LevelSelectSound()
    {
        if (otherAudioSource.isActiveAndEnabled)
            otherAudioSource.PlayOneShot(levelselect);
    }

    public void LoadOutSelectSound()
    {
        if (otherAudioSource.isActiveAndEnabled)
            otherAudioSource.PlayOneShot(loadoutselect);
    }

    public void NextLevelSelectSound()
    {
        if (otherAudioSource.isActiveAndEnabled)
            otherAudioSource.PlayOneShot(nextlevel);
    }

    public void ObjectiveOKSound()
    {
        otherAudioSource.PlayOneShot(objectiveok);
#if UNITY_EDITOR
        print("otherAudioSource : " + otherAudioSource.volume);
#endif
    }
}