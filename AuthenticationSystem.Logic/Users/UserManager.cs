using AuthenticationSystem.Domain;
using AuthenticationSystem.Domain.Services;
using AuthenticationSystem.Logic.Exceptions;
using AuthenticationSystem.Shared;
using Microsoft.Extensions.Logging;
using UserAuthenticationSystem.Domain.Core;
using Microsoft.EntityFrameworkCore;
using FluentResults;

namespace AuthenticationSystem.Logic.Users
{
    public class UserManager : IUserManager<User, CreateUserRequest>
    {
        private readonly ILogger<UserManager> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UserManager(ILogger<UserManager> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public Task<User> ChangePassword(string email, string newPassowrd)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<User>> CreateUser(CreateUserRequest user)
        {
            try
            {
                var newUser = new User
                {
                    EmailHash = HashGenerator.GetHash(user.Email, HashSalt.SALT),
                    PasswordHash = HashGenerator.GetHash(user.Password, HashSalt.SALT),
                    Surname = user.Surname,
                    Name = user.Name,
                };

                _unitOfWork.Repository<User>().Create(newUser);
                await _unitOfWork.SaveAsync();

                return newUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionMessages.CREATE_USER_ERROR);
                throw;
            }
        }

        public async Task DeleteUser(Guid userId)
        {
            try
            {
                var user = await _unitOfWork.Repository<User>().FindAsync(userId);
                if (user is null)
                {
                    throw new EntityNotFoundException(typeof(User), userId);
                }

                _unitOfWork.Repository<User>().Remove(user);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionMessages.DELETE_USER_ERROR);
                throw;
            }
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            try
            {
                var emailHash = HashGenerator.GetHash(email, HashSalt.SALT);
                return await _unitOfWork.Repository<User>().Query.FirstOrDefaultAsync(u => u.EmailHash == emailHash);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<User>> FindByFullNameAsync(string name, string surname)
        {
            try
            {
                return await _unitOfWork.Repository<User>().Query.Where(u => u.Name == name && u.Surname == surname).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<User> FindByIdAsync(Guid Id)
        {
            try
            {
                return await _unitOfWork.Repository<User>().FindAsync(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<Result<User>> LoginAsync(string email, string password)
        {
            try
            {

                var user = await FindByEmailAsync(email);
                if (user is null)
                {
                    return Result.Fail("User not found");
                }

                var passHash = HashGenerator.GetHash(password, HashSalt.SALT);

                if (passHash != user.PasswordHash)
                {
                    return Result.Fail("Incorrect password");
                }

                return Result.Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
