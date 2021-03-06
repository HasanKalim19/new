﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Common;
using Logger;
using Common.Models;
using System.Linq;

namespace DAL
{
    /// <summary>
    /// Manages all the CRUD operations performed on database.
    /// </summary>
    public class DbManager
    {
        #region Data Members
        /// <summary>
        /// An object connection to the database.
        /// </summary>
        private SqlConnection _con;
        /// <summary>
        /// A string containing the connection string of the database
        /// </summary>
        private string _conStr;
        /// <summary>
        /// An object holds the transaction object.
        /// </summary>
        private SqlTransaction _transaction;

        #endregion

        #region Stored Procedures
        private const string ProcInsertMembersRequestsLog = "spne_InsertMembersRequestsLog";
        private const string ProcCheckUserExistanceByPhoneNumber = "spne_CheckUserExistanceByPhoneNumber";
        private const string ProcDeleteUserBlockedStateByPhoneNumber = "spne_DeleteUserBlockedStateByPhoneNumber";
        private const string ProcGetContactsExistanceAndRosterStatusByContactsList = "spne_GetContactsExistanceAndRosterStatusByContactsList";
        private const string ProcDoesMembersRequestsExist = "spne_IsMembersRequestsExist";
        private const string ProcGetUserBlockingStateByPhoneNumber = "spne_GetUserBlockingStateByPhoneNumber";
        private const string ProcUpdateUserDeviceInfoByPhoneNumber = "spne_UpdateUserDeviceInfoByPhoneNumber";
        private const string ProcUpdateUserDeviceTokenByPhoneNumber = "spne_UpdateUserDeviceTokenByPhoneNumber";
        private const string ProcUpdateUserNameByPhoneNumber = "spne_UpdateUserNameByPhoneNumber";
        private const string ProcGetUserProfileNameByUserID = "spne_GetUserProfileNameByUserID";
        private const string ProcGetUserInfoByCallerIDAndReceiverIDWithOfflineMsgCount = "spne_GetUserInfoByCallerIDAndReceiverIDWiTHOfflineMsgCount";
        private const string ProcUpdateUserSettingsByUserID = "spne_UpdateUserSettingsByUserID";
        private const string ProcInsertSharedFileInformationByUserID = "spne_InsertSharedFileInformationByUserID";
        private const string ProcUpdateSharedFileDownloadCountByFileID = "spne_UpdateSharedFileDownloadCountByFileID";
        private const string ProcDeleteSharedFileByRecordIDs = "spne_DeleteSharedFileByRecordIDs";
        private const string ProcGetClientAppVersionByReceiverID = "spne_GetClientAppVersionByReceiverID";
        private const string ProcGetAppVersionByContactList = "spne_GetAppVersionByContactList";
        private const string ProcGetUserInfoByUserID = "spne_GetUserInfoByUserID";
        private const string ProcGetGroupParticipantsDataByRoomIDnSenderID = "spne_GetGroupParticipantsDataByRoomIDnSenderID";
        private const string ProcUpdateOfflineUserCountByUserList = "spne_UpdateOfflineUserCountByUserList";
        private const string ProcResetOffineMessageCountByUserID = "spne_ResetOffineMessageCountByUserID";
        private const string ProcUpdateFileSharedDatenRecipientCountByFileID = "spne_UpdateFileSharedDatenRecipientCountByFileID";
        private const string ProcGetGroupParticipantsDatanRoomDataByRoomNamenSenderID = "spne_GetGroupParticipantsDatanRoomDataByRoomNamenSenderID";
        private const string ProcGetSharedFilesByDate = "spne_GetSharedFilesByDate";
        private const string ProcGetUserGroupsDetailsByUserID = "spne_GetUserGroupsDetailsByUserID";
        private const string ProcGetLastSeenTimeByUserIDnResource = "spne_GetLastSeenTimeByUserIDnResource";
        private const string ProcInsertUploadSessionByFileInfo = "spne_InsertUploadSessionByFileInfo";
        private const string ProcGetUploadSessionBySessionID = "spne_GetUploadSessionBySessionID";
        private const string ProcDeleteUploadSessionBySessionID = "spne_DeleteUploadSessionBySessionID";
        private const string ProcGetUserSmsAttemptsCountByPhoneNumber = "spne_GetUserSmsAttemptsCountByPhoneNumber";
        private const string ProcGetResourceInfoByResourceName = "spne_GetResourceInfoByResourceName";
        private const string ProcGetNearByMeSetting = "spne_GetNearByMeSetting";
        private const string ProcUpsertNearByMeSetting = "spne_UpsertNearByMeSetting";
        private const string ProcGetNearByMeUserByLocation = "spne_GetNearByMeUserByLocation";
        private const string ProcFindUserByName = "spne_FindUserByName";
        private const string ProcUpsertUserGPSLocation = "spne_UpsertUserGPSLocation";
        private const string ProcGetFriendRequestDetails = "spne_GetFriendRequestDetails";
        private const string ProcUpsertFriendRequest = "spne_UpsertFriendRequest";
        private const string ProcDeleteFriendRequest = "spne_DeleteFriendRequest";
        private const string ProcIsFriendRequestExist = "spne_IsFriendRequestExist";
        private const string ProcNearByUsersByLocation = "spne_GetNearByUsersByLocation";
        private const string ProcGetNBMMembersCountryList = "spne_GetNBMMembersCountryList";

        //Amazon
        private const string ProcInsertSMSLog = "spne_InsertSMSLog";
        private const string ProcInsertActivationSMSLog = "spne_InsertActivationSMSLog";




        //Near By me Promotion

        private const string ProcUpsertPromotion = "spne_UpsertPromotion";
        private const string ProcGetImageById = "spne_GetImageById";
        private const string ProcUpdateFeaturedImage = "spne_UpdateFeaturedImage";
        private const string ProcGetPromotionByUserName = "spne_GetPromotionByUserName";
        private const string ProcGetPromotionById = "spne_GetPromotionById";
        private const string ProcGetTOPNPromotions = "spne_GetTOPNPromotions";
        private const string ProcDeleteNearByMePromotionImage = "spne_DeleteNearByMePromotionImage";
        private const string ProcspneGetFeaturedImage = "spne_GetFeaturedImage";
        private const string ProcDeleteNearByMePromotion = "spne_DeleteNearByMePromotion";

        //ChatBackUp
        private const string ProneCreateXMLChatBackup = "spne_neCreateXMLChatBackup";
        private const string ProneGetXMLChatBackup = "spne_neGetXMLChatBackup";

        //NearByMePromotionPackages
        private const string ProcUpdateUserPromotionPackage = "spne_UpdateUserPromotionPackage";
        private const string ProcGetNearByMePromotionPackages = "spne_GetAllPromotionPackages";
        private const string ProcAddUserPromotionPackage = "spne_AddUserPromotionPackage";
        private const string ProcDeleteNearByMeUserPromotionPackage = "spne_DeleteNearByMeUserPromotionPackage";
        private const string ProcGetUserBalance = "spne_GetUserBalance";
        private const string ProcGetUserPromotionPackages = "spne_GetUserPromotionPackages";

        //Country
        private const string ProcGetAllCountries = "spne_GetAllCountries";
        private const string ProcGetCountryByCode = "spne_GetCountryByCode";

        #endregion

        #region Constructors

        public DbManager()
        {
            _conStr = ConfigurationManager.ConnectionStrings[NeeoConstants.ConnectionStringName].ConnectionString;
            _con = new SqlConnection(_conStr);
        }

        #endregion

        #region Member Functions

        #region Transaction methods
        /// <summary>
        /// Starts transaction.
        /// </summary>
        /// <returns></returns>
        public bool StartTransaction()
        {
            if (_con.State != ConnectionState.Open)
            {
                _con.Open();
                _transaction = _con.BeginTransaction();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Commits all the changes to database.
        /// </summary>
        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        /// <summary>
        /// Rollback all the changes that are made during the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        #endregion

        #region Activation

        /// <summary>
        /// Checks whether current user exists in the registered users or not. 
        /// </summary>
        /// <param name="phoneNumber">A string containing user's phone number.</param>
        /// <returns>true if user exists in registered users; otherwise false.</returns>
        public bool CheckUserAlreadyRegistered(string phoneNumber)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcCheckUserExistanceByPhoneNumber;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@phoneNumber", SqlDbType.NVarChar, 64).Value = phoneNumber;
            cmd.Parameters.Add("@userExists", SqlDbType.Bit).Direction = ParameterDirection.Output;

            try
            {
                _con.Open();
                cmd.ExecuteNonQuery();
                return Convert.ToBoolean(cmd.Parameters["@userExists"].Value);
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        /// <summary>
        /// Deletes the user's blocked state from the database.
        /// </summary>
        /// <param name="userID">A string containing the user's phone number as user id.</param>
        /// <returns>true if user entry is successfully deleted; otherwise false.</returns>
        public void DeleteUserFromBlockList(string userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcDeleteUserBlockedStateByPhoneNumber;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@phoneNumber", SqlDbType.NVarChar, 64).Value = userID;
            cmd.Transaction = _transaction;
            try
            {
                // _con.Open();
                cmd.ExecuteNonQuery();

            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                //if (_con.State != ConnectionState.Closed)
                //{
                //    _con.Close();
                //}
            }
        }

        /// <summary>
        /// Gets user's current state whether user is blocked or not.
        /// </summary>
        /// <param name="phoneNumber">A string containing the user's phone number.</param>
        /// <returns>0 if user is not blocked; otherwise 1.</returns>
        public int GetUserBlockedState(string phoneNumber)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetUserBlockingStateByPhoneNumber;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@phoneNumber", SqlDbType.NVarChar, 64).Value = phoneNumber;
            cmd.Parameters.Add("@userState", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Transaction = _transaction;
            try
            {
                //using (_con)
                //{
                //    _con.Open();
                cmd.ExecuteNonQuery();
                return Convert.ToInt16(cmd.Parameters["@userState"].Value);
                //}
            }
            catch (SqlException sqlExp)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlExp.Message, sqlExp);
                throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                //if (!_transaction)
                //{
                //    if (_con.State != ConnectionState.Closed)
                //    {
                //        _con.Close();
                //    }
                //}
            }
        }

        /// <summary>
        /// Gets user's current state whether user is blocked or not.
        /// </summary>
        /// <param name="phoneNumber">A string containing the user's phone number.</param>
        /// <returns>0 if user is not blocked; otherwise 1.</returns>
        public Dictionary<string, int> GetUserAttemptsCount(string phoneNumber, string code)
        {
            Dictionary<string, int> userAttempsDetailsDictionary = new Dictionary<string, int>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetUserSmsAttemptsCountByPhoneNumber;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@phoneNumber", SqlDbType.NVarChar, 64).Value = phoneNumber;
            cmd.Parameters.Add("@code", SqlDbType.VarChar, 10).Value = code;
            cmd.Parameters.Add("@userState", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@attemptsCount", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Transaction = _transaction;
            try
            {
                //using (_con)
                //{
                //    _con.Open();
                cmd.ExecuteNonQuery();
                userAttempsDetailsDictionary.Add("attemptsCount", Convert.ToInt32(cmd.Parameters["@attemptsCount"].Value));
                userAttempsDetailsDictionary.Add("blockedState", Convert.ToInt32(cmd.Parameters["@userState"].Value));
                return userAttempsDetailsDictionary;
                //}
            }
            catch (SqlException sqlExp)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlExp.Message, sqlExp);
                throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                //if (!isTransactional)
                //{
                //    if (_con.State != ConnectionState.Closed)
                //    {
                //        _con.Close();
                //    }
                //}
            }
        }
        /// <summary>
        /// Inserts or updates the user's device information in the database. If a record does not exist, it creates that record and if a record already exists then it updates that record.
        /// </summary>
        /// <param name="userID">A string containing the user's phone number as user id.</param>
        /// <param name="applicationID">An string containing the application id.</param>
        /// <param name="applicationVersion">A string containing application version.</param>
        /// <param name="deviceInfo">An object of DeviceInfo class that contains the device inforamtion.</param>
        /// <param name="isTransactional">A bool,if true, is telling about this method is a part of a transaction, otherwise false.</param>
        /// <param name="insertUpdateAllFields">A bool, if true, is telling about whether method can enter a new record or update the whole record. If false, it can only update some fields not whole record.</param>
        /// <returns>true if user's device information is successfully updated; otherwise false.</returns>
        public bool UpdateUserDeviceInfo(string userID, string applicationID, string applicationVersion, DeviceInfo deviceInfo, bool isTransactional, bool insertUpdateAllFields)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcUpdateUserDeviceInfoByPhoneNumber;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@phoneNumber", SqlDbType.NVarChar, 64).Value = userID;
            cmd.Parameters.Add("@applicationID", SqlDbType.VarChar, 36).Value = applicationID ?? "";
            cmd.Parameters.Add("@applicationVersion", SqlDbType.VarChar, 15).Value = applicationVersion ?? "";
            cmd.Parameters.Add("@devicePlatform", SqlDbType.TinyInt).Value = (int)deviceInfo.DevicePlatform;
            cmd.Parameters.Add("@deviceVenderID", SqlDbType.VarChar, 36).Value = deviceInfo.DeviceVenderID;
            cmd.Parameters.Add("@deviceToken", SqlDbType.VarChar, 200).Value = deviceInfo.DeviceToken ?? "";
            cmd.Parameters.Add("@deviceTokenVoIP", SqlDbType.VarChar, 500).Value = deviceInfo.DeviceTokenVoIP ?? "";
            cmd.Parameters.Add("@deviceModel", SqlDbType.NVarChar, 50).Value = deviceInfo.DeviceModel ?? "";
            cmd.Parameters.Add("@osVersion", SqlDbType.VarChar, 30).Value = deviceInfo.OsVersion ?? "";
            cmd.Parameters.Add("@pnSource", SqlDbType.Int).Value = deviceInfo.PushNotificationSource;
            cmd.Parameters.Add("@insertUpdateAllColumns", SqlDbType.Bit).Value = insertUpdateAllFields;
            cmd.Parameters.Add("@isSuccessfullyUpdated", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue;

            try
            {
                if (isTransactional)
                {
                    cmd.Transaction = _transaction;
                }
                else
                {
                    _con.Open();
                }
                cmd.ExecuteNonQuery();
                return Convert.ToBoolean(cmd.Parameters["@isSuccessfullyUpdated"].Value);

            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
                // return false;
            }
            finally
            {
                if (!isTransactional)
                {
                    if (_con.State != ConnectionState.Closed)
                    {
                        _con.Close();
                    }
                }
            }
        }



        /// <summary>
        /// Checks whether user's registeration request exists or not. 
        /// </summary>
        /// <param name="phoneNumber">A string containing user's phone number.</param>
        /// <returns>true if user request exists otherwise false.</returns>
        public bool CheckUserRegisterationRequest(string phoneNumber)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcDoesMembersRequestsExist;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = phoneNumber;

            try
            {
                _con.Open();
                bool status = (bool)cmd.ExecuteScalar();
                return status;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public bool InsertMembersRequestsLog(string username, float latitude, float longitude)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcInsertMembersRequestsLog;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
            cmd.Parameters.Add("@latitude", SqlDbType.Float).Value = latitude;
            cmd.Parameters.Add("@longitude", SqlDbType.Float).Value = longitude;

            try
            {
                _con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }



        #endregion

        #region Amazon
        public bool InsertSMSLog(string vendorMessageId, string receiver, string messageBody, bool isResend, bool isRegenerated, short messageType, string appKey, string status)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcInsertSMSLog;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@vendorMessageId", SqlDbType.VarChar).Value = vendorMessageId;
            cmd.Parameters.Add("@receiver", SqlDbType.VarChar, 64).Value = receiver;
            cmd.Parameters.Add("@messageBody", SqlDbType.VarChar, 500).Value = messageBody;
            cmd.Parameters.Add("@isResend", SqlDbType.Bit).Value = isResend;
            cmd.Parameters.Add("@isRegenerate", SqlDbType.Bit).Value = isRegenerated;
            cmd.Parameters.Add("@messageType", SqlDbType.SmallInt).Value = messageType;
            cmd.Parameters.Add("@appKey", SqlDbType.VarChar, 250).Value = appKey;
            cmd.Parameters.Add("@status", SqlDbType.VarChar, 250).Value = status;
            try
            {
                _con.Open();
                count = cmd.ExecuteNonQuery();
                if (count == 1)
                    return true;
                else
                    return false;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }

            }
        }
        public bool InsertActivationSMSLog(string vendorMessageId, string receiver, string messageBody, bool isResend, bool isRegenerated, short messageType, string appKey, string status, string deviceInfo, bool isDebugged)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcInsertActivationSMSLog;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@messageBody", SqlDbType.VarChar, 500).Value = messageBody;
            cmd.Parameters.Add("@receiver", SqlDbType.VarChar, 64).Value = receiver;
            cmd.Parameters.Add("@isResend", SqlDbType.Bit).Value = isResend;
            cmd.Parameters.Add("@isRegenerate", SqlDbType.Bit).Value = isRegenerated;
            cmd.Parameters.Add("@vendorMessageId", SqlDbType.VarChar).Value = vendorMessageId;
            cmd.Parameters.Add("@messageType", SqlDbType.SmallInt).Value = messageType;
            cmd.Parameters.Add("@appKey", SqlDbType.VarChar, 250).Value = appKey;
            cmd.Parameters.Add("@status", SqlDbType.VarChar, 250).Value = status;
            cmd.Parameters.Add("@deviceInfo", SqlDbType.VarChar).Value = deviceInfo;
            cmd.Parameters.Add("@isDebugged", SqlDbType.Bit).Value = isDebugged;

            try
            {
                _con.Open();
                count = cmd.ExecuteNonQuery();
                if (count == 1)
                    return true;
                else
                    return false;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }

            }
        }





        #endregion


        #region Profile

        /// <summary>
        /// Updates the user's display name in the database.
        /// </summary>
        /// <param name="userID">A string containing user's phone number as user id.</param>
        /// <param name="name">A string containing user's display name.</param>
        /// <returns>true if user's information is successfully updated; otherwise false.</returns>
        public bool UpdateUsersDisplayName(string userID, string name)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcUpdateUserNameByPhoneNumber;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@phoneNumber", SqlDbType.NVarChar, 64).Value = userID;
            cmd.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = name;

            try
            {
                _con.Open();
                count = cmd.ExecuteNonQuery();
                if (count == 1)
                    return true;
                else
                    return false;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
                //return false;
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        #endregion

        #region Device Token

        /// <summary>
        /// Updates user's device information in the database.
        /// </summary>
        /// <param name="userID">A string containing user's phone number as user id.</param>
        /// <param name="deviceToken">A string containing user's device token.</param>
        public void UpdateUserDeviceToken(string userID, string deviceToken)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcUpdateUserDeviceTokenByPhoneNumber;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@phoneNumber", SqlDbType.NVarChar, 64).Value = userID;
            cmd.Parameters.Add("@deviceToken", SqlDbType.NVarChar, 200).Value = deviceToken == null ? (object)DBNull.Value : deviceToken;
            cmd.Transaction = _transaction;
            try
            {
                count = cmd.ExecuteNonQuery();
                if (count != 1)
                {
                    throw new ApplicationException(CustomHttpStatusCode.InvalidNumber.ToString("D"));
                }
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
        }

        #endregion

        #region Contact Syncing
        /// <summary>
        /// Gets contacts existance and roster status status from database.
        /// </summary>
        /// <param name="userID">A string containing the user id.</param>
        /// <param name="contacts">A string containing the contacts.</param>
        /// <param name="delimeter">A string containing the delimeter.</param>
        /// <returns>A table containing the current status of all the contacts specified in <paramref name="contacts"/></returns>
        public DataTable GetContactsExistanceAndRosterStatus(string userID, string contacts, string delimeter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetContactsExistanceAndRosterStatusByContactsList;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@userContacts", SqlDbType.VarChar).Value = contacts;
            cmd.Parameters.Add("@applicationDomain", SqlDbType.NVarChar, 30).Value = "@" + ConfigurationManager.AppSettings[NeeoConstants.Domain].ToString(); ;
            cmd.Parameters.Add("@userID", SqlDbType.NVarChar, 64).Value = userID;
            cmd.Parameters.Add("@delimeter", SqlDbType.VarChar, 5).Value = delimeter;

            cmd.CommandTimeout = 60;
            DataTable dtContactStatus = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);

            try
            {
                adp.Fill(dtContactStatus);
                return dtContactStatus;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        /// <summary>
        /// Gets user profile name from database.
        /// </summary>
        /// <param name="userID">A string containing the user id.</param>
        /// <returns>A string containing the profile name of user. </returns>
        public string GetUserProfileName(string userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetUserProfileNameByUserID;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@userID", SqlDbType.NVarChar, 64).Value = userID;
            DataTable dtUserProfileName = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);

            try
            {
                adp.Fill(dtUserProfileName);

                if (dtUserProfileName.Rows.Count > 0)
                {
                    DataRow dr = dtUserProfileName.Rows[0];
                    return (dr["name"].ToString());
                }

                else
                {
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                }
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        #endregion

        #region Push Notification

        /// <summary>
        /// Gets the name of the user for given user id.
        /// </summary>
        /// <param name="callingID">A string containing the user id.</param>
        /// <returns>A string containing the profile name of the user based on user id.</returns>
        public Dictionary<string, string> GetUserInforForNotification(string callingID, string callerID, bool getOfflineMsgCount, int mcrCount = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetUserInfoByCallerIDAndReceiverIDWithOfflineMsgCount;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@receiverID", SqlDbType.NVarChar, 64).Value = callingID;
            cmd.Parameters.Add("@callerID", SqlDbType.NVarChar, 64).Value = callerID;
            cmd.Parameters.Add("@mcrCount", SqlDbType.Int).Value = mcrCount;
            cmd.Parameters.Add("@getOfflineMsgCount", SqlDbType.Bit).Value = getOfflineMsgCount;
            DataSet dsUserInfo = new DataSet();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            Dictionary<string, string> userInfoDictionary = new Dictionary<string, string>();
            try
            {
                ad.Fill(dsUserInfo);
                userInfoDictionary.Add(NeeoConstants.ReceiverDeviceToken, dsUserInfo.Tables[0].Rows[0][NeeoConstants.ReceiverDeviceToken].ToString());
                userInfoDictionary.Add(NeeoConstants.ReceiverUserDeviceplatform, dsUserInfo.Tables[0].Rows[0][NeeoConstants.ReceiverUserDeviceplatform].ToString());
                userInfoDictionary.Add(NeeoConstants.ReceiverCallingTone, dsUserInfo.Tables[0].Rows[0][NeeoConstants.ReceiverCallingTone].ToString());
                userInfoDictionary.Add(NeeoConstants.CallerName, dsUserInfo.Tables[1].Rows.Count > 0 ? dsUserInfo.Tables[1].Rows[0][NeeoConstants.CallerName].ToString() : "");
                userInfoDictionary.Add(NeeoConstants.PushNotificationSource, dsUserInfo.Tables[0].Rows[0][NeeoConstants.PushNotificationSource].ToString());
                if (getOfflineMsgCount)
                {
                    userInfoDictionary.Add(NeeoConstants.OfflineMessageCount, dsUserInfo.Tables[0].Rows[0][NeeoConstants.OfflineMessageCount].ToString());
                }
                return userInfoDictionary;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        /// <summary>
        /// Gets the name of the user for given user id.
        /// </summary>
        /// <param name="groupID">A string containing the group id.</param>
        /// <param name="domain">A string containing the server domain.</param>
        /// <param name="senderID">A string containing the sender id.</param>
        /// <returns>A string containing the profile name of the user based on user id.</returns>
        public DataTable GetGroupParticipantsData(int groupID, string domain, string senderID, int messageType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetGroupParticipantsDataByRoomIDnSenderID;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@groupID", SqlDbType.Int).Value = groupID;
            cmd.Parameters.Add("@domain", SqlDbType.NVarChar, 30).Value = domain;
            cmd.Parameters.Add("@senderID", SqlDbType.NVarChar, 64).Value = senderID;
            cmd.Parameters.Add("@messageType", SqlDbType.Int).Value = messageType;
            DataTable dtGroupParticipantData = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            try
            {
                dataAdapter.Fill(dtGroupParticipantData);
                return dtGroupParticipantData;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="domain"></param>
        /// <param name="senderID"></param>
        /// <returns></returns>
        public DataTable GetGroupParticipantsData(string roomName, string domain, string senderID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetGroupParticipantsDatanRoomDataByRoomNamenSenderID;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@roomName", SqlDbType.Int).Value = roomName;
            cmd.Parameters.Add("@domain", SqlDbType.NVarChar, 30).Value = domain;
            cmd.Parameters.Add("@senderID", SqlDbType.NVarChar, 64).Value = senderID;
            //cmd.Parameters.Add("@messageType", SqlDbType.Int).Value = messageType;
            DataTable dtGroupParticipantData = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            try
            {
                dataAdapter.Fill(dtGroupParticipantData);
                return dtGroupParticipantData;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userList"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        public void UpdateUserOfflineCount(string userList, string delimeter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcUpdateOfflineUserCountByUserList;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@userList", SqlDbType.NVarChar).Value = userList;
            cmd.Parameters.Add("@delimeter", SqlDbType.NVarChar, 1).Value = delimeter;

            try
            {
                _con.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    LogManager.CurrentInstance.InfoLogger.LogInfo(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Group count is successfully updated for users : " + userList, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
                else
                {
                    LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Group count is not updated for users : " + userList, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                //throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }


        #endregion

        #region Settings

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="tone"></param>
        /// <returns></returns>
        public bool UpdateSettings(string userID, Enum tone, ToneType toneType)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcUpdateUserSettingsByUserID;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@userID", SqlDbType.NVarChar, 64).Value = userID;
            cmd.Parameters.Add("@tone", SqlDbType.TinyInt).Value = tone.ToString("D");
            cmd.Parameters.Add("@toneType", SqlDbType.TinyInt).Value = toneType.ToString("D");

            try
            {
                _con.Open();
                count = cmd.ExecuteNonQuery();
                if (count == 1)
                    return true;
                else
                    return false;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public DataTable GetUserGroupsDetails(string userJid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetUserGroupsDetailsByUserID;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@userJid", SqlDbType.NVarChar).Value = userJid;

            DataTable dtUserGroupsDetails = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                adapter.Fill(dtUserGroupsDetails);
                return dtUserGroupsDetails;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        #endregion

        #region File Sharing


        public string GetClientAppVersion(string senderID, string receiverID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetClientAppVersionByReceiverID;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@senderID", SqlDbType.NVarChar, 64).Value = senderID;
            cmd.Parameters.Add("@receiverID", SqlDbType.NVarChar, 64).Value = receiverID;

            try
            {
                _con.Open();
                return cmd.ExecuteScalar().ToString();
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="expiryPeriod"></param>
        /// <returns></returns>
        public DataTable GetExpiredFile(DateTime dateTime, int expiryPeriod)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetSharedFilesByDate;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@dateTimeUtc", SqlDbType.DateTime2).Value = dateTime;
            cmd.Parameters.Add("@expiryPeriod", SqlDbType.Int).Value = expiryPeriod;

            DataTable dtExpiredFiles = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                adapter.Fill(dtExpiredFiles);
                return dtExpiredFiles;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileID"></param>
        /// <param name="ownerJID"></param>
        /// <param name="mediaType"></param>
        /// <param name="mimeType"></param>
        /// <param name="fullPath"></param>
        /// <param name="creationDate"></param>
        /// <param name="recipientCount"></param>
        /// <returns></returns>
        public bool InsertSharedFileInformation(string fileID, string ownerJID, ushort mediaType, ushort mimeType, string fullPath, DateTime creationDate, ushort recipientCount, long size, string hash)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcInsertSharedFileInformationByUserID;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@fileID", SqlDbType.VarChar, 32).Value = fileID;
            cmd.Parameters.Add("@ownerJID", SqlDbType.NVarChar, 64).Value = ownerJID;
            cmd.Parameters.Add("@mediaType", SqlDbType.TinyInt).Value = mediaType;
            cmd.Parameters.Add("@mimeType", SqlDbType.TinyInt).Value = mimeType;
            cmd.Parameters.Add("@fullPath", SqlDbType.Text).Value = fullPath;
            cmd.Parameters.Add("@creationDate", SqlDbType.SmallDateTime).Value = creationDate;
            cmd.Parameters.Add("@recipientCount", SqlDbType.TinyInt).Value = recipientCount;
            cmd.Parameters.Add("@size", SqlDbType.BigInt).Value = size;
            cmd.Parameters.Add("@hash", SqlDbType.VarChar, 80).Value = hash;

            cmd.Transaction = _transaction;
            try
            {
                count = cmd.ExecuteNonQuery();
                if (count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="fileID"></param>
        /// <returns></returns>
        public bool UpdateSharedFileDownloadCount(string userID, string fileID)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcUpdateSharedFileDownloadCountByFileID;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@userID", SqlDbType.NVarChar, 64).Value = userID;
            cmd.Parameters.Add("@fileID", SqlDbType.VarChar, 32).Value = fileID;

            try
            {
                _con.Open();
                count = cmd.ExecuteNonQuery();
                if (count == 1)
                    return true;
                else
                    return false;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileID"></param>
        /// <returns></returns>
        public bool DeleteSharedFile(string recordIDs, string delimeter)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcDeleteSharedFileByRecordIDs;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@recordIDs", SqlDbType.VarChar).Value = recordIDs;
            cmd.Parameters.Add("@delimeter", SqlDbType.Char, 1).Value = delimeter;

            try
            {
                _con.Open();
                count = cmd.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        /// <summary>
        /// Updates the file shared date-time and add recipient count to the existing count.
        /// </summary>
        /// <param name="userID">A string containing the user id.</param>
        /// <param name="fileID">A string containing the file id.</param>
        /// <param name="recipientCount">An unsigned integer value containing the recipient count.</param>
        /// <returns></returns>
        public bool UpdateFileSharedDateWithRecipientCount(string userID, string fileID, ushort recipientCount)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcUpdateFileSharedDatenRecipientCountByFileID;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@userID", SqlDbType.NVarChar, 64).Value = userID;
            cmd.Parameters.Add("@fileID", SqlDbType.VarChar, 32).Value = fileID;
            cmd.Parameters.Add("@recipientCount", SqlDbType.TinyInt).Value = recipientCount;

            try
            {
                _con.Open();
                count = cmd.ExecuteNonQuery();
                if (count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        #region UploadSession
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionID"></param>
        /// <param name="fileID"></param>
        /// <param name="username"></param>
        /// <param name="mediaType"></param>
        /// <param name="mimeType"></param>
        /// <param name="fullPath"></param>
        /// <param name="creationDate"></param>
        /// <param name="size"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public async Task<bool> InsertUploadSessionAsync(string sessionID, string fileID, string username, ushort mediaType, ushort mimeType, string fullPath, DateTime creationDate, long size, string hash)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcInsertUploadSessionByFileInfo;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@sessionID", SqlDbType.VarChar, 32).Value = sessionID;
            cmd.Parameters.Add("@fileID", SqlDbType.VarChar, 32).Value = fileID;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = username;
            cmd.Parameters.Add("@mediaType", SqlDbType.TinyInt).Value = mediaType;
            cmd.Parameters.Add("@mimeType", SqlDbType.TinyInt).Value = mimeType;
            cmd.Parameters.Add("@fullPath", SqlDbType.Text).Value = fullPath;
            cmd.Parameters.Add("@creationDate", SqlDbType.SmallDateTime).Value = creationDate;
            cmd.Parameters.Add("@size", SqlDbType.BigInt).Value = size;
            cmd.Parameters.Add("@hash", SqlDbType.VarChar, 80).Value = hash;
            try
            {
                _con.Open();
                count = await cmd.ExecuteNonQueryAsync();
                if (count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }


        public async Task<DataTable> GetUploadSessionAsync(string sessionID)
        {
            return await Task<DataTable>.Factory.StartNew(() =>
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = ProcGetUploadSessionBySessionID;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _con;
                cmd.Parameters.Add("@sessionID", SqlDbType.VarChar, 50).Value = sessionID;
                DataTable dtSessionDetail = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                try
                {
                    adp.Fill(dtSessionDetail);
                    return dtSessionDetail;
                }
                catch (SqlException sqlEx)
                {
                    LogManager.CurrentInstance.ErrorLogger.LogError(
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                    if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                        throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                    else
                        throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
                }
                finally
                {
                    if (_con.State != ConnectionState.Closed)
                    {
                        _con.Close();
                    }
                }
            });
        }

        public async Task<bool> UpdateUploadSessionAsync(string p1, string p2, string p3, ushort p4, ushort p5, string p6, DateTime dateTime, long p7, string p8)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteUploadSessionAsync(string sessionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcDeleteUploadSessionBySessionID;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@sessionID", SqlDbType.VarChar, 50).Value = sessionID;

            try
            {
                _con.Open();
                if (await cmd.ExecuteNonQueryAsync() == 1)
                    return true;
                else
                    return false;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        #endregion

        #endregion

        #region Contact Capabilities

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataTable GetAppVersion(string userID, string contacts)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = ProcGetAppVersionByContactList;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.NVarChar, 64).Value = userID;
                cmd.Parameters.Add("@contacts", SqlDbType.NVarChar).Value = contacts;
                cmd.Connection = _con;

                DataTable dtContactsAppVersions = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtContactsAppVersions);
                return dtContactsAppVersions;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }

        }
        #endregion

        #region User Verification
        /// <summary>
        /// Gets user's information(deviceVenderID,applicationID) from  database.
        /// </summary>
        /// <param name="userID">A string containing user's phone number as user id.</param>
        /// <returns> It returns deviceVenderID and applicationID against a User</returns>
        /// 
        public DataTable GetUserInformation(string userID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = ProcGetUserInfoByUserID;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.NVarChar, 64).Value = userID;
                cmd.Connection = _con;

                DataTable tbUserInfo = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(tbUserInfo);
                if (tbUserInfo.Rows.Count > 0)
                {
                    return tbUserInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        #endregion

        #region Offline Message Count

        public bool ResetOfflineCount(string userID, OfflineCount offlineCountType)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcResetOffineMessageCountByUserID;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@userID", SqlDbType.NVarChar, 64).Value = userID;
            cmd.Parameters.Add("@offlineCountType", SqlDbType.TinyInt).Value = offlineCountType;

            try
            {
                _con.Open();
                count = cmd.ExecuteNonQuery();
                if (count == 1)
                    return true;
                else
                    return false;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        #endregion

        #region User Last Seen Time

        public async Task<Dictionary<string, object>> GetUserLastSeenTimeAsync(string userID, string resource)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetLastSeenTimeByUserIDnResource;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@userID", SqlDbType.NVarChar, 64).Value = userID;
            cmd.Parameters.Add("@resource", SqlDbType.NVarChar, 100).Value = resource;
            cmd.Parameters.Add("@online", SqlDbType.Bit).Direction = ParameterDirection.Output;

            try
            {
                _con.Open();
                var result = await cmd.ExecuteScalarAsync();

                if (result == null && cmd.Parameters["@online"].Value.ToString() == string.Empty)
                {
                    throw new ApplicationException(HttpStatusCode.NotFound.ToString("D"));
                }
                return new Dictionary<string, object>()
                {
                    {"last_seen", result == null ? string.Empty : result.ToString()},
                    {"is_online", cmd.Parameters["@online"].Value},
                };
            }
            catch (SqlException dbException)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, dbException.Message, dbException);
                if (dbException.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        #endregion

        #region App Resource Management

        public DataTable GetResourceInfo(string resourceId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetResourceInfoByResourceName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = resourceId;

            DataTable dtResourceInfo = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                adapter.Fill(dtResourceInfo);
                return dtResourceInfo;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        #endregion

        #region Near By Me

        public DataTable GetNearByMeSetting(string username)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetNearByMeSetting;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;

            DataTable dtNearByMeSetting = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                adapter.Fill(dtNearByMeSetting);
                return dtNearByMeSetting;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public DataTable GetNearByMeUserByLocation(string username, double latitude, double longitude, bool isCurrentLocation)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetNearByMeUserByLocation;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
            cmd.Parameters.Add("@latitude", SqlDbType.Float).Value = latitude;
            cmd.Parameters.Add("@longitude", SqlDbType.Float).Value = longitude;
            cmd.Parameters.Add("@isCurrentLocation", SqlDbType.Bit).Value = isCurrentLocation;

            DataTable dtNearByMeUserLocation = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                adapter.Fill(dtNearByMeUserLocation);
                return dtNearByMeUserLocation;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public DataTable GetFriendRequestDetails(string username)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetFriendRequestDetails;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;

            DataTable dtFriendRequest = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                adapter.Fill(dtFriendRequest);

                return dtFriendRequest;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public DataTable IsFriendRequestExist(string username, string friendUId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcIsFriendRequestExist;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = username;
            cmd.Parameters.Add("@friendUId", SqlDbType.NVarChar, 64).Value = friendUId;
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@status", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@senderId", SqlDbType.NVarChar, 64).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@recipientId", SqlDbType.NVarChar, 64).Direction = ParameterDirection.Output;

            DataTable dtFriendRequest = new DataTable();

            try
            {
                long? id;
                _con.Open();
                cmd.ExecuteNonQuery();

                dtFriendRequest.Columns.Add("Id");
                dtFriendRequest.Columns.Add("SenderId");
                dtFriendRequest.Columns.Add("RecipientId");
                dtFriendRequest.Columns.Add("Status");
                id = cmd.Parameters["@id"].Value as long?;

                if (id != null)
                {
                    DataRow row = dtFriendRequest.NewRow();

                    row["Id"] = id.Value;
                    row["SenderId"] = cmd.Parameters["@senderId"].Value;
                    row["RecipientId"] = cmd.Parameters["@recipientId"].Value;
                    row["Status"] = Convert.ToInt32(cmd.Parameters["@status"].Value);

                    dtFriendRequest.Rows.Add(row);
                }

                return dtFriendRequest;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public bool UpsertNearByMeSetting(string username, bool enabled, ushort notificationTone, bool notificationOn, bool showInfo, bool showProfileImage, bool isPrivateAccount)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcUpsertNearByMeSetting;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = username;
            cmd.Parameters.Add("@enabled", SqlDbType.Bit).Value = enabled;
            cmd.Parameters.Add("@notificationTone", SqlDbType.TinyInt).Value = notificationTone;
            cmd.Parameters.Add("@notificationOn", SqlDbType.Bit).Value = notificationOn;
            cmd.Parameters.Add("@showInfo", SqlDbType.Bit).Value = showInfo;
            cmd.Parameters.Add("@showProfileImage", SqlDbType.Bit).Value = showProfileImage;
            cmd.Parameters.Add("@isPrivateAccount", SqlDbType.Bit).Value = isPrivateAccount;

            try
            {
                _con.Open();
                count = cmd.ExecuteNonQuery();

                if (count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public bool UpsertUserGpsLocation(string username, double latitude, double longitude)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcUpsertUserGPSLocation;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = username;
            cmd.Parameters.Add("@latitude", SqlDbType.Float).Value = latitude;
            cmd.Parameters.Add("@longitude", SqlDbType.Float).Value = longitude;

            try
            {
                _con.Open();
                count = cmd.ExecuteNonQuery();

                return true;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public bool UpsertFriendRequest(string username, string friendUId, FriendRequestStatus status)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcUpsertFriendRequest;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = username;
            cmd.Parameters.Add("@friendUId", SqlDbType.NVarChar, 64).Value = friendUId;
            cmd.Parameters.Add("@status", SqlDbType.Int).Value = (int)status;

            try
            {
                _con.Open();
                count = cmd.ExecuteNonQuery();

                return true;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public bool DeleteFriendRequest(string username, string friendUId)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcDeleteFriendRequest;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = username;
            cmd.Parameters.Add("@friendUId", SqlDbType.NVarChar, 64).Value = friendUId;

            try
            {
                _con.Open();
                count = cmd.ExecuteNonQuery();

                return true;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }


        //Uzair
        public DataTable GetNearByUsersByLocation(string userID, double latitude, double longitude)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcNearByUsersByLocation;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = userID;
            cmd.Parameters.Add("@latitude", SqlDbType.Float).Value = latitude;
            cmd.Parameters.Add("@longitude", SqlDbType.Float).Value = longitude;

            cmd.CommandTimeout = 60;
            DataTable dtSearchedUser = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);

            try
            {
                adp.Fill(dtSearchedUser);

                return dtSearchedUser;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public async Task<DataTable> GetNBMMembersCountryList()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetNBMMembersCountryList;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            DataTable dbNBMMembersCountryList = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                await System.Threading.Tasks.Task.Run(() => adapter.Fill(dbNBMMembersCountryList));
                return dbNBMMembersCountryList;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }


        #endregion

        #region Near by me Promotion

        public async Task<bool> UpdateFeaturedImage(int promotionId, long imageId)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcUpdateFeaturedImage;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;

            cmd.Parameters.Add("@promotionId", SqlDbType.Int).Value = promotionId;
            cmd.Parameters.Add("@imageId", SqlDbType.BigInt).Value = imageId;

            try
            {
                await _con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }
        public async Task<DataTable> GetImageById(long imageId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetImageById;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@imageId", SqlDbType.NVarChar, 64).Value = imageId;

            DataTable dtNearByMePromotion = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                await System.Threading.Tasks.Task.Run(() => adapter.Fill(dtNearByMePromotion));
                return dtNearByMePromotion;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public async Task<DataTable> GetFeaturedImage(int promotionId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcspneGetFeaturedImage;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@promotionId", SqlDbType.Int).Value = promotionId;
          //  cmd.Parameters.Add("@promotionId", SqlDbType.NVarChar, 64).Value = imageId;

            DataTable dtGetFeaturedImage = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                await System.Threading.Tasks.Task.Run(() => adapter.Fill(dtGetFeaturedImage));
                return dtGetFeaturedImage;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }
        public async Task<bool> UpsertPromotion(string username, string description, byte status, string ImagesXml)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcUpsertPromotion;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = username;
            cmd.Parameters.Add("@description", SqlDbType.NVarChar, 1000).Value = description;
            cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = status;
            cmd.Parameters.Add("@ImagesXml", SqlDbType.NVarChar).Value = ImagesXml;
            try
            {
                await _con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
             //   throw new Exception();
                return true;
            }
            catch (SqlException sqlEx)
            {
                throw;
                //LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);

                //if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                //    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                //else
                //    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                { 
                    _con.Close();
                }
            }
        }
        public async Task<DataTable> GetTOPNPromotions(string username, int top)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetTOPNPromotions;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = username;
            cmd.Parameters.Add("@top", SqlDbType.Int).Value = top;

            DataTable dtNearByMePromotion = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                await System.Threading.Tasks.Task.Run(() => adapter.Fill(dtNearByMePromotion));
                return dtNearByMePromotion;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }
        public async Task<DataSet> GetPromotionByUserName(string username)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetPromotionByUserName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = username;

            DataSet dtNearByMePromotion = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                await System.Threading.Tasks.Task.Run(() => adapter.Fill(dtNearByMePromotion));
                return dtNearByMePromotion;
            }
            catch (SqlException sqlEx)
            {
                throw;
                //LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                //if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                //    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                //else
                //    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public async Task<DataSet> GetPromotionById(int promotionId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetPromotionById;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@promotionId", SqlDbType.Int).Value = promotionId;


            DataSet GetPromotionById = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                await System.Threading.Tasks.Task.Run(() => adapter.Fill(GetPromotionById));
                return GetPromotionById;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }
        public async Task<int> DeleteNearByMePromotionImage(int promotionId, long imageId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcDeleteNearByMePromotionImage;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@imageId", SqlDbType.BigInt).Value = imageId;
            cmd.Parameters.Add("@promotionId", SqlDbType.BigInt).Value = promotionId;
            try
            {
                await _con.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();

            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public async Task<int> DeleteNearByMePromotionImage(int promotionId, string imageId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcDeleteNearByMePromotionImage;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@imageId", SqlDbType.BigInt).Value = imageId;
            cmd.Parameters.Add("@promotionId", SqlDbType.BigInt).Value = promotionId;
            try
            {
                await _con.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();

            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }


        //by uzair
        public async Task<bool> DeleteNearByMePromotion(string username)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcDeleteNearByMePromotion;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = username;
            try
            {
                await _con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        #endregion

        #region User Search

        /// <summary>
        /// Gets contacts existance and roster status status from database.
        /// </summary>
        /// <param name="userID">A string containing the user id.</param>
        /// <param name="searchText">A string containing search text.</param>
        /// <returns>A table containing the current status of all the contacts specified in <paramref name="contacts"/></returns>
        public DataTable FindUserByName(string userID, string searchText, double latitude, double longitude, bool isCurrentLocation)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcFindUserByName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@searchText", SqlDbType.NVarChar, 100).Value = searchText;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = userID;
            cmd.Parameters.Add("@latitude", SqlDbType.Float).Value = latitude;
            cmd.Parameters.Add("@longitude", SqlDbType.Float).Value = longitude;
            cmd.Parameters.Add("@isCurrentLocation", SqlDbType.Bit).Value = isCurrentLocation;

            cmd.CommandTimeout = 60;
            DataTable dtSearchedUser = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);

            try
            {
                adp.Fill(dtSearchedUser);

                return dtSearchedUser;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }


        #endregion

        #region NearByMePromotionPackages

        public async Task<DataTable> GetNearByMePromotionPackages()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetNearByMePromotionPackages;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            DataTable dtNearByMePromotionPackages = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                await System.Threading.Tasks.Task.Run(() => adapter.Fill(dtNearByMePromotionPackages));
                return dtNearByMePromotionPackages;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public async Task<long> AddUserPromotionPackage(int packageId, int promotionId,  short numberOfDays, string country)
        {
            long count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcAddUserPromotionPackage;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@packageId", SqlDbType.Int).Value  = packageId;
            cmd.Parameters.Add("@promotionId", SqlDbType.Int).Value = promotionId;
            cmd.Parameters.Add("@numberOfDays ", SqlDbType.SmallInt).Value = numberOfDays;
            cmd.Parameters.Add("@countryIds", SqlDbType.NVarChar).Value = country;
           
            try
            {
                await _con.OpenAsync();
                count = (long)await cmd.ExecuteScalarAsync();
                return count;



            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            } 

        }

        public async Task<int> DeleteNearByMeUserPromotionPackage(int userPromotionsPackageID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcDeleteNearByMeUserPromotionPackage;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@userPromotionsPackageID", SqlDbType.Int).Value = userPromotionsPackageID;
            try
            {
                await _con.OpenAsync();
               int  delete = (int)await cmd.ExecuteScalarAsync();
                return delete;


            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public async Task<decimal> GetUserBalance(string username)
        {
            decimal balance = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetUserBalance;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = username;

            try
            {
                await _con.OpenAsync();
                balance = (decimal)await cmd.ExecuteScalarAsync();
                return balance;
            }
            catch (SqlException sqlEx)
            {

                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public async Task<long> UpdateUserPromotionPackage(int  userPromotionsPackageID , string countryIds)
        {
            int count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcUpdateUserPromotionPackage;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@userPromotionsPackageID", SqlDbType.Int).Value = userPromotionsPackageID;
            cmd.Parameters.Add("@countryIds", SqlDbType.VarChar , 100).Value = countryIds;
           

            try
            {
                await _con.OpenAsync();
                count = (int)await cmd.ExecuteScalarAsync();
                return count;



            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }

        }

        public async Task<DataTable> GetUserPromotionPackages(string username)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetUserPromotionPackages;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = username;

            DataTable dtGetUserPromotionPackages = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                await System.Threading.Tasks.Task.Run(() => adapter.Fill(dtGetUserPromotionPackages));
                return dtGetUserPromotionPackages;
            }
            catch (SqlException sqlEx)
            {
                throw;
                //LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                //if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                //    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                //else
                //    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }


        #endregion


        #region Country
        public async Task<DataTable> GetAllCountries()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetAllCountries;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            DataTable dtNearByMePromotion = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                await System.Threading.Tasks.Task.Run(() => adapter.Fill(dtNearByMePromotion));
                return dtNearByMePromotion;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }
        public DataTable GetCountryByCode(string countryCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcGetCountryByCode;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@countryCode", SqlDbType.VarChar, 3).Value = countryCode;

            DataTable dtCountry = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try
            {
                adapter.Fill(dtCountry);
                return dtCountry;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        #endregion

        #endregion


        #region User ChatBackUp


        public async Task<bool> CreateXMLChatBackup(string sender, string messagesXml)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProneCreateXMLChatBackup;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@sender", SqlDbType.NVarChar, 64).Value = sender;
            cmd.Parameters.Add("@messagesXml", SqlDbType.Xml).Value = messagesXml;
            try
            {
                await _con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        public async Task<DataTable> GetXMLChatBackup(string sender)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProneGetXMLChatBackup;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = _con;
            cmd.Parameters.Add("@sender", SqlDbType.NVarChar, 64).Value = sender;



            DataTable dbGetChatBackup = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);

            try
            {
                await System.Threading.Tasks.Task.Run(() => adp.Fill(dbGetChatBackup));

                return dbGetChatBackup;
            }
            catch (SqlException sqlEx)
            {
                LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, sqlEx.Message, sqlEx);
                if (sqlEx.Number == NeeoConstants.DbInvalidUserCode)
                    throw new ApplicationException(CustomHttpStatusCode.InvalidUser.ToString("D"));
                else
                    throw new ApplicationException(CustomHttpStatusCode.DatabaseOperationFailure.ToString("D"));
            }
            finally
            {
                if (_con.State != ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
        }

        #endregion

    }
}
