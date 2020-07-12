using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerUnit : Unit
{
    private static readonly Vector3 minVector = new Vector3(float.MinValue, float.MinValue, float.MinValue);
    private Vector3 pathrangebase;
    private bool inCombat = false;
    Vector3 targetPos = minVector;
    internal ParticleSystem _particleSystem;

    public float moveDelay;
    public float accuracyDiameter;
    public float distanceInaccuracy;
    public float VisionRange;
    public float autoAgroRange;

    public float speed;
    public Image OrderPie;
    public GameObject UnitUI;

    private NavMeshAgent pathAgent;
    private LocalNavMeshBuilder mapper;

    public PlayRandomAudioClip MoveClips, engagingClips;
    public AudioSource movingSource;

    // Start is called before the first frame update
    void Start()
    {
        OrderPie.fillAmount = 0;
        UnitUI.SetActive(false);
        _particleSystem = this.gameObject.GetComponentInChildren<ParticleSystem>();
        PlayerUnitController.Instance.RegisterUnit(this);
        this.pathAgent = gameObject.GetComponent<NavMeshAgent>();
        this.mapper = GetComponent<LocalNavMeshBuilder>();
        this.pathrangebase = mapper.m_Size;
        StartCoroutine(UpdateClosePathPoint(.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (!inCombat)
        {
            (Unit close, float dist) = CloestEnemyUnit(false);

            if(!(close is null) && dist < autoAgroRange)
            {
                pathAgent.SetDestination(close.transform.position);
                pathAgent.isStopped = false;
            }

            if (!pathAgent.isStopped)
            {
                movingSource.volume = .3f;
            }
            else
            {
                movingSource.volume = .2f;
            }


            //if (targetPos != minVector)
            //{
            //    if (targetPos == this.transform.position)
            //    {
            //        targetPos = minVector;
            //        return;
            //    }
            //    this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, speed * Time.deltaTime);
            //}
        }
        else
        {
            movingSource.volume = .2f;
        }
    }

    private IEnumerator UpdateClosePathPoint(float frequency )
    {
        while (true)
        {
            if (!inCombat && !pathAgent.isStopped)
            {
                pathAgent.destination = Vector3.MoveTowards(this.transform.position, targetPos, this.VisionRange + 3);
            }
            yield return new WaitForSeconds(frequency);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Unit Clicked");
        if (inCombat) return;
        PlayerUnitController.Instance.UnitClicked(this);
    }

    bool OrderQueued = false;

    private Coroutine delayMoveCoroutine;
    internal void QueueMoveTo( Vector3 point )
    {
        Debug.Log(string.Format("Unit {0} queuing move to {1}", this.UnitID, point));
        if (OrderQueued)
        {
            StopCoroutine(delayMoveCoroutine);
        }
        Vector3 target;
        do
        {
            float dist = Vector3.Distance(transform.position, point);
            Vector3 offset = Vector3.Normalize(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1))) * 
                Random.Range(0, (accuracyDiameter + distanceInaccuracy*dist) / 2f);
            target = point + offset;
        } while (OutOfBounds(target));
        delayMoveCoroutine = StartCoroutine(DelayMoveTo(target, moveDelay + Random.Range(-.5f, .1f)));

        bool OutOfBounds( Vector3 checkPoint )
        {
            return NavMesh.SamplePosition(checkPoint, out _, 0f, 1);
            //return checkPoint.x > PlayerUnitController.Instance.bounds.x || checkPoint.x < PlayerUnitController.Instance.bounds.y ||
            //    checkPoint.z > PlayerUnitController.Instance.bounds.z || checkPoint.z < PlayerUnitController.Instance.bounds.w;
        }
        MoveClips.Play();
    }


    public IEnumerator DelayMoveTo( Vector3 target, float delay )
    {
        OrderQueued = true;
        UnitUI.SetActive(true);
        float timer = 0;
        while (timer < delay)
        {
            timer += Time.deltaTime;
            OrderPie.fillAmount = timer / delay;
            yield return null;
        }
        //yield return new WaitForSeconds(delay);

        Debug.Log(string.Format("Unit {0} moving to {1}", this.UnitID, target));
        targetPos = target;
        pathAgent.isStopped = false;
        OrderQueued = false;
        UnitUI.SetActive(false);
    }

    public override void EnterCombat()
    {
        Debug.Log("Unit entered combat");
        StopCoroutine(delayMoveCoroutine);
        UnitUI.SetActive(false);
        //targetPos = minVector;
        pathAgent.isStopped = true;
        this.inCombat = true;
        engagingClips.Play();
    }

    public override void EndCombat()
    {
        this.inCombat = false;
    }


    private new void OnDestroy()
    {
        base.OnDestroy();
        PlayerUnitController.Instance.UnregisterUnit(this);
        StopAllCoroutines();
    }
}
