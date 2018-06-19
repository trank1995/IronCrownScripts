using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDataBase : MonoBehaviour {
    public static AudioDataBase database;
    public AudioClip test;
    public AudioClip gunShot;
    public AudioClip swordHit;
    public AudioClip arrowFire;
    public AudioClip block;
    public AudioClip holdSteady;
    public AudioClip button;
    public AudioClip cavalry;
    public AudioClip whirlwind;
    public AudioClip enguard;
    public AudioClip execute;
    public AudioClip evade;
    public AudioClip footstep;
    public AudioClip horsestep;
    public AudioClip hurt;
    public AudioClip death;



    public List<AudioClip> mercTaunt;
    public List<AudioClip> mercQuickDraw;
    // Use this for initialization
    void Start () {
        database = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public AudioClip getTaunt(Faction f)
    {
        AudioClip result = test;
        switch (f)
        {
            case Faction.mercenary:
                result = mercTaunt[Random.Range(0, mercTaunt.Count - 1)];
                break;
        }
        return result;
    }
    
}
