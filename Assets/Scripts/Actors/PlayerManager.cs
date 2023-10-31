using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MovementComponent movement;
    [SerializeField] private VisionComponent vision;
    [SerializeField] private InventoryComponent inventory;
    [SerializeField] private Animator animator;

    [Header("References")]
    [SerializeField] private Transform itemSpawn;

    private float throwTimer = 0f;

    private bool isThrowing = false;

    private void Update(){
        ThrowCurrentItem();
    }

    private void FixedUpdate(){
        Movement();
        Vision();
    }

    private void Movement(){
        Vector2 input;
        float x_input = Input.GetAxisRaw("Horizontal");
        float y_input = Input.GetAxisRaw("Vertical");
        bool isWalking = Input.GetKey(KeyCode.LeftShift);
        if(isThrowing){
            input = Vector2.zero; 
        } else {
            input = new Vector2(x_input, y_input);
        }

        if(input == Vector2.zero){
            animator.SetBool("moving", false);
        } else {
            animator.SetBool("moving", true);
        }

        movement.DoMovement(input, isWalking);
    }

    private void Vision(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vision.LookAt(mousePos);

        float angle = vision.getDirectionAngle();
        angle -= 90;
        Vector2 angleVector = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
        animator.SetFloat("moveX", angleVector.x);
        animator.SetFloat("moveY", angleVector.y);
    }
    
    private void ThrowCurrentItem(){
        if(inventory.GetItem(0).ammo <= 0){
            return;
        }
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            isThrowing = true;
        }
        if(Input.GetKey(KeyCode.Mouse0)){
            throwTimer += Time.deltaTime;
        }
        if(Input.GetKeyUp(KeyCode.Mouse0)){
            GameObject item = Object.Instantiate(inventory.GetItem(0).itemType.gameObject, itemSpawn.position, transform.rotation) as GameObject;
            Vector2 itemDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - itemSpawn.position;
            item.GetComponent<ThrownSoundItem>().Throw(itemDirection.normalized, throwTimer * 10f);
            throwTimer = 0f;
            isThrowing = false;
            inventory.GetItem(0).ammo--;
        }
    }
}