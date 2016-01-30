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
    public RotatingPlayerController Player { get { return m_Player; } }

    [SerializeField] private float m_MinRadius;
    public float MinRadius { get { return m_MinRadius; } }
    [SerializeField] private float m_MaxRadius;
    public float MaxRadius { get { return m_MaxRadius; } }

    [SerializeField] private GameObject                 m_PNJPrefab;
    [SerializeField] private float                      m_OffsetHalfRange = 5f;
    [SerializeField] [Range(0f, 180f)] private float    m_AngleHalfRange = 5f;
    [SerializeField] private float                      m_PNJPerSquareUnit = 5f;
    private List<PolarCharacter>                        m_PNJs = new List<PolarCharacter>();

    private CircularArea                                m_LastArea = new CircularArea();

    void Awake()
    {
        m_Camera.WorldCenter = m_WorldCenter;

        Vector3 playerPosition = PolarCharacter.PolarToWorld(m_PlayerSpawnAngle, m_PlayerSpawnOffset, m_WorldCenter.position);
        m_Player = ((GameObject)Instantiate(m_PlayerPrefab, playerPosition, Quaternion.LookRotation(m_WorldCenter.position - playerPosition, Vector3.up))).GetComponent<RotatingPlayerController>();
        m_Player.Initialize(m_PlayerSpawnAngle, m_PlayerSpawnOffset, m_WorldCenter);

        m_Camera.Subject = m_Player;

        m_LastArea.AngleMinMax.x = Player.Angle - m_AngleHalfRange;
        m_LastArea.AngleMinMax.y = Player.Angle + m_AngleHalfRange;
        m_LastArea.OffsetMinMax.x = Player.Offset - m_OffsetHalfRange;
        m_LastArea.OffsetMinMax.y = Player.Offset + m_OffsetHalfRange;

        PopulateArea(m_LastArea);
    }

    void Update()
    {
        CircularArea nextArea = new CircularArea();
        nextArea.AngleMinMax = new Vector2(Player.Angle - m_AngleHalfRange, Player.Angle + m_AngleHalfRange);
        nextArea.OffsetMinMax = new Vector2(Player.Offset - m_OffsetHalfRange, Player.Offset + m_OffsetHalfRange);

        if (nextArea.AngleMinMax != m_LastArea.AngleMinMax || nextArea.OffsetMinMax != m_LastArea.OffsetMinMax)
            PopulateDiffArea(nextArea);

        DestroyOutdatedPNJs(nextArea);

        m_LastArea = nextArea;
    }

    private float ComputeArea(CircularArea _source)
    {
        float deltaAngle = _source.AngleMinMax.y - _source.AngleMinMax.x;

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
        int pnjCount = Mathf.FloorToInt(surface * m_PNJPerSquareUnit);

        for (int i = 0; i < pnjCount; ++i)
        {
            float angle = Random.Range(_area.AngleMinMax.x, _area.AngleMinMax.y);
            float offset = Random.Range(_area.OffsetMinMax.x, _area.OffsetMinMax.y);

            Vector3 PNJpos = PolarCharacter.PolarToWorld(angle, offset, m_WorldCenter.position);
            GameObject instance = (GameObject)Instantiate(m_PNJPrefab, PNJpos, Quaternion.LookRotation(m_WorldCenter.position - PNJpos, Vector3.up));

            PolarCharacter PNJ = instance.AddComponent<PolarCharacter>();
            PNJ.Initialize(angle, offset, m_WorldCenter);

            m_PNJs.Add(PNJ);
        }
    }

    private void DestroyOutdatedPNJs(CircularArea testArea)
    {
        List<PolarCharacter> removePNJs = new List<PolarCharacter>();

        foreach(var pnj in m_PNJs)
        {
            if (pnj.Angle < testArea.AngleMinMax.x || pnj.Angle > testArea.AngleMinMax.y ||
                pnj.Offset < testArea.OffsetMinMax.x || pnj.Offset > testArea.OffsetMinMax.y)
            {
                Destroy(pnj.gameObject);
                removePNJs.Add(pnj);
            }
        }

        foreach(var pnj in removePNJs)
        {
            m_PNJs.Remove(pnj);
        }
    }
}
