using UnityEngine;
using System.Collections.Generic;

public class CircularArea
{
    public Vector2 AngleMinMax = Vector2.zero;
    public Vector2 OffsetMinMax = Vector2.zero;
};

public class EverythingManager : Singleton<EverythingManager>
{
    [SerializeField] private Transform m_WorldCenter;
    public Transform WorldCenter { get { return m_WorldCenter; } }

    [SerializeField] private RotatingCameraController   m_Camera;

    [SerializeField] private GameObject                 m_PlayerPrefab;
    [SerializeField] private float                      m_PlayerSpawnOffset;
    [SerializeField] private float                      m_PlayerSpawnAngle;
    private RotatingPlayerController                    m_Player;
    private Vector2                                     m_OldPlayerPos;
    private bool                                        m_PlayerIsOutcast = false;
    public RotatingPlayerController Player { get { return m_Player; } }

    [SerializeField] private float m_PlayerMinRadius;
    [SerializeField] private float m_PlayerMaxRadius;
    [SerializeField] private float m_CrowdMinRadius;
    [SerializeField] private float m_CrowdMaxRadius;
    public float MinRadius { get { return m_PlayerMinRadius; } }
    public float MaxRadius { get { return m_PlayerMaxRadius; } }
    public float PlayerPositionRatio { get { return (m_Player.Offset - MinRadius) / (MaxRadius - MinRadius); } }

    [SerializeField] private GameObject                 m_NPCPrefab;
    [SerializeField] private float                      m_OffsetHalfRange = 5f;
    [SerializeField] [Range(0f, 180f)] private float    m_AngleHalfRange = 5f;
    [SerializeField] private float                      m_NPCPerSquareUnit = 5f;
    private List<NPCController>                         m_NPCs = new List<NPCController>();
    private CircularArea                                m_LastArea = new CircularArea();

    [SerializeField] private float              m_RebelMinElectionDelay;
    [SerializeField] private float              m_RebelMaxElectionDelay;
    [SerializeField] private int                m_RitualCountUntilFirstRebel;
    [SerializeField] private float              m_RebelMinOffsetFromPlayer;
    [SerializeField] private float              m_RebelMinAngleFromPlayer;
    [SerializeField] private float              m_RebelDetectionRadius;
    [SerializeField] private int                m_RebelGatesToLive;
    private NPCController                       m_Rebel = null;
    private float                               m_RebelTimer = 0f;
    private float                               m_RebelElectionDelay;
    public NPCController Rebel { get { return m_Rebel; } }

    void Awake()
    {
        m_Camera.WorldCenter = m_WorldCenter;

        Vector3 playerPosition = PolarCharacter.PolarToWorld(m_PlayerSpawnAngle, m_PlayerSpawnOffset, m_WorldCenter.position);
        m_Player = ((GameObject)Instantiate(m_PlayerPrefab, playerPosition, Quaternion.LookRotation(m_WorldCenter.position - playerPosition, Vector3.up))).GetComponent<RotatingPlayerController>();
        m_Player.Initialize(m_PlayerSpawnAngle, m_PlayerSpawnOffset, m_WorldCenter);

        m_Camera.Subject = m_Player;

        m_LastArea.AngleMinMax.x = Player.Angle - m_AngleHalfRange;
        m_LastArea.AngleMinMax.y = Player.Angle + m_AngleHalfRange;
        m_LastArea.OffsetMinMax.x = m_CrowdMinRadius;
        m_LastArea.OffsetMinMax.y = m_CrowdMaxRadius;

        PopulateArea(m_LastArea);

        ResetRebelSearch();
    }

    void Update()
    {
        if (Mathf.Abs(Player.Angle - m_OldPlayerPos.x) > (m_AngleHalfRange / 16f))
        {
            CircularArea nextArea = new CircularArea();
            nextArea.AngleMinMax = new Vector2(Player.Angle - m_AngleHalfRange, Player.Angle + m_AngleHalfRange);
            nextArea.OffsetMinMax = new Vector2(m_CrowdMinRadius, m_CrowdMaxRadius);

            if (nextArea.AngleMinMax != m_LastArea.AngleMinMax || nextArea.OffsetMinMax != m_LastArea.OffsetMinMax)
                PopulateDiffArea(nextArea);

            DestroyOutdatedNPCs(nextArea);

            m_OldPlayerPos.x = Player.Angle; m_OldPlayerPos.y = Player.Offset;
            m_LastArea = nextArea;
        }

        RebelUpdateSubroutine();
        BooOutcastPlayer();
    }

    private float ComputeArea(CircularArea _source, float _angleBias = 0f)
    {
        float deltaAngle = (_source.AngleMinMax.y - _source.AngleMinMax.x) + _angleBias;

        float greaterArea = deltaAngle * Mathf.Deg2Rad * Mathf.Pow(_source.OffsetMinMax.y, 2f) * 0.5f;
        float smallerArea = deltaAngle * Mathf.Deg2Rad * Mathf.Pow(_source.OffsetMinMax.x, 2f) * 0.5f;

        return (greaterArea - smallerArea);
    }

    private void SubstractRectangles(CircularArea _A, CircularArea _B, out CircularArea _big, out CircularArea _small)
    {
        _big = new CircularArea();
        _small = new CircularArea();

        _big.OffsetMinMax = _B.OffsetMinMax;
        if (_A.AngleMinMax.x > _B.AngleMinMax.x) // _B is on the "left" side of _A
        {
            _big.AngleMinMax = new Vector2(_B.AngleMinMax.x, _A.AngleMinMax.x);
            _small.AngleMinMax = new Vector2(_A.AngleMinMax.x, _B.AngleMinMax.y);

            if (_A.OffsetMinMax.x > _B.OffsetMinMax.x) // _B is "below" _A
                _small.OffsetMinMax = new Vector2(_B.OffsetMinMax.x, _A.OffsetMinMax.x);
            else
                _small.OffsetMinMax = new Vector2(_B.OffsetMinMax.y, _A.OffsetMinMax.y);
        }
        else
        {
            _big.AngleMinMax = new Vector2(_A.AngleMinMax.y, _B.AngleMinMax.y);
            _small.AngleMinMax = new Vector2(_B.AngleMinMax.x, _A.AngleMinMax.y);

            if (_A.OffsetMinMax.x > _B.OffsetMinMax.x) // _B is "below" _A
                _small.OffsetMinMax = new Vector2(_B.OffsetMinMax.x, _A.OffsetMinMax.x);
            else
                _small.OffsetMinMax = new Vector2(_B.OffsetMinMax.y, _A.OffsetMinMax.y);
        }
    }

    private void PopulateDiffArea(CircularArea _next)
    {
        CircularArea _A, _B;
        SubstractRectangles(m_LastArea, _next, out _A, out _B);

        PopulateArea(_A);
        PopulateArea(_B);
    }

    private void PopulateArea(CircularArea _area)
    {
        float surface = Mathf.Abs(ComputeArea(_area));
        int npcCount = Mathf.RoundToInt(surface * m_NPCPerSquareUnit);
        //Debug.Log(surface + "; " + npcCount);

        for (int i = 0; i < npcCount; ++i)
        {
            float angle = Random.Range(_area.AngleMinMax.x, _area.AngleMinMax.y);
            float offset = Random.Range(_area.OffsetMinMax.x, _area.OffsetMinMax.y);

            if (offset < m_CrowdMaxRadius && offset > m_CrowdMinRadius)
            {
                Vector3 NPCpos = PolarCharacter.PolarToWorld(angle, offset, m_WorldCenter.position);
                /*
                Vector3 toCenterDirection = m_WorldCenter.position - NPCpos;

                float m_CrowdLateralBias = 5f;
                
                Ray neighbourRay = new Ray(NPCpos + Vector3.up * 0.5f, toCenterDirection);
                RaycastHit neighbourHit;
                if (!Physics.Raycast(neighbourRay, out neighbourHit, m_CrowdLateralBias))
                {
                if (!Physics.SphereCast(neighbourRay, m_CrowdLateralBias, 0.5f))
                */
                {
                    GameObject instance = (GameObject)Instantiate(m_NPCPrefab, NPCpos, Quaternion.LookRotation(m_WorldCenter.position - NPCpos, Vector3.up));

                    NPCController NPC = instance.AddComponent<NPCController>();
                    NPC.Initialize(angle, offset, m_WorldCenter);

                    m_NPCs.Add(NPC);
                }
            }
        }
    }

    private void DestroyOutdatedNPCs(CircularArea testArea)
    {
        List<NPCController> removeNPCs = new List<NPCController>();

        foreach(var npc in m_NPCs)
        {
            if (npc.Angle < testArea.AngleMinMax.x || npc.Angle > testArea.AngleMinMax.y ||
                npc.Offset < testArea.OffsetMinMax.x || npc.Offset > testArea.OffsetMinMax.y)
            {
                Destroy(npc.gameObject);
                removeNPCs.Add(npc);

                if (npc == m_Rebel)
                    m_Rebel = null;
            }
        }

        foreach(var npc in removeNPCs)
        {
            m_NPCs.Remove(npc);
        }
    }

    private void BooOutcastPlayer()
    {
        if (Player.Offset > m_CrowdMaxRadius || Player.Offset < m_CrowdMinRadius)
        {
            foreach(var npc in m_NPCs)
                npc.LookAt(Player.transform.position);

            if (!m_PlayerIsOutcast) m_PlayerIsOutcast = true;
        }
        else
        {
            if (m_PlayerIsOutcast)
            {
                foreach (var npc in m_NPCs)
                    npc.LookAt(m_WorldCenter.position);
                m_PlayerIsOutcast = false;
            }
        }
    }


    public void ResetRebelSearch()
    {
        if (m_Rebel != null)
            m_Rebel.GetComponentInChildren<Renderer>().material.color = new Color(1f, 1f, 1f);

        m_Rebel = null;
        m_RebelTimer = 0f;
        m_RebelElectionDelay = Random.Range(m_RebelMinElectionDelay, m_RebelMaxElectionDelay);
    }

    private void RebelUpdateSubroutine()
    {
        if (m_RitualCountUntilFirstRebel < LevelManager.Instance.RitualsCount) return;

        if (m_Rebel == null)
        {
            m_RebelTimer += Time.deltaTime;
            if (m_RebelTimer > m_RebelElectionDelay)
            {
                ElectRebel();
            }
        }
        else
        {
        }
    }

    private void ElectRebel()
    {
        List<NPCController> eligiblesNPC = new List<NPCController>();

        foreach(var npc in m_NPCs)
        {
            if (Mathf.Abs(m_Player.Angle - npc.Angle) > m_RebelMinAngleFromPlayer &&
                Mathf.Abs(m_Player.Offset - npc.Offset) > m_RebelMinOffsetFromPlayer)
            {
                Vector3 directionToCenter = npc.transform.position - Quaternion.Euler(Vector3.right * m_Camera.Pitch) * m_WorldCenter.position;
                Ray backCheckRay = new Ray(npc.transform.position, directionToCenter);
                int hitCount = Physics.SphereCastAll(backCheckRay, .5f, 1f).Length;
                if (hitCount <= 1)
                    eligiblesNPC.Add(npc);
            }
        }

        if (eligiblesNPC.Count != 0)
        {
            for (int i = 0; i < 100 && m_Rebel == null; ++i)
            {
                m_Rebel = eligiblesNPC[Random.Range(0, eligiblesNPC.Count-1)];

                m_Rebel = null;
            }
            if (m_Rebel == null)
                m_Rebel = eligiblesNPC[Random.Range(0, eligiblesNPC.Count - 1)];

            m_Rebel.GetComponentInChildren<Renderer>().material.color = new Color(0f, 0f, 0f);
            m_Rebel.YOLOTranscendSQUAD(m_RebelDetectionRadius, m_RebelGatesToLive);
        }
        else
            Debug.Log("No eligible NPC");
    }
}
