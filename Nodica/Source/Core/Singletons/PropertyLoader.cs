using System.Reflection;

namespace Nodica;

public static class PropertyLoader
{
    public static T Load<T>(string filePath) where T : new()
    {
        T stylePack = new();
        string[] fileLines = File.ReadAllLines(filePath);

        foreach (string line in fileLines)
        {
            string trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine) || !trimmedLine.Contains("="))
            {
                continue; // Skip empty lines or lines without assignment
            }

            // Split the property path and value
            int equalsIndex = trimmedLine.IndexOf("=");
            string propertyPath = trimmedLine.Substring(0, equalsIndex).Trim();
            string value = trimmedLine.Substring(equalsIndex + 1).Trim();

            // Assign the value to the appropriate property in the stylePack
            SetPropertyValue(stylePack, propertyPath, value);
        }

        return stylePack;
    }

    private static void SetPropertyValue(object obj, string propertyPath, string value)
    {
        string[] segments = propertyPath.Split('/');
        Type type = obj.GetType();
        PropertyInfo propertyInfo = null;

        for (int i = 0; i < segments.Length; i++)
        {
            string segment = segments[i];
            propertyInfo = type.GetProperty(segment, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                throw new Exception($"Property '{segment}' not found on type '{type.Name}'.");
            }

            if (i < segments.Length - 1)
            {
                obj = propertyInfo.GetValue(obj);
                type = obj.GetType();
            }
        }

        if (propertyInfo != null && propertyInfo.CanWrite)
        {
            object convertedValue = ConvertValue(propertyInfo.PropertyType, value);
            propertyInfo.SetValue(obj, convertedValue);
        }
    }

    private static object ConvertValue(Type propertyType, string value)
    {
        if (propertyType == typeof(Color))
        {
            return ParseColor(value);
        }
        else if (propertyType == typeof(float))
        {
            return float.Parse(value);
        }
        else if (propertyType == typeof(int))
        {
            return int.Parse(value);
        }
        else if (propertyType == typeof(bool))
        {
            return bool.Parse(value);
        }
        else if (propertyType == typeof(Vector2))
        {
            return ParseVector2(value);
        }
        else if (propertyType.IsEnum)
        {
            return Enum.Parse(propertyType, value);
        }

        return value; // Default to string if not matched
    }

    private static Vector2 ParseVector2(string value)
    {
        string[] components = value.Trim('(', ')').Split(',');
        return new Vector2(float.Parse(components[0]), float.Parse(components[1]));
    }

    private static Color ParseColor(string value)
    {
        string numericPart = value.Replace("Color", "").Trim('(', ')');

        string[] components = numericPart.Split(',');

        // Parse each component as a byte
        return new Color(
            byte.Parse(components[0].Trim()),
            byte.Parse(components[1].Trim()),
            byte.Parse(components[2].Trim()),
            byte.Parse(components[3].Trim())
        );
    }
}
