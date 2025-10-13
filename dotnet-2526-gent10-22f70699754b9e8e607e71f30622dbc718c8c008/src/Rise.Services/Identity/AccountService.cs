using System.Linq.Expressions;
using Rise.Shared.Common;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Facility;
using Rise.Persistence;
using Rise.Shared.Facility;
using Rise.Shared.Identity.Accounts;

namespace Rise.Services.Identity;

public class AccountService(ApplicationDbContext dbContext) : IAccountService
{
    public async Task<Result<AccountResponse.GetById>> GetByIdAsync(AccountRequest.GetById request,
        CancellationToken ctx = default)
    {
        var query = dbContext.Account.AsQueryable();
        

        Func<Facility,FacilityDto.Index> mapToFacilityDto = (f) => new FacilityDto.Index() // coole lambda TODO - vervangen met constructor
            {
                Id = f.Id,
                Name = f.Name
            }
        ;

        var account = await query.AsNoTracking()
            .Where(a => a.Id == request.AccountId)
            .Select(a => new AccountDto.Index()
            {
                Id = a.Id,
                UserId = a.UserId,
                FirstName = a.FirstName,
                LastName = a.LastName,
                BirthDate = a.BirthDate,
                Pronouns = a.Pronouns,
                Bio = a.Bio,
                FacilityDto = mapToFacilityDto(dbContext.Facilities.Find(a.Facility.Id))
            }).FirstAsync();

        return Result.Success(new AccountResponse.GetById
        {
            Account = account
        });
    }
}