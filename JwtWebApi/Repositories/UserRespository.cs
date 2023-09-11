using JwtWebApi.Models;

namespace JwtWebApi.Repositories
{
        public class UserRespository
        {
                private int _id;
                private List<User> _userList;
                private List<UserProfileDto> _userProfileList;

                public UserRespository()
                {
                        _id = 0;
                        _userList = new List<User>();
                        _userProfileList = new List<UserProfileDto>();
                }

                public User? AuthLogin(UserDto request)
                {
                        User? user = _userList.Find(u => u.Username == request.Username);

                        if (user == null)
                        {
                                return null;
                        }
                        return user;
                }

                public bool CreateUser(string username, byte[] passwordHash, byte[] passwordSalt)
                {
                        if (_userList.Exists(u => u.Username == username))
                        {
                                return true;
                        }

                        User newUser = new User()
                        {
                                Id = _id++,
                                Username = username,
                                PasswordHash = passwordHash,
                                PasswordSalt = passwordSalt
                        };

                        UserProfileDto newUserProfile = new UserProfileDto()
                        {
                                Username = username,
                        };

                        _userList.Add(newUser);
                        _userProfileList.Add(newUserProfile);

                        return false;
                }

                public UserProfileDto? GetUserProfile(string username)
                {
                        if (username == null)
                        {
                                return null;
                        }

                        return _userProfileList.Find(user => user.Username == username);
                }

                public EditUserDto? UpdateUserProfile(string username, EditUserDto editUser)
                {
                        UserProfileDto? user = GetUserProfile(username);

                        if (user == null)
                        {
                                return null;
                        }

                        int index = _userProfileList.FindIndex(u => u.Username == username);
                        _userProfileList[index] = new UserProfileDto
                        {
                                Username = username,
                                FirstName = editUser.FirstName,
                                LastName = editUser.LastName,
                                Email = editUser.Email,
                        };
                        return editUser;
                }
        }
}
