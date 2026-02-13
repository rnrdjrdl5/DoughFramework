using System.Collections.Generic;
using UnityEngine;

// 다중 Realm이 필요하면 SortingLayer 결정부분 수정 필요
public class UIAbility : Ability, IUIAbility
{
    Transform uiRootParent;
    Transform systemRoot;

    readonly Dictionary<string, UIRealmContext> contexts = new();
    Realm activeRealm;

    public void Configure(Transform rootParent, Transform systemRootOverride = null)
    {
        uiRootParent = rootParent;
        systemRoot = systemRootOverride;
    }

    public Transform GetSystemRoot()
    {
        if (systemRoot == null)
        {
            systemRoot = new GameObject("SystemUI").transform;
            systemRoot.SetParent(GetParent(), false);
        }
        return systemRoot;
    }

    public Transform GetCoreRoot(Realm realm)
    {
        return EnsureContext(realm).CoreRoot;
    }

    public Transform GetPanelRoot(Realm realm)
    {
        return EnsureContext(realm).PanelRoot;
    }

    public void SetActiveRealm(Realm realm)
    {
        activeRealm = realm;
    }

    public Realm GetActiveRealm()
    {
        return activeRealm;
    }

    public int GetPanelCount(Realm realm)
    {
        var ctx = EnsureContext(realm);
        return ctx.NonModalOverlays.Count;
    }

    public int GetModalCount(Realm realm)
    {
        var ctx = EnsureContext(realm);
        return ctx.ModalOverlays.Count;
    }

    public IReadOnlyList<GameObject> GetActivePanels(Realm realm)
    {
        var ctx = EnsureContext(realm);
        return ctx.NonModalOverlays;
    }

    public IReadOnlyList<GameObject> GetActiveModals(Realm realm)
    {
        var ctx = EnsureContext(realm);
        return ctx.ModalOverlays;
    }

    public void OpenPanel(Realm realm, GameObject ui, int extraOffset = 0)
    {
        if (realm == null || ui == null) return;
        var ctx = EnsureContext(realm);
        ui.transform.SetParent(ctx.PanelRoot, false);
        var canvas = ui.GetComponent<Canvas>();
        if (canvas == null) canvas = ui.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        var index = ctx.NonModalOverlays.Count;
        canvas.sortingOrder = UILayerOrder.Compute(UILayer.Panel, index, extraOffset);
        ctx.NonModalOverlays.Add(ui);
    }

    public void OpenModal(Realm realm, GameObject ui, int extraOffset = 0)
    {
        if (realm == null || ui == null) return;
        var ctx = EnsureContext(realm);
        ui.transform.SetParent(ctx.ModalRoot, false);
        var canvas = ui.GetComponent<Canvas>();
        if (canvas == null) canvas = ui.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        var index = ctx.ModalOverlays.Count;
        canvas.sortingOrder = UILayerOrder.Compute(UILayer.Modal, index, extraOffset);
        ctx.ModalOverlays.Add(ui);
    }

    public void ClosePanel(Realm realm, GameObject ui)
    {
        if (realm == null || ui == null) return;
        var ctx = EnsureContext(realm);
        ctx.NonModalOverlays.Remove(ui);
        // 정렬 재계산: 패널만
        for (int i = 0; i < ctx.NonModalOverlays.Count; i++)
        {
            var c = ctx.NonModalOverlays[i]?.GetComponent<Canvas>();
            if (c == null) continue;
            c.overrideSorting = true;
            c.sortingOrder = UILayerOrder.Compute(UILayer.Panel, i, 0);
        }
    }

    public void CloseModal(Realm realm, GameObject ui)
    {
        if (realm == null || ui == null) return;
        var ctx = EnsureContext(realm);
        ctx.ModalOverlays.Remove(ui);
        for (int i = 0; i < ctx.ModalOverlays.Count; i++)
        {
            var c = ctx.ModalOverlays[i]?.GetComponent<Canvas>();
            if (c == null) continue;
            c.overrideSorting = true;
            c.sortingOrder = UILayerOrder.Compute(UILayer.Modal, i, 0);
        }
    }

    public void AttachToCore(Realm realm, GameObject ui)
    {
        if (realm == null || ui == null) return;
        var root = EnsureContext(realm).CoreRoot;
        ui.transform.SetParent(root, false);

        // Core는 보통 하위 요소가 자체 Canvas를 가질 수 있음 → 필요 시 정렬만 보정
        var canvas = ui.GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = UILayerOrder.Compute(UILayer.Core, 0, 0);
        }
    }

    public void AttachToSystem(GameObject ui)
    {
        if (ui == null) return;
        var root = GetSystemRoot();
        ui.transform.SetParent(root, false);
        var canvas = ui.GetComponent<Canvas>();
        if (canvas == null) canvas = ui.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = UILayerOrder.Compute(UILayer.System, 0, 0);
    }

    UIRealmContext EnsureContext(Realm realm)
    {
        if (realm == null)
        {
            realm = activeRealm;
        }
        if (realm == null)
        {
            return EnsureDefaultContext();
        }

        var key = realm.Id;
        if (!contexts.TryGetValue(key, out var ctx) || ctx == null)
        {
            ctx = CreateContext(realm);
            contexts[key] = ctx;
        }
        return ctx;
    }

    UIRealmContext EnsureDefaultContext()
    {
        const string key = "__default__";
        if (!contexts.TryGetValue(key, out var ctx) || ctx == null)
        {
            ctx = CreateContext(null, keySuffix: "Default");
            contexts[key] = ctx;
        }
        return ctx;
    }

    UIRealmContext CreateContext(Realm realm, string keySuffix = null)
    {
        var parent = GetParent();
        var nameBase = realm != null ? $"RealmUI_{realm.Id}" : $"RealmUI_{keySuffix}";

        var root = new GameObject(nameBase).transform;
        root.SetParent(parent, false);

        var core = new GameObject("CoreUI").transform;
        core.SetParent(root, false);

        var panel = new GameObject("PanelUI").transform;
        panel.SetParent(root, false);

        var modal = new GameObject("ModalUI").transform;
        modal.SetParent(root, false);

        return new UIRealmContext
        {
            Realm = realm,
            Root = root,
            CoreRoot = core,
            PanelRoot = panel,
            ModalRoot = modal
        };
    }

    Transform GetParent()
    {
        if (uiRootParent != null)
        {
            return uiRootParent;
        }

        return Owner != null ? Owner.transform : null;
    }

    class UIRealmContext
    {
        public Realm Realm;
        public Transform Root;
        public Transform CoreRoot;
        public Transform PanelRoot;
        public Transform ModalRoot;
        public List<GameObject> NonModalOverlays = new();
        public List<GameObject> ModalOverlays = new();
    }
}
