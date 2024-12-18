using CA_Final_Persons_Reg_Sys.Data;
using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Model;
using CA_Final_Persons_Reg_Sys.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingUser = await _context.Users.AnyAsync(u => u.UserName == user.UserName);
            if (existingUser)
                throw new ArgumentException("User with such name already exists");

            if (user.UserPersonalData == null)
                throw new ArgumentException("User does not contain personal information");

            var personalDataRequest = user.UserPersonalData;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _context.Users
                .Include(pd => pd.UserPersonalData)
                .ToListAsync();
        }
        //// Using pagination
        //public async Task<IEnumerable<User>> GetAllAsync(int page = 1, int pageSize = 10)
        //{
        //    return await _context.Users
        //        .Include(pd => pd.UserPersonalData)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();
        //}

        public async Task<User?> GetByIdAsync(long id)
        {
            return await _context.Users
                .Include(pd => pd.UserPersonalData)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUserName(string userName)
        {
            var existingUser = await _context.Users
                .Include (pd => pd.UserPersonalData)
                .FirstOrDefaultAsync(u => u.UserName == userName);
            
            return existingUser;
        }

        public async Task<bool> UpdateAsync(long id, UserRequest request)  //id nusirodom is FrontEnd
        {
            var existingUser = await _context.Users
                .Include (pd => pd.UserPersonalData)
                .FirstOrDefaultAsync(u => u.Id == id);
            
            if (existingUser == null)
                return false;

            //Update user personal data
            var personalDataRequest = request.userPersonalDataRequest;

            existingUser.UserPersonalData = new UserPersonalData
            {
                Name = personalDataRequest.Name,
                LastName = personalDataRequest.LastName,
                PersonalCode = personalDataRequest.PersonalCode,
                PhoneNumber = personalDataRequest.PhoneNumber,
                Email = personalDataRequest.Email,
                ProfilePicture = personalDataRequest.ProfilePicture,
                CityName = personalDataRequest.CityName,
                StreetName = personalDataRequest.StreetName,
                HouseNumber = personalDataRequest.HouseNumber,
                ApartmentNumber = personalDataRequest.ApartmentNumber
            };

            await _context.SaveChangesAsync();
            return true;
        }

       

        public async Task<bool> DeleteAsync(long id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();  //SaveChangesAsync can produce an exception
            return true;
        }

        public async Task<bool> UpdatePersonalDataPropertyAsync(long id, string propertyName, object value)
        {
            var existingUser = await _context.Users
                .Include(pd => pd.UserPersonalData)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (existingUser == null || existingUser.UserPersonalData == null)
                return false;

            var userPersonalData = existingUser.UserPersonalData;

            //possible null exception, existingUser.UserPersonalData == null
            var propInfo = existingUser.UserPersonalData.GetType().GetProperty(propertyName);

            //compile-time check, manau netinka cia, nes ateina is controller
            //var propInfo = typeof(UserPersonalData).GetProperty(propertyName);

            if (propInfo == null)
                return false;   //bad request


            //Value type check? Need validation
            var propertyType = propInfo.PropertyType;
            if (value == null || !(propertyType.IsInstanceOfType(value)))
                return false;

            propInfo.SetValue(userPersonalData, value);

            _context.Entry(existingUser.UserPersonalData).Property(propertyName).IsModified = true;
            //try catch external resource, use logger
            await _context.SaveChangesAsync();
            return true;

            //Delete cascades together with associated User, see FluentApi @Data/UsersDbContext
        }

    }
}
