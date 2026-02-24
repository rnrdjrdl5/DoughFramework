using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PanelAbility : Ability
{
    public Panel Panel => panels.LastOrDefault();
    public event Action<Panel> OnAddPanel;
    public event Action OnRemovePanel;
    
    [SerializeField] bool autoRefreshPanelOrder = true;
    
    List<Panel> panels = new();
    
    public PanelType CreatePanel<PanelType>(string prefabPath, Parameter parameter = null, Entity ownerEntity = null) where PanelType : Panel, new()
    {
        if (ownerEntity ==null)
        {
            ownerEntity = Entity;
        }
        
        var panel = ownerEntity.AddEntity<PanelType>(prefabPath, parameter);
        panels.Add(panel);

        if (autoRefreshPanelOrder)
        {
            RefreshPanelOrder();
        }

        OnAddPanel?.Invoke(panel);
        
        return panel;
    }

    // Note : SortingOrder를 변경하기 떄문에 Canvas 내부에서 비용을 소모하게 된다.
    void RefreshPanelOrder()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            var panel = panels[i];
            if (panel.PanelCustomOffset != 0)
            {
                panel.RefreshCanvasOrder();
                continue;
            }

            var nextOrder = i;
            panel.SetPanelOrder(nextOrder);
        }
    }

    public bool RemovePanel<PanelType>(Entity ownerEntity = null)
    {
        if (ownerEntity == null)
        {
            ownerEntity = Entity;
        }

        var panel = panels.FirstOrDefault(panel => panel.GetType() == typeof(PanelType));
        if (panel == null)
        {
            return panel;
        }

        panels.Remove(panel);
        ownerEntity.RemoveChild(panel);

        if (autoRefreshPanelOrder)
        {
            RefreshPanelOrder();
        }
        
        OnRemovePanel?.Invoke();

        return true;
    }

    public bool RemovePanel(Panel panel, Entity ownerEntity = null)
    {
        if (ownerEntity == null)
        {
            ownerEntity = Entity;
        }

        if (!panels.Contains(panel))
        {
            return false;
        }

        panels.Remove(panel);
        ownerEntity.RemoveChild(panel);

        RefreshPanelOrder();
        
        OnRemovePanel?.Invoke();

        return true;
    }

    public bool ClosePanel()
    {
        if (Panel == null)
        {
            return false;
        }

        var targetPanel = Panel;
        targetPanel.Close();
        
        panels.Remove(targetPanel);

        if (autoRefreshPanelOrder)
        {
            RefreshPanelOrder();
        }
        
        OnRemovePanel?.Invoke();

        return true;
    }
    
    public TPanel GetPanel<TPanel>() where TPanel : Panel
    {
        return panels.Where(element => typeof(TPanel).IsAssignableFrom(element.GetType()))
            .Cast<TPanel>()
            .FirstOrDefault();
    }
}
