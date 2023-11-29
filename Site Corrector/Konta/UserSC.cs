using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site_Corrector.Konta
{
    public class UserSC
    {
        private string login;
        private string haslo;

        public string Login
        {
            get
            {
                return login;
            }

            set
            {
                login = value;
            }
        }

        public string Haslo
        {
            get
            {
                return haslo;
            }

            set
            {
                haslo = value;
            }
        }

        public UserSC(string login, string haslo)
        {
            this.Login = login;
            this.Haslo = haslo;
        }
    }
}
