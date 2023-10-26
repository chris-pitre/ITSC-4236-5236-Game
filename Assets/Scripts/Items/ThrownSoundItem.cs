using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundComponent))]
public class ThrownSoundItem : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SoundComponent sound;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    private Vector2 initialVelocity;
    public void Throw(Vector2 target, float speed){
        rb.velocity = target * speed;
        initialVelocity = rb.velocity;
        Debug.Log(rb.velocity);
        StartCoroutine(DoDrag());
    }

    private IEnumerator DoDrag(){
        float lerpTime = 1f;
        float currentLerpTime = 0f;
        float timer = 0f;
        while(currentLerpTime < lerpTime){
            currentLerpTime += Time.deltaTime;

            timer = currentLerpTime / lerpTime;
            timer = 1f - Mathf.Cos(timer * Mathf.PI * 0.5f);
            rb.velocity = Vector2.Lerp(initialVelocity, Vector2.zero, timer);

            yield return null;
        }
        sound.MakeSoundImpulse(1f, 0.5f);
    }
}