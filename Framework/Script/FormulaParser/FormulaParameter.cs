using System;
using System.Collections.Generic;

public class FormulaParameter
{
    public IReadOnlyDictionary<string, Func<float>> Parameters => parameters;
    Dictionary<string, Func<float>> parameters = new();

    public void ClearParameter()
    {
        parameters.Clear();
    }
    
    public void SetParameter(string name, Func<float> value)
    {
        parameters[name] = value;
    }
    
    public void SetParameterX(float xValue)
    {
        parameters["x"] = () => xValue;
    }

    public void SetParameterY(float yValue)
    {
        parameters["y"] = () => yValue;
    }
    
    public void SetParameterZ(float zValue)
    {
        parameters["z"] = () => zValue;
    }

    public void SetParameterXYZ(float? xValue = null, float? yValue = null, float? zValue = null)
    {
        if (xValue.HasValue)
        {
            SetParameterX(xValue.Value);
        }

        if (yValue.HasValue)
        {
            SetParameterY(yValue.Value);
        }

        if (zValue.HasValue)
        {
            SetParameterZ(zValue.Value);
        }
    }
}