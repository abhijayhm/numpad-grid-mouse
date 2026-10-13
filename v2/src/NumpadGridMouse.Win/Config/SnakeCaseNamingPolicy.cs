using System.Text.Json;

namespace NumpadGridMouse.Win.Config;

internal sealed class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        var buffer = new System.Text.StringBuilder();
        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsUpper(c))
            {
                if (i > 0)
                    buffer.Append('_');
                buffer.Append(char.ToLowerInvariant(c));
            }
            else
                buffer.Append(c);
        }

        return buffer.ToString();
    }
}
