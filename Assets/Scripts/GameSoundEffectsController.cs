using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundEffectsController : MonoBehaviour
{
    [SerializeField]
    private AudioClip DieRolled;

    [SerializeField]
    private AudioClip BondMade;

    [SerializeField]
    private AudioClip BondDestroyed;

    private AudioSource GameSoundEffects;

    public static GameSoundEffectsController Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        GameSoundEffects = GetComponent<AudioSource>();

        // Singleton design pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Roll()
    {
        GameSoundEffects.clip = DieRolled;
        GameSoundEffects.Play();
    }

    public void BondFormed()
    {
        GameSoundEffects.clip = BondMade;
        GameSoundEffects.Play();
    }

    public void Unbonding()
    {
        GameSoundEffects.clip = BondDestroyed;
        GameSoundEffects.Play();
    }
}
