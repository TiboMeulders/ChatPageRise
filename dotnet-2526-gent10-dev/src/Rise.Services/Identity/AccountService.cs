using System.Linq.Expressions;
using Rise.Shared.Common;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Facility;
using Rise.Persistence;
using Rise.Shared.Facility;
using Rise.Shared.Identity;
using Rise.Shared.Identity.Accounts;

namespace Rise.Services.Identity;

public class AccountService(ApplicationDbContext dbContext,  ISessionContextProvider sessionContextProvider) : IAccountService
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
            .Where(a => a.Id == request.Id)
            .Select(a => new AccountDto.Index()
            {
                Id = a.Id,
                UserId = a.UserId,
                FirstName = a.FirstName,
                LastName = a.LastName,
                BirthDate = a.BirthDate,
                Pronouns = a.Pronouns,
                Bio = a.Bio,
                FacilityDto = mapToFacilityDto(a.Facility)
            }).FirstAsync();

        return Result.Success(new AccountResponse.GetById
        {
            Account = account
        });
    }
    
    public async Task<Result> EditAsync(AccountRequest.Edit request,
        CancellationToken ctx = default)
    {
        var account = dbContext.Account.SingleOrDefaultAsync(a => a.Id == request.Id, ctx).Result;
        
        if (account is null)
            return Result.NotFound($"Account with Id '{request.Id}' was not found.");
        
        if(account.UserId != sessionContextProvider.User?.GetUserId() && (sessionContextProvider.User?.IsInRole(AppRoles.User)??true))
            return Result.Unauthorized($"You are not authorized to edit Account with Id '{request.Id}'.");

        if (request.FirstName != null)
        {
            if(!sessionContextProvider.User?.IsInRole(AppRoles.Educator)??false)
                return Result.Unauthorized("You are not authorized to edit your name.");
            
            account.FirstName = request.FirstName;
        }
        if (request.LastName != null)
        {
            if(!sessionContextProvider.User?.IsInRole(AppRoles.Educator)??false)
                return Result.Unauthorized("You are not authorized to edit your name.");
            
            account.LastName = request.LastName;
        }
        
        if (request.Bio != null)
            account.Bio = request.Bio.Trim();
        
        if (request.Pronouns != null)
            account.Pronouns = request.Pronouns.Trim();
        
        await dbContext.SaveChangesAsync(ctx);

        return Result.Success();
    }
}