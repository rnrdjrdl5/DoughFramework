using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 입력 라우팅 서비스
// 순서: System -> Modal -> UI(Panel/Core 등) -> World
// - UI 버튼/이미지 등은 Unity EventSystem이 처리
// - 본 서비스는 UI 히트/모달 여부만 판단해 World로 보낼지 결정
public sealed class InputService : Service, IInputService
{
    [Header("Deps (optional)")]
    [SerializeField] Camera worldCamera;
    [SerializeField] MonoBehaviour uiServiceRef; // IUIService 참조 가능(선택)

    IUIService ui;
    IWorldInputReceiver world;
    bool isEnabled = true;

    static readonly List<RaycastResult> rayResults = new();

    protected override void OnReady()
    {
        // IUIService 자동 획득(인스펙터 참조가 있으면 우선)
        ui = uiServiceRef as IUIService;
        if (ui == null && Services != null)
        {
            Services.TryGet<IUIService>(out ui);
        }
    }

    public void SetWorldReceiver(IWorldInputReceiver receiver) => world = receiver;
    public void SetWorldCamera(Camera cam) => worldCamera = cam;
    public void Enable(bool enabled) => isEnabled = enabled;

    void Update()
    {
        if (!isEnabled) return;

        // 마우스(데스크탑) 간단 처리
        if (Input.GetMouseButtonDown(0))
        {
            RoutePointerDown(Input.mousePosition, pointerId: -1);
        }

        // ESC / Back
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscape();
        }

#if UNITY_ANDROID || UNITY_IOS
        // 단순 터치 예시(필요 시 EnhancedTouch로 확장 가능)
        for (int i = 0; i < Input.touchCount; i++)
        {
            var t = Input.GetTouch(i);
            if (t.phase == TouchPhase.Began)
                RoutePointerDown(t.position, t.fingerId);
        }
#endif
    }

    void RoutePointerDown(Vector2 screenPos, int pointerId)
    {
        // 1) System UI 우선
        if (IsPointerOverUIUnderRoot(screenPos, ui?.GetSystemRoot()))
            return; // UI가 처리

        var realm = ui?.GetActiveRealm();

        // 2) Modal이 열려 있으면 무조건 소모(블로킹)
        if (realm != null && ui.GetModalCount(realm) > 0)
        {
            // 모달 내부에서 처리되든 안되든 아래로 전달 금지
            return;
        }

        // 3) 일반 UI 히트 여부 (Panel/Core 포함; 각 오브젝트의 raycastTarget 설정을 존중)
        if (IsPointerOverAnyUI(screenPos, pointerId))
            return; // UI가 처리

        // 4) World로 전달
        if (worldCamera != null && world != null)
        {
            var ray = worldCamera.ScreenPointToRay(screenPos);
            world.OnWorldPointerDown(screenPos, ray);
        }
    }

    void HandleEscape()
    {
        var realm = ui?.GetActiveRealm();
        if (realm == null)
        {
            world?.OnEsc();
            return;
        }

        // 최상단 Modal 우선 닫기
        var modals = ui.GetActiveModals(realm);
        if (modals != null && modals.Count > 0)
        {
            var top = modals[modals.Count - 1];
            ui.CloseModal(realm, top);
            return;
        }

        // 그 다음 최상단 Panel 닫기
        var panels = ui.GetActivePanels(realm);
        if (panels != null && panels.Count > 0)
        {
            var top = panels[panels.Count - 1];
            ui.ClosePanel(realm, top);
            return;
        }

        // 닫을게 없으면 월드/게임 로직으로
        world?.OnEsc();
    }

    // 현재 EventSystem 기준 전체 UI 히트 검사
    static bool IsPointerOverAnyUI(Vector2 screenPos, int pointerId)
    {
        if (EventSystem.current == null) return false;
        var ed = new PointerEventData(EventSystem.current) { position = screenPos };
        rayResults.Clear();
        EventSystem.current.RaycastAll(ed, rayResults);
        return rayResults.Count > 0;
    }

    // 특정 루트(System 등) 하위만 필터링해 히트 검사
    static bool IsPointerOverUIUnderRoot(Vector2 screenPos, Transform root)
    {
        if (root == null || EventSystem.current == null) return false;

        var ed = new PointerEventData(EventSystem.current) { position = screenPos };
        rayResults.Clear();
        EventSystem.current.RaycastAll(ed, rayResults);
        for (int i = 0; i < rayResults.Count; i++)
        {
            var go = rayResults[i].gameObject;
            if (go != null && go.transform != null && go.transform.IsChildOf(root))
                return true;
        }
        return false;
    }
}

