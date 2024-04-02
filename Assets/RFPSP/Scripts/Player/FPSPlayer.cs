//FPSPlayer.cs by Azuline Studios© All Rights Reserved
//Controls main player actions such as hitpoints and damage, HUD GUIText/Texture element instantiation and update,
//directs player button mappings to other scripts, handles item detection and pickup, and plays basic player sound effects.
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FPSPlayer : MonoBehaviour {
	[HideInInspector]
	public Ironsights IronsightsComponent;
	[HideInInspector]
	public InputControl InputComponent;
	[HideInInspector]
	public FPSRigidBodyWalker FPSWalkerComponent;
	[HideInInspector]
	public PlayerWeapons PlayerWeaponsComponent;
	[HideInInspector]
	public WorldRecenter WorldRecenterComponent;
	[HideInInspector]
	public WeaponBehavior WeaponBehaviorComponent;
	[HideInInspector]
	public	SmoothMouseLook MouseLookComponent;
	[HideInInspector]
	public	WeaponEffects WeaponEffectsComponent;
	[HideInInspector]
	public WeaponPivot WeaponPivotComponent;
	[HideInInspector]
	public CameraControl CameraControlComponent;
	[HideInInspector]
	public NPCRegistry NPCRegistryComponent;
	[HideInInspector]
	public GameObject NPCMgrObj;
	private AI AIComponent;
 	//other objects accessed by this script
	[HideInInspector]
	public GameObject[] children;//behaviors of these objects are deactivated when restarting the scene
	[HideInInspector]
	public GameObject weaponCameraObj;
	[HideInInspector]
	public GameObject weaponObj;

	[Tooltip("Reference to the UI Canvas object.")]
	public  GameObject canvasObj;
	[Tooltip("Object reference to the GUITexture object in the project library that renders pain effects on screen.")]
	public GameObject painFadeObj;
	private PainFade painFadeComponent;
	private Image painFadeImage;
	public GameObject levelLoadFadeObj;
	[HideInInspector] public LevelLoadFade levelLoadFadeRef;
	[Tooltip("Object reference to the Text object in the project library that renders health amounts on screen.")]
	public GameObject healthUiObj;
	[Tooltip("Object reference to the Text object in the project library that renders health amounts on screen.")]
	public GameObject healthUiObjShadow;
	[Tooltip("Object reference to the Text object in the project library that renders ammo amounts on screen.")]
	public GameObject ammoUiObj;
	[Tooltip("Object reference to the Text object in the project library that renders ammo amounts on screen.")]
	public GameObject ammoUiObjShadow;
	[Tooltip("Object reference to the Text object in the project library that renders hunger amounts on screen.")]
	public GameObject hungerUiObj;
	[Tooltip("Object reference to the Text object in the project library that renders hunger amounts on screen.")]
	public GameObject hungerUiObjShadow;
	[Tooltip("Object reference to the Text object in the project library that renders thirst amounts on screen.")]
	public GameObject thirstUiObj;
	[Tooltip("Object reference to the Text object in the project library that renders thirst amounts on screen.")]
	public GameObject thirstUiObjShadow;
	[Tooltip("Object reference to the Text object in the project library that renders help text on screen.")]
	public GameObject helpGuiObj;
	[Tooltip("Object reference to the Text object in the project library that renders help text on screen.")]
	public GameObject helpGuiObjShadow;
	[Tooltip("Object reference to the Text object in the project library that renders hhelp amounts on screen.")]
	public GameObject waveUiObj;
	[Tooltip("Object reference to the Text object in the project library that renders wave text on screen.")]
	public GameObject waveUiObjShadow;
	[Tooltip("Object reference to the Text object in the project library that renders wave amounts on screen.")]
	public GameObject warmupUiObj;
	[Tooltip("Object reference to the Text object in the project library that renders warmup text on screen.")]
	public GameObject warmupUiObjShadow;
	[Tooltip("Object reference to the Text object in the project library that renders warmup text on screen.")]
	public GameObject crosshairUiObj;
	[HideInInspector]
	public Image crosshairUiImage;
	[HideInInspector]
	public RectTransform crosshairUiRect;
	[Tooltip("Object reference to the GUITexture object in the project library that renders crosshair on screen.")]
	public GameObject hitmarkerUiObj;
	[HideInInspector]
	public Image hitmarkerUiImage;
	[HideInInspector]
	public RectTransform hitmarkerUiRect;

	private HealthText HealthText;
	private HealthText HealthText2;
	private Text healthUiText;
	
	private HungerText HungerText;
	private HungerText HungerText2;
	private Text HungerUIText;
	
	private ThirstText ThirstText;
	private ThirstText ThirstText2;
	private Text ThirstUIText;
		
	[HideInInspector]
	public float crosshairWidth;
	[Tooltip("Size of crosshair relative to screen size.")]
	public float crosshairSize;
	private float oldWidth;
	[HideInInspector]
	public float hitTime = -10.0f;
	[HideInInspector]
	public bool hitMarkerState;
	private Transform mainCamTransform;
	[TooltipAttribute("True if the prefab parent object will be removed on scene load.")]
	public bool removePrefabRoot = true;
	
	//player hit points
	public float hitPoints = 100.0f;
	public float maximumHitPoints = 200.0f;
	[Tooltip("True if player's health should be displayed on the screen.")]
	public bool showHealth = true;
	[Tooltip("True if player's ammo should be displayed on the screen.")]
	public bool showAmmo = true;
	[Tooltip("True if negative hitpoint values should be shown.")]
	public bool showHpUnderZero = true;
	[Tooltip("True if player cannot take damage.")]
	public bool invulnerable;
	[Tooltip("True if the player regenerates their health after health regen delay elapses without player taking damage.")]
	public bool regenerateHealth = false;
	[Tooltip("The maximum amount of hitpoints that should be regenerated.")]
	public float maxRegenHealth = 100.0f;
	[Tooltip("Delay after being damaged that the player should start to regenerate health.")]
	public float healthRegenDelay = 7.0f;
	[Tooltip("Rate at which the player should regenerate health.")]
	public float healthRegenRate = 25.0f;
	private float timeLastDamaged;//time that the player was last damaged
	
	//player hunger
	[Tooltip("True if player should have a hunger attribute that increases over time.")]
	public bool usePlayerHunger;
	[HideInInspector]
	public float maxHungerPoints = 100.0f;//maximum amount that hunger will increase to before players starts to starve
	[TooltipAttribute("Seconds it takes for player to accumulate 1 hunger point.")]
	public float hungerInterval = 7.0f;
	[HideInInspector]
	public float hungerPoints = 0.0f;//total hunger points 
	private float lastHungerTime;//time that last hunger point was applied
	private float lastStarveTime;//time that last starve damage was applied
	[TooltipAttribute("Seconds to wait before starve damaging again (should be less than healthRegenDelay to prevent healing of starvation damage).")]
	public float starveInterval = 3.0f;
	[TooltipAttribute("Anount of damage to apply per starve interval.")]
	public float starveDmgAmt = -5.0f;//amount to damage player per starve interval 
	
	//player thirst
	[Tooltip("True if player should have a thirst attribute that increases over time.")]
	public bool usePlayerThirst;
	[HideInInspector]
	public float maxThirstPoints = 100.0f;//maximum amount that thirst will increase to before players starts to take thirst damage
	[TooltipAttribute("Seconds it takes for player to accumulate 1 thirst point.")]
	public float thirstInterval = 7.0f;
	[HideInInspector]
	public float thirstPoints = 0.0f;//total thirst points 
	private float lastThirstTime;//time that last thirst point was applied
	private float lastThirstDmgTime;//time that last thirst damage was applied
	[TooltipAttribute("Seconds to wait before thirst damaging again (should be less than healthRegenDelay to prevent healing of thirst damage).")]
	public float thirstDmgInterval = 3.0f;
	[Tooltip("Amount to damage player per thirst damage interval.")]
	public float thirstDmgAmt = -5.0f;

	[Tooltip("True if player can activate bullet time by pressing button (default T).")]
	public bool allowBulletTime = true;
	[Tooltip("True if help text should be displayed.")]
	public bool showHelpText = true;
	[Tooltip("True if pause (default: Tab) should hide cursor.")]
	public bool pauseHidesCursor = true;
	
	//Damage feedback
	private float gotHitTimer = -1.0f;
	private Color PainColor = Color.white;
	private Color painFadeColor;//used to modify opacity of pain fade object
	[TooltipAttribute("Amount to kick the player's camera view when damaged.")]
	public float painScreenKickAmt = 0.016f;//magnitude of the screen kicks when player takes damage
	
	//Bullet Time and Pausing
	[TooltipAttribute("Percentage of normal time to use when in bullet time.")]
	[Range(0.0f, 1.0f)]
	public float bulletTimeSpeed = 0.35f;//decrease time to this speed when in bullet time
	[Tooltip("Movement multiplier during bullet time.")]
	public float sloMoMoveSpeed = 2.0f;
	private float pausedTime;//time.timescale value to return to after pausing
	[HideInInspector]
	public bool bulletTimeActive;
	[HideInInspector]
	public float backstabBtTime;
	[HideInInspector]
	public bool backstabBtState;
	private float initialFixedTime;
	[HideInInspector]
	public float usePressTime;
	[HideInInspector]
	public float useReleaseTime;
	private bool useState;
	[HideInInspector]
	public bool pressButtonUpState;
	[HideInInspector]
	public Collider objToPickup;
	
	//zooming
	private bool zoomBtnState = true;
	private float zoomStopTime = 0.0f;//track time that zoom stopped to delay making aim reticle visible again
	[HideInInspector]
	public bool zoomed = false;
	[HideInInspector]
	public float zoomStart = -2.0f;
	[HideInInspector]
	public bool zoomStartState = false;
	[HideInInspector]
	public float zoomEnd = 0.0f;
	[HideInInspector]
	public bool zoomEndState = false;
	private float zoomDelay = 0.4f;
	[HideInInspector]
	public bool dzAiming;
	
	//crosshair 
	[Tooltip("Enable or disable the aiming reticle.")]
	public bool crosshairEnabled = true;
	private bool crosshairVisibleState = true;
	private bool crosshairTextureState = false;
	[Tooltip("Set to true to display swap reticle when item under reticle will replace current weapon.")]
	public bool useSwapReticle = true;
	[Tooltip("The texture used for the aiming crosshair.")]
	public Sprite aimingReticle;
	[Tooltip("The texture used for the hitmarker.")]
	public Sprite hitmarkerReticle;
	[Tooltip("The texture used for the pick up crosshair.")]
	public Sprite pickupReticle;
	[Tooltip("The texture used for when the weapon under reticle will replace current weapon.")]
	public Sprite swapReticle;
	[Tooltip("The texture used for showing that weapon under reticle cannot be picked up.")]
	public Sprite noPickupReticle;
	[Tooltip("The texture used for the pick up crosshair.")]
	private Sprite pickupTex;

	private Color pickupReticleColor = Color.white; 
	[HideInInspector]
	public Color reticleColor = Color.white; 
	private Color initialReticleColor;
	private Color hitmarkerColor = Color.white; 
	[Tooltip("Layers to include for crosshair raycast in hit detection.")]
	public LayerMask rayMask;
	[Tooltip("Distance that player can pickup and activate items.")]
	public float reachDistance = 2.1f;

	private RaycastHit hit;
	private RaycastHit hit2;
	private Vector3 camCrosshairPos;
	[HideInInspector]
	public bool raycastCrosshair;
	
	//button and behavior states
	private bool pickUpBtnState = true;
	[HideInInspector]
	public bool restarting = false;//to notify other scripts that level is restarting
	
	//sound effects
	public AudioClip painLittle;
	public AudioClip painBig;
	public AudioClip painDrown;
	public AudioClip gasp;
	public AudioClip catchBreath;
	public AudioClip die;
	public AudioClip dieDrown;
	public AudioClip jumpfx;
	public AudioClip enterBulletTimeFx;
	public AudioClip exitBulletTimeFx;
	public AudioClip hitMarker;
	[Tooltip("Particle effect to play when player blocks attack.")]
	public GameObject blockParticles;
	[Tooltip("Distance from camera to emit blocking particle effect.")]
	public float blockParticlesPos;
	private ParticleSystem blockParticleSys;

	private AudioSource[]aSources;//access the audio sources attatched to this object as an array for playing player sound effects
	[HideInInspector]
	public AudioSource otherfx;
	[HideInInspector]
	public AudioSource hitmarkfx;
	private bool bullettimefxstate;
	[HideInInspector]
	public bool blockState;
	[HideInInspector]
	public float blockAngle;
	[HideInInspector]
	public bool canBackstab;//true if player can backstab an unalerted NPC
	private float moveCommandedTime;//last time that following NPCs were commanded to move (for command cooldown)
	
	[HideInInspector]
	public bool menuDisplayed;
	[HideInInspector]
	public float menuTime;
	[HideInInspector]
	public float pauseTime;
	[HideInInspector]
	public bool paused;
	private MainMenu MainMenuComponent;

	private Transform myTransform;

	public static int teamID;
	public static string playerName;
	public static int death, kill, score;
    public bool isReSpawning = false;
	void Start (){	

		if(removePrefabRoot){
			GameObject prefabRoot = transform.parent.transform.gameObject;
			transform.parent.transform.DetachChildren();
			Destroy(prefabRoot);
		}

      

		mainCamTransform = Camera.main.transform;
		myTransform = transform;
		//set up external script references
		IronsightsComponent = GetComponent<Ironsights>();
		InputComponent = GetComponent<InputControl>();
		FPSWalkerComponent = GetComponent<FPSRigidBodyWalker>();
		WorldRecenterComponent = GetComponent<WorldRecenter>();
		MouseLookComponent = mainCamTransform.parent.transform.GetComponent<SmoothMouseLook>();
		CameraControlComponent = mainCamTransform.GetComponent<CameraControl>();
		weaponObj = CameraControlComponent.weaponObj;
		WeaponEffectsComponent = weaponObj.GetComponent<WeaponEffects>();
		PlayerWeaponsComponent = weaponObj.GetComponent<PlayerWeapons>();
		//painFadeObj.SetActive(true);
		//painFadeComponent = painFadeObj.GetComponent<PainFade>();
		//painFadeComponent.painImageComponent = painFadeComponent.GetComponent<Image>();
		//PainColor = painFadeComponent.painImageComponent.color;
		//painFadeObj.SetActive(false);
		
		MainMenuComponent = mainCamTransform.parent.transform.GetComponent<MainMenu>();
//		MainMenuComponent.enabled = false;
		menuDisplayed = false;

		canvasObj.SetActive(true);//
		
		NPCMgrObj = GameObject.Find("NPC Manager");
		NPCRegistryComponent = NPCMgrObj.GetComponent<NPCRegistry>();
			
		aSources = GetComponents<AudioSource>();//Initialize audio source
		otherfx = aSources[0] as AudioSource;
		hitmarkfx = aSources[1] as AudioSource;
		otherfx.spatialBlend = 0.0f;
		hitmarkfx.spatialBlend = 0.0f;
		
		//Set time settings
		Time.timeScale = 1.0f;
		initialFixedTime =  Time.fixedDeltaTime;
		
		usePressTime = 0.0f;
		useReleaseTime = -8f;
		
		//player object collisions
		Physics.IgnoreLayerCollision(11, 12);//no collisions between player object and misc objects like bullet casings
		Physics.IgnoreLayerCollision (12, 12);//no collisions between bullet shells
		
		//weapon object collisions
		Physics.IgnoreLayerCollision(8, 2);//
		Physics.IgnoreLayerCollision(8, 13);//no collisions between weapon and NPCs
		Physics.IgnoreLayerCollision(8, 12);//no collisions between weapon and Objects
		Physics.IgnoreLayerCollision(8, 11);//no collisions between weapon and Player
		Physics.IgnoreLayerCollision(8, 10);//no collisions between weapon and world collision

        //Call FadeAndLoadLevel fucntion with fadeIn argument set to true to tell the function to fade in (not fade out and (re)load level)
        //		GameObject llf = Instantiate(levelLoadFadeObj) as GameObject;

        levelLoadFadeRef = levelLoadFadeObj.GetComponent<LevelLoadFade>();
        levelLoadFadeRef.LevelLoadFadeobj = levelLoadFadeObj;
        levelLoadFadeRef.fadeImage = levelLoadFadeObj.GetComponent<Image>();

        levelLoadFadeRef.FadeAndLoadLevel(Color.black, 1.5f, true);

        //		llf.GetComponent<LevelLoadFade>().FadeAndLoadLevel(Color.black, 2.0f, true);

        if (showHelpText){
			helpGuiObj.SetActive(true);
			helpGuiObjShadow.SetActive(true);
		}else{
			helpGuiObj.SetActive(false);
			helpGuiObjShadow.SetActive(false);
		}
		//create instance of GUITexture to display crosshair on hud
		crosshairUiImage = crosshairUiObj.GetComponent<Image>();
		crosshairUiRect = crosshairUiObj.GetComponent<RectTransform>();
		crosshairUiImage.sprite = aimingReticle;
		hitmarkerUiImage = hitmarkerUiObj.GetComponent<Image>();
		hitmarkerUiRect = hitmarkerUiObj.GetComponent<RectTransform>();
		hitmarkerUiImage.sprite = hitmarkerReticle;
		hitmarkerUiImage.enabled = false;
		//set alpha of hand pickup crosshair
		pickupReticleColor.a = 0.95f;
		initialReticleColor = crosshairUiImage.color;
		//set alpha of aiming reticule and make it 100% transparent if crosshair is disabled
		if(crosshairEnabled){
			reticleColor.a = initialReticleColor.a;
			hitmarkerUiImage.color = hitmarkerColor;
		}else{
			//make alpha of aiming reticle zero/transparent
			reticleColor.a = 0.0f;
			//set alpha of aiming reticle at start to prevent it from showing, but allow item pickup hand reticle 
			crosshairUiImage.color = reticleColor;
			hitmarkerUiImage.color = reticleColor;
		}
		
		//set reference for main color element of heath GUIText
		HealthText = healthUiObj.GetComponent<HealthText>();
		//set reference for shadow background color element of health GUIText
		//this object is a child of the main health GUIText object, so access it as an array
		HealthText2 = healthUiObjShadow.GetComponent<HealthText>();
		
		//initialize health amounts on GUIText objects
		HealthText.healthGui = hitPoints;
		HealthText2.healthGui = hitPoints;	
		healthUiText = HealthText.GetComponent<Text>();
		healthUiText.material.color = Color.white;
		
		if(!showHealth){
			healthUiObj.gameObject.SetActive(false);
		}

		//set reference for main color element of hunger GUIText
		HungerText = hungerUiObj.GetComponent<HungerText>();
		//set reference for shadow background color element of hunger GUIText
		//this object is a child of the main hunger GUIText object, so access it as an array
		HungerText2 = hungerUiObjShadow.GetComponent<HungerText>();
		
		//initialize hunger amounts on GUIText objects
		HungerText.hungerGui = hungerPoints;
		HungerText2.hungerGui = hungerPoints;	
		HungerUIText = HungerText.GetComponent<Text>();

		if(!usePlayerHunger){
			hungerUiObj.SetActive(false);
			hungerUiObjShadow.SetActive(false);
		}

		//set reference for main color element of thirst GUIText
		ThirstText = thirstUiObj.GetComponent<ThirstText>();
		//set reference for shadow background color element of thirst GUIText
		//this object is a child of the main thirst GUIText object, so access it as an array
		ThirstText2 = thirstUiObjShadow.GetComponent<ThirstText>();
		
		//initialize thirst amounts on GUIText objects
		ThirstText.thirstGui = thirstPoints;
		ThirstText2.thirstGui = thirstPoints;
		ThirstUIText = ThirstText.GetComponent<Text>();	

		if(!usePlayerThirst){
			thirstUiObj.SetActive(false);
			thirstUiObjShadow.SetActive(false);
		}


		if (PlayerPrefs.GetInt("Game Type") != 2){
			waveUiObj.SetActive(false);
			waveUiObjShadow.SetActive(false);
			warmupUiObj.SetActive(false);
			warmupUiObjShadow.SetActive(false);
		}else{
			waveUiObj.SetActive(true);
			waveUiObjShadow.SetActive(true);
			warmupUiObj.SetActive(true);
			warmupUiObjShadow.SetActive(true);
		}
	}
	void DelayedEnable()
    {
        enabled = true;
    }
	void LateUpdate () {
		
		if((MouseLookComponent.dzAiming || raycastCrosshair) && WeaponBehaviorComponent)
        {
			if(!WeaponBehaviorComponent.unarmed
			&& Physics.Raycast(mainCamTransform.position, WeaponBehaviorComponent.weaponLookDirection, out hit, 100.0f, rayMask)){
				camCrosshairPos = Camera.main.WorldToViewportPoint(hit.point);
			}else{
				if(WeaponBehaviorComponent.unarmed){
					camCrosshairPos = new Vector3(0.5f, 0.5f, 0.0f);
				}else{
					camCrosshairPos = Camera.main.WorldToViewportPoint(WeaponBehaviorComponent.origin + WeaponPivotComponent.childTransform.forward * 200.0f);
				}
			}
		}else{
			camCrosshairPos = new Vector3(0.5f, 0.5f, 0.0f);
		}
		crosshairUiRect.anchorMax = camCrosshairPos;
		crosshairUiRect.anchorMin = camCrosshairPos;
		hitmarkerUiRect.anchorMax = camCrosshairPos;
		hitmarkerUiRect.anchorMin = camCrosshairPos;
		hitmarkerColor.a = 0.7f;
		hitmarkerUiImage.color = hitmarkerColor;	
		
	}

    public Scrollbar zoomScroller, zoomScroller2;
    public GameObject scope1, scope2;
    public void ChangeZoomState(bool flag)
    {
        zoomed = flag;
            if (zoomed)
            {
				GameManager.Instance.ShowScope();
            }
            else
            {
				GameManager.Instance.HideScope();
            }
            if (zoomed)
            {
				if (scope1.activeInHierarchy)
				{
					WeaponBehaviorComponent.zoomFOV = (int)(100f / ((zoomScroller.value + 0.8f) * 10f));
				}
				else if (scope2.activeInHierarchy)
				{
					WeaponBehaviorComponent.zoomFOV = (int)(100f / ((zoomScroller2.value + 0.8f) * 10f));
				}
            }
    }
    void Update (){

			try
			{
				WeaponBehaviorComponent = PlayerWeaponsComponent.CurrentWeaponBehaviorComponent;
				if (zoomed)
				{
					if (scope1.activeInHierarchy)
						WeaponBehaviorComponent.zoomFOV = (int)(100f / ((zoomScroller.value + 0.8f) * 10f));
					else if (scope2.activeInHierarchy)
						WeaponBehaviorComponent.zoomFOV = (int)(100f / ((zoomScroller2.value + 0.8f) * 10f));
				}
			}
            catch { }
        

        if (allowBulletTime){//make bullet time an optional feature
			try
			{
				if (InputComponent.bulletTimePress)
				{//set bulletTimeActive to true or false based on button input
					if (!bulletTimeActive)
					{
						FPSWalkerComponent.moveSpeedMult = Mathf.Clamp(sloMoMoveSpeed, 1f, sloMoMoveSpeed);
						bulletTimeActive = true;
					}
					else
					{
						FPSWalkerComponent.moveSpeedMult = 1.0f;
						bulletTimeActive = false;
					}
				}
			}
            catch { }
					
			otherfx.pitch = Time.timeScale;//sync pitch of bullet time sound effects with Time.timescale
			hitmarkfx.pitch = Time.timeScale;
		
			if(Time.timeScale > 0 && !restarting){//decrease or increase Time.timescale when bulletTimeActive is true
				Time.fixedDeltaTime = initialFixedTime * Time.timeScale;
				if(bulletTimeActive){
					if(!bullettimefxstate){
						//otherfx.clip = enterBulletTimeFx;
						//otherfx.PlayOneShot(otherfx.clip, 1.0f);//play enter bullet time sound effect
						bullettimefxstate = true;
					}
					Time.timeScale = Mathf.MoveTowards(Time.timeScale, bulletTimeSpeed, Time.deltaTime * 3.0f);
				}else{
					if(bullettimefxstate){
						//otherfx.clip = exitBulletTimeFx;
						//otherfx.PlayOneShot(otherfx.clip, 1.0f);//play exit bullet time sound effect
						FPSWalkerComponent.moveSpeedMult = 1.0f;
						bullettimefxstate = false;
					}
					if(1.0f - Mathf.Abs(Time.timeScale) > 0.05f){//make sure that timescale returns to exactly 1.0f 
						Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1.0f, Time.deltaTime * 3.0f);
					}else{
						Time.timeScale = 1.0f;
					}
				}
			}
		}

		//set zoom mode to toggle, hold, or both, based on inspector setting
		try
		{
			switch (IronsightsComponent.zoomMode)
			{
				case Ironsights.zoomType.both:
					zoomDelay = 0.4f;
					break;
				case Ironsights.zoomType.hold:
					zoomDelay = 0.0f;
					break;
				case Ironsights.zoomType.toggle:
					zoomDelay = 999.0f;
					break;
			}
		}
		catch { }		
		//regenerate player health if regenerateHealth var is true
		if(regenerateHealth){
			try
			{
				if (hitPoints < maxRegenHealth && timeLastDamaged + healthRegenDelay < Time.time)
				{
					HealPlayer(healthRegenRate * Time.deltaTime);
						GameManager.Instance.UpdateText();
				}
			}
			catch { }
		}
		
		
        

        //toggle or hold zooming state by determining if zoom button is pressed or held
        if (InputComponent.zoomHold
		&& WeaponBehaviorComponent.canZoom 
		&& !blockState
		&& !IronsightsComponent.reloading
		&& !FPSWalkerComponent.proneMove//no zooming while crawling
		&& !FPSWalkerComponent.hideWeapon){
			if(!zoomStartState){
				zoomStart = Time.time;//track time that zoom button was pressed
				zoomStartState = true;//perform these actions only once
				zoomEndState = false;
				if(zoomEnd - zoomStart < zoomDelay * Time.timeScale){//if button is tapped, toggle zoom state
					if(!zoomed){
                        ChangeZoomState(true);
						// Hiding Banner When Zoom Is Enabled
						//GoogleMobileAdsManager.Instance.HideBanner();
					}
					else{
                        ChangeZoomState(false);
						// Showing Banner After Zoom Is Disabled
						//GoogleMobileAdsManager.Instance.ShowBanner();
					}
				}
			}
		}else{
			if(!InputComponent.zoomHold){blockState = false; }//reset block after a hit, so player needs to press block/zoom button again
			if(!zoomEndState){
				zoomEnd = Time.time;//track time that zoom button was released
				zoomEndState = true;
				zoomStartState = false;
				if(zoomEnd - zoomStart > zoomDelay * Time.timeScale){//if releasing zoom button after holding it down, stop zooming
                    ChangeZoomState(false);
                }
			}
		}
		
		//cancel zooming while crawling
		if(FPSWalkerComponent.proneMove){
			zoomEndState = true;
			zoomStartState = false;
            ChangeZoomState(false);
        }
		
		//track when player stopped zooming to allow for delay of reticle becoming visible again
		if (zoomed){
			zoomBtnState = false;//only perform this action once per button press
		}else{
			if(!zoomBtnState){
				zoomStopTime = Time.time;
				zoomBtnState = true;
			}
		}
		try
		{
			UpdateHitmarker();
		}
		catch { }

        //enable and disable crosshair based on various states like reloading and zooming
        if ((IronsightsComponent.reloading || (zoomed && (!dzAiming || WeaponBehaviorComponent.zoomIsBlock) && !WeaponBehaviorComponent.showZoomedCrosshair))
        && !CameraControlComponent.thirdPersonActive) {
            //don't disable reticle if player is using a melee weapon or if player is unarmed
            if ((WeaponBehaviorComponent.meleeSwingDelay == 0 || WeaponBehaviorComponent.zoomIsBlock) && !WeaponBehaviorComponent.unarmed) {
                if (crosshairVisibleState) {
                    //disable the GUITexture element of the instantiated crosshair object
                    //and set state so this action will only happen once.
                    crosshairUiImage.enabled = false;
                    crosshairVisibleState = false;
                }
            }
        } else if(PlayerPrefs.GetInt("Start_CutScene") == 1) {
            //Because of the method that is used for non magazine reloads, an additional check is needed here
            //to make the reticle appear after the last bullet reload time has elapsed. Proceed with no check
            //for magazine reloads.
#if UNITY_EDITOR
            print("its happening");
#endif
            if ((WeaponBehaviorComponent.bulletsPerClip != WeaponBehaviorComponent.bulletsToReload
                && WeaponBehaviorComponent.reloadLastStartTime + WeaponBehaviorComponent.reloadLastTime < Time.time)
            || WeaponBehaviorComponent.bulletsPerClip == WeaponBehaviorComponent.bulletsToReload)
            {
                //allow a delay before enabling crosshair again to let the gun return to neutral position
                //by checking the zoomStopTime value
                if (zoomStopTime + 0.2f < Time.time)
                {
                    if (!crosshairVisibleState)
                    {
                        crosshairUiImage.enabled = true;
                        crosshairVisibleState = true;
                    }
                }
            }

        }
		if (PlayerPrefs.GetInt("Start_CutScene") == 1)
		if (crosshairEnabled){
			if(WeaponBehaviorComponent.showAimingCrosshair){
				if(!WeaponPivotComponent.deadzoneZooming){
					if(!WeaponPivotComponent.deadzoneLooking){
						reticleColor.a = initialReticleColor.a;
					}else{
						reticleColor.a = 1.0f;
					}
				}else{
					if(!CameraControlComponent.thirdPersonActive){
						if(zoomed){
							reticleColor.a = 1.0f;
						}else{
							if(WeaponPivotComponent.swayLeadingMode){
								reticleColor.a = initialReticleColor.a;
							}else{
								reticleColor.a = 0.0f;//no crosshair for goldeneye/perfect dark style, non-zoomed aiming
							}
						}
					}else{
						reticleColor.a = initialReticleColor.a;
					}
				}
				crosshairUiImage.color = reticleColor;
			}else{
				//make alpha of aiming reticle zero/transparent
				reticleColor.a = 0.0f;
				//set alpha of aiming reticle at start to prevent it from showing, but allow item pickup hand reticle 
				crosshairUiImage.color = reticleColor;
			}
		}else{
			reticleColor.a = 0.0f;
			crosshairUiImage.color = reticleColor;
		}
				
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//Pick up or activate items	
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
       
		
		//only register one press of E key to make player have to press button again to pickup items instead of holding E
		if (InputComponent.useHold){
			pickUpBtnState = false;
		}else{
			pickUpBtnState = true;	
		}
	}
	

	
	//set reticle type based on the boolean value passed to this function
	public void UpdateReticle( bool reticleType ){
		if(!reticleType){
			crosshairUiImage.sprite = pickupTex;
			crosshairUiImage.color = pickupReticleColor;
			crosshairTextureState = true;
		}else{
			crosshairUiImage.sprite = aimingReticle;
			crosshairUiImage.color = reticleColor;
			crosshairTextureState = false;	
		}
	}
	
	void UpdateHitmarker(){
		if(hitTime + 0.3f > Time.time){
			if(!hitMarkerState){
				if(WeaponBehaviorComponent.meleeSwingDelay == 0 && !WeaponBehaviorComponent.meleeActive){
					hitmarkerUiImage.enabled = true;
					hitmarkfx.clip = hitMarker;
					hitmarkfx.PlayOneShot(hitmarkfx.clip, 1.0f);
					hitMarkerState = true;
				}
			}
		}else{
			if(hitMarkerState){
				hitMarkerState = false;
			}
			hitmarkerUiImage.enabled = false;
		}
	}
	
	public void UpdateHitTime(){
		hitTime = Time.time;//used for hitmarker
		hitMarkerState = false;	
	}
	
	//Activate bullet time for a specific duration
	public IEnumerator ActivateBulletTime (float duration){
		if(!bulletTimeActive){
			bulletTimeActive = true;
		}else{
			yield break;
		}
		float startTime = Time.time;
		while(true){
			if(startTime + duration < Time.time){
				bulletTimeActive = false;
				yield break;
			}
			yield return new WaitForSeconds(0.1f);
		}	
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Update player attributes
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	//add hitpoints to player health
	public void HealPlayer( float healAmt, bool isHungryThirsty = false ){
			
		if (hitPoints < 1.0f){//Don't add health if player is dead
			return;
		}
		
		//Apply healing
		if(hitPoints + healAmt > maximumHitPoints){ 
			hitPoints = maximumHitPoints;
            if (GameManager.Instance)
                GameManager.Instance.objectiveStats[(int)AssaultLevels.LevelConidtions.isMinHealth] = (int)hitPoints;
        }
        else{
			//Call Die function if player's hitpoints have been depleted
			if(healAmt < 0){
				if(!isHungryThirsty){
					ApplyDamage(healAmt);//allow items that cause damage when consumed
				}else{
					hitPoints += healAmt;//waste away from hunger or thirst
				}
			}else{
                if(GameManager.Instance)
                    GameManager.Instance.objectiveStats[(int)AssaultLevels.LevelConidtions.isMinHealth] = (int)hitPoints;
                hitPoints += healAmt;
			}
		}
			
		//set health hud value to hitpoints remaining
		HealthText.healthGui = Mathf.Round(hitPoints);
		HealthText2.healthGui = Mathf.Round(hitPoints);
			
		//change color of hud health element based on hitpoints remaining
		if (hitPoints <= 25.0f){
			healthUiText.color = Color.red;
		}else if (hitPoints <= 40.0f){
			healthUiText.color = Color.yellow;	
		}else{
			healthUiText.color = HealthText.textColor;	
		}

	}
	
	//update the hunger amount for the player
	public void UpdateHunger( float hungerAmt ){
		
		if (hitPoints < 1.0f){//Don't add hunger if player is dead
			return;
		}
		
		//Apply hungerAmt
		if(hungerPoints + hungerAmt > maxHungerPoints){ 
			hungerPoints = maxHungerPoints;
		}else{
			hungerPoints += hungerAmt;
		}
		
		hungerPoints = Mathf.Clamp(hungerPoints, 0.0f, hungerPoints);
			
		//set hunger hud value to hunger points remaining
		HungerText.hungerGui = Mathf.Round(hungerPoints);
		HungerText2.hungerGui = Mathf.Round(hungerPoints);
			
		//change color of hud hunger element based on hunger points
		if (hungerPoints <= 65.0f){
			HungerUIText.color = HungerText.textColor;
		}else if (hungerPoints <= 85.0f){
				HungerUIText.color = Color.yellow;	
		}else{
			HungerUIText.color = Color.red;	
		}
		
		lastHungerTime = Time.time;	
	}
	
	//update the thirst amount for the player
	public void UpdateThirst( float thirstAmt ){
		
		if (hitPoints < 1.0f){//Don't add thirst if player is dead
			return;
		}
		
		//Apply thirstAmt
		if(thirstPoints + thirstAmt > maxThirstPoints){ 
			thirstPoints = maxThirstPoints;
		}else{
			thirstPoints += thirstAmt;
		}
		
		thirstPoints = Mathf.Clamp(thirstPoints, 0.0f, thirstPoints);
			
		//set thirst hud value to thirst points remaining
		ThirstText.thirstGui = Mathf.Round(thirstPoints);
		ThirstText2.thirstGui = Mathf.Round(thirstPoints);
			
		//change color of hud thirst element based on thirst points
		if (thirstPoints <= 65.0f){
			ThirstUIText.color = ThirstText.textColor;
		}else if (thirstPoints <= 85.0f){
				ThirstUIText.color = Color.yellow;	
		}else{
			ThirstUIText.color = Color.red;	
		}
		
		lastThirstTime = Time.time;
	}
	
    public void ApplyDamage(float damage)
    {
		try
		{
			ApplyDamage(damage, null, false);
        }
        catch { }
    }

	//remove hitpoints from player health
	public void ApplyDamage ( float damage, Transform attacker = null, bool isMeleeAttack = false, TacticalAI.HealthScript HS = null)
    {
		try
		{
			if (hitPoints < 1.0f)
			{
				//Don't apply damage if player is dead
				if (!showHpUnderZero)
				{
					hitPoints = 0.0f;
				}
				return;
			}

			//detect if attacker is inside player block zone
			if (attacker != null
			&& WeaponBehaviorComponent.zoomIsBlock
			&& WeaponBehaviorComponent.blockDefenseAmt > 0.0f
			&& zoomed
			&& ((WeaponBehaviorComponent.onlyBlockMelee && isMeleeAttack) || !WeaponBehaviorComponent.onlyBlockMelee)
			&& WeaponBehaviorComponent.shootStartTime + WeaponBehaviorComponent.fireRate < Time.time)
			{

				Vector3 toTarget = (attacker.position - myTransform.position).normalized;
				blockAngle = Vector3.Dot(toTarget, myTransform.forward);

				if (Vector3.Dot(toTarget, myTransform.forward) > WeaponBehaviorComponent.blockCoverage)
				{

					damage *= 1f - WeaponBehaviorComponent.blockDefenseAmt;
					otherfx.clip = WeaponBehaviorComponent.blockSound;
					otherfx.PlayOneShot(otherfx.clip, 1.0f);

					if (blockParticles)
					{
						if (!CameraControlComponent.thirdPersonActive)
						{
							blockParticles.transform.position = mainCamTransform.position + mainCamTransform.forward * (blockParticlesPos + CameraControlComponent.zoomDistance + CameraControlComponent.currentDistance);
						}
						else
						{
							blockParticles.transform.position = myTransform.position + (mainCamTransform.forward * blockParticlesPos) + (transform.up * 0.5f);
						}
						foreach (Transform child in blockParticles.transform)
						{//emit all particles in the particle effect game object group stored in blockParticles var
							blockParticleSys = child.GetComponent<ParticleSystem>();
							blockParticleSys.Emit(Mathf.RoundToInt(blockParticleSys.emission.rateOverTime.constant));//emit the particle(s)
						}
					}
					blockState = true;
				}
			}

			timeLastDamaged = Time.time;

			//Quaternion painKickRotation;//Set up rotation for pain view kicks
			int painKickUpAmt = 0;
			int painKickSideAmt = 0;

			if (!invulnerable)
			{
				if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
				{
					if (PlayerPrefs.GetInt("currentLevel") <= 5 && hitPoints <= 15)
						return;
					else
						hitPoints -= damage;//Apply damage
				}
				else if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.COVERSTRIKE)
				{
					if (PlayerPrefs.GetInt("CoverStrikeCurrentLevel") <= 3 && hitPoints <= 15)
						return;
					else
						hitPoints -= damage;//Apply damage
				}
				else
					hitPoints -= damage;//Apply damage

				if (GameManager.Instance)
					GameManager.Instance.objectiveStats[(int)AssaultLevels.LevelConidtions.isMinHealth] = (int)hitPoints;
				

					GameManager.Instance.UpdateText();
				if (PlayerPrefs.GetInt("Injection") > 0 && PlayerPrefs.GetInt("AdrenalineTut") != 2)
				{
					if (hitPoints > 1 && hitPoints <= 20 && PlayerPrefs.GetInt("AdrenalineTut") != 1)
					{
						Time.timeScale = 0;
						GameManager.Instance.ShowAdrenalineTutorial();
						PlayerPrefs.SetInt("AdrenalineTut", 1);
					}
					else if (hitPoints > 1 && hitPoints <= 5 && PlayerPrefs.GetInt("AdrenalineTut") != 2)
					{
						Time.timeScale = 0;
						GameManager.Instance.ShowAdrenalineTutorial();
						PlayerPrefs.SetInt("AdrenalineTut", 2);
					}
				}
			}

			//set health hud value to hitpoints remaining
			HealthText.healthGui = Mathf.Round(hitPoints);
			HealthText2.healthGui = Mathf.Round(hitPoints);

			//change color of hud health element based on hitpoints remaining
			    if (hitPoints <= 25.0f)
				{
					GameManager.Instance.playerHealthFillImage.color = Color.red;
					healthUiText.color = Color.red;
				}
				else if (hitPoints <= 40.0f)
				{
					GameManager.Instance.playerHealthFillImage.color = Color.yellow;
					healthUiText.color = Color.yellow;
				}
				else
				{
					GameManager.Instance.playerHealthFillImage.color = Color.green;
					healthUiText.color = HealthText.textColor;
				}

			if (!blockState && PlayerPrefs.GetInt("BloodEffect").Equals(1))
			{
				if (Time.timeScale.Equals(1))
				{
					//painFadeColor = PainColor;
					//painFadeColor.a = PainColor.a + (Random.value * 0.1f);//fade pain overlay based on damage amount
					//painFadeObj.SetActive(true);
					//if ((GameManager.Instance && !GameManager.Instance.isLevelFailed && !GameManager.Instance.isLevelCompleted) || MultiPlayerManager.Instance)
					//    painFadeComponent.StartCoroutine(painFadeComponent.FadeIn(painFadeColor, 0.75f));//Call FadeIn function in painFadeObj to fade screen red when damage taken
				}
			}

			if (!FPSWalkerComponent.holdingBreath)
			{
				//Play pain sound when getting hit
				if (!blockState)
				{//don't play hit sound if blocking attack
					if (Time.time > gotHitTimer && painBig && painLittle)
					{
						// Play a big pain sound
						if (hitPoints < 40.0f || damage > 30.0f)
						{
							otherfx.clip = painBig;
							//ABDUL
							//otherfx.PlayOneShot(otherfx.clip, 1.0f);
							gotHitTimer = Time.time + Random.Range(.5f, .75f);
						}
						else
						{
							//Play a small pain sound
							otherfx.clip = painLittle;
							//ABDUL
							//otherfx.PlayOneShot(otherfx.clip, 1.0f);
							gotHitTimer = Time.time + Random.Range(.5f, .75f);
						}
					}
				}
			}
			else
			{
				if (Time.time > gotHitTimer && painDrown)
				{
					//Play a small pain sound
					otherfx.clip = painDrown;
					//ABDUL
					//otherfx.PlayOneShot(otherfx.clip, 1.0f);
					gotHitTimer = Time.time + Random.Range(.5f, .75f);

				}
			}

			if (!CameraControlComponent.thirdPersonActive)
			{
				painKickUpAmt = Random.Range(100, -100);//Choose a random view kick up amount
				if (painKickUpAmt < 50 && painKickUpAmt > 0) { painKickUpAmt = 50; }//Maintain some randomness of the values, but don't make it too small
				if (painKickUpAmt < 0 && painKickUpAmt > -50) { painKickUpAmt = -50; }

				painKickSideAmt = Random.Range(100, -100);//Choose a random view kick side amount
				if (painKickSideAmt < 50 && painKickSideAmt > 0) { painKickSideAmt = 50; }
				if (painKickSideAmt < 0 && painKickSideAmt > -50) { painKickSideAmt = -50; }

				//create a rotation quaternion with random pain kick values
				//painKickRotation = Quaternion.Euler(mainCamTransform.localRotation.eulerAngles - new Vector3(painKickUpAmt, painKickSideAmt, 0));

				//make screen kick amount based on the damage amount recieved
				if (zoomed && !WeaponBehaviorComponent.zoomIsBlock)
				{
					//appliedPainKickAmt = (damage / (painScreenKickAmt * 10)) / 3;	
				}
				else
				{
					//appliedPainKickAmt = (damage / (painScreenKickAmt * 10));			
				}

				if (blockState)
				{
					//appliedPainKickAmt = 0.025f;
				}

				//make sure screen kick is not so large that view rotates past arm models 
				//appliedPainKickAmt = Mathf.Clamp(appliedPainKickAmt, 0.0f, 0.15f); 

				//smooth current camera angles to pain kick angles using Slerp
				//appliedPainKickAmt = 0.025f;
				//mainCamTransform.localRotation = Quaternion.Slerp(mainCamTransform.localRotation, painKickRotation, appliedPainKickAmt );
			}

			if (WeaponBehaviorComponent.zoomIsBlock)
			{
				if (!WeaponBehaviorComponent.hitCancelsBlock)
				{
					blockState = false;
				}
				else
				{
					ChangeZoomState(false);
				}
			}





			//Call Die function if player's hitpoints have been depleted
			if (hitPoints < 1.0f)
			{
			    SendMessage("Die");//use SendMessage() to allow other script components on this object to detect player death
			}
		}
        catch { }
	}

	public GameObject ragDollPrefab;
	public void ApplyDamage(float damage, TacticalAI.HealthScript healthScript, int teamIDs, int statIndex, string nameEnemies, int kills, int deaths, int scores, int gunID, Transform attacker = null, bool isMeleeAttack = false)
	{
		try
		{
			if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.MULTIPLAYER)
				if (teamIDs.Equals(teamID))
					return;

			float appliedPainKickAmt;

			if (hitPoints < 1.0f)
			{
				//Don't apply damage if player is dead
				if (!showHpUnderZero)
				{
					hitPoints = 0.0f;
				}
				return;
			}

			//detect if attacker is inside player block zone
			if (attacker != null
			&& WeaponBehaviorComponent.zoomIsBlock
			&& WeaponBehaviorComponent.blockDefenseAmt > 0.0f
			&& zoomed
			&& ((WeaponBehaviorComponent.onlyBlockMelee && isMeleeAttack) || !WeaponBehaviorComponent.onlyBlockMelee)
			&& WeaponBehaviorComponent.shootStartTime + WeaponBehaviorComponent.fireRate < Time.time)
			{

				Vector3 toTarget = (attacker.position - myTransform.position).normalized;
				blockAngle = Vector3.Dot(toTarget, myTransform.forward);

				if (Vector3.Dot(toTarget, myTransform.forward) > WeaponBehaviorComponent.blockCoverage)
				{

					damage *= 1f - WeaponBehaviorComponent.blockDefenseAmt;
					otherfx.clip = WeaponBehaviorComponent.blockSound;
					otherfx.PlayOneShot(otherfx.clip, 1.0f);

					if (blockParticles)
					{
						if (!CameraControlComponent.thirdPersonActive)
						{
							blockParticles.transform.position = mainCamTransform.position + mainCamTransform.forward * (blockParticlesPos + CameraControlComponent.zoomDistance + CameraControlComponent.currentDistance);
						}
						else
						{
							blockParticles.transform.position = myTransform.position + (mainCamTransform.forward * blockParticlesPos) + (transform.up * 0.5f);
						}
						foreach (Transform child in blockParticles.transform)
						{//emit all particles in the particle effect game object group stored in blockParticles var
							blockParticleSys = child.GetComponent<ParticleSystem>();
							blockParticleSys.Emit(Mathf.RoundToInt(blockParticleSys.emission.rateOverTime.constant));//emit the particle(s)
						}
					}
					blockState = true;
				}
			}

			timeLastDamaged = Time.time;

			Quaternion painKickRotation;//Set up rotation for pain view kicks
			int painKickUpAmt = 0;
			int painKickSideAmt = 0;

			if (!invulnerable)
			{
				hitPoints -= damage;//Apply damage
			}

			GameManager.Instance.UpdateText();
			//set health hud value to hitpoints remaining
			HealthText.healthGui = Mathf.Round(hitPoints);
			HealthText2.healthGui = Mathf.Round(hitPoints);

			//change color of hud health element based on hitpoints remaining
			if (hitPoints <= 25.0f)
			{
				healthUiText.color = Color.red;
			}
			else if (hitPoints <= 40.0f)
			{
				healthUiText.color = Color.yellow;
			}
			else
			{
				healthUiText.color = HealthText.textColor;
			}

			if (!blockState && PlayerPrefs.GetInt("BloodEffect").Equals(1))
			{
				if (Time.timeScale.Equals(1))
				{
					painFadeColor = PainColor;
					painFadeColor.a = PainColor.a + (Random.value * 0.1f);//fade pain overlay based on damage amount
					painFadeObj.SetActive(true);
					//if ((GameManager.Instance && !GameManager.Instance.isLevelFailed && !GameManager.Instance.isLevelCompleted) || MultiPlayerManager.Instance)
					//    painFadeComponent.StartCoroutine(painFadeComponent.FadeIn(painFadeColor, 0.75f));//Call FadeIn function in painFadeObj to fade screen red when damage taken
				}
			}

			if (!FPSWalkerComponent.holdingBreath)
			{
				//Play pain sound when getting hit
				if (!blockState)
				{//don't play hit sound if blocking attack
					if (Time.time > gotHitTimer && painBig && painLittle)
					{
						// Play a big pain sound
						if (hitPoints < 40.0f || damage > 30.0f)
						{
							otherfx.clip = painBig;
							//ABDUL
							//otherfx.PlayOneShot(otherfx.clip, 1.0f);
							gotHitTimer = Time.time + Random.Range(.5f, .75f);
						}
						else
						{
							//Play a small pain sound
							otherfx.clip = painLittle;
							//ABDUL
							//otherfx.PlayOneShot(otherfx.clip, 1.0f);
							gotHitTimer = Time.time + Random.Range(.5f, .75f);
						}
					}
				}
			}
			else
			{
				if (Time.time > gotHitTimer && painDrown)
				{
					//Play a small pain sound
					otherfx.clip = painDrown;
					//ABDUL
					//otherfx.PlayOneShot(otherfx.clip, 1.0f);
					gotHitTimer = Time.time + Random.Range(.5f, .75f);
				}
			}

			if (!CameraControlComponent.thirdPersonActive)
			{
				painKickUpAmt = Random.Range(100, -100);//Choose a random view kick up amount
				if (painKickUpAmt < 50 && painKickUpAmt > 0) { painKickUpAmt = 50; }//Maintain some randomness of the values, but don't make it too small
				if (painKickUpAmt < 0 && painKickUpAmt > -50) { painKickUpAmt = -50; }

				painKickSideAmt = Random.Range(100, -100);//Choose a random view kick side amount
				if (painKickSideAmt < 50 && painKickSideAmt > 0) { painKickSideAmt = 50; }
				if (painKickSideAmt < 0 && painKickSideAmt > -50) { painKickSideAmt = -50; }

				//create a rotation quaternion with random pain kick values
				painKickRotation = Quaternion.Euler(mainCamTransform.localRotation.eulerAngles - new Vector3(painKickUpAmt, painKickSideAmt, 0));

				//make screen kick amount based on the damage amount recieved
				if (zoomed && !WeaponBehaviorComponent.zoomIsBlock)
				{
					//appliedPainKickAmt = (damage / (painScreenKickAmt * 10)) / 3;
				}
				else
				{
					//appliedPainKickAmt = (damage / (painScreenKickAmt * 10));
				}

				if (blockState)
				{
					//appliedPainKickAmt = 0.025f;
				}

				//make sure screen kick is not so large that view rotates past arm models 
				//appliedPainKickAmt = Mathf.Clamp(appliedPainKickAmt, 0.0f, 0.15f);

				//smooth current camera angles to pain kick angles using Slerp
				appliedPainKickAmt = 0.001f;
				mainCamTransform.localRotation = Quaternion.Slerp(mainCamTransform.localRotation, painKickRotation, appliedPainKickAmt);
			}

			if (WeaponBehaviorComponent.zoomIsBlock)
			{
				if (!WeaponBehaviorComponent.hitCancelsBlock)
				{
					blockState = false;
				}
				else
				{
					ChangeZoomState(false);
				}
			}

			
		}
        catch { }
	}
   
	void DelayedPainFade()
    {

    }
    bool receiveDamage = true;
	IEnumerator WaitReceiveDamage()
	{
		yield return new WaitForSeconds(4f);
		receiveDamage = true;

	}

	public void ApplyDamageEmerald ( object[] DamageArray ){//damage function for Emerald AI asset
		ApplyDamage((float)DamageArray[0], (Transform)DamageArray[1], (bool)DamageArray[2]);
	}
	
	void Die () {

		bulletTimeActive = false;//set bulletTimeActive to false so fadeout wont take longer if bullet time is active
		
		if(!FPSWalkerComponent.drowning){
			//play normal player death sound effect if the player is on land 
			otherfx.clip = die;
			otherfx.PlayOneShot(otherfx.clip, 1.0f);

		}else{
			//play drowning sound effect if the player is underwater 	
			otherfx.clip = dieDrown;
			otherfx.PlayOneShot(otherfx.clip, 1.0f);
		}
		
		//disable player control and sprinting on death
		FPSWalkerComponent.inputX = 0;
		FPSWalkerComponent.inputY = 0;
		FPSWalkerComponent.cancelSprint = true;
			

		//call FadeAndLoadLevel function with fadein argument set to false 
		//in levelLoadFadeObj to restart level and fade screen out from black on level load
		//levelLoadFadeRef.StopAllCoroutines();
		//levelLoadFadeRef.FadeAndLoadLevel(Color.black, 1.2f, false);
	}
	
	public void RestartMap () {
		Time.timeScale = 1.0f;//set timescale to 1.0f so fadeout wont take longer if bullet time is active

        //call FadeAndLoadLevel function with fadein argument set to false 
        //in levelLoadFadeObj to restart level and fade screen out from black on level load

        levelLoadFadeRef.StopAllCoroutines();
        levelLoadFadeRef.FadeAndLoadLevel(Color.black, 1.2f, false);

        //set restarting var to true to be accessed by FPSRigidBodyWalker script to stop rigidbody movement
        restarting = true;
		// Disable all scripts to deactivate player control upon player death
		FPSWalkerComponent.inputX = 0;
		FPSWalkerComponent.inputY = 0;
		FPSWalkerComponent.cancelSprint = true;
		WeaponBehaviorComponent.shooting = false;
	}


   

   
	private void OnDisable()
	{
		StopCoroutine("ActivateBulletTime");
	}
}