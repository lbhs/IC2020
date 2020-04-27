using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DiatomicScript : MonoBehaviour
{
    public GameObject DissociationProduct;
    public int BondDissociationEnergy;

    public Collider2D DragCollider;
    public List<GameObject> Overlapping; // Database of all overlapping elements

    // Start is called before the first frame update
    void Start()
    {
        Overlapping = new List<GameObject>();

        if (GetComponent<CompositeCollider2D>() != null)
            DragCollider = GetComponent<CompositeCollider2D>();
        else if (GetComponent<CapsuleCollider2D>() != null)
            DragCollider = GetComponent<CapsuleCollider2D>();
        else
            DragCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // overlap is different from Overlapping -- overlap contains colliders, not GameObjects!
        Collider2D[] overlap = Physics2D.OverlapAreaAll(DragCollider.bounds.min, DragCollider.bounds.max);

        // Purge GameObjects that are no longer overlapping, but are still in the Overlapping list
        for (int i = 0; i < Overlapping.Count; i++)
        {
            if (Overlapping[i].GetComponent<BondMaker>() != null)
            {
                if (!overlap.Contains(Overlapping[i].GetComponent<BondMaker>().DragCollider))
                {
                    Overlapping.Remove(Overlapping[i]);
                }
            }
            else
            {
                if (!overlap.Contains(Overlapping[i].GetComponent<DiatomicScript>().DragCollider))
                {
                    Overlapping.Remove(Overlapping[i]);
                }
            }
        }

        foreach (GameObject GO in Object.FindObjectsOfType<GameObject>())
        {
            if (GO.activeInHierarchy)
            {
                if ((GO.GetComponent<BondMaker>() != null
                    || GO.GetComponent<DiatomicScript>() != null)
                    && GO != gameObject
                    && !Overlapping.Contains(GO))
                {

                    if (GO.GetComponent<BondMaker>() != null)
                    {
                        if (overlap.Contains(GO.GetComponent<BondMaker>().DragCollider))
                        {
                            Overlapping.Add(GO);
                        }
                    }
                    else
                    {
                        if (overlap.Contains(GO.GetComponent<DiatomicScript>().DragCollider))
                        {
                            Overlapping.Add(GO);
                        }
                    }
                }
            }
        }

        if (Overlapping.Count == 0)
        {
            ChangeLayer(0);
        }
        else
        {
            ChangeLayer(10);
        }
    }

    private void ChangeLayer(int LayerID)
    {
        gameObject.layer = LayerID;
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerID;
        }
    }
}
