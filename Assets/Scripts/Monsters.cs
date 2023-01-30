using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Monsters : MonoBehaviour
{
    public GameObject monster;
    private Animator anim;

    private Vector3 mouseReleasePos;
    private float forceMultiplier = 3f;
    private Camera cameraMain;

    private void Start()
    {
        anim = GetComponent<Animator>();
        cameraMain = Camera.main;
    }

    private void OnEnable()
    {
        DragAndShot.Throwing += Throw;
        DragAndShot.Swing += Swing;
        DragAndShot.InSwing += InSwing;
        DragAndShot.InSwing += OutSwing;
    }

    private void OnDisable()
    {
        DragAndShot.Throwing -= Throw;
        DragAndShot.Swing -= Swing;
        DragAndShot.InSwing -= InSwing;
        DragAndShot.InSwing -= OutSwing;
    }

    public void InSwing()
    {
        anim.SetTrigger("InSwing");
    }

    public void OutSwing()
    {
        anim.SetTrigger("OutSwing");
    }

    public void Throw()
    {
        anim.SetTrigger("Throwing");
    }

    public void Swing()
    {
        anim.SetTrigger("Swing");
    }
}
