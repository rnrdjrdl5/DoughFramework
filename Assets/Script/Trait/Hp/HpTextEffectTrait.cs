using UnityEngine;


public class HpTextEffectTrait : Trait
{
    [SerializeField] GameObject damagePrefab;
    [SerializeField] GameObject healPrefab;
    [SerializeField] Transform effectPosition;
    [SerializeField] Transform parent;

    ObjectPoolModule objectPoolModule;
    HpTrait hpTrait;
    
    public override void Ready()
    {
        base.Ready();

        if (objectPoolModule == null)
        {
            objectPoolModule = Actor.Environment.GetModule<ObjectPoolModule>();
        }
        if (hpTrait == null)
        {
            hpTrait = Actor.GetTrait<HpTrait>();
        }
        
        hpTrait.OnChangeHp += OnChangeHp;
    }

    public override void Uninitialize()
    {
        hpTrait.OnChangeHp -= OnChangeHp;
        
        base.Uninitialize();
    }

    void OnChangeHp(float prevHp, float hp, float point)
    {
        var effectObject = objectPoolModule.AllocateGameObject(prevHp - hp > 0 ? damagePrefab : healPrefab, parent);
        effectObject.transform.position = effectPosition == null ? effectPosition.position : transform.position;
        
        var uiSetterText = effectObject.GetComponent<UISetterText>();
        var deltaHp = Mathf.Abs(prevHp - hp);
        
        uiSetterText.UpdateText($"{deltaHp}");
    }
}
