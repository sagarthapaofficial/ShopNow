﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopNow.DAL;
using ShopNow.DAL.DAO;
using ShopNow.DAL.DomainClasses;
using ShopNow.Helpers;
using System.Security.Cryptography;

namespace ShopNow.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        AppDbContext _db;
        public RegisterController(AppDbContext context)
        {
            _db = context;
        }
        //will allow the customer's to go to the register page.
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<CustomerHelper>> Index(CustomerHelper helper)
        {
            CustomerDAO dao = new CustomerDAO(_db);
            Customer already = await dao.GetByEmail(helper.email);
            if (already == null)
            {
                HashSalt hashSalt = GenerateSaltedHash(64, helper.password);
                helper.password = ""; // don’t need the string anymore
                Customer dbCustomer = new Customer();
                dbCustomer.Firstname = helper.firstname;
                dbCustomer.Lastname = helper.lastname;
                dbCustomer.Email = helper.email;
                dbCustomer.Hash = hashSalt.Hash;
                dbCustomer.Salt = hashSalt.Salt;
                dbCustomer = await dao.Register(dbCustomer);
                if (dbCustomer.Id > 0)
                    helper.token = "Customer registered";
                else
                    helper.token = "Customer registration failed";
            }
            else
            {
                helper.token = "Customer registration failed - email already in use";
            }
            return helper;
        }

        private static HashSalt GenerateSaltedHash(int size, string password)
        {
            var saltBytes = new byte[size];
            var provider = new RNGCryptoServiceProvider();
            // Fills an array of bytes with a cryptographically strong sequence of random nonzero values.
            provider.GetNonZeroBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);
            // a password, salt, and iteration count, then generates a binary key
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            HashSalt hashSalt = new HashSalt { Hash = hashPassword, Salt = salt };
            return hashSalt;
        }


        //HasSalt class with 2 properties
        public class HashSalt
        {
            public string Hash { get; set; }
            public string Salt { get; set; }
        }

    }
}
