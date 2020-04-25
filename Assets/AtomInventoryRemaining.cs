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
			names.Add(piece.name + "(Clone)");
		}
	}
	
	public static int removePiece(GameObject piece, bool removeFromInventory)
	{
		int count = 1;
		if(piece.tag == "Diatomic")
		{
			count = 2;
			piece = correspondingPieces[Array.IndexOf(diatomics, piece)];
		}
		if(Array.IndexOf(pieces, piece) == -1)
		{
			return 2;
		}
		if(amounts[Array.IndexOf(pieces, piece)] - count >= 0)
		{
			if(removeFromInventory)
			{
				amounts[Array.IndexOf(pieces, piece)] -= count;
				Debug.Log("Removed " + count + " " + piece.name + " from inventory.");
			}
			return 1;
		}
		else
		{
			return 0;
		}
	}
	
	public static string pieceToName(GameObject piece)
	{
		if(piece.name == "OxLinearE(Clone)")
		{
			piece.name = "OxygenE(Clone)";
		}
		return piece.name;
	}
	
	public static void addPiece(string name)
	{
		Debug.Log("Added 1 " + name.Remove(name.Length - 7, 7) + " to inventory.");
		amounts[names.IndexOf(name)]++;
	}
}
