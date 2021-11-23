using System;

namespace RdErp.Attributes
{
    /// <summary>
    /// A value of specified attribute.
    /// </summary>
    public class CustomAttributeValue
    {
        /// <summary>
        /// Gets or sets a string identifies the attribute.
        /// </summary>
        public string AttributeKey { get; set; }

        /// <summary>
        /// Gets or sets identifier of the entity which is value of the attribute.
        /// </summary>
        public int RefId { get; set; }
    }
}