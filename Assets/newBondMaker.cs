using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newBondMaker : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
		GameObject go = col.rigidbody.gameObject;
		GameObject othergo = col.otherRigidbody.gameObject;
		bondlattice lattice1 = go.GetComponent<DragIt>().getParent().gameObject.GetComponent<bondlattice>();
		bondlattice lattice2 = othergo.GetComponent<DragIt>().getParent().gameObject.GetComponent<bondlattice>();
		
        if(lattice1 == null && lattice2 == null)
		{
			go.AddComponent<bondlattice>().bondablelattice.SetValue(true, 3, 3);
			//need to check if adding objects succeeded, just assuming for now
			go.GetComponent<bondlattice>().addObject(go, new int[]{3, 3});
			go.GetComponent<bondlattice>().addObject(othergo, go.GetComponent<bondlattice>().findNearestLatticeLocation(othergo));
		}
		else if(lattice1 != null && lattice2 == null)
		{
			//need to check if adding objects succeeded, just assuming for now
			lattice1.addObject(othergo, lattice1.findNearestLatticeLocation(othergo));
		}
    }
}
