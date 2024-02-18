using System.Text.RegularExpressions;

namespace MQTTBroker.AppCore.Filters;

public class FilterTopicsComparer
{
    public static bool Compare(string topic, string filter)
    {
        if (string.IsNullOrEmpty(topic)) return false;
        if (string.IsNullOrEmpty(filter)) return false;

        var exactTopicRegex = new Regex("^" + Regex.Escape(filter)
            .Replace("\\+", "[^/]+")
            .Replace("\\#", ".+[^/]$") + "$");

        return exactTopicRegex.IsMatch(topic);
    }
}