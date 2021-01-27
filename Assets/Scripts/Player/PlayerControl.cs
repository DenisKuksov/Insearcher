using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    public int StartPointsAmount { get; set; }
    public int MoveCost { get; set; }

    [SerializeField] Bullet[] _bullets;
    [SerializeField] EventSystem _eventSystem;
    [SerializeField] PlayerData _playerData;
    [SerializeField] LineRenderer _lineRenderer;

    private RaycastHit2D _raycastHit2D;
    private Ray _ray;
    private Vector3 _touchPos;
    private Touch _touch;

    public MoveStrategy MoveMethod;

    private Color[] _consumingColors;
    private float[] _consumingWeights;

    [System.Serializable]
    class PointerSettings
    {
        [HideInInspector]
        public bool readyToStart;

        public AnimationCurve pointerCurve;
        public float pointerScalingSpeed;
        public Vector3 pointerMaxScale;
        public Vector3 pointerMinScale;
        public SpriteRenderer pointerSR;
    }

    [SerializeField] PointerSettings _movePointer;
    [SerializeField] PointerSettings _selectPointer;

    private void Start()
    {
        _playerData.Points = StartPointsAmount;
    }

    private void Update()
    {
        _movePointer.pointerSR.color = _playerData.PlayerSpriteRenderer.color;
        _selectPointer.pointerSR.color = _playerData.PlayerSpriteRenderer.color;
        HandleInput();

        SetRootsOfConsumingLines();
        ClearOldLines();
        SetLinesColor();
        foreach(var bullet in _bullets)
            bullet.Color = _playerData.PlayerSpriteRenderer.color;
    }

    private int _animationsInProgress;
    private void ClearOldLines()
    {
        if (_animationsInProgress == 0 && _lineRenderer.positionCount > 0)
        {
            _lineRenderer.positionCount = 0;
        }
    }

    private void SetLinesColor()
    {
        _lineRenderer.startColor = _playerData.PlayerSpriteRenderer.color;
        _lineRenderer.endColor = _playerData.PlayerSpriteRenderer.color;
    }


    private void SetRootsOfConsumingLines()
    {
        int ended = _lineRenderer.positionCount / 2 - _animationsInProgress;
        for (int i = 0; i < _lineRenderer.positionCount; i += 2)
        {
            _lineRenderer.SetPosition(i, _playerData.transform.position);
            if (ended > 0)
            {
                _lineRenderer.SetPosition(i + 1, _playerData.transform.position);
            }
            ended -= 1;
        }
    }

    private Color _midColor;
    

    private void HandleInput()
    {
#if UNITY_EDITOR
        HandleEditorInput();
#endif

#if UNITY_ANDROID
        HandleAndroidInput();
#endif
    }

    private void HandleEditorInput()
    {
        if (Input.GetMouseButtonDown(0) && !_eventSystem.IsPointerOverGameObject())
        {
            _touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _touchPos.Set(_touchPos.x, _touchPos.y, 1f);
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            _raycastHit2D = Physics2D.GetRayIntersection(_ray);

            if (_raycastHit2D.collider != null && _playerData.NowConsuming < _playerData.MaxConsumingAmount && _bullets.Where(b => !b.InShot) != null)
            {
                // print(_raycastHit2D.transform.name); // start consuming coroutine

                Food selected = _raycastHit2D.collider.gameObject.GetComponent<Food>();
                if (selected.NonConsumable)
                {
                    MoveMethod.MoveTo(_touchPos, _playerData.transform, _playerData, MoveCost);
                    return;
                }

                if (!_playerData.InConsuming.Contains(selected) && selected.IsSpawned)
                {
                    _playerData.Points += Mathf.RoundToInt(CalculateProfit(_playerData.PlayerSpriteRenderer.color,
                        selected.FoodSpriteRenderer.color) * selected.NowMass / 100f);
                    _playerData.NowConsuming += 1;
                    foreach (var bullet in _bullets)
                    {
                        if (bullet.InShot) continue;
                        bullet.ThrowIn(_raycastHit2D.collider.transform, _playerData.transform.position, () => selected.Consume(_playerData));
                        break;
                    }
                    
                    //DrawNewConsumingLine(_raycastHit2D.collider.transform, selected);

                    _playerData.InConsuming.Add(selected);
                }
            }
            else if (!_playerData.isMoving && _playerData.NowConsuming == 0)
            {
                MoveMethod.MoveTo(_touchPos, _playerData.transform, _playerData, MoveCost);
            }
        }
    }

    private void HandleAndroidInput()
    {
        if (Input.touchCount == 1 && _eventSystem.currentSelectedGameObject == null)
        {
            _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Began)
            {
                _ray = Camera.main.ScreenPointToRay(_touch.position);
                _raycastHit2D = Physics2D.GetRayIntersection(_ray);

                if (_raycastHit2D.collider == null && !_playerData.isMoving && _playerData.NowConsuming == 0)
                { //movement
                    _touchPos = Camera.main.ScreenToWorldPoint(_touch.position);
                    _touchPos.Set(_touchPos.x, _touchPos.y, 1f);

                    _movePointer.readyToStart = true;
                    _movePointer.pointerSR.transform.position = _touchPos;
                    _movePointer.pointerSR.transform.localScale = Vector3.zero;
                     
                    StartCoroutine(ChangePointerSize(_movePointer));
                }
                else if (_playerData.NowConsuming < _playerData.MaxConsumingAmount)
                { // selection
                    Transform selected = _raycastHit2D.transform;
                    if (selected == null) return;
                    if (selected.transform.position == _selectPointer.pointerSR.transform.position
                        && _selectPointer.pointerSR.transform.localScale.x > selected.transform.localScale.x) return;

                    _selectPointer.readyToStart = true;
                    _selectPointer.pointerSR.transform.position = selected.transform.position;
                    _selectPointer.pointerSR.transform.localScale = Vector3.zero;

                    _selectPointer.pointerSR.transform.localScale = selected.localScale * 0.84f;

                    _selectPointer.pointerMaxScale = selected.transform.localScale + Vector3.one * 0.1f;
                    StartCoroutine(ChangePointerSize(_selectPointer));
                }
            }
            else if (_touch.phase == TouchPhase.Moved)
            {
                if (Vector3.Distance(_touchPos, Camera.main.ScreenToWorldPoint(_touch.position)) > _movePointer.pointerSR.transform.localScale.x)
                {
                    _movePointer.readyToStart = false;
                    _selectPointer.readyToStart = false;
                }
            }
            else if (_touch.phase == TouchPhase.Ended)
            {
                if (_movePointer.readyToStart)
                {
                    //movement
                    _movePointer.readyToStart = false;
                    MoveMethod.MoveTo(_touchPos, _playerData.transform, _playerData, MoveCost);
                }

                if (_selectPointer.readyToStart)
                {
                    //consuming
                    _selectPointer.readyToStart = false;

                    Food selected = _raycastHit2D.collider.gameObject.GetComponent<Food>();
                    if (selected.NonConsumable)
                    {
                        _movePointer.readyToStart = false;
                        MoveMethod.MoveTo(_touchPos, _playerData.transform, _playerData, MoveCost);
                        return;
                    }
                    if (selected == null) return;
                    if (!_playerData.InConsuming.Contains(selected) && selected.IsSpawned && _bullets.Where(b => !b.InShot) != null)
                    {
                        _playerData.Points += Mathf.RoundToInt(CalculateProfit(_playerData.PlayerSpriteRenderer.color,
                            selected.FoodSpriteRenderer.color) * selected.NowMass / 100f);
                        _playerData.NowConsuming += 1;
                        foreach (var bullet in _bullets)
                        {
                            if (bullet.InShot) continue;
                            bullet.ThrowIn(_raycastHit2D.collider.transform, _playerData.transform.position, () => selected.Consume(_playerData));
                            break;
                        }
                        

                        _playerData.InConsuming.Add(selected);
                    }

                }
            }
        }
    }

    private IEnumerator ChangePointerSize(PointerSettings pointer)
    {
        float t = 0.05f;
        Transform pointerTransform = pointer.pointerSR.transform;
        Vector3 delta = pointer.pointerMaxScale - pointerTransform.localScale;
        Vector3 start = pointerTransform.localScale;
        while(t <= 1f)
        {
            pointerTransform.localScale = start + delta * pointer.pointerCurve.Evaluate(t);
            t += Time.deltaTime * pointer.pointerScalingSpeed;
            yield return 0;
        }

        pointerTransform.localScale = start + delta;

        yield return new WaitUntil(() => !pointer.readyToStart);

        //delta = pointer.pointerMinScale - pointerTransform.localScale;
        while(!pointer.readyToStart && t >= 0f)
        {
            pointerTransform.localScale = start + delta * pointer.pointerCurve.Evaluate(t);
            t -= Time.deltaTime * pointer.pointerScalingSpeed;
            yield return 0;
        }

        pointerTransform.transform.localScale = Vector3.zero;
    }

    private void DrawNewConsumingLine(Transform target, Food selected)
    {
        int posCount =_lineRenderer.positionCount += 2;
        _lineRenderer.SetPosition(posCount - 2, _playerData.transform.position);
        StartCoroutine(DrawConusmingLineEnumerator(target, posCount - 1, selected));
    }

    private IEnumerator DrawConusmingLineEnumerator(Transform target, int index, Food selected)
    {
        _animationsInProgress += 1;

        Vector3 start = _playerData.transform.position;
        float t = 0f;

        Vector3 delta = Vector3.zero;

        while (t < 1f)
        {
            delta = target.position - start;
            _lineRenderer.SetPosition(index, start + delta * _playerData.LineDrawingCurve.Evaluate(t));

            t += Time.deltaTime * _playerData.DrawConusmingLineSpeed;
             
            yield return 0;
        }

        selected.Consume(_playerData);

        yield return new WaitUntil(() => !_playerData.InConsuming.Contains(selected));

        t = 0f;
        while (t < 1f)
        {
            start = _playerData.transform.position;
            delta = target.position - start;
            _lineRenderer.SetPosition(index, start + delta * _playerData.LineDrawingCurve.Evaluate(1f - t));

            t += Time.deltaTime * _playerData.DrawConusmingLineSpeed;

            yield return 0;
        }

        _animationsInProgress -= 1;

    }

    private float CalculateProfit(Color st, Color nd)
    {
        /*
        float difference = Mathf.Abs(st.r - nd.r) + Mathf.Abs(st.g - nd.g) + Mathf.Abs(st.b - nd.b);
        return 110 - difference * 255f;*/
        Debug.Log(Mathf.Pow((Mathf.Sqrt(3) - Mathf.Sqrt(Mathf.Pow(st.r - nd.r, 2f) + Mathf.Pow(st.g - nd.g, 2f) + Mathf.Pow(st.b - nd.b, 2f))) / Mathf.Sqrt(3), _playerData.Steapness) - _playerData.Strictness);
        return Mathf.Lerp(-255f, 255f, Mathf.Pow((Mathf.Sqrt(3) - Mathf.Sqrt(Mathf.Pow(st.r - nd.r, 2f) + Mathf.Pow(st.g - nd.g, 2f) + Mathf.Pow(st.b - nd.b, 2f))) / Mathf.Sqrt(3), _playerData.Steapness) - _playerData.Strictness);

    }
}
