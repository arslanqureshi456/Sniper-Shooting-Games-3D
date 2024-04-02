using UnityEngine;
using System.Collections;

public class KillCam : MonoBehaviour
{

    /// <summary>
    /// Target to follow
    /// </summary>
    public Transform target = null;
    /// <summary>
    /// Distance from camera to target
    /// </summary>
	public float distance = 10.0f;
    /// <summary>
    /// Maxime Distance to target
    /// </summary>
    public float distanceMax = 15f;
    /// <summary>
    /// Min Distance to target
    /// </summary>
	public float distanceMin = 0.5f;
    /// <summary>
    /// X vector speed
    /// </summary>
	public float xSpeed = 120f;
    /// <summary>
    /// maxime y vector Limit
    /// </summary>
	public float yMaxLimit = 80f;
    /// <summary>
    /// minime Y vector limit
    /// </summary>
	public float yMinLimit = -20f;
    /// <summary>
    /// Y vector speed
    /// </summary>
	public float ySpeed = 120f;

    float x = 0;
    float y = 0;
    private int CurrentTarget = 0;
    private bool canManipulate = false;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        transform.parent = null;
        //if (target != null)
        //{
        //    transform.LookAt(target);
        //    StartCoroutine(ZoomOut());
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    //public void Update()
    //{
    //    if (target != null)
    //    {
    //        Orbit();
    //    }
    //}

    void Orbit()
    {
        if (!canManipulate)
            return;

        if (target != null)
        {
            x += ((ControlFreak2.CF2Input.GetAxis("Mouse X") * this.xSpeed) * this.distance) * 0.02f;
            y -= (ControlFreak2.CF2Input.GetAxis("Mouse Y") * this.ySpeed) * 0.02f;
            Quaternion quaternion = Quaternion.Euler(this.y, this.x, 0f);
            this.distance = Mathf.Clamp(this.distance - (ControlFreak2.CF2Input.GetAxis("Mouse ScrollWheel") * 5f), distanceMin, distanceMax);

            Vector3 vector = new Vector3(0f, 0f, -distance);
            Vector3 vector2 = target.position;
            vector2.y = target.position.y + 1f;
            Vector3 vector3 = (quaternion * vector) + vector2;
            transform.rotation = quaternion;
            transform.position = vector3;
        }

    }

    IEnumerator ZoomOut()
    {
        float d = 0;
        Vector3 next = target.position + transform.TransformDirection(new Vector3(0, 0, -3));
        Vector3 origin = target.position;
        transform.position = target.position;
        while (d < 1)
        {
            d += Time.deltaTime;
            transform.position = Vector3.Lerp(origin, next, d);
            transform.LookAt(target);
            yield return null;
        }
    }
}