using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AtomInventoryRemaining : MonoBehaviour
{
	public GameObject[] piecesInitial;
	public int[] amountsInitial;
	public GameObject[] diatomicsInitial;
	public GameObject[] correspondingPiecesInitial;
	
	private static List<string> names = new List<string>();
	private static GameObject[] pieces;
	private static int[] amounts;
	private static GameObject[] diatomics;
	private static GameObject[] correspondingPieces;
	
	void Start()
	{
		pieces = piecesInitial;  
		amounts = amountsInitial;
		diatomics = diatomicsInitial;
		correspondingPieces = correspondingPiecesInitial;
		
		foreach(GameObject piece in pieces)
		{
			names.Add(piece.name);
		}
	}
	
	public static bool removePiece(GameObject piece, bool removeFromInventory)
	{
		int count = 1;
		if(piece.tag == "Diatomic")
		{
			count = 2;  //a diatomic counts as two individual atoms of this type
			piece = correspondingPieces[Array.IndexOf(diatomics, piece)];
		}
        string correspondingpiece = pieceToName(piece);
		if(correspondingpiece.Substring(Math.Max(0, correspondingpiece.Length - 7)) == "(Clone)")
		{
			correspondingpiece = correspondingpiece.Remove(correspondingpiece.Length - 7, 7);
		}
		if(amounts[names.IndexOf(correspondingpiece)] - count >= 0)   //inventory is available, 
		{
			if(removeFromInventory)
			{
				amounts[names.IndexOf(correspondingpiece)] -= count;
				Debug.Log("Removed " + count + " " + piece.name + " from inventory.");
            }
			return true;  //yes, able to remove (or swap) atom forms
		}
		else
		{
            return false;  //this means that no more OxygenEB pieces are available--only swaps linear and bent Oxygens  
            //SEQUENCE MUST BE OxygenEB links to OxygenE which links to OxygenELinear
        }
    }
	
	public static string pieceToName(GameObject piece)
	{
		if(piece.name == "OxygenELinear(Clone)" || piece.name == "OxygenEB(Clone)" || piece.name == "OxygenELinear" || piece.name == "OxygenEB")
		{
			return "OxygenE";
		}
		if(piece.name == "CarbonEsp2(Clone)" || piece.name == "CarbonEsp2")
		{
			return "CarbonE";
		}
		return piece.name;
	}
	
	public static void addPiece(string name)
	{
		Debug.Log("Added 1 " + name.Remove(name.Length - 7, 7) + " to inventory.");
		amounts[names.IndexOf(name)]++;
	}
}
