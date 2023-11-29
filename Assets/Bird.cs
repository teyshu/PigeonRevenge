using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] AudioSource _as;
    [SerializeField] AudioClip _bratia;
    [SerializeField] AudioClip _fatality;
    [SerializeField] AudioClip _heal;
    [SerializeField] AudioClip _attack;

    public void CheckSoundHeal()
    {
        if(_as.isPlaying)
            _as.Stop();
        else
        {
            _as.clip = _heal;
            _as.Play();
        }
    }

    public void CheckSoundBratia()
    {
        if (_as.isPlaying)
            _as.Stop();
        else
        {
            _as.clip = _bratia;
            _as.Play();
        }
    }

    public void CheckSoundFatality()
    {
        if (_as.isPlaying)
            _as.Stop();
        else
        {
            _as.clip = _fatality;
            _as.Play();
        }
    }

    public void CheckSoundAttack()
    {
        if (_as.isPlaying)
            _as.Stop();
        else
        {
            _as.clip = _attack;
            _as.Play();
        }
    }
}
