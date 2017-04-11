using System;

namespace DynamicModelMigrator
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class StringLengthAttribute: Attribute
    {
        public int Length { get; set; }
        public StringLengthAttribute(int length)
        {
            Length = length;
        }

        public StringLengthAttribute()
        {

        }
    }
}
