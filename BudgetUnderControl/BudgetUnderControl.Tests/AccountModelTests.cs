using BudgetUnderControl.Contracts.Models;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Model;
using BudgetUnderControl.Model.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BudgetUnderControl.Tests
{
    public class AccountModelTests
    {
        [Fact]
        public async Task activate_account_async_should_active_selected_account()
        {
            var accountRepositoryMock = new Mock<Domain.Repositiories.IAccountRepository>();
            var accountService = new AccountService(accountRepositoryMock.Object);

            var account = Account.Create("test", 1, 1, true, "", 1, Common.Enums.AccountType.Wallet, null, true);
            account.Currency = new Currency { Code = "PLN", Symbol = "zl" };
           
            accountRepositoryMock.Setup(x => x.GetAccountAsync(It.IsAny<int>())).ReturnsAsync(account);
            accountRepositoryMock.Setup(x => x.GetActualBalanceAsync(It.IsAny<int>())).ReturnsAsync(10);
            await accountService.ActivateAccountAsync(1);

            var user = await accountService.GetEditAccountDTOAsync(1);
            Assert.True(user.IsActive);

        }
    }
}
