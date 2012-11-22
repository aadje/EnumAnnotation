﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EnumAnnotations
{
    /// <summary>
    /// Enum wrapper for more conviently accessing the Data Annotations Attributes (only the Display Attribute is supported) 
    /// </summary>
    public class EnumAnnotation<T> : IDisplayAnnotation where T : struct
    {
        private readonly T _enumValue;
        private DisplayAttribute _display;

        /// <summary>
        /// Cached and Lazy loaded DisplayAttribute
        /// </summary>
        private DisplayAttribute Display
        {
            get
            {
                return _display ?? (_display = GetDisplayAttribute());
            }
        }

        /// <summary>
        /// DisplayAttribute.Name annotation. Returns Enum.ToString() if not specified.
        /// </summary>
        public string Name
        {
            get
            {
                return Display == null || string.IsNullOrEmpty(Display.Name) ? ToString() : Display.GetName();
            }
        }

        /// <summary>
        /// DisplayAttribute.ShortName annotation. Returns Enum.ToString() if not specified.
        /// </summary>
        public string ShortName
        {
            get { return Display == null || string.IsNullOrEmpty(Display.ShortName) ? ToString() : Display.GetShortName(); }
        }

        /// <summary>
        /// DisplayAttribute.ShortName annotation. Returns an empty string if not specified.
        /// </summary>
        public string GroupName
        {
            get { return Display == null ? string.Empty : Display.GetGroupName(); }
        }

        /// <summary>
        /// DisplayAttribute.Description annotation. Returns an empty string if not specified.
        /// </summary>
        public string Description
        {
            get { return Display == null || string.IsNullOrEmpty(Display.Description) ? string.Empty : Display.GetDescription(); }
        }

        /// <summary>
        /// DisplayAttribute.Order annotation. Returns 0 if not specified.
        /// </summary>
        public int Order
        {
            get
            {
                int? order = null;
                if(Display != null)
                    order = Display.GetOrder();
                return Display == null || !order.HasValue ? 0 : order.Value;
            }
        }

        /// <summary>
        /// Enum original value as object
        /// </summary>
        public object Value
        {
            get { return _enumValue; }
        }

        /// <summary>
        /// Enum original value 
        /// </summary>
        public T EnumValue
        {
            get { return _enumValue; }
        }

        /// <summary>
        /// Enum Underlying numeric value casted to int
        /// </summary>
        public int UnderlyingValue
        {
            get { return (int)Value; }
        }

        /// <summary>
        /// Wrap an Enum in a EnumAnnotation for more conviently accessing the Annotations Attributes (only the Display Attribute is supported) 
        /// </summary>
        /// <param name="enumvalue">An Enum value of Type T</param>
        public EnumAnnotation(T enumvalue)
        {
            _enumValue = enumvalue;
        }

        private DisplayAttribute GetDisplayAttribute()
        {
            Type type = typeof(T);
            if (!type.IsEnum)
                throw new NotSupportedException(type.FullName);
            
            string name = Enum.GetName(type, _enumValue);
            
            var field = type.GetField(name);

            return field.GetCustomAttributes(true).OfType<DisplayAttribute>().SingleOrDefault();
        }

        /// <summary>
        /// Enum value casted to string
        /// </summary>
        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj != null && Value.Equals(((EnumAnnotation<T>)obj).Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    /// <summary>
    /// Static methods for getting EnumAnnotations
    /// </summary>
    public static class EnumAnnotation
    {
        /// <summary>
        /// Get a Sorted list of all the Display Attribute Annotations for the values in Enum Type of T. Usefull for datasources in databound controls. 
        /// </summary>
        /// <param name="predicate">Optional filter expression parameter for removing values</param>
        /// <returns>A sorted list of EnumAnnotations for Enum Type of T </returns>
        public static List<IDisplayAnnotation> GetDisplays<T>(Func<EnumAnnotation<T>, bool> predicate = null) where T : struct
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(v => new EnumAnnotation<T>(v))
                .Where(predicate ?? (x => true))
                .Cast<IDisplayAnnotation>()
                .OrderBy(a => a.Order)
                .ToList();
        }

        /// <summary>
        /// Get a Sorted list of Display Attribute Annotations of Enum Type of T for the supplied Enum values.
        /// </summary>
        /// <returns>A sorted list of EnumAnnotations for Enum Type of T </returns>
        /// <param name="args">Enum Type of T values</param>
        public static List<IDisplayAnnotation> GetDisplays<T>(params T[] args) where T : struct 
        {
            return args
                .Select(v => new EnumAnnotation<T>(v))
                .Cast<IDisplayAnnotation>()
                .ToList();
        }

        /// <summary>
        /// Try find a single Enum value wrapped in an EnumAnnotation by its Display attribute fields with a filter expression for Enum Type of T
        /// </summary>
        /// <param name="predicate">filter expression</param>
        /// <returns>EnumAnnotations for Enum Type of T or Null</returns>
        public static IDisplayAnnotation FindDisplay<T>(Func<EnumAnnotation<T>, bool> predicate) where T : struct 
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(v => new EnumAnnotation<T>(v))
                .SingleOrDefault(predicate);
        }

        /// <summary>
        /// Get a list of Enums
        /// </summary>
        public static List<T> GetEnums<T>() where T : struct 
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        /// <summary>
        /// Extension method which wraps a single enum value in an EnumAnnotation
        /// </summary>
        /// <param name="value">Enum value of Type of T</param>
        /// <returns>IDisplayAnnotation for Enum Type of T</returns>
        public static IDisplayAnnotation GetDisplay<T>(this T value) where T : struct
        {
            return new EnumAnnotation<T>(value);
        }

        /// <summary>
        /// Extension method which returns the DisplayAttributes Name value
        /// </summary>
        /// <param name="value">Enum value of Type of T</param>
        /// <returns>IDisplayAnnotation for Enum Type of T</returns>
        public static string GetName(this Enum value)
        {
            if (value == null)
                return null;
            var displayAttribute = GetDisplayAttribute(value);
            return displayAttribute != null ? displayAttribute.Name : value.ToString();
        }

        /// <summary>
        /// Extension method which returns the DisplayAttributes Name value or the the defaultName if the Enum value is null
        /// </summary>
        /// <param name="value">Nullable Enum value of Type of T</param>
        /// <param name="defaultName">The name to use when this nullable value has no value</param>
        /// <returns>IDisplayAnnotation for Enum Type of T</returns>
        public static string GetName(this Enum value, string defaultName)
        {
            return value == null ? defaultName : GetName(value);
        }

        private static DisplayAttribute GetDisplayAttribute(Enum enumValue)
        {
            Type type = enumValue.GetType();
            string name = Enum.GetName(type, enumValue);
            var field = type.GetField(name);
            return field.GetCustomAttributes(true).OfType<DisplayAttribute>().SingleOrDefault();
        }
    }

    /// <summary>
    /// Wrapper around the System.ComponentModel.DataAnnotations.DisplayAttribute and Enum Properties
    /// </summary>
    public interface IDisplayAnnotation
    {
        /// <summary>
        /// DisplayAttribute.Name annotation
        /// </summary>
        string Name { get; }

        /// <summary>
        /// DisplayAttribute.ShortName annotation
        /// </summary>
        string ShortName { get; }

        /// <summary>
        /// DisplayAttribute.GroupName annotation
        /// </summary>
        string GroupName { get; }

        /// <summary>
        /// DisplayAttribute.Description annotation
        /// </summary>
        string Description { get; }

        /// <summary>
        /// DisplayAttribute.Order annotation
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Enum original value
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Enum Underlying numeric value casted to int
        /// </summary>
        int UnderlyingValue { get; }
    }
}    