using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class bondlattice : MonoBehaviour
{
	public GameObject[,] objectlattice = new GameObject[7, 7];
	public bool[,] bondablelattice = new bool[7,7];
	
	public bool addObject(GameObject obj, int[] coords)
	{
		int x = coords[0];
		int y = coords[1];
		if(!bondablelattice[x,y])
		{
			return false;
		} else {
			objectlattice[x,y] = obj;
			obj.transform.SetParent(gameObject.transform, true);
			obj.transform.position = gameObject.transform.position + new Vector3(x - 3, y - 3) * 4.5f;
			bondablelattice[x,y] = false;
			for(int dx = -1; dx <= 1; ++dx) {
				for(int dy = -1; dy <= 1; ++dy) {
					if(Math.Abs(dx) != Math.Abs(dy)) {
						if(objectlattice[x + dx,y + dy] == null)
						{
							bondablelattice.SetValue(true, x + dx, y + dy);
						}
					}
				}
			}
			return true;
		}
	}
	
	public int[] findNearestLatticeLocation(GameObject obj)
	{
		float mindistance = 1000000f;
		int minx = -1;
		int miny = -1;
		for(int x = 0; x < 7; x++)
		{
			for(int y = 0; y < 7; y++)
			{
				Vector2 pos = gameObject.transform.position + new Vector3(x - 3, y - 3) * 4.5f;
				float distance = Vector2.Distance(pos, obj.transform.position);
				if(distance < mindistance)
				{
					mindistance = distance;
					minx = x;
					miny = y;
				}
			}
		}
		return new int[] {minx, miny};
	}
	
	void removeObject()
	{
		//add new bondable positions
		//remove old bondable positions
	}
}
