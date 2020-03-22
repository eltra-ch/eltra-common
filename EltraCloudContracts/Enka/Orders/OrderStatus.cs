namespace EltraCloudContracts.Enka.Orders
{
    /// <summary>
    /// Order Status
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined,
        /// <summary>
        /// Open
        /// </summary>
        Open,
        /// <summary>
        /// Accepted
        /// </summary>
        Assigned,
        /// <summary>
        /// Closed
        /// </summary>
        Closed,
        /// <summary>
        /// InProgress
        /// </summary>
        /// <summary>
        /// Refused
        /// </summary>
        Rejected,
        /// <summary>
        /// Expired
        /// </summary>
        Expired
    }
}
