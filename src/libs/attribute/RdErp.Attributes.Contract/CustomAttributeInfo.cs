using System;

using LinqToDB.Mapping;

namespace RdErp.Attributes
{
    /// <summary>
    /// A value of specified attribute.
    /// </summary>
    public class CustomAttributeInfo
    {
        /// <summary>
        /// Gets or sets a string identifies the attribute.
        /// </summary>
        public string AttributeKey { get; set; }

        /// <summary>
        /// Gets or sets identifier of the entity which is value of the attribute.
        /// </summary>
        public int RefId { get; set; }

        /// <summary>
        /// Gets or sets the title of the attribute value.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a string identifies a key of parent attribute or null if attribute hasn't parent.
        /// </summary>
        public string ParentAttributeKey { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent entity or null if attribute hasn't parent.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets a value indicates if current attribute value can be used in creating new entities.
        /// </summary>
        public bool IsActive { get; set; }
    }
}