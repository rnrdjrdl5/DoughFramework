using UnityEngine;


public class HpTextEffectAbility : Ability
{
    [SerializeField] GameObject damagePrefab;
    [SerializeField] GameObject healPrefab;
    [SerializeField] Transform effectPosition;
    [SerializeField] Transform parent;

    ObjectPoolAbility objectPoolModule;
    HpAbility hpAbility;
    
    public override void Ready()
    {
        base.Ready();

        if (objectPoolModule == null)
        {
            objectPoolModule = Entity.RootAbilitySet.GetAbility<ObjectPoolAbility>();
        }
        if (hpAbility == null)
        {
            hpAbility = Entity.GetAbility<HpAbility>();
        }
        
        hpAbility.OnChangeHp += OnChangeHp;
    }

    public override void Uninitialize()
    {
        hpAbility.OnChangeHp -= OnChangeHp;
        
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
