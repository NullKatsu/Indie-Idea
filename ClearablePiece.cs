using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearablePiece : MonoBehaviour
{
    public AnimationClip clearAnimation;
    [SerializeField]
    private GameObject AnimationPiece; 
    private Coroutine co;
    [SerializeField]
    private bool isBeingCleared = false;
    public bool IsBeingCleared 
    {
        get {return isBeingCleared;}
    }
    public bool IsPreserved = false; //review
    private GameObject animationPiece;
    protected GamePiece piece;
    void Awake() 
    {
        piece = GetComponent<GamePiece>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clear()
    {
        animationPiece = (GameObject)Instantiate(AnimationPiece, transform.position, Quaternion.identity, transform);
        co = StartCoroutine(ClearCoroutine());
        isBeingCleared = true; 
    }

    private IEnumerator ClearCoroutine()
    {
        Animator animator = animationPiece.GetComponent<Animator>();
        if (animator)
        {
            animator.Play(clearAnimation.name);
            yield return new WaitForSeconds(clearAnimation.length);
            EndCoroutine();
        }
    }

    public void EndCoroutine()
    {
        if (co != null)
        {
            //Debug.Log("co stopped");
            StopCoroutine(co);
        }
        Destroy(animationPiece.gameObject);
        isBeingCleared = false;
    }
}
