using CA_Final_Persons_Reg_Sys.Data;
using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Model;
using CA_Final_Persons_Reg_Sys.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace CA_Final_Persons_Reg_Sys.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _context;
        public UserRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<long> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
        /* Using pagination snippet
        //
        //public async Task<IEnumerable<User>> GetAllAsync(int page = 1, int pageSize = 10)
        //{
        //    return await _context.Users
        //        .Include(pd => pd.UserPersonalData)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();
        //}
        */

        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _context.Users
                .Include(pd => pd.UserPersonalData)
                .ToListAsync();
        }
        
        public async Task<User?> GetByIdAsync(long id)
        {
            try
            {
                return await _context.Users
                    .Include(pd => pd.UserPersonalData)
                    .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            try 
            {
                var existingUser = await _context.Users
                .Include(pd => pd.UserPersonalData)
                .FirstOrDefaultAsync(u => u.UserName == userName);

                return existingUser;
            }

            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<string?> GetUserPictureUrl(long id)
        {
            try
            {
                var existingUser = await _context.Users
                    .Include(pd => pd.UserPersonalData)
                    .FirstOrDefaultAsync(u => u.Id == id);

                return existingUser.UserPersonalData.ProfilePicture;
            }
            catch (Exception ex)
            {
                return null;
            }
      
        }

        public async Task<bool> UpdateUserPassword(long id, byte[] passwordHash, byte[] passwordSalt)
        {
            try
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (existingUser == null)
                    return false;

                existingUser.PasswordHash = passwordHash;
                existingUser.PasswordSalt = passwordSalt;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log ex
                return false;
            }
        }
        public async Task<bool> UpdateUserNameAsync(long id, string name)
        {
            try
            {
                var existingUser = await _context.Users
                    .Include(pd => pd.UserPersonalData)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (existingUser == null)
                    return false;

                existingUser.UserPersonalData.Name = name;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log ex
                return false;
            }
        }

        public async Task<bool> UpdateUserLastNameAsync(long id, string lastName)
        {
            try
            {
                var existingUser = await _context.Users
                    .Include(pd => pd.UserPersonalData)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (existingUser == null)
                    return false;

                existingUser.UserPersonalData.LastName = lastName;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log ex
                return false;
            }
        }
        public async Task<bool> UpdateUserPersonalCodeAsync(long id, string personalCode)
        {
            try
            {
                var existingUser = await _context.Users
                    .Include(pd => pd.UserPersonalData)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (existingUser == null)
                    return false;

                existingUser.UserPersonalData.PersonalCode = personalCode;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log ex
                return false;
            }
        }

        public async Task<bool> UpdateUserPhoneNumberAsync(long id, string phoneNumber)
        {
            try
            {
                var existingUser = await _context.Users
                    .Include(pd => pd.UserPersonalData)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (existingUser == null)
                    return false;

                existingUser.UserPersonalData.PhoneNumber = phoneNumber;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log ex
                return false;
            }
        }

        public async Task<bool> UpdateUserEmailAsync(long id, string email)
        {
            try
            {
                var existingUser = await _context.Users
                    .Include(pd => pd.UserPersonalData)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (existingUser == null)
                    return false;

                existingUser.UserPersonalData.Email = email;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log ex
                return false;
            }
        }

        public async Task<bool> UpdateUserCityNameAsync(long id, string city)
        {
            try
            {
                var existingUser = await _context.Users
                    .Include(pd => pd.UserPersonalData)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (existingUser == null)
                    return false;

                existingUser.UserPersonalData.CityName = city;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log ex
                return false;
            }
        }

        public async Task<bool> UpdateUserStreetNameAsync(long id, string street)
        {
            try
            {
                var existingUser = await _context.Users
                    .Include(pd => pd.UserPersonalData)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (existingUser == null)
                    return false;

                existingUser.UserPersonalData.StreetName = street;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log ex
                return false;
            }
        }

        public async Task<bool> UpdateUserHouseNumberAsync(long id, string houseNumber)
        {
            try
            {
                var existingUser = await _context.Users
                    .Include(pd => pd.UserPersonalData)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (existingUser == null)
                    return false;

                existingUser.UserPersonalData.HouseNumber = houseNumber;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log ex
                return false;
            }
        }

        public async Task<bool> UpdateUserApartmentNumberAsync(long id, string apartmentNumber)
        {
            try
            {
                var existingUser = await _context.Users
                    .Include(pd => pd.UserPersonalData)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (existingUser == null)
                    return false;

                existingUser.UserPersonalData.ApartmentNumber = apartmentNumber;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log ex
                return false;
            }
        }

        public async Task<bool> UpdateUserPicture(long id, string pictureFileName)
        {
            try
            {
                var existingUser = await _context.Users
                    .Include(pd => pd.UserPersonalData)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (existingUser == null)
                    return false;

                existingUser.UserPersonalData.ProfilePicture = pictureFileName;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log ex
                return false;
            }
        }


        public async void DeleteAsync(long id)
        {
            try
            {
                var user = await _context.Users
                    .FirstAsync(u => u.Id == id);
                
                if(user == null)
                    return;

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            { 
                
            }
        
        }
    }
}

