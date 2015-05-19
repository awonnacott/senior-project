using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public float speed;
	public float tilt;
	public Boundary boundary;
	private GameObject target;
	private Rigidbody selfRigidbody;

	float offsetx = 0;
	
	public GameObject shot;
	public Transform shotSpawn;

	public float fireRate = 2;
	private float nextFire = 0;
	
	void Start() {
		target = GameObject.FindWithTag ("Player");
		selfRigidbody = GetComponent <Rigidbody> ();
		offsetx = transform.position.x;
	}
	
	void Update () {
		if (transform.position.z < 17.5 && target.gameObject && Mathf.Abs(gameObject.transform.position.x - target.transform.position.x) < 1 && Time.time > nextFire) {
			Instantiate (shot, shotSpawn.position, Quaternion.identity);
			nextFire = Time.time + fireRate;
		}
	}
	
	// FixedUpdate is called once per physics
	void FixedUpdate () {
		selfRigidbody.MovePosition(new Vector3(Mathf.PingPong (Time.time * speed + offsetx, boundary.xMax - boundary.xMin) + boundary.xMin, transform.position.y, transform.position.z));
		selfRigidbody.velocity = Vector3.forward * -speed;
	}
}
