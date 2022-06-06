using UnityEngine;

public class View: MonoBehaviour
{ 
    [Header("摄像机")]
    public float EPS = 0.01f;
    public float CameraRadius = 5;
    public float ZoomSensitivity = 3;
    public float MinZoomDistance = 2;
    public float MaxZoomDistance = 10;
    public float MinVerticalDegree = -90;
    public float MaxVerticalDegree = 90;
    public float VerticalSensitivity = 3;
    public float HorizentalSensitivity = 3;

    [Header("物品栏")]
    
    public int InitialBlockIndex = 0;
    public float MaxRayDistance = 10;
    public GameObject[] CandidateBlocks;
    public float TabLeftLocation = -440;
    public float TabDownLocation = -640;
    public float SelectorMoveDistance = 110;
    // 唤醒函数
    void Awake()
    {
        Initialize();   
        StartState();
    }
    // 更新函数
    void LateUpdate()
    {
        ListenInput();
        UpdateView();
        RayTest();
    }
    // 定义引用
    private Cross _Cross;
    private Camera _Camera;
    private int BlockIndex;
    private Vector2 TabLocation;
    private RectTransform BlockSelector;
    private bool firstPerson, corssEnabled;
    private GameObject _Player, Terrain;
    private SkinnedMeshRenderer CharacterHead, CharacterBody;
    private Vector3 cameraRotate, _cameraRadius, playerHeight;
    // 获取实例
    private void Initialize()
    {
        // 摄像机
        _Cross = GetComponent<Cross>();
        _Camera = GetComponent<Camera>();
        _Player = GameObject.Find("Player");
        Terrain = GameObject.Find("Terrain");
        cameraRotate = new Vector3(0, 0, 0);
        playerHeight = new Vector3(0, 1.5f, 0);
        _cameraRadius = new Vector3(0, 0, -CameraRadius);
        CharacterHead = GameObject.Find("f1_5361a_head_hd.mesh")
            .GetComponent<SkinnedMeshRenderer>();
        CharacterBody = GameObject.Find("f1_5361_body_hd.mesh")
            .GetComponent<SkinnedMeshRenderer>();
        // 物品栏
        BlockIndex = InitialBlockIndex;
        BlockSelector = GameObject.Find("TabPartTwo")
            .GetComponent<RectTransform>();
        TabLocation = new Vector2(
            TabLeftLocation + InitialBlockIndex * SelectorMoveDistance,
            TabDownLocation
        );
    }
    // 初始状态
    private void StartState()
    {
        CharacterHead.enabled = false;
        CharacterBody.enabled = false;
        corssEnabled = firstPerson = true;
        AdjustCross(corssEnabled);
    }
    // 调整模式
    private void AdjustCross(bool flag)
    {
        if (flag)
        {
            // 显示准星
            _Cross.enabled = true;
            // 隐藏鼠标
            Cursor.visible = false;
            // 锁定鼠标
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            _Cross.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        corssEnabled = flag;
    }
    // 限制最大值和最小值
    private void LimiteValue(ref float value, float max, float min)
    {
        if (value > max) value = max;
        if (value < min) value = min;
    }
    private void AdjustSelector(int index)
    {
        BlockIndex = index;
        TabLocation.x = TabLeftLocation + index * SelectorMoveDistance;
        BlockSelector.anchoredPosition = TabLocation;
    }
    // 监听输入
    private void ListenInput()
    {
        // 监听 ESC 退出游戏
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            // 保存游戏存档
            GameDataManager.SaveGameData();
        }
        // 监听 v 切换视角
        if (Input.GetKeyDown(KeyCode.V))
        {
            firstPerson = !firstPerson;
            AdjustCross(firstPerson);
            if (firstPerson)
            {
                CharacterHead.enabled = false;
                CharacterBody.enabled = false;
            }
            else
            {
                CharacterHead.enabled = true;
                CharacterBody.enabled = true;
            }
        }
        // 监听 t 切换模式
        if (firstPerson && Input.GetKeyDown(KeyCode.T))
        {
            corssEnabled = !corssEnabled;
            AdjustCross(corssEnabled);
        }
        // 监听摄像机角度
        if (corssEnabled || !corssEnabled && Input.GetKey(KeyCode.Mouse2))
        {
            cameraRotate.x -= Input.GetAxis("Mouse Y") * VerticalSensitivity;
            cameraRotate.y += Input.GetAxis("Mouse X") * HorizentalSensitivity;
            LimiteValue(ref cameraRotate.x, MaxVerticalDegree, MinVerticalDegree);
            _Player.transform.eulerAngles = new Vector3(0, cameraRotate.y, 0);
        }
        // 监听摄像机距离
        _cameraRadius.z += Input.GetAxis("Mouse ScrollWheel") * ZoomSensitivity;
        LimiteValue(ref _cameraRadius.z, -MinZoomDistance, -MaxZoomDistance);
        // 监听被选择方块
        if (Input.GetKeyDown(KeyCode.Alpha1)) AdjustSelector(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) AdjustSelector(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) AdjustSelector(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) AdjustSelector(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) AdjustSelector(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6)) AdjustSelector(5);
        else if (Input.GetKeyDown(KeyCode.Alpha7)) AdjustSelector(6);
        else if (Input.GetKeyDown(KeyCode.Alpha8)) AdjustSelector(7);
        else if (Input.GetKeyDown(KeyCode.Alpha9)) AdjustSelector(8);
    }
    // 更新视角
    private void UpdateView()
    {
        // 调整摄像机高度和角度
        transform.position = _Player.transform.position + playerHeight;
        transform.localEulerAngles = cameraRotate;

        // 如果是第三人称模式
        if (! firstPerson)
        {
            // 调整摄像机与角色间的距离
            transform.TransformDirection(cameraRotate);
            transform.Translate(_cameraRadius);
        }
    }
    // 射线检测
    private void RayTest()
    {
        RaycastHit hit;
        Ray ray = _Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, MaxRayDistance))
        {
            // 删除方块
            if (Input.GetKeyDown(KeyCode.Mouse0) && !hit.collider.gameObject.Equals(_Player))
                ModifyBlock.DelBlock(hit.collider.gameObject);
            // 放置方块
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                // 反向追踪
                Vector3 pos = hit.point + hit.normal * EPS;
                if (BlockIndex < CandidateBlocks.Length)
                {
                    // 获取角色位置
                    Vector3 playerPos = _Player.transform.position;
                    Vector3 pp1 = playerPos + new Vector3(0, -0.5f, 0);
                    Vector3 pp2 = playerPos + new Vector3(0, 0.5f, 0);
                    // 整数化
                    ProcessCoord(ref pp1);
                    ProcessCoord(ref pp2);
                    ProcessCoord(ref pos);
                    // 如果不在角色身上且此处没有方块
                    if (
                        ModifyBlock.GetFromMap(pos) == null
                        && (pos - pp1).magnitude > EPS
                        && (pos - pp2).magnitude > EPS
                    )
                    ModifyBlock.PutBlockAt(ref CandidateBlocks[BlockIndex], pos);
                }
            }
        }
    }
    // 坐标整数化
    private void ProcessCoord(ref Vector3 pos)
    {
        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);
        pos.z = Mathf.Ceil(pos.z);
    }
}
