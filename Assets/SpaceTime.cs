using UnityEngine;

public class SpaceTime
{
	public Vector3 position;
	public Quaternion rotation;
	public Vector3 velocity;
	
	
	public SpaceTime(Vector3 vel, Vector3 pos, Quaternion rot)
	{
		position = pos;
		velocity = vel;
		rotation = rot;
	}
}
