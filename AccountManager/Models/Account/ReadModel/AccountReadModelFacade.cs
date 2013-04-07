using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccountManager.Models.Account.ReadModel.Admin;
using AccountManager.Models.Account.ReadModel.Public;

namespace AccountManager.Models.Account.ReadModel
{
    public class AccountReadModelFacade
    {
        public List<AccountBalance> AccountBalances { get; private set; }
        public List<AccountInfo> AccountInfos { get; private set; }
        public List<Notification> Notifications { get; private set; }

        public AccountReadModelFacade(AccountBalanceProjection balance, AccountInfoProjection info, NotificationProjection notification)
        {
            AccountBalances = balance.AccountBalances;
            AccountInfos = info.AccountInfos;
            Notifications = notification.Notifications;
        }
    }
}