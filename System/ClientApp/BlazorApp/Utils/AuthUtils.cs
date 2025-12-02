using System;
using System.Text.Json;

namespace BlazorApp.Utils;

public static class AuthUtils
{
    public static IEnumerable<string> ConvertFromClaim(string jsonInput)
    {
        if (jsonInput == null)
            return new List<string>();
        try
        {
            List<string>? resultList = JsonSerializer.Deserialize<List<string>>(jsonInput);
            if (resultList != null)
            {
                return resultList;
            }
            else
            {
                return new List<string> { jsonInput };
            }
        }
        catch (Exception)
        {
            return new List<string> { jsonInput };
        }
    }
}
