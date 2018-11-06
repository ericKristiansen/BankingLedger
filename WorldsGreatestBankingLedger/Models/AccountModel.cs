
using System.Collections.Generic;
using Models.Interfaces;

namespace Models
{
    public class AccountModel
    {
        readonly IDataService _ds;

        public AccountModel(IDataService ds)
        {
            _ds = ds;
        }

        public void Deposit(decimal d)
        {
            _ds.Deposit(d);
        }

        public string Balance()
        {
            return _ds.GetBalance();
        }

        public void Withdrawal(decimal d)
        {
            _ds.Withdrawal(d);
        }

        public List<string> GetHistory()
        {
            return _ds.GetHistory();
        }
    }
}
