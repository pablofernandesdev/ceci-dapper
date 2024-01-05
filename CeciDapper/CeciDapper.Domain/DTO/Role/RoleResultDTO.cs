namespace CeciDapper.Domain.DTO.Role
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the result of a role query.
    /// </summary>
    public class RoleResultDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the role.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        public string Name { get; set; }
    }
}
