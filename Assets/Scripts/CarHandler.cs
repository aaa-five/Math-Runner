using UnityEngine;

public class CarHandler : MonoBehaviour
{
    public float leftLaneX = -3.5f;
    public float rightLaneX = 3.5f;
    public float laneSwitchSpeed = 14f;
    public float snapDistance = 0.05f;
    public float startX = 0f;   // center spawn

    private int currentLane = -1; // -1 = center spawn, 0 = left, 1 = right
    private float targetX;
    private bool isMoving = false;

    private LevelConfig levelConfig;
    private float finalLaneSwitchSpeed;

    private Vector2 touchStartPos;
    private bool swipeDetected = false;

    void Start()
    {
        levelConfig = FindFirstObjectByType<LevelConfig>();

        if (levelConfig != null)
            finalLaneSwitchSpeed = laneSwitchSpeed * levelConfig.speedMultiplier;
        else
            finalLaneSwitchSpeed = laneSwitchSpeed;

        currentLane = Random.Range(0, 2);
        targetX = (currentLane == 0) ? leftLaneX : rightLaneX;
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                MoveLeft();

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                MoveRight();

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    touchStartPos = touch.position;
                    swipeDetected = false;
                }

                if (touch.phase == TouchPhase.Moved && !swipeDetected)
                {
                    float deltaX = touch.position.x - touchStartPos.x;

                    if (Mathf.Abs(deltaX) > 50f)
                    {
                        if (deltaX > 0)
                            MoveRight();
                        else
                            MoveLeft();

                        swipeDetected = true;
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                touchStartPos = Input.mousePosition;
                swipeDetected = false;
            }

            if (Input.GetMouseButton(0) && !swipeDetected)
            {
                float deltaX = Input.mousePosition.x - touchStartPos.x;

                if (Mathf.Abs(deltaX) > 50f)
                {
                    if (deltaX > 0)
                        MoveRight();
                    else
                        MoveLeft();

                    swipeDetected = true;
                }
            }
        }

        if (isMoving)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.MoveTowards(pos.x, targetX, finalLaneSwitchSpeed * Time.deltaTime);
            transform.position = pos;

            if (Mathf.Abs(transform.position.x - targetX) <= snapDistance)
            {
                transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
                isMoving = false;
            }
        }
    }

    void MoveLeft()
    {
        currentLane = 0;
        targetX = leftLaneX;
        isMoving = true;
    }

    void MoveRight()
    {
        currentLane = 1;
        targetX = rightLaneX;
        isMoving = true;
    }
}