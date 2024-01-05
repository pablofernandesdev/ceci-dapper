namespace CeciDapper.Domain.DTO.Role
{
    /// <summary>
    /// Data Transfer Object (DTO) representing the update of a role.
    /// </summary>
    public class RoleUpdateDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the role.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the new name of the role.
        /// </summary>
        public string Name { get; set; }
    }
}
