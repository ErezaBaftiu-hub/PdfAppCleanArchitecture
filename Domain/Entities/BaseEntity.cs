using System;

namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// Insert date for entries 
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// Update date for entries
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        public Guid InsertedBy { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        public Guid UpdatedBy { get; set; }

    }
}
