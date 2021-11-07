using Core.Models.DBClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Core.Models.Modules.Users
{
    public class UsersCOM
    {
        public static string GenerateAPIToken()
        {
            var key = new byte[2048];
            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(key);
            string apiKey = Convert.ToBase64String(key);
            return apiKey;
        }
        public bool Register(UsersModel oModel)
        {
            using (CoreEntity db = new CoreEntity())
            {
                UsersTbl oUser = db.Users.Where(x => x.Email == oModel.Email).FirstOrDefault();
                if (oUser == null || (oUser != null && oUser.IsDeleted))
                {
                    bool IsNew = oUser == null ? true : false;
                    if (IsNew)
                        oUser = new UsersTbl();
                    oUser.Name = oModel.Name;
                    oUser.Email = oModel.Email;
                    oUser.Password = oModel.Password;
                    oUser.DefaultLanguageID = oModel.DefaultLanguageID;
                    oUser.IsDeleted = false;
                    if (IsNew)
                        db.Users.Add(oUser);
                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }
        public UsersModel Login(string Email, string Password, string DeviceToken)
        {
            using (CoreEntity db = new CoreEntity())
            {
                UsersTbl oUser = db.Users.Where(x => x.Email == Email && x.Password == Password).FirstOrDefault();
                if (oUser != null)
                {
                    UserDevicesTbl oDevice = db.UserDevices.Where(x => x.UserID == oUser.ID).FirstOrDefault();
                    bool IsNew = oDevice == null ? true : false;
                    if (IsNew)
                        oDevice = new UserDevicesTbl();
                    oDevice.ApiToken = GenerateAPIToken();
                    oDevice.UserID = oUser.ID;
                    oDevice.DeviceToken = DeviceToken;
                    if (IsNew)
                        db.UserDevices.Add(oDevice);
                    db.SaveChanges();
                    return GetUserProfile(oUser.ID);
                }
                else
                    return new UsersModel();
            }


        }
        public List<UsersModel> GetAllUsers(int PageID, int PageSize)
        {
            using (CoreEntity db = new CoreEntity())
            {
                List<UsersModel> lstUsers = (from user in db.Users
                                             select new UsersModel()
                                             {
                                                 UserID = user.ID,
                                                 DefaultLanguageID = user.DefaultLanguageID,
                                                 Email = user.Email,
                                                 Password = user.Password,
                                                 Name = user.Name
                                             }).OrderBy(x=>x.UserID).Skip((PageID - 1) * PageSize).Take(PageSize).ToList();
                return lstUsers;
            }
        }
        public UsersModel GetUserProfile(long UserID)
        {
            using (CoreEntity db = new CoreEntity())
            {
                UsersModel oCustomer = (from user in db.Users
                                        join device in db.UserDevices on user.ID equals device.UserID
                                        where user.ID == UserID
                                        select new UsersModel()
                                        {
                                            UserID = user.ID,
                                            DefaultLanguageID = user.DefaultLanguageID,
                                            Email = user.Email,
                                            Name = user.Name,
                                            Password = user.Password,
                                            oDevice = new UserDevicesModel()
                                            {
                                                ApiToken = device.ApiToken,
                                                UserDeviceID = device.ID,
                                                UserID = device.UserID,
                                                DeviceToken = device.DeviceToken
                                            }
                                        }).FirstOrDefault();
                return oCustomer;
            }
        }
        public UsersModel UpdateUserProfile(UsersModel oModel)
        {
            using (CoreEntity db = new CoreEntity())
            {
                UsersTbl oUser = new UsersTbl();
                if (oModel.UserID > 0)
                {
                    oUser = db.Users.FirstOrDefault(x => x.ID == oModel.UserID);
                    oUser.DefaultLanguageID = oModel.DefaultLanguageID;
                    oUser.Email = oModel.Email;
                    oUser.Name = oModel.Name;
                    oUser.Password = oModel.Password;
                    db.SaveChanges();
                    UserDevicesTbl oDevice = db.UserDevices.FirstOrDefault(x => x.UserID == oUser.ID);
                    if (oDevice == null)
                    {
                        oDevice = new UserDevicesTbl();
                    }
                    oDevice.ApiToken = GenerateAPIToken();
                    oDevice.DeviceToken = oModel.oDevice.DeviceToken;
                    if (oDevice.ID == 0)
                    {
                        oDevice.UserID = oUser.ID;
                        db.UserDevices.Add(oDevice);
                    }
                    db.SaveChanges();
                    return GetUserProfile(oUser.ID);
                }
                else
                    return new UsersModel();
            }
        }
        public bool Logout(long UserID)
        {
            using (CoreEntity db = new CoreEntity())
            {
                UserDevicesTbl oUserDevice = db.UserDevices.FirstOrDefault(x => x.UserID == UserID);
                if (oUserDevice != null)
                {
                    db.UserDevices.Remove(oUserDevice);
                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }
        public static long GetUserIDByToken(string Token)
        {
            using (CoreEntity db = new CoreEntity())
            {
                return (from device in db.UserDevices
                        join user in db.Users on device.UserID equals user.ID
                        where device.ApiToken == Token
                        select device.UserID).FirstOrDefault();
            }
        }

    }
}