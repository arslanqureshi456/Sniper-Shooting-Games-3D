//WeaponPickup.cs by Azuline Studios© All Rights Reserved
//script for weapon pickups
using UnityEngine;
using System.Collections;

public class WeaponPickup : MonoBehaviour {

    public static WeaponPickup instance;

	private GameObject weaponObj;//the GameObject that is a child of FPS Weapons which has the WeaponBehavior script attatched
	private Transform myTransform;
	
	[Tooltip("The number that corresponds with this weapon's index in the PlayerWeapons.cs script weapon order array.")]
	public int weaponNumber = 0;
	private int weaponToDrop;//Weapon Order index of weapon to drop if dropCurrentWeapon is true
	[Tooltip("True if this pickup disappears when used/activated by player.")]
	public bool removeOnUse = true;
	[Tooltip("Remove weapon pickup after this time if it is greater than zero..")]
	public float removeTime;
	private float startTime;
	
	[Tooltip("Sound to play when picking up this weapon.")]
	public AudioClip pickupSound;
	[Tooltip("Sound to play when ammo is full and weapon cannot be used.")]
	public AudioClip fullSound;
	[Tooltip("If not null, this texture used for the pick up crosshair of this item.")]
	public Sprite weaponPickupReticle;

    
	
	void Start (){
        instance = this;
       // weaponNumber++;
        //Invoke("Delay", 0.2f);
        
        // if(Time.timeSinceLevelLoad > 5.5f)
        // {
        //    Destroy(gameObject,10);
        //    DestroyPickupButton();
        // }
            
    }
	void Delay()
    {
		try
		{
				myTransform = transform;//manually set transform for efficiency
										//find the PlayerWeapons script in the FPS Prefab to access weaponOrder array
				PlayerWeapons PlayerWeaponsComponent = null;
				
				//Camera.main.transform.GetComponent<CameraControl>().weaponObj.GetComponent<PlayerWeapons>();
				//PlayerWeapons PlayerWeaponsComponent = Camera.main.transform.GetComponent<CameraControl>().weaponObj.GetComponent<PlayerWeapons>();
				//iterate through the children of the FPS Weapons object (PlayerWeapon's weaponOrder array) and assign this item's weaponObj to the
				//weapon object whose weaponNumber in its WeaponBehavior script matches this item's weapon number

				weaponObj = PlayerWeaponsComponent.weaponOrder[weaponNumber];

				//      for (int i = 0; i < PlayerWeaponsComponent.weaponOrder.Length; i++)	{
				//	if(PlayerWeaponsComponent.weaponOrder[i].GetComponent<WeaponBehavior>().weaponNumber == weaponNumber){
				//		weaponObj = PlayerWeaponsComponent.weaponOrder[i];
				//		break;
				//	}
				//}

				if (PlayerWeaponsComponent.playerObj.activeInHierarchy && myTransform.GetComponent<Collider>())
				{
					Physics.IgnoreCollision(myTransform.GetComponent<Collider>(), PlayerWeaponsComponent.FPSPlayerComponent.FPSWalkerComponent.capsule, true);
				}
		}
        catch { }
    }
	public IEnumerator DestroyWeapon(float waitTime){
		startTime = Time.time;
		while (true){
			if(startTime + waitTime < Time.time){
				FreePooledObjects();
				Object.Destroy(gameObject);
				yield break;
			}
			yield return null;
		}
	}
	
	//return pooled objects back to object pool to prevent them from being destroyed when this object is destroyed after use
	private void FreePooledObjects(){
		FadeOutDecals[] decals = gameObject.GetComponentsInChildren<FadeOutDecals>(true);
		foreach (FadeOutDecals dec in decals) {
			dec.parentObjTransform.parent = AzuObjectPool.instance.transform;
			dec.parentObj.SetActive(false);
		}
		//drop arrows if object is destroyed
		ArrowObject[] arrows = gameObject.GetComponentsInChildren<ArrowObject>(true);
		foreach (ArrowObject arr in arrows) {
			arr.transform.parent = null;
			arr.myRigidbody.isKinematic = false;
			arr.myBoxCol.isTrigger = false;
			arr.gameObject.tag = "Usable";
			arr.falling = true;
		}
	}
   // private void OnTriggerEnter(Collider other)
   // {
   //     if(other.tag == "Player")
   //     {
			//if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.COVERSTRIKE)
			//	return;

   //         //PickUpItem();
   //         print("Gun To Be Pickup");
   //         if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
   //             GameManager.Instance.AssaultPickupBtn.SetActive(true);

   //         if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.MULTIPLAYER || LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.FREEFORALL || LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.BR)
   //         {
   //            //  MultiPlayerManager.Instance.MultiplayerPickupBtn.SetActive(true);
   //             //PickUpItem();
   //             print("Gun To Be Pickup In Multiplayer");
   //         }
   //     }
   // }

   // private void OnTriggerExit(Collider other)
   // {
   //     if (other.tag == "Player")
   //     {
   //         if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.COVERSTRIKE)
   //             return;

   //         if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
   //             GameManager.Instance.AssaultPickupBtn.SetActive(false);

   //         //if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.MULTIPLAYER || LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.FREEFORALL || LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.BR)
   //            // MultiPlayerManager.Instance.MultiplayerPickupBtn.SetActive(false);
   //     }
   // }

    public void PickUpItem (){
		try
		{
			weaponNumber = weaponNumber + 1;
			Delay();
			//find the PlayerWeapons script in the FPS Prefab to access weaponOrder array
			PlayerWeapons PlayerWeaponsComponent = Camera.main.transform.GetComponent<CameraControl>().weaponObj.GetComponent<PlayerWeapons>();
			WeaponBehavior WeaponBehaviorComponent = weaponObj.GetComponent<WeaponBehavior>();
			WeaponBehavior CurrentWeaponBehaviorComponent = PlayerWeaponsComponent.CurrentWeaponBehaviorComponent;
			//if player does not have this weapon, pick it up
			if (!WeaponBehaviorComponent.haveWeapon)
			{
				//if the player is at max weapons and this weapon takes up an inventory space, drop the current weapon before equipping this one
				if (PlayerWeaponsComponent.totalWeapons == PlayerWeaponsComponent.maxWeapons && WeaponBehaviorComponent.addsToTotalWeaps)
				{
					//determine if this weapon can be dropped (weapons like fists or a sidearm can be set to not be droppable)
					if (CurrentWeaponBehaviorComponent.droppable)
					{
						//drop current weapon if dropCurrentWeapon is true
						if (removeOnUse && !WeaponBehaviorComponent.dropWillDupe && !CurrentWeaponBehaviorComponent.dropWillDupe)
						{
							PlayerWeaponsComponent.DropWeapon(PlayerWeaponsComponent.currentWeapon);
						}
						else
						{//replace current weapon if dropCurrentWeapon is false
							CurrentWeaponBehaviorComponent.haveWeapon = false;
							CurrentWeaponBehaviorComponent.dropWillDupe = false;
							//prevent dropping this weapon and creating duplicated ammo by picking up the weapon again from the non-destroyable pickup item
							WeaponBehaviorComponent.dropWillDupe = true;
						}

					}
					else
					{//currently held weapon is not dropable, so find next item in the weaponOrder array that is droppable, and drop it
						for (int i = PlayerWeaponsComponent.currentWeapon; i < PlayerWeaponsComponent.weaponOrder.Length; i++)
						{
							if (PlayerWeaponsComponent.weaponOrder[i].GetComponent<WeaponBehavior>().haveWeapon
							&& PlayerWeaponsComponent.weaponOrder[i].GetComponent<WeaponBehavior>().droppable)
							{
								weaponToDrop = i;//found the weapon to drop, break loop
								break;
								//no weapon found to drop counting up from current weapon number in weaponOrder array, so start at zero and count upwards
							}
							else if (i == PlayerWeaponsComponent.weaponOrder.Length - 1)
							{
								for (int n = 0; n < PlayerWeaponsComponent.weaponOrder.Length; n++)
								{
									if (PlayerWeaponsComponent.weaponOrder[n].GetComponent<WeaponBehavior>().haveWeapon
									&& PlayerWeaponsComponent.weaponOrder[n].GetComponent<WeaponBehavior>().droppable)
									{
										weaponToDrop = n;
										break;//found the weapon to drop, break loop
									}
								}
							}
						}

						if (removeOnUse && !WeaponBehaviorComponent.dropWillDupe)
						{//drop the next weapon if dropCurrentWeapon is true and current weapon is not droppable
							PlayerWeaponsComponent.DropWeapon(weaponToDrop);
						}
						else
						{//replace the next weapon if dropCurrentWeapon is false and current weapon is not droppable
							PlayerWeaponsComponent.weaponOrder[weaponToDrop].GetComponent<WeaponBehavior>().haveWeapon = false;
							PlayerWeaponsComponent.weaponOrder[weaponToDrop].GetComponent<WeaponBehavior>().dropWillDupe = false;
							//prevent dropping this weapon and creating duplicated ammo by picking up the weapon again from the non-destroyable pickup item
							WeaponBehaviorComponent.dropWillDupe = true;
						}

					}
				}
				//update haveWeapon value of new weapon's WeaponBehavior.cs component
				WeaponBehaviorComponent.haveWeapon = true;
				if (!removeOnUse)
				{
					WeaponBehaviorComponent.dropWillDupe = true;
				}
				else
				{
					WeaponBehaviorComponent.dropWillDupe = false;
				}
				//select the weapon after picking it up
				WeaponBehaviorComponent.ammo = 999;
				PlayerWeaponsComponent.StartCoroutine(PlayerWeaponsComponent.SelectWeapon(WeaponBehaviorComponent.weaponNumber));
				
				PlayerPrefs.SetInt("selectedWeaponIndex", WeaponBehaviorComponent.weaponNumber - 1);
				//update the total weapon amount in PlayerWeapons.cs
				PlayerWeaponsComponent.UpdateTotalWeapons();
				//remove pickup item from game world and play sound
				RemovePickup();

			}
			else
			{//the player already has this weapon, so give them ammo instead
				if ((WeaponBehaviorComponent.ammo < WeaponBehaviorComponent.maxAmmo && removeOnUse)//player is not carrying max ammo for this weapon 
				&& WeaponBehaviorComponent.meleeSwingDelay == 0)
				{//player is not trying to pick up a melee weapon they already have

					if (WeaponBehaviorComponent.ammo + WeaponBehaviorComponent.bulletsPerClip > WeaponBehaviorComponent.maxAmmo)
					{
						//just give player max ammo if they only are a few bullets away from having max ammo
						WeaponBehaviorComponent.ammo = WeaponBehaviorComponent.maxAmmo;
					}
					else
					{
						//give player the bulletsPerClip amount if they already have this weapon
						WeaponBehaviorComponent.ammo += WeaponBehaviorComponent.bulletsPerClip;
					}

					RemovePickup();//remove pickup item from game world and play sound

				}
				else
				{
					//if player has weapon and is at max ammo, just play beep sound
					if (fullSound) { PlayAudioAtPos.PlayClipAt(fullSound, myTransform.position, 2.0f); }
				}
				RemovePickup();
				if (PlayerWeaponsComponent.currentWeapon == WeaponBehaviorComponent.weaponNumber)
					return;
				PlayerWeaponsComponent.StartCoroutine(PlayerWeaponsComponent.SelectWeapon(WeaponBehaviorComponent.weaponNumber));
				
				PlayerPrefs.SetInt("selectedWeaponIndex", WeaponBehaviorComponent.weaponNumber - 1);

				//update the total weapon amount in PlayerWeapons.cs
				PlayerWeaponsComponent.UpdateTotalWeapons();
			}
		}
        catch { }
	}
	
	void RemovePickup (){
		
		//play pickup sound
		if(pickupSound){PlayAudioAtPos.PlayClipAt(pickupSound, myTransform.position, 2.0f);}
		
		if(removeOnUse){
			FreePooledObjects();
			//remove this weapon pickup from the scene
			Object.Destroy(gameObject);
		}
		
	}


}