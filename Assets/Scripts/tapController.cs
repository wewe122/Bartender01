using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tapController : MonoBehaviour
{
    
    public Transform[] taps;
    [SerializeField]
    public GlassManager glassManager;
    public float PouringDelay = 3f;
    private Animator animator;

    private int currentTap = 0;
    private bool SpacePressed, TapsLocked;
    private float PouringDistance = 1.29f;
    private const int RUBIA_INDEX = 0;
    private const int ROJA_INDEX = 1;
    private const int NEGRA_INDEX = 2;
    
    void Start()
    {
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        SpacePressed = false;
        TapsLocked = false;
    }

    void Update()
    {
        /*
         * if taps is locked do nothing
         */
        if (TapsLocked)
            return;

        /*
         * handle beer pouring
         * player needs to be close to active tap for pouring to happen
         */
        if (Input.GetKeyDown(KeyCode.Space) && !SpacePressed && glassManager.isHoldingEmptyGlass())
        {
            SpacePressed = true;
            var distToRubia = Vector3.Distance(taps[RUBIA_INDEX].position, GameObject.FindWithTag("Player").transform.position);
            var distToRoja = Vector3.Distance(taps[ROJA_INDEX].position, GameObject.FindWithTag("Player").transform.position);
            var distToNegra = Vector3.Distance(taps[NEGRA_INDEX].position, GameObject.FindWithTag("Player").transform.position);
            Debug.Log("distToRubia: " + distToRubia + ", distToRoja: " + distToRoja + "\ndistToNegra: " + distToNegra);

            if (distToRubia <= PouringDistance)
            {
                Debug.Log("pouring rubia beer");
                TapsLocked = true;// lock taps while pouring beer
                currentTap = RUBIA_INDEX;
                GameObject.FindWithTag("Player").transform.position = new Vector3(taps[RUBIA_INDEX].position.x + 0.34f, -2.69f, 0);
                StartCoroutine(PourBeer());

            }
            else if (distToRoja <= PouringDistance)
            {
                Debug.Log("pouring roja beer");
                TapsLocked = true;// lock taps while pouring beer
                currentTap = ROJA_INDEX;
                GameObject.FindWithTag("Player").transform.position = new Vector3(taps[ROJA_INDEX].position.x + 0.34f, -2.69f, 0);
                StartCoroutine(PourBeer());
            }
            else if (distToNegra <= PouringDistance)
            {
                Debug.Log("pouring negra beer");
                TapsLocked = true;// lock taps while pouring beer
                currentTap = NEGRA_INDEX;
                GameObject.FindWithTag("Player").transform.position = new Vector3(taps[NEGRA_INDEX].position.x + 0.34f, -2.69f, 0);
                StartCoroutine(PourBeer());
            }
            
        }
        // avoid multiple actions from one long press
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SpacePressed = false;
        }
    }
    IEnumerator PourBeer()
    {
        glassManager.ReleaseEmptyGlass();
        animator.SetBool("pouring", true);
        BarManMovement.ToggleFreezeMovement();
        yield return new WaitForSeconds(PouringDelay);
        TapsLocked = false;// unlock taps
        glassManager.FillGlass(currentTap);
        Debug.Log("Beer is Ready");
        animator.SetBool("pouring", false);
        BarManMovement.ToggleFreezeMovement();
    }
}
