using DemoApi.Models;
using DemoApi.Repositories;

namespace DemoApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            ValidateUser(user);
            return await _userRepository.UpdateAsync(id, user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        private static void ValidateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                throw new ArgumentException("User name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(user.Email) || !user.Email.Contains('@'))
            {
                throw new ArgumentException("A valid email is required.");
            }
        }
    }
}
