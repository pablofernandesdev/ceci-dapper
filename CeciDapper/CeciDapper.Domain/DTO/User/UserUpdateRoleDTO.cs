namespace CeciDapper.Domain.DTO.User
{
    /// <summary>
    /// Data Transfer Object (DTO) representing user information for updating user role.
    /// </summary>
    public class UserUpdateRoleDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user whose role is to be updated.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the new role identifier to be assigned to the user.
        /// </summary>
        public int RoleId { get; set; }
    }
}
