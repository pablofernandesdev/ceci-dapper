using System;
using TimeZoneConverter;

namespace CeciDapper.Domain.Entities
{
    /// <summary>
    /// Represents a base entity that provides common properties for all entities.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntity"/> class.
        /// Sets default values for common properties.
        /// </summary>
        public BaseEntity()
        {
            Active = true;
            RegistrationDate = TimeZoneInfo.ConvertTime(DateTime.Now, TZConvert.GetTimeZoneInfo("E. South America Standard Time"));
        }

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public virtual bool Active { get; set; }

        /// <summary>
        /// Gets or sets the registration date of the entity.
        /// The date is converted to the "E. South America Standard Time" time zone.
        /// </summary>
        public virtual DateTime RegistrationDate { get; set; }
    }
}
