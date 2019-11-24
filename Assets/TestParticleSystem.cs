using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParticleSystem : MonoBehaviour {
	[SerializeField]
	List<Rigidbody2D> particles = new List<Rigidbody2D>();
	List<Vector2> SearchedPositon = new List<Vector2>();
	[SerializeField]
	private int particleAmount;
	[SerializeField]
	private FlowField flowField;
	[SerializeField]
	private GameObject particle;


	[SerializeField]
	private float particleSpeed;
	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0))
		{
			SpawnParticle();
		}
	}

	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	Vector3 flowFieldVector;
	void FixedUpdate()
	{
		float dt = Time.fixedDeltaTime;
		for (int i = 0; i < particles.Count; i++)
		{
			flowFieldVector = flowField.GetVector(particles[i].position);
			// print("FlowfieldVector: " +flowFieldVector);
			particles[i].AddForce(flowFieldVector * dt * particleSpeed);
		}
	}

	void SpawnParticle(){
		Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(flowField.GetQuadtree().boundary.contains(mouse))
		    particles.Add(Instantiate(particle, mouse, Quaternion.identity).GetComponent<Rigidbody2D>());
	}

	void OnGUI()
	{
		Rect r = new Rect(100,100,250,250);
		string s = "Amount of particles";
		s += particles.Count.ToString();
		GUI.Label(r,s);
	}
	void OnDrawGizmos()
	{
		// foreach(Rigidbody2D part in particles)
		// {
		// 	Gizmos.DrawCube(part.position,Vector2.one * 0.3f);
		// }
		// QuadTree.Draw(flowField.GetQuadtree());
	}
}
