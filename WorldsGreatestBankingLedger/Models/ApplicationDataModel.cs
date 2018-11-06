
using Models.Data;
using Models.Interfaces;

namespace Models
{

    public class ApplicationDataModel
    {
        private readonly IDataService _ds;

        public ApplicationDataModel(IDataService ds)
        {
            _ds = ds;
        }

        public User GetCurrentUser()
        {
            return _ds.GetCurrentUser();
        }

        public void RemoveCurrentUser()
        {
            _ds.RemoveCurrentUser();
        }

    }
}
