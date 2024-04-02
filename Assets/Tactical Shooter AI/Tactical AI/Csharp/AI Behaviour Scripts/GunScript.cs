using UnityEngine;
using System.Collections;

/*
 * Manages the firing of the gun, and has minor influence on aiming.
 * Also deals with secondary fire
 * */

namespace TacticalAI{
public class GunScript : MonoBehaviour {

        public AiGunsSounds sounds;
	//Stuff
	public TacticalAI.BaseScript myAIBaseScript;
	public TacticalAI.AnimationScript animationScript;
    public TacticalAI.SoundScript soundScript;
    public AudioSource audioSource;

	int[] enemyTeams;	

	//Bullet stuff	
	public GameObject bulletObject;
	public AudioClip bulletSound;
	[Range (0.0f, 1.0f)]
	public float bulletSoundVolume = 1;	
	public Transform  bulletSpawn;
    public GameObject muzzleFlash;
	public Transform muzzleFlashSpawn;
	public float flashDestroyTime = 0.3f;
	bool canCurrentlyFire = true;
	
	//Used for shotguns; 1 is default and should be used for non-shotgiun weapons
	public int pelletsPerShot = 3;	
	
	//Just to determine whether or not or projectiles should home in on our enemies
	public bool isRocketLauncher = false;
        public bool isSniper = false;
	//Secondary Fire
	public GameObject secondaryFireObject;
	//1 = highest probability
	[Range (0.0f, 1.0f)]
	public float oddsToSecondaryFire = 0.1f;
	public float minDistForSecondaryFire = 10;
	public float maxDistForSecondaryFire  = 50;
	bool canFireGrenadeAgain = false;	
	//Vector3 lastPosTargetSeen = Vector3.zero;
	public bool needsLOSForSecondaryFire = false;
	bool canThrowGrenade = false;
	public  float minTimeBetweenSecondaryFire = 4;	

	
	//RoF, burst and timer Stuff		
	public float minPauseTime = 0.5f;
	public float randomPauseTimeAdd  = 0.5f;	
	public int minRoundsPerVolley = 1;
	public int maxRoundsPerVolley = 2;
	public int minBurstsPerVolley = 1;
	public int maxBurstsPerVolley = 3;	
	public int currentRoundsPerVolley = 2;	
	public float rateOfFire = 2;
	float timeBetweenBursts = 0.5f;	
	public float burstFireRate = 2;
	public int shotsPerBurst = 1;
	float timeBetweenBurstBullets = 0.5f;
	
	//Reloading
	public int bulletsUntilReload = 60;
	public AudioClip reloadSound;	
	[Range (0.0f, 1.0f)]
	public float reloadSoundVolume = 1;
	bool isReloading = false;
	int currentBulletsUntilReload = 0;
	public float reloadTime = 2;	

	//Accuracy
	public float inaccuracy = 1;	
	[Range (0.0f, 90.0f)]
	public float maxFiringAngle = 10;	
	[Range (0.0f, 90.0f)]
	public float maxSecondaryFireAngle = 40;
	Quaternion fireRotation;	
	
	//Transforms
    
	public Transform targetTransform;
	Transform LOSTargetTransform;
	
	//LoS stuff
	private LayerMask LOSLayermask;
	public float timeBetweenLOSChecks = 2;
	//bool canSeePlayer = true;
	//bool canDoLOSCheck = false;
	
	//Private status stuff
	bool aware = false;
	bool isFiring = false;
	bool isWaiting = false;
	
	//Cover	
	public float distInFrontOfTargetAllowedForCover = 3;
	public float coverTransitionTime = 0.45f;
	float rayDist;

	//Sound
	public float soundRadius = 7;

    public float minimumDistToFireGun = 0;
    public float maximumDistToFireGun = 9999;
    public int currentWeapon;
    public Transform WeaponContainer;

        public MeshRenderer muzzleFlashObject;

        void Awake (){
            if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.MULTIPLAYER)
                ChangeGun();
            //ABDUL
            if (PlayerPrefs.GetInt("levelUnlocked-3").Equals(1))
                canThrowGrenade = true;
            else
                canThrowGrenade = false;

		//Set default values, calculate squared distances, etc.
		LOSLayermask = TacticalAI.ControllerScript.currentController.GetLayerMask();
		
        if (!audioSource && bulletSpawn)
            if (bulletSpawn.gameObject.GetComponent<AudioSource>())
                audioSource = bulletSpawn.gameObject.GetComponent<AudioSource>();

        if(gameObject.GetComponent<SoundScript>())
            {
                soundScript = gameObject.GetComponent<SoundScript>();
            }

            if (!grenadeSpawn)
                grenadeSpawn = bulletSpawn;
        isFiring = false;
		isWaiting= false;
		currentBulletsUntilReload = bulletsUntilReload;	
		timeBetweenBurstBullets = 1/burstFireRate;
		timeBetweenBursts = 1/rateOfFire;	
		minBurstsPerVolley = (int)(minRoundsPerVolley/shotsPerBurst);
		maxBurstsPerVolley = (int)(maxRoundsPerVolley/shotsPerBurst);	
		maxFiringAngle /= 2;
		maxSecondaryFireAngle /= 2;
        minimumDistToFireGun = minimumDistToFireGun * minimumDistToFireGun;
        maximumDistToFireGun = maximumDistToFireGun * maximumDistToFireGun;
    }

        // Stuff we need done after all other stuff is set up
        void Start()
        {
            bulletSound = sounds.sounds[currentWeapon];
            if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
            {
                //oddsToSecondaryFire = 0;
                WeaponContainer.GetChild(currentWeapon).gameObject.SetActive(true);
            }
            if(LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
            {
                Invoke("SetAssaultGun", 0.75f);
            }
            // put guns in enemies hand after removing guns from enemy prefabs
            if(LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.FREEFORALL || LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.BR)
            {
                WeaponContainer.GetChild(currentWeapon).gameObject.SetActive(true);
            }
            enemyTeams = myAIBaseScript.GetEnemyTeamIDs();
        }
        public Transform GetCurrentGun()
        {
             return WeaponContainer.GetChild(currentWeapon);
            
        }
        public void HideWeapon()
        {
#if UNITY_EDITOR
            print("here in HideWeapon");
#endif
            //WeaponContainer.GetChild(currentWeapon).gameObject.SetActive(false);
            WeaponContainer.gameObject.SetActive(false);
        }
        public void ChangeGun()
        {
            WeaponContainer.GetChild(currentWeapon).gameObject.SetActive(false);
            currentWeapon = Random.Range(0, 25);
            WeaponContainer.GetChild(currentWeapon).gameObject.SetActive(true);
            if (currentWeapon >= 19)
            {
                //pelletsPerShot = 1;
                //shotsPerBurst = 1;
                //burstFireRate = 4;
                //rateOfFire = 1.25f;
                //maxRoundsPerVolley = 3;
                //minRoundsPerVolley = 1;
                //minPauseTime = 1;
                //randomPauseTimeAdd = 2;
                //bulletsUntilReload = 7;

            }
            else
            {
                //pelletsPerShot = 1;
                //shotsPerBurst = 1;
                //burstFireRate = 10;
                //rateOfFire = 12f;
                //maxRoundsPerVolley = 40;
                //minRoundsPerVolley = 8;
                //minPauseTime = 1;
                //randomPauseTimeAdd = 2;
                //bulletsUntilReload = 30;

                
            }
            bulletSound = sounds.sounds[currentWeapon];
        }
        void SetAssaultGun()
        {
            WeaponContainer.GetChild(currentWeapon).gameObject.SetActive(false);
            int rand = Random.Range(0, GameManager.Instance.activeObjectives.GunsIndexes.Length);
           // Debug.Log("Number : " + rand);
            currentWeapon = GameManager.Instance.activeObjectives.GunsIndexes[rand];
           
           WeaponContainer.GetChild(currentWeapon).gameObject.SetActive(true);

            //Getting Current Weapon (As Child Number) Active In Heirarchy To Spawn Muzzle Flash
            GameObject obj = WeaponContainer.GetChild(currentWeapon).gameObject;
#if UNITY_EDITOR
            print("Assault Enemy Has Gun : " + obj.name);
#endif

#if UNITY_EDITOR
            print("Enemy has gun number : " + currentWeapon);
#endif
            if(currentWeapon >= 19)
            {
                //pelletsPerShot = 1;
                //shotsPerBurst = 1;
                //burstFireRate = 4;
                //rateOfFire = 1.25f;
                //maxRoundsPerVolley = 3;
                //minRoundsPerVolley = 1;
                //minPauseTime = 1;
                //randomPauseTimeAdd = 2;
                //bulletsUntilReload = 7;

            }else
            {
                //pelletsPerShot = 1;
                //shotsPerBurst = 1;
                //burstFireRate = 10;
                //rateOfFire = 12f;
                //maxRoundsPerVolley = 40;
                //minRoundsPerVolley = 8;
                //minPauseTime = 1;
                //randomPauseTimeAdd = 2;
                //bulletsUntilReload = 30;


            }
            bulletSound = sounds.sounds[currentWeapon];
            muzzleFlashSpawn = WeaponContainer.GetChild(currentWeapon).transform.GetChild(1).transform;
        }
     float timer = 30;
	// Update is called once per frame
	void LateUpdate ()
        {
            if (GameManager.Instance && GameManager.Instance.isLevelCompleted)
                enabled = false;
				if(aware)
					{
                //If we're not doing anythingm start the bullet firing cycle
						if(!isFiring && !isWaiting && bulletObject)
							{						
								StartCoroutine(BulletFiringCycle());
							}	
						else if(!bulletObject)	
							{
#if UNITY_EDITOR
                    Debug.LogWarning("Can't fire because there is no bullet object selected!");
#endif
                }
					}
            timer--;

    }

    public bool checkForFriendlyFire;
    public string friendlyTag;
	
	//Shooting////////////////////////////////////////////////////////	
	IEnumerator BulletFiringCycle()
	{
          
            //Fire
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Game" && !isFiring)
            {
                GameManager.totalFiringAgents++;
                if (GameManager.totalFiringAgents >= 4 && GameManager.Instance.allowCoverSound)
                {
                    if (GameManager.isFirstGang || (Random.Range(0f, 1f) > 0.75))
                    {
                        GameManager.isFirstGang = false;
                        GameManager.Instance.PlayCoverSound();
                    }
                }
            }
		isFiring = true;
        //Wait for the animation transitioning the agent from hiding to a firing positiont o finish.
		if(myAIBaseScript.inCover)
			yield return new WaitForSeconds(coverTransitionTime*1.5f);

        //Don't fire if the agent is unaware of the target or meleeing the target.
        if (myAIBaseScript.IsEnaging() && !myAIBaseScript.isMeleeing && !myAIBaseScript.inParkour)
			{
                //If we have clear LoS to the LOSTargetTransform, fire
                //You may want to check for line of sight to a position right above the target's head (for example)
                //This will allow your agent to lay down suppressing fire even if they can't see the target.
				if(LOSTargetTransform && !animationScript.isSprinting() && Random.value < oddsToSecondaryFire)
					{
                        //See if we can use our secondary fire
                        //While a grenade may not need LoS, a homing missile might
						if(!Physics.Linecast(bulletSpawn.position, LOSTargetTransform.position, LOSLayermask) || !locatedNewGrenadeTargetYet)	
							{
								//lastPosTargetSeen = targetTransform.position;
                                locatedNewGrenadeTargetYet = true;
                                canFireGrenadeAgain = true;							
								FireOneGrenade();
								canFireGrenadeAgain = true;										
							}
						else if(!needsLOSForSecondaryFire)
							{
                                //lastPosTargetSeen = targetTransform.position;
                                if (canFireGrenadeAgain)
									FireOneGrenade();
                                canFireGrenadeAgain = true;	
							}
					}
                //Create the sound that will be heard by Paragon AI agents
                //This sound is not going to be heard by the player
                //Waqas AI
				if(!ControllerScript.isSimpleAI && soundRadius > 0)
					{
						TacticalAI.ControllerScript.currentController.CreateSound(bulletSpawn.position, soundRadius, enemyTeams);
					}
                //Shoot regular bullets	
                if (animationScript.currentlyRotating)
                {
                    try
                    {
                       /* yield return*/ StartCoroutine(Fire());
                    }
                    catch { }
#if UNITY_EDITOR
                    print("GunScript : Regular Bullets");
#endif
                }
			}
		
		//Transition
		isWaiting = true;
            try
            {
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Game" && isFiring)
                    GameManager.totalFiringAgents--;
            }
            catch { }
            isFiring = false;
	
					
        //If we aren't reloading wait for a while before firing another burst
		if(currentBulletsUntilReload > 0 && reloadTime > 0)
			{
				yield return new WaitForSeconds(minPauseTime + Random.value * randomPauseTimeAdd);
			}
		else
			{
                isReloading = true;
                //If we're out of ammo, reload.
				if(reloadSound)
					{
                        audioSource.volume = reloadSoundVolume;
                        audioSource.PlayOneShot(reloadSound);
					}
				if(animationScript)	
					{
						animationScript.PlayReloadAnimation();
					}
                if(soundScript)
                    {
                        soundScript.PlayReloadAudio();
                    }
				yield return new WaitForSeconds(reloadTime);
				currentBulletsUntilReload = bulletsUntilReload;
                isReloading = false;
                yield return new WaitForSeconds(minPauseTime * Random.value);
			}
		isWaiting= false;
	}

        IEnumerator Fire()
        {
            if (LOSTargetTransform == null)
                yield break;
            //Check Distances
            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Game")
            {
                if (GameManager.isFirstFire)
                {
                    if (Random.Range(0, 2) > 0)
                    {
                        AudioManager.instance.PlayFire();
                    }else
                    {
                        AudioManager.instance.PlayAttack();
                    }
                    GameManager.isFirstFire = false;
                }
            }
            
            float distSqr = Vector3.SqrMagnitude(bulletSpawn.position - LOSTargetTransform.position);
            if (minimumDistToFireGun <= distSqr && maximumDistToFireGun >= distSqr)
                //Make sure we don't fire more bullets than the number remaining in the agent's magazine.
                currentRoundsPerVolley = Mathf.Min(Random.Range(minBurstsPerVolley, maxBurstsPerVolley), currentBulletsUntilReload);

            //Make sure we have bullets left and stop firing if the agent is dead.
            while (currentRoundsPerVolley > 0 && this.enabled && !animationScript.isSprinting() && !myAIBaseScript.inParkour)
            {
                if (LOSTargetTransform && canCurrentlyFire)
                {
                    //Make sure ray is always facing "forward".
                    //Make sure we have clear LoS to the target
                    //Ray can be stopped short so that the agent will still fire at the target even if they are behind a thin layer of cover
                    rayDist = Mathf.Max(0.00001f, Vector3.Distance(bulletSpawn.position, LOSTargetTransform.position) - distInFrontOfTargetAllowedForCover);
                    if (rayDist == 0 || !Physics.Raycast(bulletSpawn.position, LOSTargetTransform.position - bulletSpawn.position, rayDist, LOSLayermask))
                    {
                        bool canFire = true;
                        if (LevelSelectionNew.modeSelection != LevelSelectionNew.modeType.FREEFORALL && checkForFriendlyFire)
                        {
                            RaycastHit hit;
                            if (Physics.Raycast(bulletSpawn.position, targetTransform.position - bulletSpawn.position, out hit, Vector3.Distance(bulletSpawn.position, LOSTargetTransform.position), LOSLayermask))
                            {
                                if (hit.transform.tag == friendlyTag)
                                {
                                    canFire = false;
                                }
                            }
                        }
                        if (canFire)
                        {
                            //Fire a burst of a fixed number of bullets.  Usually this number is one.
                            for (int i = 0; i < shotsPerBurst; i++)
                            {
                                if (i < shotsPerBurst - 1)
                                    yield return new WaitForSeconds(timeBetweenBurstBullets);
                                currentBulletsUntilReload--;
                                try
                                {
                                    FireOneShot();
                                }
                                catch { }
                            }
                        }
                    }
                    currentRoundsPerVolley--;
                }
                else
                {
                    yield break;
                }
                if (currentRoundsPerVolley > 0)
                {
                    yield return new WaitForSeconds(timeBetweenBursts);
                }
            }
        }

        private Coroutine coroutine;
        void FireOneShot()
	{
		//Look At Target
		if(targetTransform && !myAIBaseScript.inParkour)
			{
                //Snap our aim to the target even if we're aiming slightly off
                //This is because the RotateToAimGunScript will rarely point directly at the target- merely close enough
                bool amAtTarget = Vector3.Angle(bulletSpawn.forward, targetTransform.position - bulletSpawn.position) < maxFiringAngle;

                //Fire Shot
                //Most bullets will have one bullet.  However, shotguns and similar weapons will have more.
                try
                {
                    for (int j = 0; j < pelletsPerShot; j++)
                    {
                        if (amAtTarget)
                        {
                            fireRotation.SetLookRotation(targetTransform.position - bulletSpawn.position);
                        }
                        else
                        {
                            fireRotation = Quaternion.LookRotation(bulletSpawn.forward);
                        }

                        //Modify our aim by a random amound to simulate inaccuracy.
                        fireRotation *= Quaternion.Euler(Random.Range(-inaccuracy, inaccuracy), Random.Range(-inaccuracy, inaccuracy), 0);

                        try
                        {
                            bulletObject.GetComponent<BulletScript>().teamID = GetComponent<TacticalAI.HealthScript>().teamID;
                            bulletObject.GetComponent<BulletScript>().statIndex = GetComponent<TacticalAI.HealthScript>().statIndex;
                            bulletObject.GetComponent<BulletScript>().nameEnemies = GetComponent<TacticalAI.HealthScript>().nameEnemies;
                            bulletObject.GetComponent<BulletScript>().kills = GetComponent<TacticalAI.HealthScript>().kills;
                            bulletObject.GetComponent<BulletScript>().death = GetComponent<TacticalAI.HealthScript>().death;
                            bulletObject.GetComponent<BulletScript>().score = GetComponent<TacticalAI.HealthScript>().score;
                            bulletObject.GetComponent<BulletScript>().gunID = GetComponent<TacticalAI.HealthScript>().gunID;
                            bulletObject.GetComponent<BulletScript>().healthScript = GetComponent<TacticalAI.HealthScript>();
                        }
                        catch { }
                        // GameObject bullet = (GameObject)(Instantiate(bulletObject, bulletSpawn.position, fireRotation));
                        GameObject bullet = (GameObject)(Instantiate(bulletObject, WeaponContainer.GetChild(currentWeapon).transform.GetChild(1).position, fireRotation));
                        //If this is using the TacticalAI Bullet Script and is a rocket launcher
                        if (isRocketLauncher && bullet.GetComponent<TacticalAI.BulletScript>())
                        {
                            bullet.GetComponent<TacticalAI.BulletScript>().SetAsHoming(targetTransform);
                        }
                    }
                }
                catch { }

                // Spawning Muzzle Flash At Exact Point On Enemies Guns (For Companions In Multiplayer Mode) 
                GameObject muzzle = (GameObject) Instantiate(Resources.Load("EnemyMuzzleFlash"), WeaponContainer.GetChild(currentWeapon).transform.GetChild(1).position, Quaternion.identity);
                muzzle.transform.rotation = Quaternion.LookRotation(WeaponContainer.GetChild(currentWeapon).transform.GetChild(1).forward);

                //Play the sound that is audible by the player
                try
                {
                    if (bulletSound)
                    {
                        audioSource.volume = bulletSoundVolume;
                        audioSource.PlayOneShot(bulletSound);
                    }

                    if (animationScript)
                    {
                        animationScript.PlayFiringAnimation();
                    }
                }
                catch { }
	

			}
	}
	
	
	//Shooting////////////////////////////////////////////////////////
	//Secondary Fire////////////////////////////////////////////////////////
	public void FireOneGrenade()
	{
            //Debug.Log("weehoo");

            if (canThrowGrenade && Vector3.Angle(bulletSpawn.forward, targetTransform.position - bulletSpawn.position)  < maxSecondaryFireAngle && secondaryFireObject)
			{
                //Debug.Log("lol");
				float dist = Vector3.Distance(targetTransform.position, bulletSpawn.position);
				if(dist < maxDistForSecondaryFire && dist > minDistForSecondaryFire && timer < 0)
					{
						canFireGrenadeAgain = false;	
						StartCoroutine(SetTimeUntilNextGrenade());

                    
                    myAIBaseScript.ThrowGrenade(secondaryFireObject, targetTransform.position, grenadeSpawn);
                        /*
                        //Fire Secondary Fire
                        //Debug.Log("eeee");
                        //Debug.Break();
                        GameObject currentGrenade = (GameObject)(Instantiate(secondaryFireObject, bulletSpawn.position, bulletSpawn.rotation));

                        //Stuff to deal with Grenades
                        currentGrenade.transform.LookAt(lastPosTargetSeen);
                        if(currentGrenade.GetComponent<TacticalAI.GrenadeScript>())
                            {
                                currentGrenade.GetComponent<TacticalAI.GrenadeScript>().SetTarget(lastPosTargetSeen);
                            }	*/
                    }
            }			
	}
        //Secondary Fire////////////////////////////////////////////////////////
        public Transform grenadeSpawn;
		
	//Setters
	public void EndEngage()
	{
		targetTransform = null;
		aware = false;
	}
	
	public void AssignTarget(Transform newTarget, Transform newLOSTarget){	
		targetTransform = newTarget;
		LOSTargetTransform = newLOSTarget;
		aware = true;			
	}	
	
	public void SetCanCurrentlyFire(bool b)
	{
		canCurrentlyFire = b;
	}
	
	public void SetAware()
	{
		aware = true;
	}
	
	public void SetEnemyBaseScript(TacticalAI.BaseScript x)
	{
		myAIBaseScript = x;
	}	
	
	public void ChangeTargets(Transform newl,Transform newt)
	{
		LOSTargetTransform = newl;
		targetTransform = newt;				
	}

    bool locatedNewGrenadeTargetYet;

	IEnumerator SetTimeUntilNextGrenade()
	{
		canThrowGrenade = false;
		yield return new WaitForSeconds(minTimeBetweenSecondaryFire);
		canThrowGrenade = true;
	}		

	public bool IsFiring()
	{
		return isFiring;
	}	
        
    public bool IsReloading()
	{
		return isReloading;
	}


        private void OnDestroy()
        {
            if(isFiring)
            {
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Game")
                    GameManager.totalFiringAgents--;
            }

        }

        private IEnumerator HideAfterTime()
        {
            yield return new WaitForSeconds(0.1f);
            muzzleFlashObject.enabled = false;
        }

        private void OnDisable()
        {
            CancelInvoke("SetAssaultGun");
            StopCoroutine("BulletFiringCycle");
            StopCoroutine("Fire");
            StopCoroutine("SetTimeUntilNextGrenade");
            StopCoroutine("HideAfterTime");
        }
    }
}