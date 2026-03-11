using System;

namespace DIContainer
{
    [AttributeUsage(AttributeTargets.Field | 
                    AttributeTargets.Property | 
                    AttributeTargets.Constructor |
                    AttributeTargets.Method)]
    public class InjectAttribute : Attribute
    {
        
    }
}