using System.Collections.Generic;
using UnityEngine;

public interface IUIAbility
{
    // 전역(System) UI 루트 반환
    Transform GetSystemRoot();

    // Realm별 Core/Panel 루트 반환(없으면 생성)
    Transform GetCoreRoot(Realm realm);
    Transform GetPanelRoot(Realm realm);

    // 활성 Realm 관리(입력 라우팅 등에서 사용 예정)
    void SetActiveRealm(Realm realm);
    Realm GetActiveRealm();

    // Panel/Modal 관리
    int GetPanelCount(Realm realm);
    int GetModalCount(Realm realm);
    IReadOnlyList<GameObject> GetActivePanels(Realm realm);
    IReadOnlyList<GameObject> GetActiveModals(Realm realm);
    void OpenPanel(Realm realm, GameObject ui, int extraOffset = 0);
    void OpenModal(Realm realm, GameObject ui, int extraOffset = 0);
    void ClosePanel(Realm realm, GameObject ui);
    void CloseModal(Realm realm, GameObject ui);

    // 부착 헬퍼
    void AttachToCore(Realm realm, GameObject ui);
    void AttachToSystem(GameObject ui);
}
