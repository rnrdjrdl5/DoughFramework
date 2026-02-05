using System;
using System.Collections.Generic;

// 외부 저장/로드 요청을 이벤트로 위임하는 저장 Ability
public sealed class DataStorageAbility : Ability
{
    public IReadOnlyDictionary<string, SavePayload> Payloads => payloads;

    // 저장 요청 시 외부에서 payload를 저장한다
    public event Action<IReadOnlyDictionary<string, SavePayload>> SaveRequested;
    // 로드 요청 시 외부에서 payload를 채운다
    public event Action<Dictionary<string, SavePayload>> LoadRequested;

    readonly Dictionary<string, SavePayload> payloads = new();

    // 저장용 payload를 등록한다
    public void Register(SavePayload payload)
    {
        if (payload == null)
        {
            return;
        }

        var key = payload.Key;
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }

        payloads[key] = payload;
    }

    // 저장용 payload를 제거한다
    public bool Remove(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        return payloads.Remove(key);
    }

    // 저장용 payload를 조회한다
    public bool TryGet(string key, out SavePayload payload)
    {
        payload = null;
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        return payloads.TryGetValue(key, out payload);
    }

    // 저장을 요청한다
    public bool TryRequestSave()
    {
        if (SaveRequested == null)
        {
            return false;
        }

        try
        {
            // TBD: 실제 저장 처리 구현 예정
            // SavePayloads(payloads);
            SaveRequested.Invoke(payloads);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // 로드를 요청하고 payload를 갱신한다
    public void RequestLoad()
    {
        if (LoadRequested == null)
        {
            return;
        }

        payloads.Clear();
        try
        {
            // TBD: 실제 로드 처리 구현 예정
            // LoadPayloads(payloads);
            LoadRequested.Invoke(payloads);
        }
        catch
        {
            // 외부 예외는 흡수한다
        }
    }

    // 저장 처리를 수행한다 (후속 구현 예정)
    void SavePayloads(IReadOnlyDictionary<string, SavePayload> payloads)
    {
    }

    // 로드 처리를 수행한다 (후속 구현 예정)
    void LoadPayloads(Dictionary<string, SavePayload> payloads)
    {
    }
}

// 저장 데이터의 공통 기반 타입
public abstract class SavePayload
{
    public string Key => string.IsNullOrWhiteSpace(key) ? GetDefaultKey() : key;

    readonly string key;

    protected SavePayload(string key = null)
    {
        this.key = key;
    }

    // 타입 이름을 기본 키로 사용한다
    string GetDefaultKey()
    {
        return GetType().FullName;
    }
}
