using DomainModel.DTO.UserModel;

namespace UnitTest.Models.User
{
    public class UserListAndSearchModel
    {
        public UserSearchModel sm { get; set; }
        public List<UserListItem> UserListItems { get; set; }
    }
}
