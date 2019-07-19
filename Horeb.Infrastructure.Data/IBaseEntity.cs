using System;


namespace Horeb.Infrastructure.Data
{
    public interface IBaseEntity : IActivityDetails
    {
        string Name { get; set; }
    }

    /// <summary>
    ///     Provides a base class for your objects which will be persisted to the database.
    ///     Benefits include the addition of an Id property along with a consistent manner for comparing
    ///     entities.
    /// </summary>
    public abstract class BaseEntity : ValueClass<string>, IBaseEntity 
    {
        public virtual void InitializeEntity()
        {
            Name = string.Empty;
            Id = string.Empty;
            CreatedOn = DateTime.Now;
            LastestUpdateOn = DateTime.Now;
            CreatedById = string.Empty;
            LastestUpdateById = string.Empty;
        }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastestUpdateOn { get; set; }

        public string CreatedById { get; set; }

        public string LastestUpdateById { get; set; }
    }    
}