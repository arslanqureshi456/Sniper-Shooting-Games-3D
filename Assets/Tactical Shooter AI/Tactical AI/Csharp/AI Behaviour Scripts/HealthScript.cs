using UnityEngine;
using System.Collections;

/*
 * Manages the agent's health. 
 * Will trigger the suppresion state on agents using cover if shields are down.
  */

namespace TacticalAI
{
	public class HealthScript : MonoBehaviour
	{
		bool isAllied = false;
		bool dead = false;
		GameObject BloodParticle;
		public int teamID;
		public int statIndex;
		public string nameEnemies;
		public int kills;
		public int death;
		public int score;
		public int gunID;

		public float health = 100;
		public float shields = 25;
		private float maxShields = 10;
		public bool shieldsBlockDamage = false;
		public float timeBeforeShieldRegen = 5;
		private float currentTimeBeforeShieldRegen;
		public float shieldRegenRate = 10;
		public TacticalAI.TargetScript myTargetScript;
		public TacticalAI.BaseScript myAIBaseScript;
		public TacticalAI.SoundScript soundScript;

		public DropWeapon dropWeapon;
		//public bool infiniteDecal = false;
		//public GameObject bloodFX;
		//public GameObject bloodAttach;

		public Rigidbody[] rigidbodies;
		public Collider[] collidersToEnable;
		public TacticalAI.RotateToAimGunScript rotateToAimGunScript;
		public Animator animator;

		public TacticalAI.GunScript gunScript;

		private bool beenHitYetThisFrame = false;
		private RaycastHit _hit;
		public Spawner spawner = null;
		//Initiation stuff.
		void Awake()
		{
			if (transform.name.Contains("Ally"))
				isAllied = true;
			soundScript = gameObject.GetComponent<TacticalAI.SoundScript>();
			if (shields < 0)
			{
				shields = 0.1f;
			}
			maxShields = shields;
			if (!isAllied && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Game") //if(LevelSelectionNew.modeSelection == 1)
				BloodParticle = transform.GetChild(0).GetChild(2).gameObject;
		}
		void Update()
		{
			currentTimeBeforeShieldRegen -= Time.deltaTime;
			timeTillNextStagger -= Time.deltaTime;

			//Only let us take explosion damage once per frame. (could also be used for weapons that would pass through an agent's body)
			//This will prevent the agent from taking the damage multiple times- once for each hitbox.
			beenHitYetThisFrame = false;


			if (currentTimeBeforeShieldRegen < 0 && shields < maxShields)
			{
				shields = Mathf.Clamp(shields + shieldRegenRate * Time.deltaTime, 0, maxShields);

				//When our shields are fully charged, stop being suppressed.
				if (shields == maxShields)
				{
					myAIBaseScript.ShouldFireFromCover(true);
				}
			}
		}

		public void ShowKillCam()
		{
			GetComponent<ComponentNotifier>().cameraObject.SetActive(true);
		}


		public void Damage(float damage, RaycastHit hit)
		{
			if (dead)
				return;
			if (isAllied && !myAIBaseScript.canEngage)
			{
				myAIBaseScript.canEngage = true;
				myAIBaseScript.Retaliate();
				//
				//myTargetScript.maxDistToNoticeTarget = 9999;
				//myAIBaseScript.isStatic = true;
			}
			
			_hit = hit;

			//Look for the source of the damage.
			if (myTargetScript)
				myTargetScript.CheckForLOSAwareness(true);

			ReduceHealthAndShields(damage);
			myAIBaseScript.CheckToSeeIfWeShouldDodge();

			if (health <= 0)
			{
				//ABDUL
					if (dropWeapon)
						dropWeapon.enabled = true;
					//InstantiateBloodFX(hit);
				DeathCheck();
			}
		}

		bool once = true;
		public void Damage(float damage)
		{
			if (dead)
				return;
			
				//Look for the source of the damage.
				if (myTargetScript)
					myTargetScript.CheckForLOSAwareness(true);

				ReduceHealthAndShields(damage);
				myAIBaseScript.CheckToSeeIfWeShouldDodge();

				if (health <= 0)
				{
					DeathCheck();
				}
		}

		public void Damage(HealthScript healthScript, float damage, int teamID1, int statIndex1, string nameEnemies1, int kills1, int death1, int score1, int gunID1)
		{
			if (dead)
				return;
		}

		public IEnumerator SingleHitBoxDamage(float damage)
		{
			if (dead)
				yield break;
			//Look for the source of the damage.
			if (myTargetScript)
				myTargetScript.CheckForLOSAwareness(true);

			//Only let us take explosion damage once per frame. (could also be used for weapons that would pass through an agent's body)
			//This will prevent the agent from taking the damage multiple times- once for each hitbox.
			if (!beenHitYetThisFrame)
			{
				ReduceHealthAndShields(damage);

				if (health <= 0)
				{
					DeathCheck();
				}
				beenHitYetThisFrame = true;
			}

			yield return null;
			beenHitYetThisFrame = false;
		}


		public void ReduceHealthAndShields(float damage)
		{
			//Shields are mandatory for the suppressioon mechanic to work.
			//However, as you may not want the agent to have any sort of regenerating health, you can choose whether or not they will actually block damage or merely work as a recent damage counter.
			if (shieldsBlockDamage)
			{
				if (damage > shields)
				{
					if (soundScript && myAIBaseScript.HaveCover() && shields > 0)
						soundScript.PlaySuppressedAudio();

					//Eliminate shields and pass on remaining damage to health.
					damage -= shields;
					shields = 0;
					health -= damage;

					//If the agent's shields go down, become suppressed (ie: agent will stay in cover as much as possible, and will avoid standing up to fire)
					myAIBaseScript.ShouldFireFromCover(false);
				}
				else
				{
					shields -= damage;
				}
			}
			else
			{
				if (damage > shields)
				{
					if (soundScript && myAIBaseScript.HaveCover() && shields > 0)
						soundScript.PlaySuppressedAudio();

					//If the agent's shields go down, become suppressed (ie: agent will stay in cover as much as possible, and will avoid standing up to fire)
					myAIBaseScript.ShouldFireFromCover(false);
				}

				//Do the same amount of damage to shields AND health.
				shields = Mathf.Max(shields - damage, 0);
#if UNITY_EDITOR
                print("here");
#endif
				health -= damage;
			}

			currentTimeBeforeShieldRegen = timeBeforeShieldRegen;

			//Sound
			if (soundScript)
				soundScript.PlayDamagedAudio();

			if (health > 0 && damage > staggerThreshhold && canStagger && Random.value < staggerOdds && timeTillNextStagger < 0)
			{
				myAIBaseScript.StaggerAgent();
				timeTillNextStagger = timeBetweenNextStaggers;
			}
		}

		public float staggerThreshhold = 1.0f;
		public bool canStagger = false;
		public float staggerOdds = 0.5f;
		private float timeTillNextStagger = 1.0f;
		public float timeBetweenNextStaggers = 1.0f;

		//Check to see if we are dead.
		void DeathCheck()
		{

					if (GetComponent<AudioSource>().isActiveAndEnabled)
					{
						GetComponent<AudioSource>().volume = 0.6f;
						GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.EnemyDeathSound[Random.Range(0, AudioManager.instance.EnemyDeathSound.Length)]);
					}
					if (isAllied)
						GameManager.Instance.ForceLevelFail();
			KillAI();
			if (myAIBaseScript)
				myAIBaseScript.KillAI();
			this.enabled = false;
		}
		public bool useDeathAnimation = false;

		void KillAI()
		{
			if (this.enabled)
			{
				dead = true;
				int i;
				//Enable the ragdoll
				for (i = 0; i < rigidbodies.Length; i++)
				{
					rigidbodies[i].isKinematic = false;
				}

				for (i = 0; i < collidersToEnable.Length; i++)
				{
					collidersToEnable[i].enabled = true;
				}

				if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
					_hit.rigidbody.AddForce(-transform.forward * 50f, ForceMode.Impulse);

				if (spawner != null)
					spawner.UnregisterSpawnedNPC(this);
				//        if(GetComponent<HealthBar>())
				//GetComponent<HealthBar>().OnDeath();
				//ABDUL
				//if (!MultiPlayerManager.Instance && !isAllied)
					GameManager.Instance.OnEnemyKilled();
				if (BloodParticle && PlayerPrefs.GetInt("BloodEffect") == 1)
					BloodParticle.SetActive(true);

				//Disable scripts
				if (rotateToAimGunScript)
					rotateToAimGunScript.enabled = false;

				if (animator && !useDeathAnimation)
				{
					animator.enabled = false;
				}
				else
				{
					gameObject.SendMessage("PlayDeathAnimation", SendMessageOptions.DontRequireReceiver);
				}

				if (gunScript)
				{
					gunScript.enabled = false;
				}

				this.enabled = false;
			}

			//ABDUL
			//public void InstantiateBloodFX(RaycastHit hit)
			//{
			//	var direction = hit.normal;
			//	float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 180;

			//          if (SystemInfo.deviceModel.Contains("SAMSUNG") || SystemInfo.deviceModel.Contains("samsung") 
			//              || SystemInfo.deviceModel.Contains("HUAWEI") || SystemInfo.deviceModel.Contains("huawei"))
			//          {
			//              if (SystemInfo.systemMemorySize >= 3000)
			//              {
			//                  var instance = Instantiate(bloodFX, hit.point, Quaternion.Euler(0, angle + 90, 0));

			//                  var settings = instance.GetComponent<BFX_BloodSettings>();
			//                  settings.FreezeDecalDisappearance = infiniteDecal;
			//                  //settings.LightIntensityMultiplier = directionLight.intensity;

			//                  if (!infiniteDecal)
			//                      Destroy(instance, 20);
			//              }
			//          }

			//          if (SystemInfo.deviceModel.Contains("SAMSUNG") || SystemInfo.deviceModel.Contains("samsung")
			//             || SystemInfo.deviceModel.Contains("HUAWEI") || SystemInfo.deviceModel.Contains("huawei"))
			//          {
			//              if (SystemInfo.systemMemorySize > 2500)
			//              {
			//                  var nearestBone = GetNearestObject(hit.transform.root, hit.point);
			//                  if (nearestBone != null)
			//                  {
			//                      var attachBloodInstance = Instantiate(bloodAttach);
			//                      var bloodT = attachBloodInstance.transform;
			//                      bloodT.position = hit.point;
			//                      bloodT.localRotation = Quaternion.identity;
			//                      bloodT.localScale = Vector3.one * Random.Range(0.75f, 1.2f);
			//                      bloodT.LookAt(hit.point + hit.normal, direction);
			//                      bloodT.Rotate(90, 0, 0);
			//                      bloodT.transform.parent = nearestBone;
			//                  }
			//              }
			//          }

			//}

			//private Transform GetNearestObject(Transform hit, Vector3 hitPos)
			//{
			//	var closestPos = 100f;
			//	Transform closestBone = null;
			//	var childs = hit.GetComponentsInChildren<Transform>();

			//	foreach (var child in childs)
			//	{
			//		var dist = Vector3.Distance(child.position, hitPos);
			//		if (dist < closestPos)
			//		{
			//			closestPos = dist;
			//			closestBone = child;
			//		}
			//	}

			//	var distRoot = Vector3.Distance(hit.position, hitPos);
			//	if (distRoot < closestPos)
			//	{
			//		closestPos = distRoot;
			//		closestBone = hit;
			//	}
			//	return closestBone;
			//}
		}
	}
}
