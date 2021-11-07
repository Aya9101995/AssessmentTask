using API.Models.APIBaseModel;
using Core.Models.Modules.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.APIs.UsersAPI
{
    public class UsersAPI
    {
        public UserResponseAPI Login(UsersRequestAPI oUsersRequestAPI)
        {
            UsersCOM oUsersCOM = new UsersCOM();
            UserResponseAPI oUserResponseAPI = new UserResponseAPI();

            oUserResponseAPI.oUser = oUsersCOM.Login(oUsersRequestAPI.Email, oUsersRequestAPI.Password, oUsersRequestAPI.DeviceToken);
            if (oUserResponseAPI.oUser.UserID > 0)
            {
                oUserResponseAPI.APIStatus = 1;
                oUserResponseAPI.APIMessage = "Success";
            }
            else
            {
                oUserResponseAPI.APIStatus = -1;
                oUserResponseAPI.APIMessage = "wrong Email or password";
            }
            return oUserResponseAPI;
        }

        public UserResponseAPI GetUserProfile(UsersRequestAPI oUsersRequestAPI)
        {
            UsersCOM oUsersCOM = new UsersCOM();
            UserResponseAPI oUserResponseAPI = new UserResponseAPI();
            long UserID = UsersCOM.GetUserIDByToken(oUsersRequestAPI.APIToken);
            oUserResponseAPI.oUser = oUsersCOM.GetUserProfile(UserID);
            if (oUserResponseAPI.oUser != null)
            {
                oUserResponseAPI.APIStatus = 1;
                oUserResponseAPI.APIMessage = "Success";
            }
            else
            {
                oUserResponseAPI.APIStatus = -3;
                oUserResponseAPI.APIMessage = "Something went wrong";
            }
            return oUserResponseAPI;
        }

        public UsersResponseAPI GetAllUsers(UsersRequestAPI oUsersRequestAPI)
        {
            UsersCOM oUsersCOM = new UsersCOM();
            UsersResponseAPI oUsersResponseAPI = new UsersResponseAPI();
            oUsersResponseAPI.lstUsers = oUsersCOM.GetAllUsers(oUsersRequestAPI.PageID, oUsersRequestAPI.PageSize);
            if (oUsersResponseAPI.lstUsers.Count >= 0)
            {
                oUsersResponseAPI.APIStatus = 1;
                oUsersResponseAPI.APIMessage = "Success";
            }
            else
            {
                oUsersResponseAPI.APIStatus = -3;
                oUsersResponseAPI.APIMessage = "Something went wrong";
            }
            return oUsersResponseAPI;
        }

        public UserResponseAPI Register(UsersRequestAPI oUsersRequestAPI)
        {
            UsersCOM oUsersCOM = new UsersCOM();
            UserResponseAPI oUserResponseAPI = new UserResponseAPI();

            bool IsRegistered = oUsersCOM.Register(oUsersRequestAPI.oUser);
            if (IsRegistered == true)
            {
                oUserResponseAPI.APIStatus = 1;
                oUserResponseAPI.APIMessage = "Success";
            }
            else
            {
                oUserResponseAPI.APIStatus = -3;
                oUserResponseAPI.APIMessage = "Something went wrong";
            }
            return oUserResponseAPI;
        }

        public UserResponseAPI UpdateUserProfile(UsersRequestAPI oUsersRequestAPI)
        {
            UsersCOM oUsersCOM = new UsersCOM();
            UserResponseAPI oUserResponseAPI = new UserResponseAPI();
            oUsersRequestAPI.oUser.UserID = UsersCOM.GetUserIDByToken(oUsersRequestAPI.APIToken);
            oUserResponseAPI.oUser = oUsersCOM.UpdateUserProfile(oUsersRequestAPI.oUser);
            if (oUserResponseAPI.oUser != null)
            {
                oUserResponseAPI.APIStatus = 1;
                oUserResponseAPI.APIMessage = "Success";
            }
            else
            {
                oUserResponseAPI.APIStatus = -3;
                oUserResponseAPI.APIMessage = "Something went wrong";
            }
            return oUserResponseAPI;
        }

        public UserResponseAPI Logout(UsersRequestAPI oUsersRequestAPI)
        {
            UsersCOM oUsersCOM = new UsersCOM();
            UserResponseAPI oUserResponseAPI = new UserResponseAPI();
            long UserID = UsersCOM.GetUserIDByToken(oUsersRequestAPI.APIToken);
            bool IsLoggedOut = oUsersCOM.Logout(UserID);
            if (IsLoggedOut == true)
            {
                oUserResponseAPI.APIStatus = 1;
                oUserResponseAPI.APIMessage = "Success";
            }
            else
            {
                oUserResponseAPI.APIStatus = -3;
                oUserResponseAPI.APIMessage = "Something went wrong";
            }
            return oUserResponseAPI;
        }

    }
    public class UsersRequestAPI : APIModel
    {
        public UsersModel oUser { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DeviceToken { get; set; }

    }
    public class UserResponseAPI : APIModel
    {
        public UsersModel oUser { get; set; }
    }
    public class UsersResponseAPI : APIModel
    {
        public List<UsersModel> lstUsers { get; set; }
    }
}