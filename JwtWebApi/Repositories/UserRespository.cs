using JwtWebApi.Models;

namespace JwtWebApi.Repositories
{
        public class UserRespository
        {
                private int _id;
                private List<User> _userList;
                private List<UserProfile> _userProfileList;

                public UserRespository()
                {
                        _id = 0;
                        _userList = new List<User>();
                        _userProfileList = new List<UserProfile>();
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

                        UserProfile newUserProfile = new UserProfile()
                        {
                                Username = username,
                        };

                        _userList.Add(newUser);
                        _userProfileList.Add(newUserProfile);

                        return false;
                }

                public UserProfile? GetUserProfile(string username)
                {
                        if (username == null)
                        {
                                return null;
                        }

                        return _userProfileList.Find(user => user.Username == username);
                }

                public UserProfile? UpdateUserProfile(UserProfile userProfile)
                {
                        UserProfile? user = GetUserProfile(userProfile.Username);

                        if (user == null)
                        {
                                return null;
                        }

                        int index = _userProfileList.FindIndex(u => u.Username == user.Username);
                        _userProfileList[index] = userProfile;

                        index = _userList.FindIndex(u => u.Username == user.Username);
                        _userList[index].Username = userProfile.Username;
                        return userProfile;
                }
        }
}
