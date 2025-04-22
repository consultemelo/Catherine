using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CatherineDesktopApp.Services;

namespace CatherineDesktopApp.Shared
{
    public class Enums
    {
        public enum DetailViewMode
        {
            New,
            Edit
        }
    }

    public abstract class EnumDisplay
    {
        public int Index { get; protected set; }
        public object Value { get; protected set; }
        public string Display { get; protected set; }
    }

    public class EnumDisplay<T> : EnumDisplay
    {
        internal EnumDisplay(T value, string display, int index)
        {
            Index = index;
            Value = value;
            Display = display;
        }

        public new T Value { get; }

        public override string ToString() => $"Display: {Display}, Value: {Value}";

    }

    public static class EnumExtensions
    {
        public static string Description(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = fieldInfo
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .OfType<DescriptionAttribute>()
                .ToList();

            return attributes.Any() ? attributes.First().Description : value.ToString();
        }

        public static ReadOnlyCollection<T> GetValues<T>()
            where T : struct, IComparable, IFormattable, IConvertible
        {
            var itemType = typeof(T);

            if (!itemType.IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            var fields = itemType
                .GetFields()
                .Where(field => field.IsLiteral);

            return fields
                .Select(field => field.GetValue(itemType))
                .Cast<T>()
                .ToList()
                .AsReadOnly();
        }


        public static EnumDisplay<T> CreateValueDisplayStruct<T>(this T value) where T : struct, IComparable, IFormattable, IConvertible
        {
            if (typeof(T).IsEnum)
            {
                var enumValue = (Enum)(object)value; // Safely cast value to Enum
                var description = enumValue.Description(); // Now you can call the Description method
                return new EnumDisplay<T>(value, description, System.Convert.ToInt32(enumValue));
            }
            throw new ArgumentException("T must be an enumerated type");
        }

        public static ReadOnlyCollection<EnumDisplay<T>> GenerateEnumDisplays<T>(bool sortByDisplay = false)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            var itemType = typeof(T);

            if (!itemType.IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            var values = GetValues<T>();
            var result = values
                .Select(v => v.CreateValueDisplayStruct())
                .ToList();

            if (sortByDisplay)
                result.Sort((p1, p2) => string.Compare(p1.Display, p2.Display, StringComparison.InvariantCultureIgnoreCase));

            return result.AsReadOnly();
        }

    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class ResourceDescriptionAttribute : DescriptionAttribute
    {
        private IResourceLocationService _resourceService;
        private readonly string _resourceKey;

        public ResourceDescriptionAttribute(string resourceKey)
        {
            _resourceKey = resourceKey;
            _resourceService = (IResourceLocationService)App.Current.ServiceProvider.GetService(typeof(IResourceLocationService));
        }

        public override string Description => _resourceService.GetEnumDisplayString(_resourceKey);
    }
}
