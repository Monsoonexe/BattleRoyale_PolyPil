using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	public Transform target;

	void Start()
	{
		target = GameObject.FindGameObjectWithTag ("MainCamera").transform;
	}

	void Update()
	{
		this.transform.LookAt (new Vector3(target.position.x, this.transform.position.y, target.position.z));
	}
}