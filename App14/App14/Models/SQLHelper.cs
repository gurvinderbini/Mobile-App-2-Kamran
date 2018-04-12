using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

using PCLStorage;
using App14.Models;

namespace App14
{
    /*public class SqlHelper
    {
        static object locker = new object();
        static object lockerEvent = new object();
        SQLiteConnection database;

        public SqlHelper()
        {
            database = GetConnection();
            // create the tables  
            database.CreateTable<RegEntity>();

            database.CreateTable<EventsList>();
        }
        public SQLite.SQLiteConnection GetConnection()
        {
            SQLiteConnection sqlitConnection;
            var sqliteFilename = "cloudSchool.db3";
            IFolder folder = FileSystem.Current.LocalStorage;
            string path = PortablePath.Combine(folder.Path.ToString(), sqliteFilename);
            sqlitConnection = new SQLite.SQLiteConnection(path);
            return sqlitConnection;
        }

        public IEnumerable<RegEntity> GetItems()
        {
            lock (locker)
            {
                return (from i in database.Table<RegEntity>() select i).ToList();
            }
        }
        public RegEntity GetItemFirst()
        {
            lock (locker)
            {
                return database.Table<RegEntity>().FirstOrDefault(x => x.ID == 1);
            }
        }
        public RegEntity GetItem(string userName)
        {
            lock (locker)
            {
                return database.Table<RegEntity>().FirstOrDefault(x => x.Username == userName);
            }
        }
        public RegEntity GetItem(string userName, string passWord)
        {
            lock (locker)
            {
                return database.Table<RegEntity>().FirstOrDefault(x => x.Username == userName && x.Password == passWord);
            }
        }
        public int SaveItem(RegEntity item)
        {
            lock (locker)
            {
                if (item.ID != 0)
                {
                    //Update Item  
                    database.Update(item);
                    return item.ID;
                }
                else
                {
                    //Insert item  
                    return database.Insert(item);
                }
            }
        }
        public int DeleteItem(int id)
        {
            lock (locker)
            {
                return database.Delete<RegEntity>(id);
            }
        }


        public IEnumerable<EventsList> GetItemsEvents()
        {
            lock (lockerEvent)
            {
                return (from i in database.Table<EventsList>() select i).ToList();
            }
        }

        public Task<List<EventsList>> getAllEvents()
        {
             return database.Table<EventsList>().ToListAsync();
        }

        public EventsList GetItemEventsFirst()
        {
            lock (lockerEvent)
            {
               // return database.Table<EventsList>().Where(x => x.ID == 2);
               return database.Table<EventsList>().FirstOrDefault(x => x.ID == 1);
            }
        }
        public EventsList GetItemEvent(string title)
        {
            lock (lockerEvent)
            {
                return database.Table<EventsList>().FirstOrDefault(x => x.title == title);
            }
        }
        //public EventsList GetItemEvent(string userName, string passWord)
        //{
        //    lock (lockerEvent)
        //    {
        //        return database.Table<EventsList>().FirstOrDefault(x => x.Username == userName && x.Password == passWord);
        //    }
        //}
        public int SaveEvents(EventsList item)
        {
            lock (lockerEvent)
            {
                if (item.ID != 0)
                {
                    //Update Item  
                    database.Update(item);
                    return item.ID;
                }
                else
                {
                    //Insert item  
                    return database.Insert(item);
                }
            }
        }

    }
    */

    public class SqlHelper
    {
        static object locker = new object();
        static object lockerEvent = new object();
        static object lockerLogin = new object();
        static object TokenLock = new object();
        static object NotificationLock = new object();
        SQLiteAsyncConnection database;

        public SqlHelper()
        {
            database = GetConnection();
            // create the tables  for 
            database.CreateTableAsync<RegEntity>().Wait();
            // create the tables  for events
            database.CreateTableAsync<EventsList>().Wait();
            // create the tables  for save credentials
            database.CreateTableAsync<RememberMeCredentials>().Wait();

            database.CreateTableAsync<NotificationBO>().Wait();

            database.CreateTableAsync<DeviceTokenBO>().Wait();

        }
        public SQLiteAsyncConnection GetConnection()
        {
            SQLiteAsyncConnection sqlitConnection;
            var sqliteFilename = "cloudSchool.db3";
            IFolder folder = FileSystem.Current.LocalStorage;
            string path = PortablePath.Combine(folder.Path.ToString(), sqliteFilename);
            sqlitConnection = new SQLiteAsyncConnection(path);
            return sqlitConnection;
        }
        /*
        public Task<RegEntity> GetItems()
        {
            lock (locker)
            {
                return (from i in database.Table<RegEntity>() select i).ToListAsync();
            }
        }*/

        public Task<int> InsertToken(DeviceTokenBO token)
        {
            lock (TokenLock)
            {
                database.DropTableAsync<DeviceTokenBO>();
                database.CreateTableAsync<DeviceTokenBO>();
                return database.InsertAsync(token);
            }
        }
        public Task<DeviceTokenBO> GetToken()
        {
            lock (TokenLock)
            {
                try
                {
                    return database.Table<DeviceTokenBO>().FirstOrDefaultAsync();
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }

        public Task<int> InsertNotification(NotificationBO item)
        {
            lock (NotificationLock)
            {
                return database.InsertAsync(item);
            }
        }
        public Task<List<NotificationBO>> GetAllNotfications()
        {
            lock (NotificationLock)
            {
                return database.Table<NotificationBO>().ToListAsync();
            }

            lock (locker)
            {

            }
        }

        public Task<RegEntity> GetItemFirst()
        {
            lock (locker)
            {
                return database.Table<RegEntity>().Where(x => x.ID == 1).FirstOrDefaultAsync();
            }
        }
        public Task<RegEntity> GetItem(string userName)
        {
            lock (locker)
            {
                return database.Table<RegEntity>().Where(x => x.Username == userName).FirstOrDefaultAsync();
            }
        }
        public Task<RegEntity> GetItem(string userName, string passWord)
        {
            lock (locker)
            {
                return database.Table<RegEntity>().Where(x => x.Username == userName && x.Password == passWord).FirstOrDefaultAsync();
            }
        }
        public Task<int> SaveItem(RegEntity item)
        {
            lock (locker)
            {
                if (item.ID != 0)
                {
                    //Update Item  
                    return database.UpdateAsync(item);
                    //return item.ID;
                }
                else
                {
                    //Insert item  
                    return database.InsertAsync(item);
                }
            }
        }
        public Task<int> DeleteItem(RegEntity id)
        {
            lock (locker)
            {
                return database.DeleteAsync(id);
            }
        }

        public Task<List<EventsList>> getAllEvents()
        {
            return database.Table<EventsList>().ToListAsync();
        }
        public Task<EventsList> GetItemEventsFirst()
        {
            lock (lockerEvent)
            {
                // return database.Table<EventsList>().Where(x => x.ID == 2);
                return database.Table<EventsList>().Where(x => x.ID == 1).FirstOrDefaultAsync();
            }
        }
        public Task<EventsList> GetItemEvent(string title)
        {
            lock (lockerEvent)
            {
                return database.Table<EventsList>().Where(x => x.title == title).FirstOrDefaultAsync();
            }
        }
        public Task<int> SaveEvents(EventsList item)
        {
            lock (lockerEvent)
            {
                if (item.ID != 0)
                {
                    //Update Item  
                    return database.UpdateAsync(item);
                    //return item.ID;
                }
                else
                {
                    //Insert item  
                    return database.InsertAsync(item);
                }
            }
        }

        public async Task<int> noOfEvents()
        {
            int allItems = await database.Table<EventsList>().CountAsync();
            return allItems;
        }

        ///
        public Task<RememberMeCredentials> GetSaveRemeber()
        {
            lock (lockerLogin)
            {
                return database.Table<RememberMeCredentials>().Where(x => x.ID == 1).FirstOrDefaultAsync();
            }
        }
        public Task<int> SaveRememberMeInfo(RememberMeCredentials item)
        {
            lock (lockerLogin)
            {
                if (item.ID != 0)
                {
                    //Update Item  
                    return database.UpdateAsync(item);
                    //return item.ID;
                }
                else
                {
                    //Insert item  
                    return database.InsertAsync(item);
                }
            }
        }
        public Task<int> DeleteSaveUser(RememberMeCredentials id)
        {
            lock (lockerLogin)
            {
                return database.DeleteAsync(id);
            }
        }
    }
}
