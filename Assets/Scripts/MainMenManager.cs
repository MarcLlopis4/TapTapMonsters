using TMPro;
using UnityEngine;
using System.Collections;

public class MainMenManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject clickToContinueText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("Clicked", true);
            clickToContinueText.SetActive(false);
        }
    }
}
