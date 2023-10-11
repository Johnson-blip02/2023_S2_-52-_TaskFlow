using SQLiteNetExtensions.Extensions;

namespace TaskFlow.Model
{
    public class UserProfileModel : Database<UserProfile>, IDatabase<UserProfile>
    {
        /// <summary>
        /// Creates a new object for managing the user's profile in the database.
        /// </summary>
        /// <remarks>
        /// Initializes its abstract parent: <see cref="Database{T}"/>
        /// </remarks>
        public UserProfileModel() : base()
        {
            this.hasUpdates = true;
        }

        protected override void CreateTableAsync()
        {
            dbConn.CreateTable<UserProfile>();
        }

        protected override List<UserProfile> GetDataAbstract()
        {
            try
            {
                return dbConn.GetAllWithChildren<UserProfile>();
            }
            catch
            {
                return null;
            }
        }
    }
}
